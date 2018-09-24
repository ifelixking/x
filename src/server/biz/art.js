var router = require('express').Router();
const { query } = require('../data')
const elasticsearch = require('elasticsearch')
const esClient = new elasticsearch.Client({ host: '192.168.31.187:9200', log: 'trace' });

const pageSize = 50

function sqlArtList(params, tagID) {
	// 查询art列表, 并同时查出art的所有actors
	let sql = `SELECT art.id, art.text, art.downloads, art.images, art.date,
		(SELECT GROUP_CONCAT(CONCAT(actor.id, ',', actor.name) SEPARATOR ';') FROM rel_actor_art LEFT JOIN actor ON rel_actor_art.actorID=actor.id WHERE rel_actor_art.artID=art.id) AS actors
		FROM art`
	if (typeof tagID !== 'undefined') {
		sql += ' LEFT JOIN rel_art_tag ON art.id = rel_art_tag.artID WHERE rel_art_tag.tagID = ?'
		params.push(tagID);
	} else {
		sql += ' WHERE TRUE'
	}
	return sql
}

router.get('/', function (req, res) {
	let page = req.query.page; if (typeof page == 'undefined') { page = 0 }
	const tagID = req.query.tag;
	let params = [];
	let sql = sqlArtList(params, tagID, page)
	sql += ` ORDER BY art.date desc LIMIT ${page * pageSize}, ${pageSize} `;
	query(sql, params, (err, result, fields) => {
		if (err) { console.log(err) }
		res.send(JSON.stringify({ page, pageSize, items: result }));
	})
})

router.get('/pageCount', function (req, res) {
	const tagID = req.query.tag;
	let params = [];
	let sql;
	if (typeof tagID == 'undefined') {
		sql = `SELECT count(*) AS recordCount FROM art`
	} else {
		sql = `SELECT count(*) AS recordCount FROM art 
			LEFT JOIN rel_art_tag 	ON art.id = rel_art_tag.artID
			WHERE rel_art_tag.tagID = ?`
		params.push(tagID);
	}
	query(sql, params, (err, result, fields) => {
		res.send(JSON.stringify({ pageCount: Math.ceil(result[0]['recordCount'] / pageSize) }));
	})
})

router.get('/search', function (req, res) {
	let k = req.query.k;
	if (typeof k == 'undefined' || k == '') { res.redirect('/api/art') }
	let page = req.query.page; if (typeof page == 'undefined') { page = 0 }
	esClient.search({
		index: 'x',
		type: 'art',
		body: { "query": { "bool": { "must": [{ "term": { "text": k } }] } }, "from": page * pageSize, "size": pageSize, }
	}).then(function (resp) {
		var items = Array.from(resp.hits.hits, i => i._source);
		res.send(JSON.stringify({ page, pageSize, items, pageCount: Math.ceil(resp.hits.total / pageSize), tookTime: resp.took }));
	}, function (err) {
		console.trace(err.message);
	});
})

router.get('/recent', function (req, res) {
	const tagID = req.query.tag;
	let params = [];
	let sql = sqlArtList(params, tagID)
	sql += ' AND art.date > DATE_ADD((select date from art ORDER BY date DESC limit 1) ,INTERVAL -7 day) ORDER BY art.date desc LIMIT 100';
	query(sql, params, (err, result, fields) => {
		if (err) { console.log(err) }
		res.send(JSON.stringify({ items: result }));
	})
})

module.exports = router;

// const sql_orderby_limit = ` ORDER BY art.date desc LIMIT ${page * pageSize}, ${pageSize} `;	
// // 查询art列表, 并同时查出art的所有actors
// let sql = `SELECT art.id, art.text, art.downloads, art.images, art.date,
// 	(SELECT GROUP_CONCAT(CONCAT(actor.id, ',', actor.name) SEPARATOR ';') FROM rel_actor_art LEFT JOIN actor ON rel_actor_art.actorID=actor.id WHERE rel_actor_art.artID=art.id) AS actors
// 	FROM art`
// if (typeof tagID !== 'undefined') {
// 	sql += ' LEFT JOIN rel_art_tag ON art.id = rel_art_tag.artID WHERE rel_art_tag.tagID = ?'
// 	params.push(tagID);
// }
// sql += sql_orderby_limit


// if (typeof tagID == 'undefined') {
// 	sql = `SELECT art.id, art.text, art.downloads, art.images, art.date FROM art where ${sql_cond_date} ${sql_orderby_limit}`
// } else {
// 	sql = `SELECT art.id, art.text, art.downloads, art.images, art.date FROM art 
// 		LEFT JOIN rel_art_tag 	ON art.id = rel_art_tag.artID
// 		WHERE ${sql_cond_date} AND rel_art_tag.tagID = ?
// 		${sql_orderby_limit}`
// 	params.push(tagID);
// }