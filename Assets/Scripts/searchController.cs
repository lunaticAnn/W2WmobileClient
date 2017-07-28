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
	Queue<string> keywords;

	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		//open the search panel
		searchPanel.SetActive(true);
		searchKeywords.text = "";
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
		//close the search panel 	
	}

	#region serverCommunication
	void queryNewSearch() {
		string searchContent = searchKeywords.text;
		Debug.LogWarning("Query new search with keywords:"+searchContent);
	}
	#endregion
}
