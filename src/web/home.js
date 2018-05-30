import React from 'react'
import API from './api'
import Page from './common/page'
import Dialog from './common/dlg'
import IconLeft from './css/left.svg'
import IconRight from './css/right.svg'
import Styles from './style.css'
import dateFormat from 'dateformat'
import { debug } from 'util';
import Img_waiting from './waiting.png'
import Img_404 from './404.png'

class HomePage extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return (
			<div style={{ padding: '0px 80px' }}>
				<ArtList />
			</div>
		)
	}
}

class ArtList extends React.Component {
	constructor(props) {
		super(props)

		this.onPageTo = this.onPageTo.bind(this)
		this.onArtImageClick = this.onArtImageClick.bind(this)
		this.onArtMoreDownloadClick = this.onArtMoreDownloadClick.bind(this)
		this.onItemClick = this.onItemClick.bind(this)
		this.flushPageCount = this.flushPageCount
		this.flushList = this.flushList

		this.state = {
			currentPage: 0,
			currentTagID: null,
			totalPageCount: 1,
			artList: [],
			dlgImageGallery: false,
			dlgImageGallery_images: [],
			dlgMoreDownload: false,
			dlgMoreDownload_data: [],
			tags: [],
		}

		this.m_lastActiveItem = null;
	}

	componentWillMount() {
		API.getTag().then((res) => {
			res.json().then((data) => {
				this.setState({ tags: data.items })
			})
		})

		this.flushPageCount(this.state.currentTagID);
		this.flushList(0, this.state.currentTagID);
	}

	onPageTo(page) {
		this.flushList(page, this.state.currentTagID);
	}

	onTagFilter(tagID) {
		this.flushPageCount(tagID);
		this.flushList(0, tagID);
	}

	flushPageCount(tagID) {
		API.getArtPageCount(tagID).then((res) => {
			res.json().then((data) => {
				this.setState({ totalPageCount: data.pageCount, currentTagID: tagID })
			})
		})
	}

	flushList(page, tagID) {
		API.getArt(page, tagID).then((res) => {
			res.json().then((data) => {
				this.setState({ artList: data.items, currentPage: data.page, currentTagID: tagID })
			})
		})
	}

	onArtImageClick(images) {
		this.setState({ dlgImageGallery: true, dlgImageGallery_images: images })
	}

	onArtMoreDownloadClick(downloads) {
		this.setState({ dlgMoreDownload: true, dlgMoreDownload_data: downloads })
	}

	onItemClick(e) {
		if (this.m_lastActiveItem == e.currentTarget) { return; }
		if (this.m_lastActiveItem) {
			this.m_lastActiveItem.removeAttribute('active')
		}
		this.m_lastActiveItem = e.currentTarget
		if (this.m_lastActiveItem) {
			this.m_lastActiveItem.setAttribute('active', 'active')
		}
	}

