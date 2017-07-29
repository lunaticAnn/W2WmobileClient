﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class detailController : baseController
{
	//singleton
	public static detailController instance = null;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	//singleton

	public GameObject detailField;
	public movieInfo myInfo;
	public Text descriptions;
	public Text title;
	public RawImage poster;

	public override void enterState()
	{
		//do the registration here? on the main controller for current state
		base.enterState();
		//switch to location panel
		detailField.SetActive(true);
		updateDetails();
	}

	public override void inputEventHandler()
	{
		//get the input event data and parse it to responses

	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();
		detailField.SetActive(false);
	}

	/*-----------------LayOUt--------------------------
	*	movieTitle
	*	titleYear/region
	*	genres                   CONtentRating
	*	duration:				  IMDB-Rating

	Director:
	actor1/actor2/actor3
	description:
	--------------------------------------------*/
	private void updateDetails(){
		title.text = myInfo.movie_title+" ("+ myInfo.title_year.ToString() + ")";
		string tmp = "";
		tmp += myInfo.country + "/" + myInfo.language+"\n";
		tmp += myInfo.genres + "\n";
		tmp += myInfo.content_rating + "\n\n";
		tmp += "<size=40><b>Director</b> " + myInfo.director_name + "</size>\n";
		tmp += "<color=#515151ff>"+myInfo.actor_1_name + "/" 
			 + myInfo.actor_2_name +"/" 
			 + myInfo.actor_3_name+ "</color>\n\n";
		tmp += myInfo.description;
		descriptions.text = tmp;
		IEnumerator c = updatePoster(myInfo.image_url);
		StartCoroutine(c);
	}

	IEnumerator updatePoster(string url){
		// Start a download of the given URL
		WWW www = new WWW(url);
		yield return www;
		poster.texture = www.texture;
	}
}