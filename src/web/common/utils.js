import dateFormat from 'dateformat'

export default {
	toDateString: (str) => {
		return str && dateFormat(new Date(str), 'yyyy-mm-dd');
	},
	toDateTimeString: (str) => {
		return str && dateFormat(new Date(str), 'yyyy-mm-dd hh:MM');
	},
	injectCSS: (styleString, id = null, doc = null) => {
		doc = doc || document;
		let styleElement = doc.createElement('style');
		id && (styleElement.id = id);
		styleElement.type = 'text/css';
		styleElement.innerHTML = styleString;
		doc.getElementsByTagName('HEAD').item(0).append(styleElement)
	},
	setCookie: (name, value) => {
		// let Days = 30;
		// let exp = new Date();
		// exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
		// document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
		window.localStorage.setItem(name, value)
	},
	getCookie: (name) => {
		// let arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
		// if (arr = document.cookie.match(reg))
		// 	return unescape(arr[2]);
		// else
		// 	return null;
		return window.localStorage.getItem(name)
	},
	delCookie: (name) => {
		// let exp = new Date();
		// exp.setTime(exp.getTime() - 1);
		// let cval = getCookie(name);
		// if (cval != null)
		// 	document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
		window.localStorage.removeItem(name)
	},
	xssFilter: (content) => {
		const xss = require('xss');
		return xss(content, {
			whiteList: {
				a: ["href", "title", "target"],
				span: ['style'], br: [], ul: [], ol: [], li: ['style'], blockquote: [], img: ['src', 'style'],
				p: ['style'], h1: [], h2: [], h3: [], h4: [], h5: [], h6: [], h7: [],
				table: ['border', 'width', 'cellpadding', 'cellspacing'], thead: [], tbody: [], tr: [], th: [], td: [],
				iframe: ['src', 'height', 'width', 'frameborder', 'allowfullscreen']
			}
		})
	}
}