const router = require('express').Router();
const { query, queryP } = require('../data')

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

router.put('/:id', function (req, res) {
	query('update actor set matchWords=? where id=?', [req.body.matchWords, req.params.id], (err, result) => {
		res.send(JSON.stringify({ items: result }));
	})
})

// router.get('/:id/match', function (req, res) {
// 	function *(){}()

// 	queryP(`select * from actor where id=${req.params.id}`).then((result) => {
		
// 	})

// 	// query(`select * from actor where id=${req.params.id}`, (err, result) => {

// 	// }

// 	// query(`select * from rel_actor_art where actorID=${req.params.id}`, (err, result) => {
// 	// 	result.map()


// 	// })


// })



module.exports = router;