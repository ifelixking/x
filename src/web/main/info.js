import React from 'react'
import { combineReducers } from 'redux'
import { connect } from 'react-redux'
import API from '../common/api'
import utils from '../common/utils'
import Page from '../common/Page'

class InfoPage extends React.Component {
	constructor(props) {
		super(props)
		this.ref_this = React.createRef();
		this.onPageTo = this.onPageTo.bind(this)
	}

	componentWillMount() {
		this.props.items || this.props.fetchList(0)
		this.props.totalPageCount || this.props.getPageCount()
	}

	componentDidMount() {
		utils.injectCSS(`
			.infopage_table {
				width: 100%;
				font-size: 14px;
				border-collapse:collapse;
				margin-top:16px;
			}
			.infopage_table thead tr tH {
				padding:4px;

			}
			.infopage_table tbody tr td {
				text-align:center;
				padding:4px;
			}
			.infopage_table tbody tr td:nth-child(2) {
				text-align:left
			}
		`, 'STYLE_INFOPAGE', this.ref_this.current.ownerDocument)
	}

	componentWillUnmount() {
		this.ref_this.current.ownerDocument.getElementById('STYLE_INFOPAGE').remove()
	}

	onPageTo(page) {
		this.props.fetchList(page)
	}

	render() {
		let items = this.props.items && this.props.items.map((a, i) => {
			return (
				<tr key={a.id}><td>{i + 1}</td><td><a href={`/info/${a.id}`} target='_blank'>{a.title}</a></td><td>{utils.toDateTimeString(a.time)}</td><td>{utils.toDateTimeString(a.lastReplyTime)}</td></tr>
			)
		})
		const css_pub = { margin: '16px 0px', float: 'right', width: '100px', height: '32px', lineHeight: '32px', border: '1px solid red', textAlign: 'center' }
		return (
			<div ref={this.ref_this} style={{ padding: '0px 80px' }}>
				<div>
					<div style={{ margin: '16px 0px', float: 'left' }}><Page current={this.props.currentPage} count={this.props.totalPageCount} onPageTo={this.onPageTo} /></div>
					<a style={css_pub} href='/infopub'>匿名发布</a>
				</div>
				<table border={1} bordercolor={'#a0c6e5'} className={'infopage_table'}>
					<thead><tr><th width={'20px'}>&nbsp;</th><th>标题</th><th width={'150px'}>时间</th><th width={'150px'}>回复</th></tr></thead>
					<tbody>{items}</tbody>
				</table>
				<div>
					<div style={{ margin: '16px 0px', float: 'left' }}><Page current={this.props.currentPage} count={this.props.totalPageCount} onPageTo={this.onPageTo} /></div>
					<a style={css_pub} href='/infopub' target='_blank'>匿名发布</a>
				</div>
			</div>
		)
	}
}

const Module = {
	Component: connect(
		state => ({
			items: state.info.postList && state.info.postList.items,
			currentPage: state.info.postList && state.info.postList.page,
			totalPageCount: state.info.postListPageCount
		}),
		dispatch => ({
			fetchList: (page) => { dispatch(Module.Actions.fetchList(page)) },
			getPageCount: () => { dispatch(Module.Actions.fetchListPageCount()) }
		})
	)(InfoPage),
	Actions: {
		fetchList(page) {
			return dispatch => {
				API.getPostList(page).then((res) => {
					res.json().then((data) => {
						dispatch({ type: 'FLUSH_POST_LIST', postList: data })
					})
				})
			}
		},
		fetchListPageCount() {
			return dispatch => {
				API.getPostListPageCount().then((res) => {
					res.json().then((data) => {
						dispatch({ type: 'FLUSH_POST_LIST_PAGE_COUNT', pageCount: data.pageCount })
					})
				})
			}
		}
	},
	Reducers: combineReducers({
		postList: (postList = null, action) => { return action.type == 'FLUSH_POST_LIST' ? action.postList : postList },
		postListPageCount: (postListPageCount = 0, action) => { return action.type == 'FLUSH_POST_LIST_PAGE_COUNT' ? action.pageCount : postListPageCount }
	})
}

export default Module