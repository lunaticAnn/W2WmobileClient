using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*open detail panel if searched*/
public class searchController : baseController {
	//singleton
	public static searchController instance = null;
	public searchBarContent searchBar;
	public GameObject smallerDetailPanel;

	private void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		searchButton.onClick.AddListener(startPhotoSearch);
		confirmSearch.onClick.AddListener(queryNewSearch);
		searchKeywords.onValueChanged.AddListener(searchFullName);
	}
	//singleton
	
	public Button searchButton;
	public GameObject searchPanel;
	public InputField searchKeywords;
	public Button confirmSearch;
	public GameObject searchTab;
	public GameObject visionPanel;

	public Sprite exitSearch;
	public Sprite enterSearch;
	public bool synchronizer = true;

	List<movieInfo> searchTags = new List<movieInfo>();

	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		//open the search panel
		searchPanel.SetActive(true);
		searchKeywords.text = "";
		searchTags.Clear();
		//clearSearchPanel();
		mainController.instance.startNewSearch.image.sprite = exitSearch;
		searchBar.updateBarContent(new List<movieInfo>());
		visionPanel.SetActive(false);
	}

	public override void inputEventHandler(){
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
	public void addSearchTag(movieInfo m) {
	
		Debug.Log("searchTags:" + searchTags.Count);
		searchKeywords.text = m.movie_title;			
	}

	public void storeResult(List<movieInfo> toStore) {
		searchTags = toStore;
	}



	void startPhotoSearch() {
		visionPanel.SetActive(true);
		visionPanel.transform.GetChild(0).GetComponent<WebCamTextureToCloudVision>().Initialize();
	}
	
	void removeFromTags(movieInfo m, GameObject s) {
		searchTags.Remove(m);
		Destroy(s);
	}

	#region serverCommunication
	void searchFullName(string inputKeyword) {
		infoContainer.instance.sendSearchQuery(inputKeyword, 10, "title");		
	}

	public void searchForActor(string name) {
		synchronizer = false;
		infoContainer.instance.sendSearchQuery(name, 10, "actor");
		StartCoroutine("newSearch");
		mainController.instance.recommend.initialized = true;
	}
	
	void queryNewSearch() {
		mainController.instance.recommend.initialized = true;
		//send searching query
		StartCoroutine("newSearch");
		//================== testing like / dislike ============================		
	}

	

	IEnumerator newSearch() {
		mainController.instance.changeStateTo(mainController.instance.recommend,
											  mainController.instance.activeController);
		yield return mainController.instance.recommend.recommendHelper.clearTags();
		while (!synchronizer) {
			yield return new WaitForEndOfFrame();
		}
		mainController.instance.recommend.recommendHelper.createTags(searchTags.Count, searchTags);
	}


	#endregion
}
