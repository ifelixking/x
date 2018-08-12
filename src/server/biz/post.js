var router = require('express').Router();
const { query } = require('../data')
const pageSize = 20

// 发帖
router.post('/', function (req, res) {
	getOrCreateUserByIP(req.client.remoteAddress, true, function (creator) {
		query('insert into post(title, content, creator, time, attachment, hasAttachment) values(?,?,?,now(),?,?)',
			[req.body.title, req.body.content, creator, req.body.attachment, req.body.hasAttachment], function (err, result) {
				if (err) { res.send(err); return }
				const postID = result.insertId
				res.send({ successed: true, postID })
			})
	})
})

// 贴子列表
router.get('/', function (req, res) {
	let page = req.query.page || 0
	const sql_orderby_limit = ` ORDER BY post.id desc LIMIT ${page * pageSize}, ${pageSize} `;
	query(`select post.id, post.title, post.time, post.lastReplyUser, post.lastReplyTime, post.creator as creatorID, user.ip, post.hasAttachment 
		from post left join user on post.creator=user.id where type=0 ${sql_orderby_limit}`, function (err, result) {
			if (err) { res.send(err); return }
			res.send(JSON.stringify({ page, pageSize, items: result }));
		})
})

// 帖子页数
router.get('/pageCount', function (req, res) {
	let sql = 'select count(*) as recordCount from post where type=0'
	query(sql, [], (err, result, fields) => {
		res.send(JSON.stringify({ pageCount: Math.ceil(result[0]['recordCount'] / pageSize) }));
	})
})

// 回复
router.post('/reply', function (req, res) {
	getOrCreateUserByIP(req.client.remoteAddress, true, function (userID) {
		// insert reply
		query('insert into post(content, creator, time, attachment, hasAttachment, parentID, hostID, type) values(?,?,now(),?,?,?,?,1)',
			[req.body.content, userID, req.body.attachment, req.body.hasAttachment, req.body.parentID, req.body.hostID], function (err, result) {
				if (err) { res.send(err); return }
				const postID = result.insertId
				// update post
				query(`update post set lastReplyUser=${userID}, lastReplyTime=now(), hasAttachment=hasAttachment|${req.body.hasAttachment}, hasReply=1 
					where id=${req.body.parentID}`, function (err, result) {
						res.send({ successed: true, postID })
					})
			})
	})
})

// 帖子详情, 一级回复列表
router.get('/:id', function (req, res) {
	let page = req.query.page || 0
	const sql_orderby_limit = ` ORDER BY post.id LIMIT ${page * pageSize}, ${pageSize} `;
	query(`select post.id, post.title, post.content, post.time, post.creator as creatorID, user.ip, post.attachment 
		from post left join user on post.creator=user.id where post.id=${req.params.id} or post.parentID=${req.params.id} ${sql_orderby_limit}`, function (err, result) {
			if (err) { res.send(err); return }
			res.send(JSON.stringify({ page, pageSize, items: result }));
		})
})

// 帖子详情, 一级回复列表 页数
router.get('/:id/pageCount', function (req, res) {
	let sql = `select count(*) as recordCount from post where post.id=${req.params.id} or post.parentID=${req.params.id}`
	query(sql, [], (err, result, fields) => {
		res.send(JSON.stringify({ pageCount: Math.ceil(result[0]['recordCount'] / pageSize) }));
	})
})

// 回复列表
router.get('/reply/:id', function (req, res) {
	query(`select post.id, post.title, post.content, post.time, post.creator as creatorID, user.ip, post.attachment 
		from post left join user on post.creator=user.id where post.parentID=${req.params.id} order by post.id desc limit 50`, function (err, result) {
			if (err) { res.send(err); return }
			res.send(result)
		})
})

function getOrCreateUserByIP(ip, forPost, callback) {
	query(`select id from user where ip='${ip}'`, function (err, result) {
		if (err) { res.send(err); return }
		if (result.length) {
			const userID = result[0].id
			if (forPost) {
				query('update user set lastPostTime=now(), postCount=postCount+1', function (err, result) {
					if (err) { res.send(err); return }
					callback(userID)
				})
			} else {
				callback(userID)
			}
		} else {
			let sql, params
			if (forPost) {
				sql = 'insert user(ip, lastPostTime, postCount) values(?,now(),?)'
				params = [ip, 1]
			} else {
				sql = 'insert user(ip) values(?)'
				params = [ip]
			}
			query(sql, params, function (err, result) {
				if (err) { res.send(err); return }
				const userID = result.insertId
				callback(userID)
			})
		}
	})

}

module.exports = router;