using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class searchController : baseController {
	//singleton
	public static searchController instance = null;
	public searchBarContent searchBar;

	private void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		//searchButton.onClick.AddListener(addSearchTag);
		confirmSearch.onClick.AddListener(queryNewSearch);
		searchKeywords.onValueChanged.AddListener(searchFullName);
	}
	//singleton
	
	public Button searchButton;
	public GameObject searchPanel;
	public InputField searchKeywords;
	public Button confirmSearch;
	public GameObject searchTab;
	public Transform tabPanel;

	public Sprite exitSearch;
	public Sprite enterSearch;

	List<movieInfo> searchTags = new List<movieInfo>();

	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		//open the search panel
		searchPanel.SetActive(true);
		searchKeywords.text = "";
		searchTags.Clear();
		clearSearchPanel();
		mainController.instance.startNewSearch.image.sprite = exitSearch;
		searchBar.updateBarContent(new List<movieInfo>());
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
		//update the search tag list
		GameObject s = Instantiate(searchTab);
		s.transform.SetParent(tabPanel, false);
		RectTransform r = searchTab.GetComponent<RectTransform>();
		r.localScale = Vector3.one;
		s.GetComponent<Button>().onClick.AddListener(delegate { removeFromTags(m ,s); });
		s.GetComponentInChildren<Text>().text = m.movie_title;
		searchTags.Add(m);
		updateTagArea();
		searchKeywords.text = "";			
	}

	void updateTagArea() {
		//display the search tags in the tag area				
		for (int i = 0; i < searchTags.Count; i++) {
			tabPanel.GetChild(i).GetComponent<RectTransform>().localPosition =
			new Vector3(0f, 180f - 50 * i);
		}
		
	}

	void clearSearchPanel() {
		for (int i = 0; i < tabPanel.childCount; i++)
			Destroy(tabPanel.GetChild(i).gameObject);
	}

	void removeFromTags(movieInfo m, GameObject s) {
		searchTags.Remove(m);
		Destroy(s);
	}

	#region serverCommunication
	void searchFullName(string inputKeyword) {
		infoContainer.instance.sendSearchQuery(inputKeyword, 5);		
	}
	
	void queryNewSearch() {
		//send searching query
		StartCoroutine("newSearch");		
		//================== testing like / dislike ============================	
	}

	IEnumerator newSearch() { 
		yield return mainController.instance.recommend.recommendHelper.clearTags();
		infoContainer.instance.updateRecList(searchTags,10);
		mainController.instance.changeStateTo(mainController.instance.recommend,
											  mainController.instance.activeController);
		Debug.Log("creating search tags:" + searchTags.Count);
		//mainController.instance.recommend.recommendHelper.createTags(searchTags.Count, searchTags);
	}
	#endregion
}
