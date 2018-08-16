import React from 'react'
import API from '../common/api'
import utils from '../common/utils'
import Page from '../common/Page'
import Editor from 'wangeditor'
import emotions from '../common/emotions'
import Styles from '../res/style.css'

export default class InfoDetail extends React.Component {
	constructor(props) {
		super(props)
		this.onPage = this.onPage.bind(this)
		this.onReply = this.onReply.bind(this)
		this.ref_editor = React.createRef()
		this.state = {
			items: [],
			currentPage: null,
			pageCount: 0,
			title: '',
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

	componentDidMount() {
		let editor = new Editor(this.ref_editor.current)
		editor.customConfig.onchange = html => { this.reply_content = html }
		editor.customConfig.emotions = emotions
		editor.create()
	}

	onPage(page) {
		const postID = this.props.match.params.id
		API.getPost(postID, page).then(res => {
			res.json().then(data => {
				let newState = { currentPage: data.page, items: data.items }
				if (data.page == 0 && data.items && data.items.length) {
					newState.title = data.items[0].title
				}
				this.setState(newState)
			})
		})
	}

	onReply() {
		const xss = require('xss');
		const content = xss(this.reply_content)
		const hostID = this.props.match.params.id
		const parentID = hostID
		API.reply(hostID, parentID, content).then(res => {
			res.json().then(data => {
				window.location.reload()
			})
		})
	}

	render() {
		const css_item = { borderTop: '1px solid #f2f4f5', margin: '16px 0px', padding: '4px 2px' }
		let items = this.state.items.map((a, i) => {
			let the_css_item = css_item
			if (this.state.currentPage == 0 && i == 0) {
				the_css_item = Object.assign({}, css_item, { borderTop: '1px solid transparence', })
			}
			return (
				<div style={the_css_item} key={a.id}>
					<div style={{ textAlign: 'right' }}><span class={Styles.time}>{utils.toDateTimeString(a.time)}</span></div>
					<div dangerouslySetInnerHTML={{ __html: a.content }} />
				</div>
			)
		})
		const css_title = { width: '100%', textOverflow: 'ellipsis', whiteSpace: 'nowrap', overflow: 'hidden', fontSize: '24px', fontWeight: 'bold', margin: '24px 0px' }
		// let post = ()
		// post = post && (<div>{post}</div>)
		return (
			<div style={{ padding: '0px 80px' }}>
				<div className={Styles.form}>
					<div style={css_title}>{this.state.title}</div>
					<div>{items}</div>
					<div style={{ marginTop: '32px' }}><Page current={this.state.currentPage} count={this.state.pageCount} onPageTo={this.onPageTo} /></div>
					<div><div className={Styles.editor} ref={this.ref_editor} /></div>
					<div><a className={[Styles.btn, Styles.btn_primary].join(' ')} href={'javascript:;'} onClick={this.onReply}>回复</a></div>
				</div>
			</div>
		)
	}
}