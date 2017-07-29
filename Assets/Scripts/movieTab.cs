using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class movieTab : MonoBehaviour {

	public Text movieDetails;
	public RawImage poster;
	public movieInfo myInfo;
	
	public void updateUI(movieInfo m){
		myInfo = m;
		string tmp = "";
		tmp += m.movie_title + "\n";
		tmp += "Director : " + m.director_name + "\n";
		tmp +=  m.title_year;
		movieDetails.text = tmp;
		
		//update listener for the current button
		GetComponent<Button>().onClick.RemoveAllListeners();
		GetComponent<Button>().onClick.AddListener(viewDetails);
		
		IEnumerator c = updatePoster(m.image_url);
		StartCoroutine(c);
	}

	IEnumerator updatePoster(string url){
		// Start a download of the given URL
		WWW www = new WWW(url);
		yield return www;
		poster.texture = www.texture;
	}

	void viewDetails() {
		mainController.instance.details.myInfo = myInfo;
		mainController.instance.changeStateTo(mainController.instance.details, 
		mainController.instance.activeController);		
	}


}
