using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class distChanged : MonoBehaviour, IPointerUpHandler {
	Slider dist;

	public void OnPointerUp(PointerEventData e) {
		dist = GetComponent<Slider>();
		locationController.instance.inputEventHandler();
	}
}
