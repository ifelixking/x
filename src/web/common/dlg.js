import React from 'react'
import IconClose from '../css/close.svg'
import Styles from '../style.css'

class Dialog extends React.Component {
	constructor(props) {
		super(props)
	}

	componentDidMount() {
		window.addEventListener('keydown', this.props.onBtnCloseClick)
	}

	componentWillUnmount() {
		window.removeEventListener('keydown', this.props.onBtnCloseClick)
	}

	render() {
		const css_frame = { width: '100%', height: '100%', position: 'fixed', left: '0px', top: '0px', padding: '50px 100px', boxSizing: 'border-box' }
		const css_bg = { width: '100%', height: '100%', position: 'fixed', left: '0px', top: '0px', backgroundColor: '#777', filter: 'alpha(Opacity=80)', '-moz-opacity': '0.80', opacity: '0.80', zIndex: '-1' }
		let css_div = { backgroundColor: '#333', border: '3px solid #F50', boxSizing: 'border-box', padding: '100px 32px 32px 32px' };
		let css_caption = { position: 'fixed', fontSize: '32px', textAlign: 'center', color: '#fff', left: '0px', width: '100%' }
		let css_btnClose = { position: 'fixed' }
		if (this.props.size == 'small') {
			Object.assign(css_div, { width: '640px', height: '320px', position: 'fixed', left: '50%', top: '50%', marginLeft: '-320px', marginTop: '-160px' })
			Object.assign(css_caption, { top: '50%', marginTop: '-128px' })
			Object.assign(css_btnClose, { top: '50%', left: '50%', marginLeft: '240px', marginTop: '-140px' })
		} else {
			Object.assign(css_div, { width: '100%', height: '100%' })
			Object.assign(css_caption, { top: '80px' })
			Object.assign(css_btnClose, { right: '120px', top: '70px' })
		}

		const css_div_content = { color: '#fff', padding: '0px', width: '100%', height: '100%', boxSizing: 'border-box', overflow: 'auto', border: '2px solid #777' }
		return (
			<div style={css_frame}>
				<div style={css_bg} />
				<div style={css_div}>
					<div style={css_caption}>{this.props.caption}</div>
					<div title={"按[ESC]关闭"} style={css_btnClose} className={Styles.btn_close} onClick={this.props.onBtnCloseClick}><IconClose style={{ width: '32px', height: '32px' }} /></div>
					<div style={css_div_content}>{this.props.children}</div>
				</div>
			</div>
		)
	}
}

export default Dialog


// ,