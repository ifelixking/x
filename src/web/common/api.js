const API = {
	getArt: (page = 0, tag = null) => {
		let url = `/api/art?page=${page}`;
		if (tag != null) { url = `${url}&tag=${tag}` }
		return fetch(url);
	},

	getArtPageCount: (tag = null) => {
		let url = '/api/art/pageCount';
		if (tag != null) { url = `${url}?tag=${tag}` }
		return fetch(url);
	},

	getTag: () => {
		return fetch('/api/tag');
	},

	searchArt: (page, keyword) => {
		let url = `/api/art/search?page=${page}&k=${keyword}`;
		return fetch(url);
	},

	// getActor: () => {
	// 	let url = `/api/actor`;
	// 	return fetch(url);
	// },

	getActorArt: (actorID) => {
		let url = `/api/actor/${actorID}/art`;
		return fetch(url);
	},

	getActor: (actorID) => {
		let url = typeof actorID == 'undefined' ? `/api/actor` : `/api/actor/${actorID}`;
		return fetch(url);
	}
}

export default API;