import React from 'react'
import API from '../common/api'

export default class InfoPub extends React.Component {
	constructor(props) {
		super(props)
		this.publish = this.publish.bind(this)
		this.ref_txtTitle = React.createRef();
		this.ref_txtContent = React.createRef();
	}

	publish() {
		let title = this.ref_txtTitle.current.value
		let content = this.ref_txtContent.current.value
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
				<div>
					<textarea ref={this.ref_txtContent} placeholder='详情' />
				</div>
				<div>
					<a href={'javascript:;'} onClick={this.publish}>发布</a>
				</div>
			</div>
		)
	}
}