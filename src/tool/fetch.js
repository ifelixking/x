var fs = require("fs");
var path = require("path");
var request = require("request");
var mysql = require('mysql')
var async = require('async')

var pool = mysql.createPool({
	host: 'localhost',
	user: 'root',
	password: '000000',
	database: 'x',
	port: 3306
})

let data = require('./data.json')

var dirPath = path.join(__dirname, "pic");
if (!fs.existsSync(dirPath)) {
	fs.mkdirSync(dirPath);
	console.log("文件夹创建成功");
} else {
	console.log("文件夹已存在");
}

pool.getConnection((err, conn) => {
	async.parallelLimit(data.map((a, i) => (callback) => {
		let filename = path.basename(a.image);
		let stream = fs.createWriteStream(path.join(dirPath, filename));
		request(a.image).pipe(stream).on("close", function (err) {
			conn.query('insert into actor2(name, image, order1) values(?,?,?)', [a.name, filename, i], (err) => {
				console.log(`${i} - ${a.name} [OK]`);
				callback()
			})
		});
	}), 10, () => {
		conn.release()
		console.log('[finish!]')
	})
})
