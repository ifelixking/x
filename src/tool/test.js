const co = require('co')
const { promisify } = require("es6-promisify");

// 异步处理 data - 1
function getData1(data, callback) {
	setTimeout(() => {
		callback(null, data - 1)
	}, 1)
}

// 异步处理 data + 2
function getData2(data, callback) {
	setTimeout(() => {
		let err = null
		err = 'err2'		// 解除注释 可以看到catch方法被调用
		callback(err, 2 + data)
	}, 1)
}

// 异步处理 data * 3
function getData3(data, callback) {
	setTimeout(() => {
		callback(null, 3 * data)
	}, 1)
}

// 这是一个 Generator 的执行器, 可以不断的执行一个Generator直到done为true
function myCo(gen) {
	// 创建 Generator
	let itor = gen()

	// 关键: 用于保存 全局 的 Promise 的 resolve 和  reject 方法
	let co_resolve, co_reject

	// 执行 next
	const executeNext = function (param) {

		// 调用 next, 外部的 yield 代码变为非阻塞, 但目前还没有返回值, 需要下次next时给出
		// next的返回值为 { done: bool, value: Promise }
		let ret = itor.next(param)

		// 判断 是否 done
		if (ret.done) {
			// 如果 done, 则立刻结束, 标记全局 Primose 为 resolved
			// 这时 外部调用 myCo 的返回值的 then 方法将会获得参数(执行的结果), 并执行
			co_resolve(ret.value)
		} else {
			// 还没有 done
			// 因为 ret.value 是一个Promise, 所有要运行它, 就要调用 then...catch
			ret.value.then(result => {
				// 如果 Promise 执行成功, 则继续 调用 next
				// 主要 要将结果告知外部, 使用next的参数, 给出 yield 的返回值
				executeNext(result)
			}).catch(err => {
				// 如果失败, 就提前结束, 标记全局 Primose 为 rejected
				// 这时 外部调用 myCo 的返回值的 catch 方法 将会 获得错误信息, 并执行其 catch 中的代码
				co_reject(err)
			})
		}
	}

	// 这里很关键
	// myCo 函数显然不是同一个同步完成的函数, 需要获得最终的结果或者错误, 必须依靠 Promise
	// 所有这里返回了一个 Promise 使得外部能 then...catch 结果
	// Promise.resolve 的好处是能立即(非同步)执行then方法
	return Promise.resolve({
		then: (resolve, reject) => {
			// 关键点: 
			// 将这个最顶层的 resolve 和 reject 保存在 myCo闭包全局变量中, 能使 任意一级 executeNext 有机会能 返回结果 或是 抛出错误
			co_resolve = resolve
			co_reject = reject
			// 执行 next
			// 注: 这里的没有给参数, 是因为第一次next不需要参数
			executeNext()
		}
	})
}

// 最终效果
co(function* () {
	// promisify(getData1) 的结果是 一个 Promise Creator
	// promisify(getData1)(2) 的结果 是 一个 Promise
	// yield promisify(getData1)(2) 会等待 generator 调用 next
	// next 的返回值 中就包含了 刚刚的 Promise, 注: next 返回 { done: bool, value: obj }
	let a = yield promisify(getData1)(2)
	console.log('a:', a)
	let b = yield promisify(getData2)(a)
	console.log('b:', b)
	let c = yield promisify(getData3)(b)
	console.log('c:', c)
	return c + 1000	

	// 也可以更加嚣张的鱼刺调用, 注: yield 语句的范围最好用 括号明确 框定
	// return (yield promisify(getData3)(yield promisify(getData2)(yield promisify(getData1)(2)))) + 1000
}).then(result => {
	console.log('result:', result)
}).catch((err) => {
	console.log('error:', err)
})
console.log('end')