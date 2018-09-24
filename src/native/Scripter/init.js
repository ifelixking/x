(function () {

	if (!Array.from) {
		Array.from = function (el) {
			return Array.apply(this, el);
		}
	}

	var scriptJQuery = document.createElement('script'); scriptJQuery.src = 'https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(scriptJQuery);

	window._x_select = function (selector) {
		debugger;
		try {
			var ceList = JSON.parse(selector)

			var func = function (nodes, query) {
				if (!nodes || !nodes.length) { return }
				if (nodes.length == 1) {
					var node = nodes[0]
					query.strJQuery += node.tagName + ' '
					if (node.children && node.children.length) {
						func(node.children, query)
					}
					if (node.output || !node.children || !node.children.length) {
						query.node = node
					}
				} else {
					nodes.forEach(function (node) {
						var subQuery = { strJQuery: node.tagName, subs: [] }
						if (node.output || !node.children || !node.children.length) {
							subQuery.node = node
						}
						func(node.children, subQuery)
						query.subs.push(subQuery)
					})
				}
			}
			var query = { strJQuery: '', subs: [] }; func(ceList, query)

			var func2 = function (q, parentItems) {
				var items = q.strJQuery == '' ? $(document) : $(q.strJQuery)
				items.each(function () {
					var selectItem = {}, ele = this
					if (q.node) {
						selectItem.attrs = []
						if (q.node.config.col_content) {
							selectItem.attrs.push({ name: 'content', value: ele.innerText })
						}
						q.node.config.attrs.forEach(function (i) {
							var attrName = q.node.attributes[i].name
							selectItem.attrs.push({ name: attrName, value: ele.attributes[attrName] })
						})
					}
					parentItems.push(selectItem)
					if (q.subs.length) {
						selectItem.subItems = []
						q.subs.forEach(function (subQuery) {
							func2(subQuery, selectItem.subItems)
						})
					}
				})
			}
			var result = []; func2(query, result)

			return JSON.stringify(result);
		} catch (ex) {
			console.log(ex)
			return 'error:' + ex
		}
	}

	var g_eleLastHover, g_eleLastBgColor;
	var highLightElement = function (e) {
		if (e.target != g_eleLastHover) {
			if (g_eleLastHover) { g_eleLastHover.style.backgroundColor = g_eleLastBgColor }
			g_eleLastHover = e.target
			g_eleLastBgColor = g_eleLastHover.style.backgroundColor
			g_eleLastHover.style.backgroundColor = '#A0C5E8'
		}
	}

	var captureElement = function (e) {
		//var processElement = function (ele) {
		//	// debugger
		//	var obj = {}
		//	obj.tagName = ele.tagName
		//	obj.innerText = ele.innerText
		//	obj.classNames = ele.className.split(' ')

		//	if (ele.parentElement) {
		//		obj.index = Array.from(ele.parentElement.children).indexOf(ele)
		//		obj.isLast = obj.index == (ele.parentElement.children.length - 1)
		//	}

		//	obj.attributes = []
		//	if (ele.attributes) for (var i = 0; i < ele.attributes.length; ++i) {
		//		obj.attributes.push({ name: ele.attributes[i].name, value: ele.attributes[i].value })
		//	}

		//	return obj
		//}
		//var result = []
		//for (var element = e.target; element != null; element = element.parentElement) {
		//	result.push(processElement(element))
		//}
		//// console.log(result)
		//window.external.onCaptureElement(JSON.stringify(result))

		g_captureElement.push(e.target)
		e.preventDefault();
		e.stopPropagation();
		e.stopImmediatePropagation()
		return false
	}

	var g_captureElement = [];
	window._x_captureStart = function () {
		g_captureElement = []
		window.document.addEventListener("mouseover", highLightElement, true)
		window.document.addEventListener("click", captureElement, true)
	}

	window._x_captureFinish = function () {

		debugger;

		var resultNodes = []

		var eleToNode = function (ele) {
			var node = {
				tagName: ele.tagName,
				innerText: ele.innerText,
				classNames: ele.className.split(' '),
				attributes: [],
				children: [],
				_element: ele
			}
			if (ele.parentElement) {
				node.index = Array.from(ele.parentElement.children).indexOf(ele)
				node.isLast = node.index == (ele.parentElement.children.length - 1)
			}
			if (ele.attributes) for (var i = 0; i < ele.attributes.length; ++i) {
				node.attributes.push({ name: ele.attributes[i].name, value: ele.attributes[i].value })
			}
			return node
		}

		var findNodeByEle = function (nodes, ele) {
			for (var i = 0; i < nodes.length; ++i) {
				var node = nodes[i];
				if (node._element == ele) { return node; }
				var childNode = findNodeByEle(node.children, ele)
				if (childNode) { return childNode }
			}
		}

		g_captureElement.forEach(function (ele) {
			// g_captureElement 中的 element 都是要 输出的, 如果直接就在 result 中, 标记 output 就行
			var existNode = findNodeByEle(resultNodes, ele)
			if (existNode) { existNode.output = true; return }
			// 如果不在 result 中, 就需要递归 parent, 找到 same node, 并挂到 result 上
			var lastChildNode = null
			for (var itorEle = ele; ; itorEle = itorEle.parentElement) {
				var node = eleToNode(itorEle)
				lastChildNode && node.children.push(lastChildNode)
				lastChildNode = node
				if (itorEle.parentElement) {
					var existParentNode = findNodeByEle(resultNodes, itorEle.parentElement)
					if (existParentNode) {
						// 如果找到, 就直接挂上去, 不再向 parent 递归了
						existParentNode.children.push(node); return
					}
				} else { break; }
			}
			resultNodes.push(lastChildNode)
		})

		window._x_captureCancel()

		var deleteElementField = function (nodes) {
			nodes.forEach(function (node) {
				delete node._element
				deleteElementField(node.children)
			})
		}
		deleteElementField(resultNodes)
		window.external.onCaptureElement(JSON.stringify(resultNodes))
	}

	window._x_captureCancel = function () {
		g_captureElement = []
		if (g_eleLastHover) { g_eleLastHover.style.backgroundColor = g_eleLastBgColor }
		g_eleLastHover = null; g_eleLastBgColor = null
		window.document.removeEventListener("mouseover", highLightElement, true)
		window.document.removeEventListener("click", captureElement, true)
	}

})()