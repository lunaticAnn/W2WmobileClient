using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class recommendController : baseController
{
	//singleton
	public static recommendController instance = null;
	public bool initialized;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		initialized = false;
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

		if (!initialized) {
			infoContainer.instance.updateRecList();
			initialized = true;
		}
			
	}

	public override void inputEventHandler() {
		//get the input event data and parse it to responses

	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();
		recommendField.SetActive(false);
	}

	
}
