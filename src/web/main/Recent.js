import React from 'react'
import Menu from '../common/Menu'
import API from '../common/api'
import {ArtList} from '../common/ArtList'

export default class Recent extends React.Component {
	constructor(props) {
		super(props)
		this.onMenuChange = this.onMenuChange.bind(this)
		this.state = {
			menuItems: [],
			currentMenuItem: null,
			artList: []
		}
	}

	componentWillMount() {
		API.getTagList().then(res => {
			res.json().then(data => {
				this.setState({ menuItems: data.items, currentMenuItem: data.items.length && data.items[0].id })
				this.onMenuChange(data.items[0])
			})
		})
	}

	onMenuChange(item) {
		this.setState({ currentMenuItem: item.id })
		API.getArtListRecent(item.id).then(res => {
			res.json().then(data => {
				this.setState({ artList: data.items })
			})
		})
	}

	render() {
		const css_frame = { boxSizing: 'border-box', padding: '16px', overflow: 'auto', position: 'absolute', top: '85px', left: '160px', width: 'calc(100% - 160px)', height: 'calc(100% - 85px)' }
		return (
			<div>
				<Menu items={this.state.menuItems} selected={this.state.currentMenuItem} onSelectChange={this.onMenuChange} />
				<div style={css_frame}>
					<ArtList groupByDate={true} items={this.state.artList} />
				</div>
			</div>
		)
	}
}