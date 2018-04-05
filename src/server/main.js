const express = require('express')
const path = require('path')
var app = express();

app.use(express.static(path.join(__dirname, '../../dist/static')));
app.use('/actor', require('./biz/actor.js'))

var server = app.listen(80, function(){
	var host = server.address().address;
	var port = server.address().port;
	console.log(`server is started at host:${host} port:${port}`);
});