import React from 'react'
import API from '../common/api'
import Editor from 'wangeditor'
import emotions from '../common/emotions'
import Styles from '../res/style.css'
import Location from '../common/Location'

export default class InfoPub extends React.Component {
	constructor(props) {
		super(props)
		this.publish = this.publish.bind(this)
		this.ref_txtTitle = React.createRef();
		this.ref_editor = React.createRef();
		this.content = ''
	}

	componentDidMount() {
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
			<div className={Styles.form} style={{ padding: '24px 80px' }}>
				<div>
					<Location />
					<span style={{fontSize:'14px'}}>&nbsp;>&nbsp;发布信息</span>
				</div>
				<div>
					<input className={Styles.input} ref={this.ref_txtTitle} placeholder='标题' type='text' />
				</div>
				<div>
					<div ref={this.ref_editor}></div>
				</div>
				<div>
					<a className={[Styles.btn, Styles.btn_primary].join(' ')} href={'javascript:;'} onClick={this.publish}>发布</a>
				</div>
			</div>
		)
	}
}