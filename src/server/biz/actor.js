var router = require('express').Router();
const { query } = require('../data')

router.get('/', function (req, res) {
	query('SELECT * FROM actor order by `order1`,`order2` limit 0,500', (err, result, fields) => {
		if (err) { console.log(err) }
		res.send(JSON.stringify({ items: result }));
	})
})

router.get('/:id/art', function (req, res) {
	query(`SELECT art.id, art.text, art.downloads, art.images, page.date FROM art 
		RIGHT JOIN rel_actor_art ON art.id=rel_actor_art.artID
		LEFT JOIN page on art.pageID=page.id where rel_actor_art.actorID=${req.params.id}`, (err, result, fields) => {
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



module.exports = router;