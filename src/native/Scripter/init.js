(function () {

	if (!Array.from) {
		Array.from = function (el) {
			return Array.apply(this, el);
		}
	}

	var scriptJQuery = document.createElement('script'); scriptJQuery.src = 'https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(scriptJQuery);

	window._x_select = function (ceList) {
		debugger;
		try {
			// var ceList = JSON.parse(selector)

			var getItemStrJQuery = function (node) {
				var result = ''
				node.config.tag && (result += node.tagName)
				result += node.config.classes.map(function (a) { return node.classNames[a] ? '.' + node.classNames[a] : '' }).join('')
				node.config.index && (result += ':nth-child(' + (node.index + 1) + ')')
				node.config.first && (result += ':first')
				node.config.last && (result += ':last')
				node.config.odd && (result += ':odd')
				node.config.even && (result += ':even')
				node.config.context && (result += ':contains("' + node.innerText + '")')
				result += node.config.attrs.map(function (a) { return '[' + node.attributes[a].name + '="' + node.attributes[a].value + '"]' }).join('')
				return result;
			}

			var func = function (nodes, query) {
				if (!nodes || !nodes.length) { return }
				if (nodes.length == 1) {
					var node = nodes[0]
					query.strJQuery && (query.strJQuery += '>')
					query.strJQuery += getItemStrJQuery(node)
					if (node.children && node.children.length) {
						func(node.children, query)
					}
					if (node.output || !node.children || !node.children.length) {
						query.node = node
					}
				} else {
					nodes.forEach(function (node) {
						var subQuery = { strJQuery: getItemStrJQuery(node), subs: [] }
						if (node.output || !node.children || !node.children.length) {
							subQuery.node = node
						}
						func(node.children, subQuery)
						query.subs.push(subQuery)
					})
				}
			}
			var query = { strJQuery: '', subs: [] }; func(ceList, query)

			var func2 = function (q, parentItems, parent) {
				var items
				if (parent) {
					items = parent.find(q.strJQuery)
				} else {
					items = q.strJQuery == '' ? $(document) : $(q.strJQuery)
				}
				items.each(function (idx) {
					var selectItem = { jq: q.strJQuery, idx: idx }, ele = this
					if (q.node) {
						selectItem.attrs = []
						if (q.node.config.col_content) {
							selectItem.attrs.push({ name: 'content', value: encodeURI(ele.innerText) })
						}
						q.node.config.col_attrs.forEach(function (i) {
							var attrName = q.node.attributes[i].name
							selectItem.attrs.push({ name: attrName, value: ele.attributes[attrName] ? encodeURI(ele.attributes[attrName].value) : null })
						})
					}
					parentItems.push(selectItem)
					if (q.subs.length) {
						selectItem.subItems = []
						q.subs.forEach(function (subQuery) {
							func2(subQuery, selectItem.subItems, $(ele))
						})
					}
				})
			}
			var result = []; func2(query, result, null)

			return JSON.stringify({ query: query, data: result });
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
		//debugger;
		g_captureElement.push(e.target)
		context.jsInvoke('captureElements', _x_captureFinish(true))
		e.preventDefault(); e.stopPropagation(); e.stopImmediatePropagation(); return false
	}

	var g_captureElement = [];
	window._x_captureStart = function () {
		g_captureElement = []
		window.document.addEventListener("mouseover", highLightElement, true)
		window.document.addEventListener("click", captureElement, true)
	}

	window._x_captureFinish = function (continued = false) {
		//debugger;
		var resultNodes = []

		var eleToNode = function (ele) {
			var node = {
				tagName: ele.tagName,
				innerText: encodeURI(ele.innerText),
				classNames: ele.className.split(' '),
				attributes: [],
				children: [],
				config: { tag: true, col_content: true },
				_element: ele
			}
			if (ele.parentElement) {
				node.index = Array.from(ele.parentElement.children).indexOf(ele)
				node.isLast = node.index == (ele.parentElement.children.length - 1)
			}
			if (ele.attributes) for (var i = 0; i < ele.attributes.length; ++i) {
				node.attributes.push({ name: ele.attributes[i].name, value: encodeURI(ele.attributes[i].value) })
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
			lastChildNode && (resultNodes.push(lastChildNode))
		})

		if (!continued) { window._x_captureCancel() }

		// 根据 node 上的 _element, 生成 config, 干预 config 的前提时, 默认的 tag:first 选择
		//var analysisConfig = function (nodes, ancestorsNodes) {
		//	nodes.forEach(function (node) {
		//	})
		//}
		//analysisConfig(resultNodes, [])

		var gen = function (nodes, tree) {
			if (!nodes || nodes.length == 0) { return }
			if (nodes.length == 1) {
				tree.nodes.push(nodes[0])
				gen(nodes[0].children, tree)
			} else {
				nodes.forEach(function (node) {
					var sub = { nodes: [node], subs: [] }
					tree.subs.push(sub)
					gen(node.children, sub)
				})
			}
		}
		// 修改 config 使 在 ele 下, 使用nodes 构成的 JQuery 必须 唯一命中 nodes 中的 _element
		var iden = function (element, nodes) {
			var ele = $(element)
			var lastNode = nodes[nodes.length - 1]
			var targetElement = lastNode._element

			// 
			var strJQ = ''; nodes.forEach(function (node) { strJQ && (strJQ += '>'); strJQ += node.tagName })
			var result = ele.find(strJQ); if (result.length == 0) { throw 'error 1' }

			if (result.length == 1) {
				if (result[0] == targetElement) { return; }
				throw 'error 2'
			}

			// if (result[0] == targetElement) { lastNode.config.index = true }
		}
		
		var signIndex = function (nodes) {
			nodes && nodes.forEach(function (node) { node.config.index = true; signIndex(node.subs) })
		}

		var tree = { nodes: [], subs: [] }; gen(resultNodes, tree)
		var func = function (dataItemNode, subs) {
			subs.forEach(function (treeNode) {
				// 唯一化
				// iden(dataItemNode._element, treeNode.nodes)

				// 标记index
				// 说明: 分枝后, 每分枝必须唯一化一个字段值, 最基本的方法就是使用nth-child
				signIndex(treeNode.nodes)

				if (treeNode.subs.length > 0) {
					func(treeNode.nodes[treeNode.nodes.length-1], treeNode.subs)
				}
			})
			
		}
		var dataItemNode = tree.nodes[tree.nodes.length  - 1]
		
		func(dataItemNode, tree.subs);

		var deleteElementField = function (nodes) {
			nodes.forEach(function (node) {
				delete node._element
				deleteElementField(node.children)
			})
		}
		deleteElementField(resultNodes)
		// window.external.onCaptureElement(JSON.stringify(resultNodes))
		return JSON.stringify(resultNodes)
	}

	window._x_captureCancel = function () {
		g_captureElement = []
		if (g_eleLastHover) { g_eleLastHover.style.backgroundColor = g_eleLastBgColor }
		g_eleLastHover = null; g_eleLastBgColor = null
		window.document.removeEventListener("mouseover", highLightElement, true)
		window.document.removeEventListener("click", captureElement, true)
	}

})()


//var siblings = $(ele.parentElement).find(ele.tagName)
//node.index = siblings.index(ele)
//node.isLast = node.index == (siblings.length - 1)