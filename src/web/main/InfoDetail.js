import React from 'react'
import API from '../common/api'
import utils from '../common/utils'
import Page from '../common/Page'
import Editor from 'wangeditor'
import emotions from '../common/emotions'

export default class InfoDetail extends React.Component {
	constructor(props) {
		super(props)
		this.onPage = this.onPage.bind(this)
		this.onReply = this.onReply.bind(this)
		this.ref_editor = React.createRef()
		this.state = {
			items: [],
			currentPage: null,
			pageCount: 0
		}
	}

	componentWillMount() {
		const postID = this.props.match.params.id
		API.getPostDetailPageCount(postID).then(res => {
			res.json().then(data => {
				this.setState({ pageCount: data.pageCount })
				this.onPage(0)
			})
		})
	}

	componentDidMount(){
		let editor = new Editor(this.ref_editor.current)
		editor.customConfig.onchange = html => { this.reply_content = html }
		editor.customConfig.emotions = emotions
		editor.create()
	}

	onPage(page) {
		const postID = this.props.match.params.id
		API.getPost(postID, page).then(res => {
			res.json().then(data => {
				this.setState({ currentPage: data.page, items: data.items })
			})
		})
	}

	onReply(){
		const xss = require('xss');
		const content = xss(this.reply_content)
		const hostID = this.props.match.params.id
		const parentID = hostID
		API.reply(hostID, parentID, content).then(res=>{
			res.json().then(data=>{
				window.location.reload()
			})
		})
	}

	render() {
		let items = this.state.items.map((a) => {			
			return (
				<div key={a.id}>
					{utils.toDateTimeString(a.time)}<br />
					<div dangerouslySetInnerHTML={{ __html: a.content }} />
				</div>
			)
		})
		const css_pub = { margin: '16px 0px', float: 'right', width: '100px', height: '32px', lineHeight: '32px', border: '1px solid red', textAlign: 'center' }
		return (
			<div style={{ padding: '0px 80px' }}>
				<div style={{ margin: '16px 0px' }}><Page current={this.state.currentPage} count={this.state.pageCount} onPageTo={this.onPageTo} /></div>
				<div>{items}</div>
				<div style={{ margin: '16px 0px' }}><Page current={this.state.currentPage} count={this.state.pageCount} onPageTo={this.onPageTo} /></div>
				<div>
					<div ref={this.ref_editor} />
					<a href={'javascript:;'} onClick={this.onReply}>回复</a>
				</div>
			</div>
		)
	}
}