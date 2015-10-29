using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

    public enum Pause {
        Initial,
        FirstTilt,
        SecondTilt,
        ThirdTilt,
        Final,
        NoPause
    }

    //
    // Events
    //
    public event EventHandler InitialPauseEndEvent;
    public event EventHandler LogTurnEevent; // NO ASSOCIATED PAUSE
    public event EventHandler FirstGooseSwoopEvent; // NO ASSOCIATED PAUSE
    public event EventHandler FlockSwoopEvent; // NO ASSOCIATED PAUSE
    public event EventHandler FlockSwoopEvent2; // NO ASSOCIATED PAUSE
    public event EventHandler FirstTiltStartEvent;
    public event EventHandler FirstTiltEndEvent;
    public event EventHandler SecondTiltStartEvent;
    public event EventHandler SecondTiltEndEvent;
    public event EventHandler ThirdTiltStartEvent;
    public event EventHandler ThirdTiltEndEvent;
    public event EventHandler OgopogoSwimEvent; // NO ASSOCIATED PAUSE
    public event EventHandler OgopogoScaleEvent; // NO ASSOCIATED PAUSE
    public event EventHandler FinalPauseStartEvent;

	public GameObject waterAnim;
	public GameObject singleGoose;
    public GameObject flock1;
    public GameObject flock2;
    public GameObject light1;
    public GameObject light2;
	public GameObject ogopogoRenderer;
	public GameObject sideTracker;
    public GameObject log;

    public GameController gameController;

    public AudioManager audioManager;

    private Pause currentPause = Pause.Initial;
	public float skyboxBlend;
    public float normalSpeed,waterScale,stormSpeed;
	private Animator cameraAnim;
	private Sound bgMusic;
    private Sound boatDrive;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().speed = 0.0f;
		bgMusic = AudioManager.Main.PlayNewSound("IntroMusic",true);

    }

    // Update is called once per frame
    void Update() {

        //update skybox blend
        RenderSettings.skybox.SetFloat("_Blend", skyboxBlend);
		Vector3 newScale = waterAnim.transform.localScale;
		newScale.y = waterScale;
		waterAnim.transform.localScale = newScale;

        if (Input.GetKeyDown(KeyCode.R)) {
            Resume();
        }
	}

    // This method should be triggered by PSMove and Left/Right stuff in order to progress the game
    public void Resume() {
        switch (currentPause) {
            case Pause.Initial:
                triggerInitialPauseEndEvent();
                break;
            case Pause.FirstTilt:
                triggerFirstTiltEndEvent();
                break;
            case Pause.SecondTilt:
                triggerSecondTiltEndEvent();
                break;
            case Pause.ThirdTilt:
                triggerThirdTiltEndEvent();
                break;
            case Pause.Final:
                print("TRIED TO RESUME FROM FINAL PAUSE");
                break;
        }
    }

    public void triggerInitialPauseEndEvent() {
		AudioManager.Main.PlayNewSound("BoatHorn");

        if (this.InitialPauseEndEvent != null) {
            this.InitialPauseEndEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = normalSpeed;
        currentPause = Pause.NoPause;
    }
    public void triggerLogTurnEvent() {
        if (this.LogTurnEevent != null) {
            this.LogTurnEevent(this, EventArgs.Empty);
        }
        log.GetComponent<Animator>().SetTrigger("logShouldTurn");
    }
    public void triggerFirstGooseSwoopEvent() {
        if (this.FirstGooseSwoopEvent != null) {
            this.FirstGooseSwoopEvent(this, EventArgs.Empty);
        }
    }
    public void triggerFlockSwoopEvent() {
        if (this.FlockSwoopEvent!= null) {
            this.FlockSwoopEvent(this, EventArgs.Empty);
        }
        light1.GetComponent<Animator>().SetTrigger("shouldDarken");
        light2.GetComponent<Animator>().SetTrigger("shouldDarken");
    }
    public void triggerFlockSwoopEvent2() {

		if (this.FlockSwoopEvent2 != null) {
            this.FlockSwoopEvent2(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = stormSpeed;
    }
    public void triggerFirstTiltStartEvent() {
        if (this.FirstTiltStartEvent != null) {
            this.FirstTiltStartEvent(this, EventArgs.Empty);
        }
        
		GetComponent<Animator>().speed = 0;
        currentPause = Pause.FirstTilt;

        print("GO TO THE LEFT SIDE");
		if (sideTracker.GetComponent<SideTracker>().GetFullFloorSide() != FloorSide.Left) {
			gameController.ActivateLeftUpOutsideState ();
		} else {
			gameController.ActivateRightUpOutsideState ();
		}
    }
    public void triggerFirstTiltEndEvent() {
		AudioManager.Main.PlayNewSound("ScaryAmbient",true);

		if (this.FirstTiltEndEvent != null) {
            this.FirstTiltEndEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = stormSpeed;
        currentPause = Pause.NoPause;
    }
    public void triggerSecondTiltStartEvent() {
        if (this.SecondTiltStartEvent != null) {
            this.SecondTiltStartEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = 0;
        currentPause = Pause.SecondTilt;

        print("GO TO THE RIGHT SIDE");
		if (sideTracker.GetComponent<SideTracker>().GetFullFloorSide() != FloorSide.Right) {
			gameController.ActivateRightUpOutsideState ();
		} else {
			gameController.ActivateLeftUpOutsideState ();
		}
    }
    public void triggerSecondTiltEndEvent() {
        if (this.SecondTiltEndEvent != null) {
            this.SecondTiltEndEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = stormSpeed;
        currentPause = Pause.NoPause;
    }
    public void triggerThirdTiltStartEvent() {

		if (this.ThirdTiltStartEvent != null) {
            this.ThirdTiltStartEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = 0;
        currentPause = Pause.ThirdTilt;

        print("GO TO THE LEFT SIDE");
		if (sideTracker.GetComponent<SideTracker>().GetFullFloorSide() != FloorSide.Left) {
			gameController.ActivateLeftUpOutsideState ();
		} else {
			gameController.ActivateRightUpOutsideState ();
		}
    }
    public void triggerThirdTiltEndEvent() {
		//bgMusic.playing = false;

		if (this.ThirdTiltEndEvent != null) {
            this.ThirdTiltEndEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = stormSpeed;
        currentPause = Pause.NoPause;
		light1.GetComponent<Animator>().SetTrigger("shouldBrighten");
		light2.GetComponent<Animator>().SetTrigger("shouldBrighten");

		gameController.isOgopogAppearanceTriggerEnabled = true;

    }
    public void triggerOgopogoSwimEvent() {
		AudioManager.Main.PlayNewSound ("GettingEaten");
		if (this.OgopogoSwimEvent!= null) {
            this.OgopogoSwimEvent(this, EventArgs.Empty);
        }
		ogopogoRenderer.GetComponent<SkinnedMeshRenderer> ().enabled = true;
    }
    public void triggerOgopogoScaleEvent() {
        if (this.OgopogoScaleEvent != null) {
            this.OgopogoScaleEvent(this, EventArgs.Empty);
        }
    }
    public void triggerFinalPauseStartEvent() {
        if (this.FinalPauseStartEvent != null) {
            this.FinalPauseStartEvent(this, EventArgs.Empty);
        }
        GetComponent<Animator>().speed = 0;
        currentPause = Pause.Final;

    }
}