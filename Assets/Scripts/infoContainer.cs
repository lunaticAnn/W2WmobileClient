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
		developerLogs.log("send to server add target");
	}

	public static void removeFromFav(movieInfo target) {
		instance.favs.Remove(target.id);
		developerLogs.log("send to server remove target");
	}

	public static void addToDislike(movieInfo target){
		instance.dislikes.Add(target.id);
		developerLogs.log("send to server dislike target");
	}

	public static bool isDislike(movieInfo target){
		return instance.dislikes.Contains(target.id);		
	}

	public static void removeFromDislike(movieInfo target) {
		instance.dislikes.Remove(target.id);
		developerLogs.log("send to server remove target");
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

	public void updateRecList(List<movieInfo> movieIds, int n = 5) {
		WWWForm form = new WWWForm();
		for (int i = 0; i < movieIds.Count; i++) {
			form.AddField("movieIds", movieIds[i].id);
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
			mainController.instance.recommend.recommendHelper.createTags(recommendList.Count, recommendList);
		}
		else
			Debug.LogWarning(w.error);
	}

	public void sendSearchQuery(string s, int n){
		IEnumerator c = searchMoviesByTitle(s, n);
		StartCoroutine(c);
	}

	IEnumerator searchMoviesByTitle(string s, int n){
		Dictionary<string, string> header = new Dictionary<string, string>();
		header["Authorization"] = "Bearer " + token;
		
		WWW w = new WWW(server + parseSearchQuery(s,n), null, header);
		yield return w;
		if (w.error == ""){
			movieList result = JsonUtility.FromJson<movieList>("{\"myList\":"+w.text+"}");
			searchResult = result.myList;
			searchController.instance.searchBar.updateBarContent(searchResult);
		}
		else
			Debug.LogWarning(w.error);
	}

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
			Debug.Log(w.text);
			movieList result = JsonUtility.FromJson<movieList>("{\"myList\":" + w.text + "}");
			updateHashSet(targetList, result.myList);
		}
		else
			Debug.LogWarning(w.error);
	}

	string parseSearchQuery(string s, int n) {
		return "/movies?q=" + s + "&n=" + n.ToString();
	}

	string parseListUpdateQuery(string listName){
		return "/users/"+ usrInfo.id + "/movies/"+listName;
	}
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
