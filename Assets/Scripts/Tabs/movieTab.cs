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

	public void processRefresh(){
		if (infoContainer.isDislike(myInfo)) {
			deleteThis();
			return;
		}
		if (infoContainer.inFavorites(myInfo))
			likeButton.image.color = Color.red;
		else
			likeButton.image.color = Color.grey;
	}


	public void updateUI(movieInfo m, bool customize = false){
		myInfo = m;
		string tmp = "";
		tmp += "<size=34>" + m.movie_title +"</size>" + " <size=28>("+m.title_year+")</size>\n";
		tmp += "<color=#515151ff>" + m.director_name + "</color>\n";
		int len = 0, i = 0;
		while(i < 20) {
			i = m.genres.IndexOf("|",i+1);
			if (i == -1) {
				len = m.genres.Length;
				break; }
			len = i;	
		}
		tmp += m.genres.Substring(0,len) + "\n";
		if(customize)
			tmp +=  "<size=24><color=#a6a6a6ff>"+m.resultMemo+"</color></size>";
		movieDetails.text = tmp;

		if (infoContainer.inFavorites(myInfo))
			likeButton.image.color = Color.red;
		else
			likeButton.image.color = Color.grey;

		// update listener for the current button
		GetComponent<Button>().onClick.RemoveAllListeners();
		GetComponent<Button>().onClick.AddListener(viewDetails);
		likeButton.onClick.RemoveAllListeners();
		likeButton.onClick.AddListener(likeThisOne);
		deleteThisTab.onClick.RemoveAllListeners();
		deleteThisTab.onClick.AddListener(deleteThis);
		
		if (customize) {
			switch (m.resultFrom) {
				case "recommend":
					GetComponent<Image>().sprite = mainController.instance.customizeBackground[1];
					break;
				case "location":
					GetComponent<Image>().sprite = mainController.instance.customizeBackground[2];
					break;
				case "actor":
					GetComponent<Image>().sprite = mainController.instance.customizeBackground[3];
					break;
				case "title":
					GetComponent<Image>().sprite = mainController.instance.customizeBackground[4];
					break;
				default:
					GetComponent<Image>().sprite = mainController.instance.customizeBackground[0];
					break;					
			}
		}
		//update the poster
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
		//set myself to the toBeRefresh
		transform.parent.GetComponent<contentHelper>().toBeRefresh = this;	
		mainController.instance.details.myInfo = myInfo;
		mainController.instance.changeStateTo(mainController.instance.details, 
		mainController.instance.activeController);		
	}

	void likeThisOne() {
		if (!infoContainer.inFavorites(myInfo)){
			//add to the list
			infoContainer.addToMyFav(myInfo);
			//change sprite to red 			
			likeButton.image.color = Color.red;
		}
		else{
			infoContainer.removeFromFav(myInfo);
			//change the sprite back to grey
			likeButton.image.color = Color.grey;
		}
	}

	void deleteThis() {
		infoContainer.addToDislike(myInfo);
		transform.parent.GetComponent<contentHelper>().removeTab(myIndex);
	}

}
