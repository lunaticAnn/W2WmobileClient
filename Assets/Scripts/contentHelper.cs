using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contentHelper : MonoBehaviour {

	public GameObject tagTemplate;
	public int numOfContent;

	const float offset = 210f;
	
	void Start () {
		createTags(numOfContent);
		clearTags();	
	}

	public void createTags(int num) {
		if (num == 0) return;
		tagTemplate.SetActive(true);
		for (int i = 1; i < num; i++){
			GameObject newTag = Instantiate(tagTemplate);
			newTag.transform.SetParent(transform);
			RectTransform r = newTag.GetComponent<RectTransform>();
			r.localScale = Vector3.one;
			r.localPosition = tagTemplate.GetComponent<RectTransform>().localPosition
								- new Vector3(0f, i*offset);				
		}			
	}

	public void clearTags() {
		int i = 1;
		while (i < transform.childCount) {
			Destroy(transform.GetChild(i).gameObject);
			i++;
		}
		transform.GetChild(0).gameObject.SetActive(false);	
	}
	
	
}
