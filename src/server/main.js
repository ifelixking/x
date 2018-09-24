const express = require('express')
const bodyParser = require('body-parser')
const path = require('path')
const { log } = require('./data')
const proxy = require('express-http-proxy');

const app = express();
app.use(bodyParser.json())

app.use('/api', express.Router()
	.use('/page', require('./biz/page.js'))
	.use('/art', require('./biz/art.js'))
	.use('/tag', require('./biz/tag.js'))
	.use('/actor', require('./biz/actor.js'))
	.use('/post', require('./biz/post.js'))
);

app.use('/proxy', proxy('www.liyh.com', {
	userResDecorator: function(proxyRes, proxyResData, userReq, userRes) {
		return `
			<script>
				window.alert = function(msg){
					console.log('liyh:', msg)
				}
			</script>
		` + proxyResData.toString('utf8');
	}
}))

app.use('/asset', express.static(path.join(__dirname, '../../dist/static/asset')));
app.use('/admin', express.Router().get('/*', function(req, res){
	// console.log(req);
	// res.send(req.baseUrl);
	res.sendFile(path.join(__dirname, '../../dist/static/admin.html'))
}));
app.use('/*', express.Router().get('/*', function(req, res){
	log(req.ip, '/');
	res.sendFile(path.join(__dirname, '../../dist/static/index.html'))
}));

var server = app.listen(80, function(){
	var host = server.address().address;
	var port = server.address().port;
	console.log(`server is started at host:${host} port:${port}`);
});