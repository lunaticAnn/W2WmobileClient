using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class developerLogs : MonoBehaviour {
	//single
	public static developerLogs instance = null;
	LimitedQueue<string> myHistory = new LimitedQueue<string>(5);
	
	private void Awake(){
		if (!Debug.isDebugBuild) 
			DestroyImmediate(gameObject);
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);		
	}

	// Update is called once per frame
	public static void log(string logContent) {
		instance.myHistory.Enqueue(logContent);				
		Text t = instance.transform.GetChild(0).GetChild(0).GetComponent<Text>();
		t.text = "";
		for (int i = 0; i < instance.myHistory.currentSize(); i++){
			string last = instance.myHistory.Dequeue();
			t.text += last + "\n";
			instance.myHistory.Enqueue(last);
		}	
	}
}
