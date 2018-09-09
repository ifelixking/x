const {promisify} = require("es6-promisify");

// 函数 参数 callback 前有不止一个参数
function connectDatabase(connectionString, param1, param2, callback) {
	setTimeout(() => {
		console.log(connectionString, param1, param2)
		let err = null
		// err = 'error'			// 解开注释, 可以触发 Promise 的 reject
		let conn = {}
		// 注: 这里的写法在转Promise时是有问题的, 不支持多个参数, 只支持 两个参数(err, result), 
		//     因为 Promise 的 回调函数 resolve(result) 和 reject(err) 都只认一个参数
		callback(err, conn, 'result1', 'result2')
	}, 100)
}
// 没有参数
function getWeather(callback){
	setTimeout(()=>{
		let err = null
		// err = 'error'			// 解开注释, 可以触发 Promise 的 reject
		let result = '晴天'
		callback(err, result)
	})
}

// // 手动转为Promise版
// function connectDatabase_promise(connectionString) {
// 	return new Promise((resolve, reject) => {
// 		connectDatabase(connectionString, (err, conn) => {
// 			if (err) {
// 				reject(err)
// 			} else {
// 				resolve(conn)
// 			}
// 		})
// 	})
// }

// function promisify(func) {
// 	return function () {		// 注: 这里不能使用()=>{}, 因为箭头函数里的arguments为上级函数的参数
// 		return new Promise((resolve, reject) => {
// 			// 注: 这里的 arguments 是再上一层函数的参数
// 			func(...arguments, (err, ...results) => {
// 				// 这里的写法有问题
// 				// 这里即使是 使用 ...results 也无法传递多个参数
// 				// 正确的做法是, reject 时, 额外信息都放在 err 对象中, resolve 时参数都压到数组中
// 				err ? reject(err, ...results) : resolve(...results)
// 			})
// 		})
// 	}
// }

const connectDatabase_promise = promisify(connectDatabase)
const getWeather_promise = promisify(getWeather)

// 调用
connectDatabase_promise("host:localhost;user:root;password:123456", 1, 2).then((conn, result1, result2) => {
	console.log('连接成功', conn, result1, result2)			// result1 和 result2 并没有 传过来
	return getWeather_promise()
}).then((result)=>{
	console.log('获取天气成功', result)
}).catch((err, result0, result1, result2) => {
	console.log('错误', err, result0, result1, result2)	// result0 result1 和 result2 并没有 传过来
})