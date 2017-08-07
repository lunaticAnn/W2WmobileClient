using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userInfoController : baseController
{
	public Text welcome;
	public GameObject userInfoPanel;
	public contentHelper userContent;
	public bool synchronizer = false;
		
	//singleton
	public static userInfoController instance = null;
	private void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);		
	}
	//singleton

	private void Start(){
		welcome.text = "Welcome, " + infoContainer.instance.usrInfo.name;
		mainController.instance.viewUserInfo.transform.GetChild(0).GetComponent<Text>().text
		= infoContainer.instance.usrInfo.name;
		userInfoPanel.SetActive(true);
	}

	public override void enterState(){
		userInfoPanel.SetActive(true);
		base.enterState();
		synchronizer = false;
		infoContainer.instance.initLists();
		StartCoroutine("viewLikedList");
	}

	IEnumerator viewLikedList() {
		yield return userContent.clearTags();
		while(!synchronizer) {
			yield return new WaitForEndOfFrame();	
		}
		userContent.createTags(infoContainer.instance.localLikeList.Count,
								 infoContainer.instance.localLikeList);
	}

	public override void inputEventHandler(){
		//get the input event data and parse it to responses
		
	}

	public override void exitState()
	{
		//de-register on the main controller
		userInfoPanel.SetActive(false);
		base.exitState();	
	}
}
