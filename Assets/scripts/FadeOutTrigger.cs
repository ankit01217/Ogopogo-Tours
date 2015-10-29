using UnityEngine;
using System.Collections;
using System;

public class FadeOutTrigger : MonoBehaviour {

    public event EventHandler FadeOutEvent;
	private GameObject floorController;
    public void triggerFadeOutEvent() {
        if (this.FadeOutEvent != null) {
            this.FadeOutEvent(this, EventArgs.Empty);
        }
		floorController = GameObject.FindGameObjectWithTag ("FloorController");

		floorController.SendMessage("Shake",4f);
		DontDestroyOnLoad (floorController);
		Fader.LoadLevel("Ogopogo" ,3f,1f,Color.black);
    }
}
