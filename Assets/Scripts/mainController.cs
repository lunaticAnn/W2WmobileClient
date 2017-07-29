﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainController : MonoBehaviour {

	public static mainController instance = null;
	public enum mainState { Recommendations, newSearch, nearbyTrending };
	public mainState currentState;

	//registration
	public baseController activeController;
	public baseController previousController;

	public searchController search;
	public recommendController recommend;
	public locationController location;

	#region buttons
	public Button startNewSearch;
	public Button viewRecommendation;
	public Button viewNearby;
	#endregion
	
	//my event info structure

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		currentState = mainState.Recommendations;
		changeStateTo(recommend);
	}
 
	bool changeStateTo(baseController toActivate, baseController from = null) {
		if (from){
			from.exitState();
			previousController = from;
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

}