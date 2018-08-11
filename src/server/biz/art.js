var router = require('express').Router();
const { query } = require('../data')
const elasticsearch = require('elasticsearch')
const esClient = new elasticsearch.Client({ host: '192.168.31.187:9200', log: 'trace' });

const pageSize = 50

router.get('/', function (req, res) {
	let page = req.query.page; if (typeof page == 'undefined') { page = 0 }
	const tagID = req.query.tag;
	const sql_orderby_limit = ` ORDER BY page.date desc LIMIT ${page * pageSize}, ${pageSize} `;
	let params = [];
	let sql;
	if (typeof tagID == 'undefined') {
		sql = `SELECT art.id, art.text, art.downloads, art.images, page.date FROM art LEFT JOIN page ON art.pageID = page.id ${sql_orderby_limit}`
	} else {
		sql = `SELECT art.id, art.text, art.downloads, art.images, page.date FROM art 
			LEFT JOIN page 			ON art.pageID = page.id 
			LEFT JOIN rel_art_tag 	ON art.id = rel_art_tag.artID
			WHERE rel_art_tag.tagID = ?
			${sql_orderby_limit}`
		params.push(tagID);
	}
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
		body: { "query": { "bool": { "must": [{ "term": { "text": k } }] } }, "from": page*pageSize, "size": pageSize, }
	}).then(function (resp) {
		var items = Array.from(resp.hits.hits, i=>i._source); 
		res.send(JSON.stringify({ page, pageSize, items, pageCount: Math.ceil(resp.hits.total / pageSize), tookTime: resp.took }));
	}, function (err) {
		console.trace(err.message);
	});
})

module.exports = router;