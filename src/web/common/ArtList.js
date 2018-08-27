import React from 'react'
import Img_404 from '../res/404.png'
import Styles from '../res/style.css'
import utils from '../common/utils'
import Modal from '../common/Modal'

export class ArtList extends React.Component {
	constructor(props) {
		super(props)
		this.onArtImageClick = this.onArtImageClick.bind(this)
		this.onItemSelected = this.onItemSelected.bind(this)
		this.refMain = React.createRef()

		this.state = {
			showDlg: false,
			dlgImages: [],
			dlgDownloads: [],
		}

		this.m_lastActiveItem = null;
	}

	componentDidMount() {
		utils.injectCSS(`
			._CSS_ARTLIST_download{
				
				text-decoration: none;
			}
			._CSS_ARTLIST_download:hover{
				color: #ff8400;
				text-decoration: underline;
			}
		`, '_CSS_ARTLIST', this.refMain.current.ownerDocument)
	}

	componentWillUnmount() {
		this.refMain.current.ownerDocument.getElementById('_CSS_ARTLIST').remove()
	}

	onArtImageClick(images, downloads) {
		this.setState({ showDlg: true, dlgImages: images, dlgDownloads: downloads })
	}

	onItemSelected(e) {
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
		let items
		if (this.props.groupByDate) {
			let groups = []
			this.props.items.forEach(a => {
				const comp = <ArtItem key={a.id} data={a} onImageClick={this.onArtImageClick} onClick={this.onItemSelected} />
				if (!groups.length || groups[groups.length - 1].date != a.date) { groups.push({ date: a.date, items: [comp] }) } else { groups[groups.length - 1].items.push(comp) }
			});
			items = groups.map(a => {
				return (
					<div key={a.date}>
						<div style={{}}>
							<span style={{ display: 'inline-block', color: '#666', width: '88px', textAlign: 'center', fontWeight: 'bold' }}>{utils.toDateString(a.date)}</span>
							<hr size={1} color={'#aaa'} style={{ float: 'right', display: 'inline-block', color: '#333', width: 'calc(100% - 92px)', marginTop: '11px', color: '#aaa' }} />
						</div>
						<div style={{ paddingLeft: '80px' }}>{a.items}</div>
					</div>
				)
			})
		} else {
			items = this.props.items.map(a => (<ArtItem key={a.id} data={a} onImageClick={this.onArtImageClick} onClick={this.onItemSelected} />))
		}

		let dlgImages = this.state.showDlg && this.state.dlgImages.map((a, i) => <img src={a} key={i} onError={(e) => e.target.src = Img_404} />)
		let dlgDownloads = this.state.showDlg && this.state.dlgDownloads.map((a, i) => <a style={{ display: 'block' }} target='_blank' href={a} key={i} >{a}</a>)
		return (
			<div ref={this.refMain}>
				<div style={{}}>{items}</div>
				<Modal fullSize={true} showButtons={false} title={'下载'} visible={this.state.showDlg} onCancel={() => this.setState({ showDlg: false })} >
					<div style={{ padding: '16px', boxSizing: 'border-box', overflow: 'auto', width: '100%', height: '100%' }}>
						<div style={{ margin: '16px 0px 32px 0px', }}>{dlgDownloads}</div>{dlgImages}
					</div>
				</Modal>
			</div>
		)
	}
}

class ArtItem extends React.Component {
	constructor(props) {
		super(props)
		this.ref_img = React.createRef();
	}

	// componentWillMount() {
	// 	// let imgs; try { imgs = JSON.parse(this.props.data.images) } catch (ex) { console.log(this.props.data); console.log(ex); }
	// 	// if (!imgs || imgs.length == 0) { return }
	// 	// let img = new Image(); img.src = imgs[0]
	// 	// img.onload = function () { this.ref_img.current && (this.ref_img.current.style.backgroundImage = 'url(' + imgs[0] + ')') }.bind(this)
	// 	// img.onerror = img.onabort = function () { this.ref_img.current && (this.ref_img.current.style.backgroundImage = 'url(' + Img_404 + ')') }.bind(this)
	// }

	render() {
		const css_content = { height: '47px', overflow: 'hidden', marginTop: '8px', cursor: 'default' };
		const css_link = { marginRight: '8px' }
		const content = this.props.data.text;

		// let imgUrl = Img_waiting;
		// let css_div_img = { width: '220px', height: '220px', background: '#fff url(' + imgUrl + ') no-repeat center', backgroundSize: '100% auto', cursor: 'pointer' }
		let css_div_img = { maxWidth: '220px', maxHeight: '220px', cursor: 'pointer' }

		let downloads = JSON.parse(this.props.data.downloads);
		let imgs; try { imgs = JSON.parse(this.props.data.images) } catch (ex) { console.log(this.props.data); console.log(ex); } imgs = imgs || []

		let links = [<a key='download' className={'_CSS_ARTLIST_download'} style={css_link} target='_blank' href={downloads[0]}>下载</a>]
		if (downloads.length > 1) {
			links.push(<a key='more' className={'_CSS_ARTLIST_download'} style={css_link} href="javascript:;" onClick={() => this.props.onImageClick(imgs, downloads)}>更多下载...</a>)
		}

		// let hiddenImages = imgs.map((a,i) => <img key={i} style={{ display: 'none' }} src='a' />)
		links.push(<a key="img" href='javascript:;' onClick={() => this.props.onImageClick(imgs, downloads)}>{`[${imgs.length}图] `}</a>)
		let date = utils.toDateString(this.props.data.date)
		links.push(<span key="date" style={{ float: 'right' }}>{date && date.toString()}</span>)

		return (
			<div className={[Styles.art_item, Styles.shadow].join(' ')} onClick={this.props.onClick} style={{ textAlign: 'center' }}>
				{/* <div ref={this.ref_img} style={css_div_img} onClick={() => this.props.onImageClick(imgs)}></div> */}
				<img style={css_div_img} src={imgs && imgs.length ? imgs[0] : Img_404} onError={(e) => e.target.src = Img_404} onClick={() => this.props.onImageClick(imgs, downloads)} />
				<div style={{ textAlign: 'left' }}>
					<p title={content} style={css_content}>{content}</p>
					<p style={{ marginTop: '8px' }}>{links}</p>
				</div>
			</div>
		)
	}
}