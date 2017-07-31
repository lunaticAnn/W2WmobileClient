using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userInfoController : baseController
{
	public Text welcome;
		
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
	}

	public override void enterState(){
		base.enterState();
	
	}

	public override void inputEventHandler(){
		//get the input event data and parse it to responses
		
	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();	
	}
}
