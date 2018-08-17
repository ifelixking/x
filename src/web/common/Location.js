import React from 'react'
import IconLocation from '../res/location.svg'
import Modal from '../common/Modal'
import provinces from '../common/provinces'

export default class Location extends React.Component {
	constructor(props) {
		super(props)
		this.onClick = this.onClick.bind(this)
		this.onDlgOK = this.onDlgOK.bind(this)
		this.state = {
			showDialog: true
		}
	}

	onClick() {
		this.setState({
			showDialog: true
		})
	}

	onDlgOK() {
		this.setState({ showDialog: fales })
	}

	render() {
		const css_area = { fontSize: '14px', textDecoration: 'none' }
		const items = provinces.map(a => {
			
		})
		return (
			<div>
				<a style={css_area} href={'javascript:;'} onClick={this.onClick}><IconLocation style={{ verticalAlign: 'middle' }} />北京</a>
				<Modal visible={this.state.showDialog} onCancel={() => { this.setState({ showDialog: false }) }} onOK={this.onDlgOK}>
					<div style={{ height: '100%', overflow: 'auto', boxSizing: 'border-box', padding: '16px' }}>{items}</div>
				</Modal>
			</div>
		)
	}
}