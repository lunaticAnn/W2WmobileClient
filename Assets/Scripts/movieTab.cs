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
	public movieInfo myInfo;
	
	public void updateUI(movieInfo m){
		string tmp = "";
		tmp += m.movie_title + "\n";
		tmp += "Director : " + m.director_name + "\n";
		tmp +=  m.title_year;
		movieDetails.text = tmp;
		IEnumerator c = updatePoster(m.image_url);
		StartCoroutine(c);
	}

	IEnumerator updatePoster(string url)
	{
		// Start a download of the given URL
		WWW www = new WWW(url);
		yield return www;
		poster.texture = www.texture;
	}


}
