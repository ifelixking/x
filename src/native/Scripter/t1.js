//(function () { var _x_script = document.createElement('script'); _x_script.src = 'https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script); })()

//JSON.stringify($('#moderate table tbody tr td:nth-last-child(2).max-td table.article tbody tr.article-title td a[target]').map(function (i, a) {
//	return { text: a.innerText, url: a.href, date: new Date((new Date()).getFullYear() + '-' + $(a).children('span').text()) }
//}).toArray())


//JSON.stringify($('td.t_f').map(function (i, a) {
//	return {
//		text: a.innerText,
//		downloads: JSON.stringify($(a).children('a').filter(function () { return $(this).find('img').length == 0 }).map(function (ii, aa) { return aa.href }).toArray()),
//		images: JSON.stringify($(a).find('img').map(function (ii, aa) { return aa.src }).toArray())
//	}
//}).toArray())

// JSON.stringify($('#fd_page_bottom div.pg a.nxt').length ? $('#fd_page_bottom div.pg a.nxt')[0].href : '')


//JSON.stringify($('table#threadlisttableid tbody tr')
//	.filter(function () {
//		return $(this).children('th').children('a.s.xst').length > 0
//			&& $(this).children('td.by').length > 0
//			&& $(this).children('th').children('a.showhide').length == 0
//	})
//	.map(function (i, a) {
//		var item = $(a).children('th').children('a.s.xst')[0]
//		return item && { text: item.innerText, url: item.href, date: $('td.by:first em span span', a).attr('title') }
//	}
//	).toArray())

//JSON.stringify(
//	{
//		text: $('td.t_f')[0].innerText,
//		downloads: $('dl.tattl dd p.attnm a').map(function (i, a) { return a.href }).toArray(),
//		images: JSON.stringify($('td.t_f img').map(function (ii, aa) { return aa.src }).toArray())
//	}
//)

//JSON.stringify(
//	$('a[onclick="hideWindow(\'imc_attachad\')"]')[0].href
//)