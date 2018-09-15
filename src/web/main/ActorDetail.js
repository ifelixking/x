import React from 'react'
import API from '../common/api'
import Styles from '../res/style.css'
import { ArtList } from '../common/ArtList'
import Config from '../common/config'
import Modal from '../common/Modal'

export default class ActorDetail extends React.Component {
	constructor(props) {
		super(props)
		this.onImageError = this.onImageError.bind(this)
		this.onDlgOK = this.onDlgOK.bind(this)
		this.onMatchWordsClick = this.onMatchWordsClick.bind(this)
		this.state = {
			actor: {},
			artList: [],
			dlg: false,
			dlgText: ''
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

	onImageError(e) {
		e.target.src = `${Config.ActorImageBasePath}${this.state.actor.image}`
	}

	onMatchWordsClick() {
		this.setState({ dlg: true, dlgText: this.state.actor.matchWords && this.state.actor.matchWords.replace(/;/g, '\n') });
	}

	onDlgOK() {
		const matchWords = this.state.dlgText ? this.state.dlgText.replace(/\n/g, ';') : ''
		const actorID = this.props.match.params.id
		API.setActor(actorID, matchWords).then(result=>{
			this.setState({ dlg: false });
		})		
	}

	render() {
		return (
			<div style={{ padding: '0px 80px' }}>
				<div style={{ height: '180px', borderBottom: '1px solid #eee' }}>
					<div className={[Styles.actor_item, Styles.shadow].join(' ')} style={{ margin: '20px 0px' }}>
						<img style={{ width: '125px', height: '125px' }} src={this.state.actor.oriImage} onError={this.onImageError} />
					</div>
					<div style={{ display: 'inline-block', border: '0px solid red', position: 'absolute', top: '108px', left: '260px', fontSize: '24px' }}>
						<p>{this.state.actor.name}</p>
						<p>
							{this.state.actor.matchWords && this.state.actor.matchWords.replace(/;/g, '、')}
							<button onClick={this.onMatchWordsClick}>编辑</button>
						</p>
					</div>
				</div>
				<ArtList items={this.state.artList} />
				<Modal title='Match Words' visible={this.state.dlg} onCancel={() => { this.setState({ dlg: false }) }} onOK={this.onDlgOK} showButtons={true} fullSize={false}>
					<div style={{ padding: '16px', boxSizing: 'border-box', width: '100%', height: '100%' }}>
						<textarea style={{ width: '100%', height: '100%' }} value={this.state.dlgText} onChange={(e) => { this.setState({ dlgText: e.target.value }) }} />
					</div>
				</Modal>
			</div>
		)
	}
}

// this.ref_matchWords.current.value = this.state.actor.matchWords && this.state.actor.matchWords.replace(/;/g, '\n')