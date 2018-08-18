import React from 'react'
import { combineReducers } from 'redux'
import { connect } from 'react-redux'
import API from '../common/api'
import utils from '../common/utils'
import Page from '../common/Page'
import Styles from '../res/style.css'
import Location from '../common/Location'

class InfoPage extends React.Component {
	constructor(props) {
		super(props)
		this.ref_this = React.createRef();
		this.onPageTo = this.onPageTo.bind(this)
		this.onAreaChange = this.onAreaChange.bind(this)
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
		this.props.fetchList(page, this.areaID)
	}

	onAreaChange(areaID) {
		this.areaID = areaID
		this.props.fetchList(0, areaID)
		this.props.getPageCount(areaID)
	}

	render() {
		let content
		if (this.props.items && this.props.items.length) {
			let items = this.props.items.map((a, i) => {
				return (
					<tr key={a.id}><td>
						<a href={`/info/${a.id}`} target='_blank'>{a.title}</a>
					</td><td>{utils.toDateTimeString(a.time)}</td><td>{utils.toDateTimeString(a.lastReplyTime)}</td></tr>
				)
			})
			content = (
				<div>
					<table className={'infopage_table'}>
						<thead><tr><th>标题</th><th width={'120px'}>时间</th><th width={'120px'}>回复</th></tr></thead>
						<tbody>{items}</tbody>
					</table>
					<div style={{ margin: '16px 0px' }}><Page current={this.props.currentPage} count={this.props.totalPageCount} onPageTo={this.onPageTo} /></div>
				</div>
			)
		} else {
			content = <p style={{ textAlign: 'center',color:'#333', margin:'48px' }}>该地区还没有信息, 欢迎&nbsp;<a href="/infopub" target='_blank'>发布</a></p>
		}


		return (
			<div ref={this.ref_this} style={{ padding: '0px 80px' }}>
				<div style={{ position:'relative', padding: '24px 0px 8px 0px' }}>
					<Location onAreaChange={this.onAreaChange} />
					<a style={{position:'absolute', top:'18px', right:'0px'}} className={[Styles.btn, Styles.btn_primary].join(' ')} target="_blank" href='/infopub'>发布信息</a>
				</div>

				{content}
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
			fetchList: (page, areaID) => { dispatch(Module.Actions.fetchList(page, areaID)) },
			getPageCount: (areaID) => { dispatch(Module.Actions.fetchListPageCount(areaID)) }
		})
	)(InfoPage),
	Actions: {
		fetchList(page, areaID) {
			return dispatch => {
				API.getPostList(page, areaID).then((res) => {
					res.json().then((data) => {
						dispatch({ type: 'FLUSH_POST_LIST', postList: data })
					})
				})
			}
		},
		fetchListPageCount(areaID) {
			return dispatch => {
				API.getPostListPageCount(areaID).then((res) => {
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