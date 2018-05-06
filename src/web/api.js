const API = {
	getArt: (page=0)=>{
		return fetch('/api/art?page='+page);
		// .then((res)=>{
		// }).catch((res) => { console.log(res.status) })
	},

	getArtPageCount: ()=>{
		return fetch('/api/art/pageCount');
	}
}

export default API;