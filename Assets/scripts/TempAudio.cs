using UnityEngine;
using System.Collections;
using System;

public class TempAudio : MonoBehaviour {

    public GameObject CameraNode;

    private AudioSource thunder;
    private AudioSource boatTilt;
    private AudioSource monsterGrowlBeforeEating;
    private AudioSource geeseFlying;

    // Use this for initialization
    void Start () {
        CameraNode.GetComponent<CameraController>().FlockSwoopEvent += new EventHandler(this.OnFlockSwoopEvent);
        CameraNode.GetComponent<CameraController>().FirstTiltStartEvent += new EventHandler(this.OnFirstTiltStartEvent);
        CameraNode.GetComponent<CameraController>().SecondTiltStartEvent += new EventHandler(this.OnSecondTiltStartEvent);
        CameraNode.GetComponent<CameraController>().ThirdTiltStartEvent+= new EventHandler(this.OnThirdTiltStartEvent);
        CameraNode.GetComponent<CameraController>().OgopogoScaleEvent += new EventHandler(this.OnOgopogoScaleEvent);


        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources) {
            switch (audioSource.priority) {
                case 1:
                    thunder = audioSource;
                    break;
                case 2:
                    boatTilt = audioSource;
                    break;
                case 3:
                    monsterGrowlBeforeEating = audioSource;
                    break;
                case 4:
                    geeseFlying = audioSource;
                    break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnFlockSwoopEvent (object sender, EventArgs e) {
        thunder.Play();
        geeseFlying.Play();
    }

    void OnFirstTiltStartEvent(object sender, EventArgs e) {
        boatTilt.Play();
    }

    void OnSecondTiltStartEvent(object sender, EventArgs e) {
        boatTilt.Play();
    }

    void OnThirdTiltStartEvent(object sender, EventArgs e) {
        boatTilt.Play();
    }

    void OnOgopogoScaleEvent(object sender, EventArgs e) {
        monsterGrowlBeforeEating.Play();
    }
}