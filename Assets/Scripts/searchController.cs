using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class searchController : baseController {
	//singleton
	public static searchController instance = null;
	private void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		searchButton.onClick.AddListener(queryNewSearch);
		
	}
	//singleton
	public Button searchButton;
	public GameObject searchPanel;
	public InputField searchKeywords;
	public Sprite exitSearch;
	public Sprite enterSearch;

	LimitedQueue<string> searchTags = new LimitedQueue<string>(10);

	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		//open the search panel
		searchPanel.SetActive(true);
		searchKeywords.text = "";
		mainController.instance.startNewSearch.image.sprite = exitSearch;
		//keywords.Clear();
		
	}

	public override void inputEventHandler()
	{
		//get the input event data and parse it to responses
		

	}

	public override void exitState(){
		//de-register on the main controller
		base.exitState();
		searchPanel.SetActive(false);
		mainController.instance.startNewSearch.image.sprite = enterSearch;
		//close the search panel 	
	}

	#region serverCommunication
	void queryNewSearch() {
		string searchContent = searchKeywords.text;
		developerLogs.log("Query new search with keywords:"+searchContent);
		developerLogs.log("refreshing the content:" + searchContent);
		mainController.instance.changeStateTo(mainController.instance.recommend,
											  mainController.instance.activeController);
		mainController.instance.recommend.recommendHelper.clearTags();
		mainController.instance.recommend.recommendHelper.createTags(10);
		developerLogs.log("sending input location:" + Input.location.lastData.longitude + ","
						  + Input.location.lastData.latitude);
	}
	#endregion
}
