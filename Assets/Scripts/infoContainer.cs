using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoContainer : MonoBehaviour {
	public static infoContainer instance = null;
	public static string server = "http://35.185.195.244/api";

	public string token;
	public userInfo usrInfo;
	public List<movieInfo> recommendList;
	public List<movieInfo> searchResult;

	//this must be initialized when conneted with server
	private HashSet<string> favs;
	private HashSet<string> dislikes;

	List<movieInfo> localLikeList;

	void Awake () {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		favs = new HashSet<string>();
		dislikes = new HashSet<string>();
	}

	#region list related static functions
	public static bool inFavorites(movieInfo target) {
		return instance.favs.Contains(target.id);	
	}

	public static void addToMyFav(movieInfo target) {
		instance.favs.Add(target.id);
		instance.modifyTargetList(target, "add", "liked");
		//developerLogs.log("send to server add target");
	}

	public static void removeFromFav(movieInfo target) {
		instance.favs.Remove(target.id);
		instance.modifyTargetList(target, "remove", "liked");
		//developerLogs.log("send to server remove target");
	}

	public static void addToDislike(movieInfo target){
		instance.dislikes.Add(target.id);
		instance.modifyTargetList(target, "add", "disliked");
		//developerLogs.log("send to server dislike target");
	}

	public static bool isDislike(movieInfo target){
		return instance.dislikes.Contains(target.id);		
	}

	public static void removeFromDislike(movieInfo target) {
		instance.modifyTargetList(target, "remove", "disliked");
		instance.dislikes.Remove(target.id);
		//developerLogs.log("send to server remove target");
	}
	#endregion

	public void initLists(){
		updateLocalList("liked", favs);
		updateLocalList("disliked", dislikes);
	}

	private void updateHashSet(HashSet<string> targetList, List<movieInfo> requestList) {
		foreach (movieInfo mi in requestList)
			targetList.Add(mi.id);
	}

	#region update recommendation list
	public void updateRecList(List<movieInfo> movieInfos, int n = 5) {
		WWWForm form = new WWWForm();
		for (int i = 0; i < movieInfos.Count; i++) {
			form.AddField("movieIds", movieInfos[i].id);
		}
		
		form.AddField("n", n);
		form.AddField("longitude", Input.location.lastData.longitude.ToString());
		form.AddField("latitude", Input.location.lastData.altitude.ToString());
		IEnumerator c = requestRecommendation(form);
		StartCoroutine(c);		
	}

	IEnumerator requestRecommendation(WWWForm form) {
		Dictionary<string, string> header = form.headers;
		header["Authorization"] = "Bearer " + token ;
		byte[] rawData = form.data;
		WWW w = new WWW(server + "/users/" + usrInfo.id + "/recommendations", rawData, header);
		yield return w;
		if (w.error == ""){
			recommendation recommendResult = JsonUtility.FromJson<recommendation>(w.text);
			recommendList =recommendResult.outputMovies;
			//update recommendation list
			Debug.Log(w.text);
			mainController.instance.recommend.recommendHelper.createTags(recommendList.Count, recommendList);
		}
		else
			Debug.LogWarning(w.error);
	}
	#endregion

	#region send search query
	//wrapper function 
	public void sendSearchQuery(string s, int n, string b = "title"){
		IEnumerator c = searchMoviesByTitle(s, n, b);
		StartCoroutine(c);
	}

	IEnumerator searchMoviesByTitle(string s, int n, string b){
		Dictionary<string, string> header = new Dictionary<string, string>();
		header["Authorization"] = "Bearer " + token;

		WWW w = new WWW(server + parseSearchQuery(s,n,b), null, header);
		yield return w;
		if (w.error == ""){
			movieList result = JsonUtility.FromJson<movieList>("{\"myList\":"+w.text+"}");
			searchResult = result.myList;			
			searchController.instance.searchBar.updateBarContent(searchResult);		
		}
		else
			Debug.LogWarning(w.error);
	}

	string parseSearchQuery(string s, int n,string b){
		
		return "/movies?q=" + s + "&b=" + b + "&n=" + n.ToString();
	}
	#endregion

	#region sync user list 
	//wrapper function
	void updateLocalList(string listName, HashSet<string> targetList) {
		IEnumerator c = updateMovieList(listName, targetList);
		StartCoroutine(c);
	}
	
	IEnumerator updateMovieList(string listName, HashSet<string> targetList) {
		//authorization
		Dictionary<string, string> header = new Dictionary<string, string>();
		header["Authorization"] = "Bearer " + token;

		WWW w = new WWW(server + parseListUpdateQuery(listName), null, header);
		yield return w;

		if (w.error == ""){
			//Debug.Log(w.text);
			movieList result = JsonUtility.FromJson<movieList>("{\"myList\":" + w.text + "}");
			updateHashSet(targetList, result.myList);
			
			
		}
		else
			Debug.LogWarning(w.error);
	}


	string parseListUpdateQuery(string listName){
		return "/users/"+ usrInfo.id + "/movies/"+listName;
	}
	#endregion

	#region send like/dislike data
	//wrapper functions
	public void modifyTargetList(movieInfo m,string action, string targetList) {
		WWWForm form = new WWWForm();
		//movieId, action, longitude, latitude
		form.AddField("movieId", m.id);
		form.AddField("action", action);
		form.AddField("longitude", Input.location.lastData.longitude.ToString());
		form.AddField("latitude", Input.location.lastData.altitude.ToString());
		IEnumerator c = modifyList(form, targetList);
		StartCoroutine(c);
	}

	IEnumerator modifyList(WWWForm form, string targetList){
		Dictionary<string, string> header = form.headers;
		header["Authorization"] = "Bearer " + token;
		byte[] rawData = form.data;
		WWW w = new WWW(server + parseListUpdateQuery(targetList), rawData, header);
		yield return w;

		if (w.error != "")	
			Debug.LogWarning(w.error);
	}
	#endregion

	#region get nearby trending
	public void getNearbyTrending(float distance, int n = 5){
		mainController.instance.location.locationHelper.clearTags();
		IEnumerator c = getNearby(distance, n);
		StartCoroutine(c);
	}

	IEnumerator getNearby(float distance, int n) {
		Dictionary<string, string> header = new Dictionary<string, string>();
		header["Authorization"] = "Bearer " + token;

		WWW w = new WWW(server + parseNearby(distance,n), null, header);
		yield return w;
		if (w.error == ""){
			//Debug.Log(w.text);
			List<movieWithCount> result = JsonUtility.FromJson<nearByReponse>(w.text).topLikedMovies;
			List<movieInfo> res = new List<movieInfo>();
			for (int i = 0; i < result.Count; i++) {
				res.Add(result[i].movie);
			}
			locationController.instance.locationHelper.createTags(res.Count, res);
		}
		else
			Debug.LogWarning(w.error);
	}

	string parseNearby(float distance, int n) {
		//long, lat, dist, n
		string tmp = "/nearby?long=";
		tmp += Input.location.lastData.longitude.ToString();
		tmp += "&lat=" + Input.location.lastData.latitude.ToString();
		tmp += "&dist=" + distance.ToString();
		tmp += "&n=" + n.ToString();
		return tmp;
	}

	#endregion

}

[System.Serializable]
public class userInfo{
	public string id;
	public string email;
	public string name;
	public string picture;
}

[System.Serializable]
public class loginResponse{
	public string token;
	public userInfo user;
}

[System.Serializable]
public class recommendation {
	public string id;
	public List<movieInfo> inputMovies;
	public List<movieInfo> outputMovies;
}

[System.Serializable]
public class movieList {
	public List<movieInfo> myList;
}

//{topLikedMovies: [{movie, count}], topSeenMovies: [{movie, count}]}
[System.Serializable]
public class nearByReponse {
	public List<movieWithCount> topLikedMovies;
	public List<movieWithCount> topSeenMovies;
}

[System.Serializable]
public class movieWithCount {
	public movieInfo movie;
	public int count;
}
