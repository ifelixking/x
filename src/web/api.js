const API = {
	getArt: (page=0, tag=null)=>{
		let url = `/api/art?page=${page}`;
		if (tag != null) { url = `${url}&tag=${tag}` }
		return fetch(url);
	},

	getArtPageCount: (tag=null)=>{
		let url = '/api/art/pageCount';
		if (tag != null) { url = `${url}?tag=${tag}` }
		return fetch(url);
	},

	getTag: ()=>{
		return fetch('/api/art/tag');
	}
}

export default API;