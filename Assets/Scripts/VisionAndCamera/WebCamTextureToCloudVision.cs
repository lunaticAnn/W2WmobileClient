using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WebCamTextureToCloudVision : MonoBehaviour {
	string url = "https://westus.api.cognitive.microsoft.com/vision/v1.0/models/celebrities/analyze?model=celebrities";
	string apiKey = "f77da84c816141998a1134aab3ec09c4";
	public float captureIntervalSeconds = 1f;
	public int requestedWidth = 480;
	public int requestedHeight = 640;
	public int maxResults = 10;
	public Button takePhotoButton;
	public Button retakeButton;
	public Button searchForIt;

	WebCamTexture webcamTexture;
	Texture2D texture2D;
	Dictionary<string, string> headers;
	string actorName;
	

	// Use this for initialization
	void Awake () {
		takePhotoButton.onClick.AddListener(takePhoto);
		retakeButton.onClick.AddListener(retakePhoto);
		searchForIt.onClick.AddListener(searchForActor);
		headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/octet-stream");
		headers.Add("Ocp-Apim-Subscription-Key", apiKey);

		WebCamDevice[] devices = WebCamTexture.devices;
		for (var i = 0; i < devices.Length; i++) {
			Debug.Log (devices [i].name);
		}
		if (devices.Length > 0) {
			webcamTexture = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight);
			RawImage r = GetComponent<RawImage> ();
			if (r != null) {
				r.texture = webcamTexture;
			}
			webcamTexture.Play();			
		}
		retakeButton.gameObject.SetActive(false);
		searchForIt.gameObject.SetActive(false);
	}

	public void Initialize(){
		takePhotoButton.gameObject.SetActive(true);
		retakeButton.gameObject.SetActive(false);
		searchForIt.gameObject.SetActive(false);
		webcamTexture.Play();
	}

	void takePhoto() {
		StartCoroutine("Capture");
		webcamTexture.Pause();
	}

	void retakePhoto() {
		searchForIt.gameObject.SetActive(false);
		retakeButton.gameObject.SetActive(false);
		webcamTexture.Play();
		takePhotoButton.gameObject.SetActive(true);
	}

	private IEnumerator Capture() {
		takePhotoButton.gameObject.SetActive(false);
		Color[] pixels = webcamTexture.GetPixels();
		if (pixels.Length == 0)
			yield return null;
		if (texture2D == null || webcamTexture.width != texture2D.width || webcamTexture.height != texture2D.height) {
			texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
		}

		texture2D.SetPixels(pixels);
	
		byte[] jpg = texture2D.EncodeToJPG();
						
		using(WWW www = new WWW(url, jpg, headers)) {
			yield return www;
			if (www.error == null || www.error == ""){
				//a celebrity is found
				if (www.text.Contains("\"name\":")) {
					int i = www.text.IndexOf("\"name\":");
					i += 8;
					string tmp = "";
					while(www.text[i] != '"') {
						tmp += www.text[i];
						i++;
					}
					actorName = tmp;
					searchForIt.gameObject.SetActive(true);
					searchForIt.transform.GetChild(0).GetComponent<Text>().text = actorName;		
				}				
			}
			else
				Debug.Log("Error: " + www.error);					
		}
		yield return new WaitForEndOfFrame();	
		retakeButton.gameObject.SetActive(true);
	}
	//set my self inactive
	//use search controller's api
	void searchForActor() {
		searchController.instance.searchForActor(actorName);
	}

}
