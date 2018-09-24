const API = {
	getArtListRecent: (tag = null) => {
		let url = `/api/art/recent`;
		if (tag != null) { url = `${url}?tag=${tag}` }
		return fetch(url);
	},

	getArtList: (page = 0, tag = null) => {
		let url = `/api/art?page=${page}`;
		if (tag != null) { url = `${url}&tag=${tag}` }
		return fetch(url);
	},

	getArtPageCount: (tag = null) => {
		let url = '/api/art/pageCount';
		if (tag != null) { url = `${url}?tag=${tag}` }
		return fetch(url);
	},

	getTagList: () => {
		return fetch('/api/tag');
	},

	searchArt: (page, keyword) => {
		let url = `/api/art/search?page=${page}&k=${keyword}`;
		return fetch(url);
	},

	getActorArt: (actorID) => {
		let url = `/api/actor/${actorID}/art`;
		return fetch(url);
	},

	getActors: (kw) => {
		let url = kw ? `/api/actor/search?keyword=${kw}` : `/api/actor`;
		return fetch(url);
	},

	getActor: (actorID) => {
		let url = `/api/actor/${actorID}`;
		return fetch(url);
	},

	getPostList: (page = 0, areaID = 0) => {
		let url = `/api/post?page=${page}&area=${areaID}`
		return fetch(url)
	},

	getPostListPageCount: (areaID = 0) => {
		const url = `/api/post/pageCount?&area=${areaID}`
		return fetch(url)
	},

	createPost: (title, content, attachment, areaID) => {
		const url = '/api/post'
		return fetch(url, {
			headers: {
				'Content-Type': 'application/json; charset=utf-8'
			},
			method: 'POST', body: JSON.stringify({
				title, content, attachment, hasAttachment: !!attachment, area: areaID
			})
		})
	},

	getPost: (id, page) => {
		const url = `/api/post/${id}?page=${page}`
		return fetch(url)
	},

	getPostDetailPageCount: (id) => {
		const url = `/api/post/${id}/pageCount`
		return fetch(url)
	},

	reply: (hostID, parentID, content, attachment) => {
		const url = '/api/post/reply'
		return fetch(url, {
			headers: {
				'Content-Type': 'application/json; charset=utf-8'
			},
			method: 'POST', body: JSON.stringify({
				hostID, parentID, content, attachment, hasAttachment: !!attachment
			})
		})
	},

	setActor: (actorID, matchWords) => {
		const url = `/api/actor/${actorID}`
		return fetch(url, {
			headers: { 'Content-Type': 'application/json; charset=utf-8' }, method: 'PUT',
			body: JSON.stringify({
				matchWords
			})
		})
	},

	buildRelActorArt: ()=>{
		const url = `/api/actor/buildRel`
		return fetch(url, {
			headers: { 'Content-Type': 'application/json; charset=utf-8' }, method: 'POST',
		})
	},
}

export default API;