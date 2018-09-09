function connectDatabase(connectionString, callback) {
	setTimeout(() => {
		let err = null
		// err = 'error'			// 解开注释, 可以触发 Promise 的 reject
		let conn = {}
		callback(err, conn)
	}, 100)
}

const connectionString = "host:localhost;user:root;password:123456"

// 参数为 非 Promise, 非 thenable, 直接返回 resolved 状态的 Promise
Promise.resolve(connectionString).then(connectionString=>{
	console.log('before Promise.resolve called')
	// 参数为 thenable, 构造Promise, 并立即(其实并不是同步的)执行 then 方法
	let promise = Promise.resolve({then:(resolve, reject)=>{
		console.log('then method called')
		connectDatabase(connectionString, (err, conn)=>{
			err ? reject(err) : resolve(conn)
		})
	}})
	console.log('after Promise.resolve called')
	return promise
}).then(conn=>{
	console.log('连接成功', conn)
}).catch(err=>{
	console.log('错误', err)
})

console.log('code end')