import React from 'react'
import API from '../common/api'
import Styles from '../res/style.css'
import { ArtList } from '../common/ArtList'

export class ActorDetail extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			actor: {},
			artList: []
		}
	}

	componentWillMount() {
		API.getActor(this.props.match.params.id).then((res) => {
			res.json().then((data) => {
				this.setState({ actor: data.data })
			})
		})
		API.getActorArt(this.props.match.params.id).then((res) => {
			res.json().then((data) => {
				this.setState({ artList: data.items })
			})
		})
	}

	render() {
		return (
			<div style={{ padding: '0px 80px' }}>
				<div style={{ height: '180px', borderBottom: '1px solid #eee' }}>
					<div className={[Styles.actor_item, Styles.shadow].join(' ')} style={{ margin: '20px 0px' }}>
						<img style={{ width: '125px', height: '125px' }} src={this.state.actor.image} />
					</div>
					<div style={{ display: 'inline-block', border: '0px solid red', position: 'absolute', top: '108px', left: '260px', fontSize: '24px' }}>{this.state.actor.name}</div>
				</div>
				<ArtList items={this.state.artList} />
			</div>
		)
	}
}