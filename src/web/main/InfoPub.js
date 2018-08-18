import React from 'react'
import API from '../common/api'
import Editor from 'wangeditor'
import emotions from '../common/emotions'
import Styles from '../res/style.css'
import Location from '../common/Location'
import utils from '../common/utils'

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
		editor.customConfig.zIndex = 0
		editor.customConfig.menus = [
			'head',  // 标题
			'bold',  // 粗体
			'fontSize',  // 字号
			'fontName',  // 字体
			'italic',  // 斜体
			'underline',  // 下划线
			'strikeThrough',  // 删除线
			'foreColor',  // 文字颜色
			'backColor',  // 背景颜色
			'link',  // 插入链接
			'list',  // 列表
			'justify',  // 对齐方式
			'quote',  // 引用
			'emoticon',  // 表情
			'image',  // 插入图片
			'table',  // 表格
			'video',  // 插入视频
			'undo',  // 撤销
			'redo'  // 重复
		]
		editor.create()
	}

	publish() {
		let title = this.ref_txtTitle.current.value
		const content = utils.xssFilter(this.content)
		API.createPost(title, content, null, this.areaID).then((res) => {
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
					<Location onAreaChange={(areaID) => { this.areaID = areaID }} />
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