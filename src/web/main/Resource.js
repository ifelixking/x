import React from 'react'
import API from '../common/api'
import Page from '../common/Page'
import { ArtList } from '../common/ArtList'
import Styles from '../res/style.css'
import { connect } from 'react-redux';
import { combineReducers } from 'redux';

class Resource extends React.Component {
	constructor(props) {
		super(props)
		this.onPageTo = this.onPageTo.bind(this)
		this.onSearchClick = this.onSearchClick.bind(this);
		this.ref_keyword = React.createRef();
	}

	componentWillMount() {
		this.props.tags || this.props.fetchTag()
	}

	onPageTo(page) {
		this.props.fetchArt(page, this.props.currentTagID, this.props.currentKeyword)
	}

	onTagFilter(tagID) {
		this.ref_keyword.current && (this.ref_keyword.current.value == '')
		this.props.changeTag(tagID)
		this.props.fetchArt(0, tagID, null)
	}

	onSearchClick() {
		if (this.ref_keyword.current == null) { return; }
		this.props.fetchArt(0, null, this.ref_keyword.current.value)
	}

	render() {
		let tags = this.props.tags && this.props.tags.map(a => { let active = a.id == this.props.currentTagID ? 'active' : null; return <Tag active={active} text={a.name} key={a.id} onClick={() => { this.onTagFilter(a.id) }} />; })

		return (
			<div style={{ padding: '0px 80px' }}>
				<form style={{ paddingTop: '32px', textAlign: 'center' }} onSubmit={(e) => { e.preventDefault(); return false }}>
					<input style={{ fontSize: '26px', padding: '4px', width: '560px' }} type={'text'} ref={this.ref_keyword} />
					<input style={{ fontSize: '20px', padding: '6px', marginLeft: '8px', width: '80px' }} type={'submit'} value={'搜索'} onClick={this.onSearchClick} />
				</form>
				<div style={{ paddingTop: '32px' }}>{tags}</div>
				<div style={{ paddingTop: '32px' }}><Page current={this.props.currentPage} count={this.props.totalPageCount} onPageTo={this.onPageTo} /></div>
				<ArtList items={this.props.artList || []} />
				<div style={{ paddingTop: '8px' }}><Page current={this.props.currentPage} count={this.props.totalPageCount} onPageTo={this.onPageTo} /></div>
			</div>
		)
	}
}

class Tag extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		let active = this.props.active ? 'active' : null;
		return (
			<a style={{}} active={active} className={Styles.tag} href='javascript:;' onClick={this.props.onClick}>{this.props.text}</a>
		)
	}
}

const Module = {
	Component: connect(
		(state) => {
			return { tags: state.home.tags, currentTagID: state.home.currentTagID, totalPageCount: state.home.totalPageCount, ...state.home.artData }
		},
		(dispatch) => {
			return {
				fetchTag: () => { dispatch(Module.Actions.fetchTag()) },
				changeTag: (tagID) => { dispatch(Module.Actions.changeTag(tagID)) },
				fetchArt: (page, tagID, keyword) => { dispatch(Module.Actions.fetchArt(page, tagID, keyword)) }
			}
		}
	)(Resource),
	Actions: {
		fetchTag: () => {
			return dispatch => {
				API.getTagList().then((res) => {
					res.json().then((data) => {
						dispatch({ type: 'FLUSH_TAG_LIST', tags: data.items })
						let theTag = data.items.find((i) => i.name == '日本无码')
						dispatch(Module.Actions.changeTag(theTag && theTag.id))
						dispatch(Module.Actions.fetchArt(0, theTag && theTag.id, null))
					})
				})
			}
		},
		changeTag: (tagID) => {
			return dispatch => {
				dispatch({ type: 'CHANGE_CURRENT_TAG', currentTagID: tagID })
				API.getArtPageCount(tagID).then((res) => {
					res.json().then((data) => {
						dispatch({ type: 'FLUSH_PAGE_COUNT', totalPageCount: data.pageCount })
					})
				})
			}
		},
		fetchArt: (page, tagID, keyword) => {
			return dispatch => {
				if (keyword != null) {
					API.searchArt(page, keyword).then((res) => {
						res.json().then((data) => {
							dispatch({ type: 'FLUSH_ART_LIST', artList: data.items, currentPage: data.page, currentTagID: null, currentKeyword: keyword })
						})
					})
				} else {
					API.getArtList(page, tagID).then((res) => {
						res.json().then((data) => {
							dispatch({ type: 'FLUSH_ART_LIST', artData: { artList: data.items, currentPage: data.page, currentTagID: tagID, currentKeyword: null } })
						})
					})
				}
			}
		}
	},
	Reducers: combineReducers({
		tags: (tags = null, action) => {
			return action.type == 'FLUSH_TAG_LIST' ? action.tags : tags;
		},
		currentTagID: (currentTagID = null, action) => {
			return action.type == 'CHANGE_CURRENT_TAG' ? action.currentTagID : currentTagID
		},
		totalPageCount: (totalPageCount = 0, action) => {
			return action.type == 'FLUSH_PAGE_COUNT' ? action.totalPageCount : totalPageCount
		},
		artData: (artData = null, action) => {
			return action.type == 'FLUSH_ART_LIST' ? action.artData : artData
		}
	})
}

export default Module