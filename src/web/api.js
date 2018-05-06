const API = {
	getArt: (page=0)=>{
		return fetch('/api/art?page='+page);
		// .then((res)=>{
		// }).catch((res) => { console.log(res.status) })
	}
}

export default API;