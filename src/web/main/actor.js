import React from 'react'
import API from '../common/api'
import Styles from '../res/style.css'
import { connect } from 'react-redux';
import { combineReducers } from 'redux';
import Config from '../common/config'
import IconClose from '../res/close2.svg'

class ActorPage extends React.Component {
	constructor(props) {
		super(props)
		this.ref_keyword = React.createRef();
		this.onSearchClick = this.onSearchClick.bind(this)
		this.onClearSearch = this.onClearSearch.bind(this)
	}

	componentWillMount() {
		this.props.items || this.props.fetchActors()
	}

	onSearchClick() {
		const kw = this.ref_keyword.current.value
		this.props.fetchActors(kw)
	}

	onClearSearch() {
		this.ref_keyword.current.value = ''
		this.props.fetchActors()
	}

	render() {
		let visible = this.props.visible ? 'block' : 'none'
		let items = this.props.items && this.props.items.map((i) => <ActorItem key={i.id} data={i} />)
		const btnClear = this.ref_keyword.current && this.ref_keyword.current.value != '' ? 'visible' : 'hidden'
		return (
			<div style={{ display: visible, padding: '0px 80px' }}>
				<form style={{ paddingTop: '32px', textAlign: 'center' }} onSubmit={(e) => { e.preventDefault(); return false }}>
					<input className={Styles.txt_search} type={'text'} ref={this.ref_keyword} />
					<IconClose onClick={this.onClearSearch} style={{ fill: '#aaa', position: 'relative', left: '-32px', top: '1px', cursor: 'pointer', visibility: btnClear }} />
					<input style={{ fontSize: '20px', padding: '6px', marginLeft: '8px', width: '80px' }} type={'submit'} value={'搜索'} onClick={this.onSearchClick} />
				</form>
				<div>{items}</div>
			</div>
		)
	}
}

ActorPage.defaultProps = {
	visible: true
};

class ActorItem extends React.Component {
	constructor(props) {
		super(props)
		this.onImageError = this.onImageError.bind(this)
	}

	onImageError(e) {
		const backupSrc = `${Config.ActorImageBasePath}${this.props.data.image}`
		if (e.target.src != backupSrc) { e.target.src = backupSrc }
	}

	render() {
		return (
			<a className={[Styles.actor_item, Styles.shadow].join(' ')} href={`/actor/${this.props.data.id}`} target={'_blank'} title={this.props.data.matchWords}>
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
			fetchActors: (kw) => { dispatch(Module.Actions.fetchActors(kw)) }
		})
	)(ActorPage),
	Actions: {
		fetchActors: (kw) => {
			return dispatch => {
				API.getActors(kw).then((res) => {
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