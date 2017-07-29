﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class contentHelper : MonoBehaviour {

	public GameObject tagTemplate;
	public int numOfContent;

	const string cachingPathPrefix = "/StreamingAssets/data_";
	const float offset = 210f;
	
	void Start () {
		createTags(numOfContent);		
	}

	movieInfo readFromLocal(int id) {
		string filePath = Application.dataPath + cachingPathPrefix + id.ToString()+".json";
		if (!File.Exists(filePath))
			return null;
		string jsStr;
		jsStr = File.ReadAllText(filePath);
		movieInfo myInfo = JsonUtility.FromJson<movieInfo>(jsStr);
		return myInfo;
	}

	public void createTags(int num) {
		if (num == 0) return;
		//shrink content size
		RectTransform rctParent = tagTemplate.transform.parent.GetComponent<RectTransform>();
		rctParent.sizeDelta = new Vector2(0f, num * offset);
		tagTemplate.SetActive(true);
		tagTemplate.GetComponent<RectTransform>().localPosition =
		new Vector3(450f, -110f);

		tagTemplate.GetComponent<movieTab>().updateUI(readFromLocal(0));

		for (int i = 1; i < num; i++){
			GameObject newTag = Instantiate(tagTemplate);
			newTag.transform.SetParent(transform);
			RectTransform r = newTag.GetComponent<RectTransform>();
			r.localScale = Vector3.one;
			r.localPosition = tagTemplate.GetComponent<RectTransform>().localPosition
								- new Vector3(0f, i*offset);
			newTag.GetComponent<movieTab>().updateUI(readFromLocal(i));
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