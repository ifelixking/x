import React from 'react'
import { NavLink } from 'react-router-dom'

const sty_main = {
	backgroundColor: 'black',
	color: 'white',
	height: '80px',
	paddingLeft: '200px',
	borderBottom: '3px solid #F50'
};

const sty_link = {
	display: 'inline-block',
	color: 'white',
	textDecoration: 'none',
	marginRight: '24px',
	height: '38px',
	fontSize: '24px',
	padding: '12px',
	marginTop:'20px'
};

const sty_link_active = {
	backgroundColor: '#F50',
};


export default class Nav extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		let links = this.props.items.map((item) => {
			return (<NavLink exact={true} key={item.text} activeStyle={sty_link_active} style={sty_link} to={item.href}>{item.text}</NavLink>)
		})
		return (
			<div style={sty_main}>
				{links}</div>
		)
	}
}