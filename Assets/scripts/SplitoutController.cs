using UnityEngine;
using System.Collections;

public class SplitoutController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void slowerCameraSpeed(){
		GetComponent<Animator>().speed = 0.1f;
	}

	public void fastenCameraSpeed(){
		GetComponent<Animator>().speed = 0.4f;
	}

	public void playSplashSound(){
		//AudioManager.Main.PlayNewSound("Splash")
	}

	public void onGameEnd(){
		//Fader.LoadLevel("GameEnd" ,0.5f,0.5f,Color.black);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
