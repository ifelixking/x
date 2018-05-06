import React from 'react'

class Page extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		const headPageCount = 2
		const maxPageCount = 5
		let items = [];
		const css_href = {
			border: '1px solid #ccc',
			backgroundColor: '#eee', marginRight: '5px', padding: '0px 14px', height: '36px', lineHeight: '36px',
			display: 'block', float: 'left', textAlign: 'center', fontSize: '14px', textDecorationLine: 'none', color: '#000'
		}
		const css_current = { borderColor: '#fff', backgroundColor: '#fff' }

		for (let i = 0; i < this.props.count; ++i) {
			let item;
			if (i < headPageCount) {
				item = <a style={css_href} key={i} href='javascript:;'>{i + 1}</a>
			} else {
				if (i == headPageCount) {
					if (this.props.current - 1 - (maxPageCount >> 1) > headPageCount) {						
						items.push(<p style={Object.assign({}, css_href, css_current)} key={'-...'}>...</p>);	// 加首省略号
						i = this.props.current - 1 - (maxPageCount >> 1); if (i >= this.props.count) { break; }	// 调整 i
					}
				} else if (i + 1 > Math.max(parseInt(this.props.current) + (maxPageCount >> 1), headPageCount + maxPageCount)) {
					items.push(<p style={Object.assign({}, css_href, css_current)} key={'+...'}>...</p>); // 加尾省略号
					break;
				}
				item = <a style={css_href} key={i} href='javascript:;'>{i + 1}</a>
			}
			// 修正当前页
			if (this.props.current == (i + 1)) {
				item = <p style={Object.assign({}, css_href, css_current)} key={i}>{i + 1}</p>
			}
			items.push(item);
		}

		return (
			<div style={{ display: 'inline-block' }}>
				<span>
					<a style={css_href} key='-1' href="javascript:;">上一页</a>
					{items}
					<a style={css_href} key='+1' href="javascript:;">下一页</a>
				</span>
			</div>
		)
	}
}

export default Page