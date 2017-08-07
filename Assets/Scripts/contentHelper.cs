using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class contentHelper : MonoBehaviour {

	public GameObject tagTemplate;
	public int numOfContent;
	public movieTab toBeRefresh;

	const float offset = 210f;
	const int leastChildCount = 5;
	const int animFrames = 10;

	public void refreshViewedTab() {
		if (toBeRefresh == null) return;
		toBeRefresh.processRefresh();
	}

	movieInfo readFromLocal(int id) {
		TextAsset jsFile =Resources.Load(id.ToString()) as TextAsset;
		string jsStr = jsFile.text;
		movieInfo myInfo = JsonUtility.FromJson<movieInfo>(jsStr);
		return myInfo;
	}
	
	public void createTags(int num, List<movieInfo> targetInfo, bool customize = false) {
		Debug.Log("get create tag request, for :" + num);
		if (num == 0) return;
		//shrink content size
		int cCount = transform.childCount;
		RectTransform rctParent = GetComponent<RectTransform>();
		rctParent.sizeDelta = new Vector2(0f, (num + cCount) * offset);
		tagTemplate.SetActive(true);
		tagTemplate.GetComponent<RectTransform>().localPosition =
		new Vector3(450f, -110f);
		
		if (transform.childCount == 1){
			tagTemplate.GetComponent<movieTab>().updateUI(targetInfo[0], customize);
			cCount = 0;
		}

		GameObject newTag;
		for (int i = 1; i < num + cCount; i++){
			if (i >= cCount){
				newTag = Instantiate(tagTemplate);
				newTag.transform.SetParent(transform);
				newTag.GetComponent<movieTab>().myIndex = i;
				//Debug.Log("creating:"+i);
				newTag.GetComponent<movieTab>().updateUI(targetInfo[i - cCount], customize);				
				newTag.name = "tab0";				
			}
			else {
				newTag = transform.GetChild(i).gameObject;
			}
			RectTransform r = newTag.GetComponent<RectTransform>();
			r.localScale = Vector3.one;
			r.localPosition = tagTemplate.GetComponent<RectTransform>().localPosition
								- new Vector3(0f, i*offset);
			
		}			
	}

	public bool clearTags() {
		int i = 1;
		while (i < transform.childCount) {
			Destroy(transform.GetChild(i).gameObject);
			i++;
		}
		transform.GetChild(0).GetComponent<Image>().sprite = mainController.instance.customizeBackground[0];
		transform.GetChild(0).gameObject.SetActive(false);
		return true;
	}

/// <summary>
/// remove the tab that is at certain index
/// </summary>
/// <param name="index"></param>
	public void removeTab(int index) {
		IEnumerator c = removeAnimation(index);
		StartCoroutine(c);
	}

	IEnumerator removeAnimation(int index) {
		//if there is only one tab, set it inactive?
		if (index >= transform.childCount) {
			yield break;
		}

		if (transform.childCount == 1 && index == 0){
			//set inactive
			transform.GetChild(0).gameObject.SetActive(false);
			yield break;
		}
		//update index of following tabs;
		for (int i = index + 1; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<movieTab>().myIndex--;
		}
		Destroy(transform.GetChild(index).gameObject);
		//update template if it is deleted 
		if (index == 0) tagTemplate = transform.GetChild(1).gameObject;
		yield return new WaitForEndOfFrame();

		//2s animation?
		int frameCounter = animFrames;
		while (frameCounter > 0) {
			for (int i = index; i < transform.childCount; i++){
				RectTransform rc =transform.GetChild(i).GetComponent<RectTransform>();
				rc.localPosition += new Vector3(0f,offset/animFrames);				
			}
			frameCounter--;
			yield return new WaitForEndOfFrame();
		}

		if (transform.childCount < leastChildCount){
			Debug.Log("..");
		}	
	}
	
}
