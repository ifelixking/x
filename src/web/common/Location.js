import React from 'react'
import PropTypes from 'prop-types';
import IconLocation from '../res/location.svg'
import Modal from '../common/Modal'
import provinces from '../common/provinces'
import utils from '../common/utils'

export default class Location extends React.Component {
	constructor(props) {
		super(props)
		this.onClick = this.onClick.bind(this)
		this.refMain = React.createRef()
		this.onDlgAreaClick = this.onDlgAreaClick.bind(this)
		this.state = {
			showDialog: false,
			current: '',
		}
	}

	componentWillMount() {
		let area = utils.getCookie("area")
		let areaID = utils.getCookie("areaID")
		if (!area) {
			area = '北京'; areaID = 0
			utils.setCookie('area', area)
			utils.setCookie('areaID', areaID)
		}
		this.setState({ current: area })
		this.props.onAreaChange && this.props.onAreaChange(areaID)
	}

	componentDidMount() {
		utils.injectCSS(`
			._CSS_LOCATION_TAG {
				background-color:#eee;
				color: #333;
			}
			._CSS_LOCATION_TAG:hover {
				background-color:rgb(135, 208, 104);
				color:#FFF;
			}
			._CSS_LOCATION_TAG[active] {
				background-color:rgb(135, 208, 104);
				color:#FFF;
			}
		`, '_CSS_LOCATION', this.refMain.current.ownerDocument)
	}

	componentWillUnmount() {
		this.refMain.current && this.refMain.current.ownerDocument.getElementById('_CSS_LOCATION').remove()
	}

	onClick() {
		this.setState({
			showDialog: true
		})
	}

	onDlgAreaClick(e, areaID) {
		const area = e.target.innerText
		this.setState({ current: area, showDialog: false })
		utils.setCookie('area', area)
		utils.setCookie('areaID', areaID)
		this.props.onAreaChange && this.props.onAreaChange(areaID)
	}

	render() {
		const css_area = { fontSize: '14px', textDecoration: 'none' }
		const items = provinces.map((a, i) => {
			return (
				<div key={i} style={{ textAlign: 'left', marginBottom: '24px' }}>
					{a.n && (<span style={{ display: 'inline-block', margin: '2px 0px', fontSize: '14px', color: '#777' }}>{a.n}</span>)}
					{a.n && (<hr size={'1'} color={'#dedede'} />)}
					{a.c.map((b, ii) => <span key={ii} active={(b == this.state.current) ? 'active' : undefined} className={'_CSS_LOCATION_TAG'} style={{ display: 'inline-block', padding: '4px 8px', margin: '4px', fontSize: '14px', borderRadius: '4px', cursor: 'pointer' }} onClick={(e) => this.onDlgAreaClick(e, i * 1000 + ii)}>{b}</span>)}
				</div>
			)
		})
		return (
			<div style={{display:'inline-block'}} ref={this.refMain}>
				<a style={css_area} href={'javascript:;'} onClick={this.onClick}><IconLocation style={{ verticalAlign: 'middle' }} />{this.state.current}</a>
				<Modal showButtons={false} title={(<span>选择地区&nbsp;&nbsp;<IconLocation style={{ verticalAlign: 'middle' }} /><font style={{ fontSize: '14px', fontWeight: 'normal' }}>{this.state.current}</font></span>)} visible={this.state.showDialog} onCancel={() => { this.setState({ showDialog: false }) }} >
					<div style={{ height: '100%', overflow: 'auto', boxSizing: 'border-box', padding: '16px' }}>{items}</div>
				</Modal>
			</div>
		)
	}
}

Location.propTypes = {
	onAreaChange: PropTypes.func
}