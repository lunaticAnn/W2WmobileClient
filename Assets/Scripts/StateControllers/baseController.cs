using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//basic state machine state class
public class baseController : MonoBehaviour{
	public virtual void inputEventHandler() { }
	public virtual void enterState() {
		mainController.instance.activeController = this;
	}

	public virtual void exitState() {
		mainController.instance.activeController = null;
	}	
}
