import React from 'react'
import API from '../common/api'

export default class Tool extends React.Component {
	constructor(props) {
		super(props)
	}

	buildRelActorArt() {
		API.buildRelActorArt().then(result => {
			return result.json()
		}).then(result => {
			alert(result)
		})
	}

	render() {
		return (
			<div>
				<p><button onClick={this.buildRelActorArt}>build Actor Art relation</button></p>
			</div>
		);
	}
}