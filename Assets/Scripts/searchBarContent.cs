using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class searchBarContent : MonoBehaviour {

	// Update is called once per frame
	public void updateBarContent (List<movieInfo> searchResult) {
		//search result 
		int maxIndex = searchResult.Count < 3 ? searchResult.Count - 1 : 2;
		for (int i = 0; i < 3; i++) {
			if (i <= maxIndex){
				transform.GetChild(i).gameObject.SetActive(true);
				Button res = transform.GetChild(i).GetComponent<Button>();
				res.GetComponentInChildren<Text>().text = searchResult[i].movie_title;
				res.onClick.RemoveAllListeners();
				movieInfo m = searchResult[i];
				res.onClick.AddListener(delegate { chooseMovie(m); });
			}
			else
				transform.GetChild(i).gameObject.SetActive(false);
		}
		searchController.instance.storeResult(searchResult);
	}


	void chooseMovie(movieInfo m) {
		searchController.instance.addSearchTag(m);
	}
}
