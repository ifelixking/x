import React from 'react'
import utils from '../common/utils'

export default class Menu extends React.Component {
	constructor(props) {
		super(props)
		this.refMain = React.createRef();
	}

	componentDidMount() {
		let doc = this.refMain.current.ownerDocument
		utils.injectCSS(`
			ul.menu{
				background-color: #000;
				color: #fff;
				display: inline-block;
				padding: 16px 0px 16px 32px;
			}
			ul.menu>li{
				padding: 16px 32px;
				list-style: none;
				cursor:pointer;
			}
			ul.menu>li[active]{
				background-color: rgb(255, 85, 0);
			}
		`, '_CSS_MENU', doc)
	}

	render() {
		let items = this.props.items.map((a) => (<li active={a.id == this.props.selected ? 'active' : undefined} key={a.id} onClick={() => this.props.onSelectChange(a)}>{a.name}</li>))
		return (
			<ul style={{display:this.props.items && this.props.items.length ? 'inline-block' : 'none'}} className={'menu'} ref={this.refMain}>
				{items}
			</ul>
		)
	}
}