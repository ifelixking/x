var router = require('express').Router();

router.get('/', function(req, res){
	res.send("actor get");
})

module.exports = router;