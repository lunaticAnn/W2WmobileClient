using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour {
	public GameObject[] StartScreenPanels;
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
		signUpSubmission = StartScreenPanels[2].transform.GetChild(3).GetComponent<Button>();

		clearPanels();
		ChangePanel(0);
		logIn.onClick.AddListener(delegate { ChangePanel(1); });
		signUp.onClick.AddListener(delegate { ChangePanel(2); });
		signUpBack.onClick.AddListener(delegate { ChangePanel(0); });
		logInBack.onClick.AddListener(delegate { ChangePanel(0); });
		logInSubmission.onClick.AddListener(confirmLogIn);
		signUpSubmission.onClick.AddListener(confirmSignUp);
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
	}

	void confirmLogIn() {
		string userName = loginUserName.text;
		string pwd = loginPwd.text;
		if (submitLogIn(userName, pwd)) {
			SceneManager.LoadScene("main");
		}
	}

	void confirmSignUp(){
		string userName = loginUserName.text;
		string pwd = loginPwd.text;
		if (submitSignUp(userName, pwd)){
			SceneManager.LoadScene("main");
		}
	}
	#region serverComminication
	bool submitLogIn(string uid, string pwd) {		
		Debug.LogWarning("login with username and password:" + uid + "," + pwd);
		return true;
	}

	bool submitSignUp(string uid, string pwd){
		Debug.LogWarning("login with username and password:" + uid + "," + pwd);
		return true;
	}

	#endregion
}
