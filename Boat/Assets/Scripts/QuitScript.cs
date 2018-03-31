using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScript : MonoBehaviour {
	public void quitGame(){
		Debug.Log("game quit");
		Application.Quit();
	}
}
