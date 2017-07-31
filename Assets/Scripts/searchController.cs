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
		searchButton.onClick.AddListener(addSearchTag);
		confirmSearch.onClick.AddListener(queryNewSearch);
	}
	//singleton
	
	public Button searchButton;
	public GameObject searchPanel;
	public InputField searchKeywords;
	public Text tagArea;
	public Button confirmSearch;

	public Sprite exitSearch;
	public Sprite enterSearch;

	LimitedQueue<string> searchTags = new LimitedQueue<string>(5);

	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		//open the search panel
		searchPanel.SetActive(true);
		searchKeywords.text = "";
		tagArea.text = "Tell us your favorite movies!";
		searchTags.Clear();
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
	
/// <summary>
/// return true if there is eviction
/// </summary>
/// <returns></returns>
	void addSearchTag() {
		string searchContent = searchKeywords.text;
		searchTags.Enqueue(searchFullName(searchContent));
		updateTagArea();			
	}

	void updateTagArea() {
		//display the search tags in the tag area
		tagArea.text = "<b>";
		for (int i = 0; i < searchTags.currentSize(); i++) {
			string tag = searchTags.Dequeue();
			tagArea.text += tag + "\n";
			searchTags.Enqueue(tag);
		}
		tagArea.text += "</b>";
	}

	#region serverCommunication
	string searchFullName(string inputKeyword) {
		//ask for auto filling content
		return inputKeyword;
	}
	
	void queryNewSearch() {
		string searchContent = tagArea.text.Substring(3,tagArea.text.Length - 7);
	
		developerLogs.log("Query new search with keywords:\n"+searchContent);
		developerLogs.log("refreshing the content:" + searchContent);
		mainController.instance.changeStateTo(mainController.instance.recommend,
											  mainController.instance.activeController);
		mainController.instance.recommend.recommendHelper.createTags(5);
		developerLogs.log("sending input location:" + Input.location.lastData.longitude + ","
						  + Input.location.lastData.latitude);
	}
	#endregion
}
