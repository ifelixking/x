import React from 'react'
import API from '../common/api'
import Editor from 'wangeditor'
import emotions from '../common/emotions'

export default class InfoPub extends React.Component {
	constructor(props) {
		super(props)
		this.publish = this.publish.bind(this)
		this.ref_txtTitle = React.createRef();
		this.ref_editor = React.createRef();
		this.content = ''
	}

	componentDidMount(){
		const editor = new Editor(this.ref_editor.current)
		editor.customConfig.onchange = html => { this.content = html }
		editor.customConfig.emotions = emotions
		editor.create()
	}

	publish() {
		let title = this.ref_txtTitle.current.value
		const xss = require('xss');
		const content = xss(this.content)
		API.createPost(title, content).then((res) => {
			res.json().then((data) => {
				window.opener && window.opener.location.reload()
				window.close();
			})
		})
	}

	render() {
		return (
			<div style={{ padding: '0px 80px' }}>
				<div>匿名发布</div>
				<div>
					<input ref={this.ref_txtTitle} placeholder='标题' type='text' />
				</div>
				<div ref={this.ref_editor}></div>
				<div>
					<a href={'javascript:;'} onClick={this.publish}>发布</a>
				</div>
			</div>
		)
	}
}