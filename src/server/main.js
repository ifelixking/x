const express = require('express')
const path = require('path')
var app = express();

app.use('/api', express.Router()
	.use('/actor', require('./biz/actor.js'))
);
app.use('/asset', express.static(path.join(__dirname, '../../dist/static/asset')));
app.use('/admin', express.Router().get('/*', function(req, res){
	// console.log(req);
	// res.send(req.baseUrl);
	res.sendFile(path.join(__dirname, '../../dist/static/admin.html'))
}));
app.use('/*', express.Router().get('/*', function(req, res){
	// console.log(req);
	// res.send(req.baseUrl);
	res.sendFile(path.join(__dirname, '../../dist/static/index.html'))
}));

var server = app.listen(80, function(){
	var host = server.address().address;
	var port = server.address().port;
	console.log(`server is started at host:${host} port:${port}`);
});