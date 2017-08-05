using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WebCamTextureToCloudVision : MonoBehaviour {
	string url = "https://westus.api.cognitive.microsoft.com/vision/v1.0/models/celebrities/analyze?model=celebrities";
	string apiKey = "f77da84c816141998a1134aab3ec09c4";
	public float captureIntervalSeconds = 3f;
	public int requestedWidth = 480;
	public int requestedHeight = 640;
	public int maxResults = 10;

	WebCamTexture webcamTexture;
	Texture2D texture2D;
	Dictionary<string, string> headers;
	bool locker = false;
	
	

	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)&&!locker) {
			locker = true;
			StartCoroutine("Capture");
			webcamTexture.Pause();
		}
	}

	private IEnumerator Capture() {
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
			if (www.error == null||www.error == "") 
				Debug.Log(www.text.Replace("\n", "").Replace(" ", ""));						
			else 
				Debug.Log("Error: " + www.error);					
		}
		yield return new WaitForSeconds(captureIntervalSeconds);
		locker = false;
		webcamTexture.Play();			
	}


}
