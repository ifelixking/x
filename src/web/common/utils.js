import dateFormat from 'dateformat'

export default {
	toDateString: (str) => {
		return str && dateFormat(new Date(str), 'yyyy-mm-dd');
	},
	toDateTimeString: (str) => {
		return str && dateFormat(new Date(str), 'yyyy-mm-dd hh:MM:ss');
	},
	injectCSS: (styleString, id = null, doc = null) => {
		doc = doc || document;
		let styleElement = doc.createElement('style');
		id && (styleElement.id = id);
		styleElement.type = 'text/css';
		styleElement.innerHTML = styleString;
		doc.getElementsByTagName('HEAD').item(0).append(styleElement)
	}
}