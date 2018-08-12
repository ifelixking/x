var mysql  = require('mysql')

var pool = mysql.createPool({
	host: 'localhost',
	user: 'root',
	password: '000000',
	database: 'x',
	port: 3306
})


var query = function(sql, options, callback){
	if (typeof options == 'function' && typeof callback == 'undefined'){
		callback = options;
		options = null;
	}
	pool.getConnection(function(err, conn){
		if (err){
			callback(err, null, null)
		}else{
			conn.query(sql, options, function(err, results, fields){
				conn.release();
				callback(err, results, fields);
			})
		}
	})
}

var log = function(remoteIP, action, callback){
	const sql = 'insert into access(ip, action, `time`) values(?,?,now())'
	pool.getConnection(function(err, conn){
		if (err){
			callback && callback(err, null, null)
		}else{
			conn.query(sql, [remoteIP, action], function(err, results){
				conn.release();
				callback && callback(err, results);
			})
		}
	})
}

module.exports = { query, log };