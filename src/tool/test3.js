function* gen() {
	yield 1				// 函数协程挂起, 等待下次 next 的调用
	yield 2				// 函数协程挂起, 等待下次 next 的调用
	return 3			// 遇到 return 协程结束
}
let itor = gen()
console.log(itor.next().value)		// next().done 为 false, 输出 1
console.log(itor.next().value)		// next().done 为 false, 输出 2
console.log(itor.next().value)		// next().done 为 true, 输出 3

function* gen() {
	let a = yield 1
	console.log('a:', a)		// a = 100
	let b = yield 2 + a
	console.log('b:', b)		// b = 3389
	return b * 10
}
let itor = gen()
let c = itor.next().value
console.log('c:', c)				// c = 1
let d = itor.next(c * 100).value;
console.log('d:', d)				// d = 102
let e = itor.next(3389).value;
console.log('e:', e)				// e = 33890