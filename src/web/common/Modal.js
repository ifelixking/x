import React from 'react'
import ReactDOM from 'react-dom'
import PropTypes from 'prop-types';
import IconClose from '../res/close2.svg'
import Styles from '../res/style.css'

export default class Modal extends React.Component {
	constructor(props) {
		super(props)
		this.refMain = React.createRef()
		this.onKeydownForESC = this.onKeydownForESC.bind(this)
	}

	onKeydownForESC(e) {
		if (e.keyCode == 27) {
			this.props.onCancel()
		}
	}

	componentDidMount() {
		let doc = document;
		this.popup = doc.createElement('div')
		doc.body.appendChild(this.popup)
	}

	componentDidUpdate() {
		ReactDOM.render(this._render(), this.popup)
	}

	componentWillUnmount() {
		ReactDOM.unmountComponentAtNode(this.popup)
		this.popup.remove();
	}

	_render() {
		let css_bg = { position: 'absolute', top: '0px', left: '0px', width: '100%', height: '100%', transition: 'background-color 0.3s', textAlign: 'center' }
		let ele
		if (this.props.visible) {
			css_bg.backgroundColor = 'rgb(0,0,0,0.5)'; css_bg.zIndex = 999999			
			const width = this.props.fullSize ? 'calc(100% - 200px)' : '800px'
			const height = this.props.fullSize ? 'calc(100% - 200px)' : '600px'
			const css_frame = { position: 'relative', boxSize:"border-box", marginTop: '100px', width, height, backgroundColor: '#fff', borderRadius: '4px', display: 'inline-block' }
			const css_content = { position: 'absolute', top: '0px', left: '0px', height: '100%', width: '100%', boxSizing: 'border-box', padding: `58px 0px ${this.props.showButtons ? '64px' : '4px'} 0px` }
			ele = (
				<div style={css_frame}>
					<div style={{ position: 'absolute', top: '0px', right: '0px', padding: '16px', cursor: 'pointer', zIndex: '1000000' }} onClick={this.props.onCancel} ><IconClose style={{ fill: '#aaa' }} /></div>
					<div style={{ height: '25px', padding: '16px', textAlign: 'left', borderBottom: '1px solid rgb(232, 232, 232)', fontSize: '16px', fontWeight: 'bold' }} >{this.props.title}</div>
					<div style={css_content}>{this.props.children}</div>
					{this.props.showButtons &&
						<div style={{ position: 'absolute', padding: '16px', textAlign: 'right', boxSizing: 'border-box', bottom: '0px', borderTop: '1px solid rgb(232, 232, 232)', width: '100%' }}>
							<button className={Styles.btn} onClick={this.props.onCancel}>取消</button>
							<button style={{ marginLeft: '8px' }} className={[Styles.btn, Styles.btn_primary].join(' ')} onClick={this.onOK}>确定</button>
						</div>}
				</div>
			)
			window.addEventListener('keydown', this.onKeydownForESC)
		} else {
			window.removeEventListener('keydown', this.onKeydownForESC)
			css_bg.backgroundColor = 'rgb(0,0,0,0.0)';
			setTimeout(() => { this.refMain.current && (this.refMain.current.style.zIndex = -1) }, 301)
			ele = null;
		}
		return (
			<div ref={this.refMain} style={css_bg}>{ele}</div>
		)
	}

	render() { return null; }
}

Modal.propTypes = {
	title: PropTypes.node.isRequired,
	visible: PropTypes.bool.isRequired,
	onCancel: PropTypes.func.isRequired,
	onOK: PropTypes.func,
	showButtons: PropTypes.bool,
	fullSize: PropTypes.bool,
}