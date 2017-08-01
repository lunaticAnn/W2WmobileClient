﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainController : MonoBehaviour {

	public static mainController instance = null;

	//registration
	public baseController activeController;
	public baseController previousController;

	public searchController search;
	public recommendController recommend;
	public locationController location;
	public detailController details;
	public userInfoController user;

	#region buttons
	public Button startNewSearch;
	public Button viewRecommendation;
	public Button viewNearby;
	public Button backToList;
	#endregion

	//my event info structure

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		changeStateTo(recommend);

		//initialize recommendation field
		infoContainer.instance.sendSearchQuery("", 5, 1);
	}
 
	public bool changeStateTo(baseController toActivate, baseController from = null) {
		if (from){
			from.exitState();
			previousController = from;
		}
		if (!toActivate) {
			Debug.LogWarning("No previous state found, check the FSM.");
			return false;
		}
		toActivate.enterState();
		return true;
	}

	/*when the current state is recommendations, the ContentLoader will load 
		recommendation result as a heap/queue? and will parse it to the main 
		content viewport.		
	*/
	private void Start(){
		installHandlers();
	}

	void installHandlers() {
		startNewSearch.onClick.AddListener(startSearchHandler);
		viewRecommendation.onClick.AddListener(viewRecommendHandler);
		viewNearby.onClick.AddListener(viewLocationHandler);
		backToList.onClick.AddListener(delegate { changeStateTo(previousController, activeController); });
	}

	private void startSearchHandler() {
		if (activeController != search)
			changeStateTo(search, activeController);
		else
			changeStateTo(previousController, activeController);
	}

	private void viewRecommendHandler(){
		if (activeController != search)
			changeStateTo(recommend, activeController);
		else
			Debug.Log("Is in search mode, other operations are banned.");
	}

	private void viewLocationHandler(){
		if (activeController != search)
			changeStateTo(location, activeController);
		else
			Debug.Log("Is in search mode, other operations are banned.");
	}

	#region server connections
	

	#endregion
}
