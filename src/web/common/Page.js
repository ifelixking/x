import React from 'react'

class Page extends React.Component {
	constructor(props) {
		super(props)
		this.onPageClick = this.onPageClick.bind(this)
		this.onPagePrevClick = this.onPagePrevClick.bind(this)
		this.onPageNextClick = this.onPageNextClick.bind(this)
	}

	onPageClick(e){
		let page = e.target.attributes['data-page'].value
		this.props.onPageTo(page)
	}

	onPagePrevClick(){
		let page = parseInt(this.props.current)
		if (page <= 0) { return; }
		this.props.onPageTo(page - 1);
	}

	onPageNextClick(){
		let page = parseInt(this.props.current)
		if (page >= this.props.count - 1) { return; }
		this.props.onPageTo(page + 1);
	}

	render() {
		const headPageCount = 2
		const maxPageCount = 5
		let items = [];
		const css_href = {
			border: '1px solid #ddd',
			backgroundColor: '#f5f5f5', marginRight: '5px', padding: '0px 14px', height: '36px', lineHeight: '36px',
			display: 'block', float: 'left', textAlign: 'center', fontSize: '14px', textDecorationLine: 'none', color: '#333'
		}
		const css_current = { borderColor: '#fff', backgroundColor: '#fff' }
		let current = parseInt(this.props.current); if (current >= this.props.count) { current = this.props.count - 1; } if (current <= 0) { current = 0 }

		for (let i = 0; i < this.props.count; ++i) {
			let item;
			if (i < headPageCount) {
				item = <a onClick={this.onPageClick} data-page={i} style={css_href} key={i} href='javascript:;'>{i + 1}</a>
			} else {
				if (i == headPageCount) {
					if (current - (maxPageCount >> 1) > headPageCount) {						
						items.push(<p style={Object.assign({}, css_href, css_current)} key={'-...'}>...</p>);	// 加首省略号
						i = current  - (maxPageCount >> 1); if (i<headPageCount){i=headPageCount} if (i >= this.props.count) { break; }	// 调整 i
					}
				} else if (i >= Math.max(current + 1 + (maxPageCount >> 1), headPageCount + maxPageCount)) {
					items.push(<p style={Object.assign({}, css_href, css_current)} key={'+...'}>...</p>); // 加尾省略号
					items.push(<a onClick={this.onPageClick} data-page={this.props.count - 1} style={css_href} key={this.props.count - 1} href='javascript:;'>{this.props.count}</a>);
					break;
				}
				item = <a onClick={this.onPageClick} data-page={i} style={css_href} key={i} href='javascript:;'>{i + 1}</a>
			}
			// 修正当前页
			if (current == i) {
				item = <p style={Object.assign({}, css_href, css_current)} key={i}>{i + 1}</p>
			}
			items.push(item);
		}

		return (
			<div style={{ display: 'inline-block' }}>
				<span>
					<a onClick={this.onPagePrevClick} style={css_href} key='-1' href="javascript:;">上一页</a>
					{items}
					<a onClick={this.onPageNextClick} style={css_href} key='+1' href="javascript:;">下一页</a>
				</span>
			</div>
		)
	}
}

export default Page