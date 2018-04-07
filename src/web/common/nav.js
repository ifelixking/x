import React from 'react'
import {Link} from 'react-router-dom'

export default class Nav extends React.Component{
	constructor(props){
		super(props)
	}

	render(){
		let links = this.props.items.map((item)=>{
			return (<Link to={item.href}>{item.text}</Link>)
		})
		return (
			<div>{links}</div>
		)
	}
}