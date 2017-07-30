﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class movieTab : MonoBehaviour {

	public Text movieDetails;
	public RawImage poster;
	public movieInfo myInfo;

	public Button likeButton;
	public Button deleteThisTab;

	//to help adjust margin when remove
	public int myIndex;

	public void updateUI(movieInfo m){
		myInfo = m;
		string tmp = "";
		tmp += m.movie_title + "\n";
		tmp += "Director : " + m.director_name + "\n";
		tmp +=  m.title_year;
		movieDetails.text = tmp;

		if (infoContainer.instance.inFavorites(myInfo.movie_title))
			likeButton.image.color = Color.red;
		else
			likeButton.image.color = Color.grey;

		// update listener for the current button
		GetComponent<Button>().onClick.RemoveAllListeners();
		GetComponent<Button>().onClick.AddListener(viewDetails);
		likeButton.onClick.AddListener(likeThisOne);
		deleteThisTab.onClick.AddListener(deleteThis);
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

	void likeThisOne() {
		if (!infoContainer.instance.inFavorites(myInfo.movie_title)){
			//add to the list
			infoContainer.instance.addToMyFav(myInfo);
			//change sprite to red 
			developerLogs.log("send with location"+ Input.location.lastData.longitude+
								","+ Input.location.lastData.latitude);
			likeButton.image.color = Color.red;
		}
		else{
			infoContainer.instance.removeFromFav(myInfo);

			developerLogs.log("send with location" + Input.location.lastData.longitude +
											"," + Input.location.lastData.latitude);
			//change the sprite back to grey
			likeButton.image.color = Color.grey;
		}
	}

	void deleteThis() {
		developerLogs.log("send dislike list");
		developerLogs.log("send with location" + Input.location.lastData.longitude +
													"," + Input.location.lastData.latitude);
		transform.parent.GetComponent<contentHelper>().removeTab(myIndex);
	}
}
