using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class distChanged : MonoBehaviour, IPointerUpHandler {
	Slider dist;

	public void OnPointerUp(PointerEventData e) {
		dist = GetComponent<Slider>();
		infoContainer.instance.getNearbyTrending(locationController.sliderToDist(), 5);
	}
}
