import React from 'react'
import API from './api'
import Page from './common/page'
import Dialog from './common/dlg'

class HomePage extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return (
			<ArtList />
		)
	}
}

class ArtList extends React.Component {
	constructor(props) {
		super(props)
		this.onPageTo = this.onPageTo.bind(this)
		this.state = {
			currentPage: 0,
			totalPageCount: 1,
			artList: []
		}
	}

	componentWillMount() {
		API.getArtPageCount().then((res) => {
			res.json().then((data) => {
				this.setState({ totalPageCount: data.pageCount })
			})
		})

		API.getArt(this.state.currentPage).then((res) => {
			res.json().then((data) => {
				this.setState({ artList: data.items })
			})
		})
	}

	onPageTo(page) {
		API.getArt(page).then((res) => {
			res.json().then((data) => {
				this.setState({ artList: data.items, currentPage: data.page })
			})
		})
	}

	render() {
		let items = this.state.artList.map(a => { return <ArtItem key={a.id} data={a} /> });
		return (
			<div>
				<div style={{ textAlign: 'right', paddingTop: '8px' }}>
					<Page current={this.state.currentPage} count={this.state.totalPageCount} onPageTo={this.onPageTo} />
				</div>
				<div style={{}}>{items}</div>
				<div style={{ textAlign: 'right', paddingTop: '8px' }}>
					<Page current={this.state.currentPage} count={this.state.totalPageCount} onPageTo={this.onPageTo} />
				</div>
				<Dialog>
					<ImageGallery />
				</Dialog>
			</div>
		)
	}

}

class ArtItem extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		const css_div = { border: '1px solid gray', width: '200px', display: 'inline-block', padding: '8px', margin: '8px', fontSize: '12px', fontFamily: 'tahoma, arial, "Microsoft YaHei", "Hiragino Sans GB", sans-serif' }
		const css_more_download = { float: 'right' }
		const css_content = { height: '47px', overflow: 'hidden', marginTop: '8px' };
		const css_download = { marginTop: '8px' }
		const content = this.props.data.text;

		let imgUrl = 'https://www.baidu.com/img/baidu_jgylogo3.gif';
		var imgs = JSON.parse(this.props.data.images);
		if (imgs.length > 0) { imgUrl = imgs[0] }
		let css_div_img = { width: '200px', height: '200px', background: '#fff url(' + imgUrl + ') no-repeat center', backgroundSize: '100% auto' }

		var downloads = JSON.parse(this.props.data.downloads);
		let links = [<a key='download' title={downloads[0].text} href={downloads[0].href}>下载</a>]
		if (downloads.length > 1) {
			links.push(<a key='more' style={css_more_download} href="asdf">更多下载...</a>)
		}

		return (
			<div style={css_div}>
				<div style={css_div_img}></div>
				<p title={content} style={css_content}>{content}</p>
				<p style={css_download}>{links}</p>
			</div>
		)
	}
}

class ArtDownload extends React.Component {
	constructor(props) {
		super(props)
	}
	render() {
		return (
			<Dialog>{"asdf"}</Dialog>
		)
	}
}

class ImageGallery extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return null
	}
}

export default HomePage