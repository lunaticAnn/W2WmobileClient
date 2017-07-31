using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoContainer : MonoBehaviour {
	public static infoContainer instance = null;
	
	public string userId;
	
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
	
	public static bool inFavorites(string target) {
		return instance.favs.Contains(target);	
	}

	public static void addToMyFav(movieInfo target) {
		instance.favs.Add(target.movie_title);
		developerLogs.log("send to server add target");
	}

	public static void removeFromFav(movieInfo target) {
		instance.favs.Remove(target.movie_title);
		developerLogs.log("send to server remove target");
	}

	public static void addToDislike(movieInfo target){
		instance.dislikes.Add(target.movie_title);
		developerLogs.log("send to server dislike target");
	}

	public static bool isDislike(movieInfo target){
		return instance.dislikes.Contains(target.movie_title);		
	}

	public static void removeFromDislike(movieInfo target) {
		instance.dislikes.Remove(target.movie_title);
		developerLogs.log("send to server remove target");
	}


	private void updateHashSet(HashSet<string> targetList, List<movieInfo> requestList) {
		foreach (movieInfo mi in requestList)
			targetList.Add(mi.movie_title);
	} 
	
}
