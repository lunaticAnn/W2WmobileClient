using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour {
	public GameObject[] StartScreenPanels;
	

	int serverResponse = 0;
	bool locker = false; 

	enum logInType { native, google, facebook};
	logInType currentLogType = logInType.native;
	/* 0 .startPanel structure:
		child index | functionality
			0		| login
			1		| sign up
	==========================*/

	/*1. LoginPanel structure:
			child index | functionality
				0		| username
				1		| password
				2		| login
				3		| back
			   *4		| login with facebook
			   *5		| login with google
		==========================*/

	/* 2.SignupPanel structure:
			child index | functionality
				0		| username
				1		| password
				2		| sign up
				3		| back
		==========================*/
	public InputField loginUserName;
	public InputField loginPwd;
	public InputField signUpUserName;
	public InputField signUpPwd;
	public InputField signUpName;
	public Text errorMessage;
	public InputField confirmPassword;


	Button signUp;
	Button signUpBack;
	Button logIn;
	Button logInBack;
	Button signUpSubmission;
	Button logInSubmission;

	void Start () {
		logIn = StartScreenPanels[0].transform.GetChild(0).GetComponent<Button>();
		signUp = StartScreenPanels[0].transform.GetChild(1).GetComponent<Button>();
		logInBack = StartScreenPanels[1].transform.GetChild(3).GetComponent<Button>();
		signUpBack = StartScreenPanels[2].transform.GetChild(3).GetComponent<Button>();
		logInSubmission  = StartScreenPanels[1].transform.GetChild(2).GetComponent<Button>();
		signUpSubmission = StartScreenPanels[2].transform.GetChild(2).GetComponent<Button>();

		clearPanels();
		ChangePanel(0);
		logIn.onClick.AddListener(delegate { ChangePanel(1); });
		signUp.onClick.AddListener(delegate { ChangePanel(2); });
		signUpBack.onClick.AddListener(delegate { ChangePanel(0); });
		logInBack.onClick.AddListener(delegate { ChangePanel(0); });
		logInSubmission.onClick.AddListener(delegate { confirmLogIn(logInType.native); });
		signUpSubmission.onClick.AddListener(confirmSignUp);

		errorMessage.text = "";
		Input.location.Start();
	}

	void clearPanels() {
		foreach (GameObject panel in StartScreenPanels)
			panel.SetActive(false);
	}

	// Update is called once per frame
	void ChangePanel (int idx) {
		if (idx >= StartScreenPanels.GetLength(0)){
			Debug.LogError("Panel not found. Check the index.");
			return;
		}
		clearPanels();
		StartScreenPanels[idx].SetActive(true);
		errorMessage.text = "";		
	}

	void confirmLogIn(logInType myType) {
		if (locker) return;
		string userName = loginUserName.text;
		string pwd = loginPwd.text;
		
		switch (myType) {
			case logInType.native:
				submitLogIn(userName, pwd);					
				return;
			case logInType.facebook:				
				return;
			case logInType.google:				
				return;
			default:
				return;
		}
	}


	void confirmSignUp(){
		if (locker) return;
		string userName = signUpUserName.text;
		string pwd = signUpPwd.text;
		string name = signUpName.text;
		string cPwd = confirmPassword.text;
		if (cPwd != pwd){
			errorMessage.text = "Passwords don't match.";
			return;
		}
		submitSignUp(userName, pwd, name);		
	}


	#region serverComminication
	void submitLogIn(string uid, string pwd) {		

		WWWForm form = new WWWForm();	
		form.AddField("email", uid);
		form.AddField("password",pwd);
		IEnumerator c = sendForm("/auth/login", form);

		locker = true;
		StartCoroutine(c);
	}

	string parseError(string input) {
		int idx = input.IndexOf("Error:");
		if (idx == -1) {
			return "";
		}
		int i = idx+6;
		while (input[i] != '.'&&i<input.Length)
			i++;
		return input.Substring(idx + 6, i - idx - 5); 
	}

	IEnumerator sendForm(string api, WWWForm myform) {
			#if UNITY_WEBGL
			Dictionary<string, string> header = myform.headers;
			header["Access-Control-Allow-Credentials"]="true";
			header["Access-Control-Allow-Headers"]= "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time";
			header["Access-Control-Allow-Methods"]= "GET, POST, OPTIONS";
			header["Access-Control-Allow-Origin"]= "*";
			byte[] rawData = myform.data;
			WWW w = new WWW(infoContainer.server+api, rawData, header);
			#else
			WWW w = new WWW(infoContainer.server+api, myform);
			#endif		
			yield return w;
		if (w.error != ""){
			errorMessage.text = parseError(w.text);
			Debug.LogWarning(w.error);
		}
		else {
			//load new scene
			errorMessage.text = "";
			loginResponse lr = JsonUtility.FromJson<loginResponse>(w.text);
			infoContainer.instance.usrInfo = lr.user;
			infoContainer.instance.token = lr.token;
			//initialize like/ dislike list
			infoContainer.instance.initLists();
			SceneManager.LoadScene("main");
		}
			
		//unlock 
		locker = false;
		//Debug.Log(w.text);
	}

	void submitSignUp(string uid, string pwd, string name){
	
		WWWForm form = new WWWForm();

		form.AddField("email", uid);
		form.AddField("password", pwd);
		form.AddField("name", name);
		IEnumerator c = sendForm("/auth/signup", form);
		locker = true;
		StartCoroutine(c);
	}

//=======================current unsupported======================
	void logInWithFB(string uid, string pwd) {
		//developerLogs.log("loging with facebook account.." + uid + "," + pwd);
		//developerLogs.log("encoding for password needed");
		
		//developerLogs.log("sending input location:" + Input.location.lastData.longitude + ","
		//				  + Input.location.lastData.latitude);
	}

	void logInWithGoogle(string uid, string pwd){
		//developerLogs.log("loging with google account.." + uid + "," + pwd);
		//developerLogs.log("encoding for password needed");
		
		//developerLogs.log("sending input location:" + Input.location.lastData.longitude + ","
		//				  + Input.location.lastData.latitude);
	}
	//=======================current unsupported======================
	#endregion
}
