const router = require('express').Router();
const { query, queryP } = require('../data')
const co = require('co')
const utils = require('../../web/common/utils')

router.get('/', function (req, res) {
	query('SELECT * FROM actor order by `order1`,`order2` limit 0,100', (err, result, fields) => {
		if (err) { console.log(err) }
		res.send(JSON.stringify({ items: result }));
	})
})

router.get('/:id/art', function (req, res) {
	query(`SELECT art.id, art.text, art.downloads, art.images, art.date FROM art 
		RIGHT JOIN rel_actor_art ON art.id=rel_actor_art.artID
		where rel_actor_art.actorID=${req.params.id}`, (err, result, fields) => {
			if (err) { console.log(err) }
			res.send(JSON.stringify({ items: result }));
		})
})

router.get('/:id', function (req, res) {
	query(`SELECT * FROM actor WHERE id=${req.params.id}`, (err, result, fields) => {
		if (err) { console.log(err) }
		if (result.length == 0) { res.status(404).send('{}'); return; }
		res.send(JSON.stringify({ data: result[0] }));
	})
})

// 修改 actor
router.put('/:id', function (req, res) {
	queryP('update actor set matchWords=? where id=?', [req.body.matchWords, req.params.id]).then((result) => {
		// 执行匹配
		if (typeof req.body.matchWords !== 'undefined') {
			return queryP(`select * from actor where id=${actorID}`).then(result => {
				if (result.length) {
					return doActorMatchP(result[0])
				} else {
					return Promise.reject('data not found')
				}
			})
		} else {
			return Promise.resolve(result)
		}
	}).then(result => {
		res.send(JSON.stringify({ data: result }));
	}).catch(err => {
		console.log(err)
	})
})

router.post('/buildRel', function (req, res) {
	co(function* () {
		let actors = yield queryP(`select * from actor`)
		for (let i = 0; i < actors.length; ++i) {
			yield doActorMatchP(actors[i])
		}
	}).then(() => {
		res.send(JSON.stringify({ data: 'success' }))
	}).catch(err => {
		console.log(err)
	})
})

// =======================================================================================================================================
function doActorMatchP(actor) {
	return co(function* () {
		const matchWords = [].concat([actor.name], actor.matchWords ? actor.matchWords.split(';').filter(a => a.trim() != '') : [])
		let artIDsList = yield Promise.all(matchWords.map(a => queryP(`select art.id from art left join rel_art_tag on art.id=rel_art_tag.artID where (rel_art_tag.tagID=1 or rel_art_tag.tagID=2) and art.text like '%${a}%'`)))
		let newRels = [].concat(...artIDsList).map(a => a.id)
		let oldRels = (yield queryP(`select * from rel_actor_art where actorID=${actor.id}`)).map(a => a.artID)
		let diff = utils.arrayDiff(oldRels, newRels);
		return Promise.all([].concat(
			...(diff.news.map(a => queryP(`insert into rel_actor_art(actorID, artID) values(?,?)`, [actor.id, a]))),
			...(diff.deletes.map(a => queryP(`delete from rel_actor_art where actorID=? and artID=?`, [actor.id, a])))
		))
	})
}

module.exports = router;