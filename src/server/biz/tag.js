var router = require('express').Router();
const query = require('../data')

router.get('/', function (req, res) {
	query(`SELECT * FROM tag`, (err, result, fields) => {
		if (err) { console.log(err) }
		res.send(JSON.stringify({ items: result }));
	})
})

module.exports = router;