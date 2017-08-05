using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class searchKeywordTab : MonoBehaviour,IPointerDownHandler, IPointerUpHandler {
	public movieInfo m;
	public void OnPointerDown(PointerEventData e) {
		searchController.instance.smallerDetailPanel.SetActive(true);
		searchController.instance.updateDetailInfo(m);	
	}

	public void OnPointerUp(PointerEventData e) {
		searchController.instance.smallerDetailPanel.SetActive(false);
	}
	
}
