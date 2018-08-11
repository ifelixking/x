var router = require('express').Router();
const { query } = require('../data')

router.get('/', function(req, res){
	// 	res.send("actor get");
	query("SELECT * FROM page LIMIT 10", (err, result, fields)=>{
		res.send(JSON.stringify(result));
	})
})

module.exports = router;