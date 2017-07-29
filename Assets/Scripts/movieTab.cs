using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class movieTab : MonoBehaviour {
/*-------------------------------------------
*	movieTitle
*	titleYear/region
*	genres                   CONtentRating
*	duration:				  IMDB-Rating

Director:
description:
actor1/actor2/actor3
--------------------------------------------*/
	public Text movieDetails;
	public RawImage poster;
	string cachingPath = "/StreamingAssets/data_0.json";

	private void Start(){
		updateMovieContent();
	}

	
	public bool updateMovieContent () {
		string filePath = Application.dataPath + cachingPath;
		string jsStr;
		if (!File.Exists(filePath))
			return false;
		jsStr = File.ReadAllText(filePath);
		movieInfo myInfo = JsonUtility.FromJson<movieInfo>(jsStr);
		updateUI(myInfo);
		return true;
	}

	private void updateUI(movieInfo m){
		string tmp = "";
		tmp +="Title:" + m.movie_title + "\n";
		tmp += "Director:" + m.director_name + "\n";
		tmp += "Year:" + m.title_year;
		movieDetails.text = tmp;
	}


}
