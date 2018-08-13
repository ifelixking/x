import React from 'react'
import { combineReducers } from 'redux'
import { connect } from 'react-redux'
import API from '../common/api'
import utils from '../common/utils'
import Page from '../common/Page'
import Styles from '../res/style.css'
import IconLocation from '../res/location.svg'

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
			.infopage_table thead tr {
				border-bottom: 1px solid #e8e8e8;
			}
			.infopage_table thead tr th {
				text-align:left;
				padding:16px;
				background-color: rgb(250, 250, 250);
			}
			.infopage_table tbody tr td {
				padding:16px;
			}
			.infopage_table tbody tr td a {
				text-decoration:none;
				color: #1890ff;
			}
			.infopage_table tbody tr {
				border-bottom: 1px solid #e8e8e8;
				-webkit-transition: all .3s;
				transition: all .3s;
			}
			.infopage_table tbody tr:hover {
				background: #e6f7ff;
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
				<tr key={a.id}><td>
					<a href={`/info/${a.id}`} target='_blank'>{a.title}</a>
				</td><td>{utils.toDateTimeString(a.time)}</td><td>{utils.toDateTimeString(a.lastReplyTime)}</td></tr>
			)
		})
		const css_area = { fontSize: '14px', textDecoration: 'none' }
		return (
			<div ref={this.ref_this} style={{ padding: '0px 80px' }}>
				<div style={{ padding: '24px 0px 8px 0px' }}>
					<a style={css_area} href='/infopub'><IconLocation style={{ verticalAlign: 'middle' }} />北京</a>
					<a className={[Styles.btn, Styles.btn_primary].join(' ')} style={{ float: 'right' }} target="_blank" href='/infopub'>匿名发布</a>
				</div>
				<table className={'infopage_table'}>
					<thead><tr><th>标题</th><th width={'150px'}>时间</th><th width={'150px'}>回复</th></tr></thead>
					<tbody>{items}</tbody>
				</table>
				<div style={{ margin: '16px 0px' }}><Page current={this.props.currentPage} count={this.props.totalPageCount} onPageTo={this.onPageTo} /></div>
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