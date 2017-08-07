using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class locationController : baseController {

	//singleton
	public static locationController instance = null;
	public static float maxDistance = 20f, minDistance = 1f;

	public Slider dist;
	public Text distNum;
	private void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		locationHelper.clearTags();
		dist.onValueChanged.AddListener(updateDistNum);
		initialized = false;
	}
	//singleton

	public GameObject locationField;
	public contentHelper locationHelper;
	public bool initialized;

	public override void enterState() {
		//do the registration here? on the main controller for current state
		base.enterState();
		//switch to location panel
		locationField.SetActive(true);
		locationHelper.refreshViewedTab();
		updateDistNum(0f);
		if (!initialized) {
			initialized = true;
			infoContainer.instance.getNearbyTrending(sliderToDist(), 10);
		}
	}

	public override void inputEventHandler() {
		//get the input event data and parse it to responses
		locationHelper.clearTags();
		infoContainer.instance.getNearbyTrending(sliderToDist(), 10);
	}

	public override void exitState()
	{
		//de-register on the main controller
		base.exitState();
		locationField.SetActive(false);
	}

	public static float sliderToDist() {
		float d = instance.dist.value * maxDistance + (1-instance.dist.value) * minDistance;
		return d;
	}

	private void updateDistNum(float f){
		distNum.text = sliderToDist().ToString("N1")+" miles";
	}


}
