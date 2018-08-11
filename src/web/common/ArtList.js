import React from 'react'
import Dialog from '../common/Dialog'
import Img_waiting from '../res/waiting.png'
import Img_404 from '../res/404.png'
import Styles from '../res/style.css'
import IconLeft from '../res/left.svg'
import IconRight from '../res/right.svg'

export class ArtList extends React.Component {
	constructor(props) {
		super(props)
		this.onArtImageClick = this.onArtImageClick.bind(this)
		this.onArtMoreDownloadClick = this.onArtMoreDownloadClick.bind(this)
		this.onItemClick = this.onItemClick.bind(this)

		this.state = {
			dlgImageGallery: false,
			dlgImageGallery_images: [],
			dlgMoreDownload: false,
			dlgMoreDownload_data: [],
		}

		this.m_lastActiveItem = null;
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
		let items = this.props.items.map(a => { return <ArtItem key={a.id} data={a} onImageClick={this.onArtImageClick} onMoreDownloadClick={this.onArtMoreDownloadClick} onClick={this.onItemClick} /> });
		let dlg = null;
		if (this.state.dlgImageGallery) {
			dlg = <DialogImageGallery images={this.state.dlgImageGallery_images} onBtnCloseClick={() => { this.setState({ dlgImageGallery: false }) }} />
		} else if (this.state.dlgMoreDownload) {
			dlg = <DialogMoreDownload downloads={this.state.dlgMoreDownload_data} onBtnCloseClick={() => { this.setState({ dlgMoreDownload: false }) }} />
		}
		return (
			<div>
				<div style={{}}>{items}</div>
				{dlg}
			</div>
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
		img.onload = function () { this.ref_img.current && (this.ref_img.current.style.backgroundImage = 'url(' + imgs[0] + ')') }.bind(this)
		img.onerror = img.onabort = function () { this.ref_img.current && (this.ref_img.current.style.backgroundImage = 'url(' + Img_404 + ')') }.bind(this)
	}

	render() {
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
		let date = this.props.data.date && dateFormat(new Date(this.props.data.date), 'yyyy-mm-dd');
		links.push(<span key="date" style={{ float: 'right' }}>{date && date.toString()}</span>)

		return (
			<div className={[Styles.art_item, Styles.shadow].join(' ')} onClick={this.props.onClick}>
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