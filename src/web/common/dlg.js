import React from 'react'
import IconClose from '../css/close.svg'

class Dialog extends React.Component {
	constructor(props) {
		super(props)
	}
	render() {
		const css_frame = { width: '100%', height: '100%', position: 'fixed', left: '0px', top: '0px', padding: '32px', boxSizing: 'border-box', 
			
		}
		const css_div = {
			width: '100%', height: '100%', backgroundColor: '#000', boxSizing: 'border-box',
			border: '4px solid #F50', padding: '100px 32px 32px 32px',
			filter: 'alpha(Opacity=80)', '-moz-opacity': '0.80', opacity: '0.80'
		}
		const css_btn = { width: '32px', height: '32px' }
		const css_div_head = { padding: '16px', textAlign: 'right', position:'fixed', right:'48px', top:'48px' }
		const css_div_content = { color: '#fff', padding: '32px', width: '100%', height: '100%', boxSizing: 'border-box', overflow:'auto', border:'2px solid #777' }
		return (
			<div style={css_frame}>
				<div style={css_div}>
					<div style={css_div_head}><IconClose style={css_btn} /></div>
					<div style={css_div_content}>{this.props.children}</div>
				</div>
			</div>
		)
	}
}

export default Dialog