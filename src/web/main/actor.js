import React from 'react'
import API from '../common/api'
import Styles from '../res/style.css'
import { connect } from 'react-redux';
import { combineReducers } from 'redux';
import Config from '../common/config'

class ActorPage extends React.Component {
	constructor(props) {
		super(props)
	}

	componentWillMount() {
		this.props.items || this.props.fetchActors()
	}

	render() {
		let visible = this.props.visible ? 'block' : 'none'
		let items = this.props.items && this.props.items.map((i) => <ActorItem key={i.id} data={i} />)
		return (
			<div style={{ display: visible, padding: '0px 80px' }}>{items}</div>
		)
	}
}

class ActorItem extends React.Component {
	constructor(props) {
		super(props)
		this.onImageError = this.onImageError.bind(this)
	}

	onImageError(e){
		e.target.src = `${Config.ActorImageBasePath}${this.props.data.image}`
	}

	render() {
		return (
			<a className={[Styles.actor_item, Styles.shadow].join(' ')} href={`/actor/${this.props.data.id}`} target={'_blank'}>
				<div><img style={{ width: '125px', height: '125px' }} src={`${this.props.data.oriImage}`} onError={this.onImageError} /></div>
				{this.props.data.name}
			</a>
		)
	}
}

const Module = {
	Component: connect(
		state => ({ items: state.actor.items }),
		dispatch => ({
			fetchActors: () => { dispatch(Module.Actions.fetchActors()) }
		})
	)(ActorPage),
	Actions: {
		fetchActors: () => {
			return dispatch => {
				API.getActor().then((res) => {
					res.json().then((data) => {
						dispatch({ type: 'FETCH_ACTOR_LIST', items: data.items })
					})
				})
			}
		}
	},
	Reducers: combineReducers({
		items: (items = null, action) => {
			return action.type == 'FETCH_ACTOR_LIST' ? action.items : items
		}
	})
}

export default Module