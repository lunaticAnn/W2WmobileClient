using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoContainer : MonoBehaviour {
	public static infoContainer instance = null;
	
	private HashSet<string> favs;

	void Awake () {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		favs = new HashSet<string>();
	}
	
	public bool inFavorites(string target) {
		return favs.Contains(target);	
	}

	public void addToMyFav(movieInfo target) {
		favs.Add(target.movie_title);
		developerLogs.log("send to server add target");
	}

	public void removeFromFav(movieInfo target) {
		favs.Remove(target.movie_title);
		developerLogs.log("send to server remove target");
	}

	
	private void updateHashSet(List<movieInfo> userFavorites) {
		foreach (movieInfo mi in userFavorites)
			favs.Add(mi.movie_title);
	} 
	
}
