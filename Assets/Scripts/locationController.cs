using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class locationController : baseController
{
	//singleton
	public static locationController instance = null;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	//singleton

	public GameObject locationField;
	public override void enterState(){
		//do the registration here? on the main controller for current state
		base.enterState();
		//switch to location panel
		locationField.SetActive(true);
	}

	public override void inputEventHandler()
	{
		//get the input event data and parse it to responses
		
	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();
		locationField.SetActive(false);		
	}
}
