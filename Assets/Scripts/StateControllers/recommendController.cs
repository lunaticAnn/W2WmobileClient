using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class recommendController : baseController
{
	//singleton
	public static recommendController instance = null;
	public bool initialized;
	public Sprite getRecommend;
	public Sprite home;
	public Button recommendButton;
	
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		initialized = false;
		recommendButton.image.color = new Color(0f, 0f, 0f);
	}
	//singleton

	public GameObject recommendField;
	public contentHelper recommendHelper;
	//maintain for refreshing status
	

	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		recommendField.SetActive(true);
		recommendHelper.refreshViewedTab();
		recommendButton.image.sprite = getRecommend;
		recommendButton.image.color = new Color(1f, 1f, 1f);
		if (!initialized) {
			infoContainer.instance.updateRecList();
			initialized = true;
		}
			
	}

	public override void inputEventHandler() {
		//get the input event data and parse it to responses		
		infoContainer.instance.updateRecList();
		
	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();
		recommendButton.image.sprite = home;
		recommendButton.image.color = new Color(0f, 0f, 0f);
		recommendField.SetActive(false);
	}

	
}
