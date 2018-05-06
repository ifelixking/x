var router = require('express').Router();
const query = require('../data')

const pageSize = 50

router.get('/', function (req, res) {
	let page = req.query.page; if (typeof page == 'undefined') { page = 0 }
	query(`SELECT * FROM art LIMIT ${page * pageSize}, ${pageSize}`, (err, result, fields) => {
		if (err) { console.log(err) }
		res.send(JSON.stringify({ page, pageSize, items: result }));
	})
})

router.get('/pageCount', function (req, res) {
	query(`SELECT count(*) AS recordCount FROM art`, (err, result, fields) => {
		res.send(JSON.stringify({ pageCount: Math.ceil(result[0]['recordCount'] / pageSize) }));
	})
})

module.exports = router;