	render() {
		let items = this.state.artList.map(a => { return <ArtItem key={a.id} data={a} onImageClick={this.onArtImageClick} onMoreDownloadClick={this.onArtMoreDownloadClick} onClick={this.onItemClick} /> });
		let tags = this.state.tags.map(a => { let active = a.id == this.state.currentTagID ? 'active' : null; return <Tag active={active} text={a.name} key={a.id} onClick={() => { this.onTagFilter(a.id) }} />; })

		let dlg = null;
		if (this.state.dlgImageGallery) {
			dlg = <DialogImageGallery images={this.state.dlgImageGallery_images} onBtnCloseClick={() => { this.setState({ dlgImageGallery: false }) }} />
		} else if (this.state.dlgMoreDownload) {
			dlg = <DialogMoreDownload downloads={this.state.dlgMoreDownload_data} onBtnCloseClick={() => { this.setState({ dlgMoreDownload: false }) }} />
		}

		return (
			<div>
				<div style={{ paddingTop: '32px', textAlign: 'center' }}><input style={{ fontSize: '26px', padding: '4px', width: '560px' }} type={'text'} /><input style={{ fontSize: '20px', padding: '6px', marginLeft: '8px', width: '80px' }} type={'button'} value={'搜索'} /></div>
				<div style={{ paddingTop: '32px' }}>{tags}</div>
				<div style={{ paddingTop: '32px' }}><Page current={this.state.currentPage} count={this.state.totalPageCount} onPageTo={this.onPageTo} /></div>
				<div style={{}}>{items}</div>
				<div style={{ paddingTop: '8px' }}><Page current={this.state.currentPage} count={this.state.totalPageCount} onPageTo={this.onPageTo} /></div>
				{dlg}
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

class ArtItem extends React.Component {
	constructor(props) {
		super(props)
		this.ref_img = React.createRef();
	}

	componentWillMount() {		
		// return;
		var imgs = JSON.parse(this.props.data.images)
		if (imgs.length == 0) { return }
		let img = new Image(); img.src = imgs[0]
		img.onload = function () { this.ref_img.current.style.backgroundImage = 'url(' + imgs[0] + ')'; }.bind(this)
		img.onerror = img.onabort = function () { this.ref_img.current.style.backgroundImage = 'url(' + Img_404 + ')'; }.bind(this)
	}

	render() {
		const css_div = {
			width: '240px', display: 'inline-block', padding: '10px', margin: '10px 20px 10px 0px', fontSize: '12px', boxSizing: 'border-box',
			fontFamily: 'tahoma, arial, "Microsoft YaHei", "Hiragino Sans GB", sans-serif',
		}
		const css_content = { height: '47px', overflow: 'hidden', marginTop: '8px' };
		const css_link = { marginRight: '8px' }
		const content = this.props.data.text;

		let imgUrl = './waiting.png';
		var imgs = JSON.parse(this.props.data.images);
		if (imgs.length > 0) { imgUrl = imgs[0] }
		imgUrl = Img_waiting;
		let css_div_img = { width: '220px', height: '220px', background: '#fff url(' + imgUrl + ') no-repeat center', backgroundSize: '100% auto', cursor: 'pointer' }

		var downloads = JSON.parse(this.props.data.downloads);
		let links = [<a key='download' style={css_link} title={downloads[0].text} target='_blank' href={downloads[0].href}>下载</a>]
		if (downloads.length > 1) {
			links.push(<a key='more' style={css_link} href="javascript:;" onClick={() => this.props.onMoreDownloadClick(downloads)}>更多下载...</a>)
		}
		links.push(<a key="img" href='javascript:;' onClick={() => this.props.onImageClick(imgs)}>{`[${imgs.length}图] `}</a>)
		let date = dateFormat(new Date(this.props.data.date), 'yyyy-mm-dd');
		links.push(<span key="date" style={{ float: 'right' }}>{date.toString()}</span>)

		return (
			<div style={css_div} className={Styles.shadow} onClick={this.props.onClick}>
				<div ref={this.ref_img} style={css_div_img} onClick={() => this.props.onImageClick(imgs)}></div>
				<p title={content} style={css_content}>{content}</p>
				<p style={{ marginTop: '8px' }}>{links}</p>
			</div>
		)
	}
}

class DialogImageGallery extends React.Component {
	constructor(props) {
		super(props)
		this.state = { imgIndex: 0 }
		this.onClickArrow = this.onClickArrow.bind(this)
	}

	onClickArrow(inc) {
		if (this.props.images && this.props.images.length > 0) {
			let idx = this.state.imgIndex + inc; if (idx < 0) { idx = this.props.images.length - 1; }
			idx %= this.props.images.length
			this.setState({ imgIndex: idx })
		}
	}

	render() {
		const css_img = { width: 'auto', margin: '8px auto' }
		var imgs = this.props.images.map((item) => (<img style={css_img} src={item} />))
		return (
			<Dialog onBtnCloseClick={this.props.onBtnCloseClick} caption={`${this.state.imgIndex + 1} / ${imgs.length}`}>
				<table style={{ width: '100%', height: '100%' }}>
					<tbody>
						<tr><td className={Styles.btn_arrow} onClick={() => this.onClickArrow(-1)}><IconLeft /></td>
							<td>
								<div style={{ overflow: 'auto', height: '100%', textAlign: 'center' }}>
									{imgs[this.state.imgIndex]}
								</div>
							</td>
							<td className={Styles.btn_arrow} onClick={() => this.onClickArrow(1)}><IconRight /></td></tr>
					</tbody>
				</table>
			</Dialog>
		)
	}
}

class DialogMoreDownload extends React.Component {
	constructor(props) {
		super(props);
	}

	render() {
		const css_link = { fontSize: '12px' }
		let links = this.props.downloads.map(item => (<p><a style={css_link} title={item.text} target='_blank' href={item.href}>{item.text}</a></p>))
		return (
			<Dialog onBtnCloseClick={this.props.onBtnCloseClick} caption={'下载'} size={'small'}>
				<div style={{ padding: '4px', backgroundColor: '#fff', height: '100%', boxSizing: 'border-box', overflow: 'auto' }}>
					{links}
				</div>
			</Dialog>
		)
	}
}

export default HomePage