using UnityEngine;
using System.Collections;
using System;

public enum GameState {
	OnBoardRight,
	OnBoardLeft,
	Calm,
	LeftUpOutside,
	RightUpOutside,
	SeaMonster
}

public class GameController : MonoBehaviour {

	public GameState gameState;
	public SideTracker sideTracker;
	public static GameObject floorController;  
	public float stormWaterScale;
	private int seaMonsterTiltCount = 0;
	public const int seaMonsterTiltGoal = 4;
	public FloorSide seaMonsterSideGoal = FloorSide.Left;
	public GameObject cameraNode;
	public event EventHandler StomachHitEvent;

	private Animator cameraAnim;
	private MoveButton prevButon;
	private  const string OgopogoScene = "Ogopogo";
	private  const string TourScene = "Tour";
	public event EventHandler InitialPauseEndEvent;
	public bool isOgopogAppearanceTriggerEnabled = false;
	public bool isInitialInteractionTriggerEnabled = true;
	public bool isOgopogInteractionsTriggerEnabled = false;

	// Use this for initialization
	void Start () {
		cameraAnim = cameraNode.GetComponent<Animator>();
		floorController = GameObject.FindGameObjectWithTag ("FloorController");
		sideTracker.LeftSideStartEvent += new EventHandler(this.OnLeftSideStartEvent);
		sideTracker.RightSideStartEvent += new EventHandler(this.OnRightSideStartEvent);
		sideTracker.LeftSideOnBoardEvent += new EventHandler(this.OnLeftSideOnBoardEvent);
		//sideTracker.RightSideOnBoardEvent += new EventHandler(this.OnRightSideOnBoardEvent);

		DontDestroyOnLoad (floorController);

		if (Application.loadedLevelName == TourScene) {
			isInitialInteractionTriggerEnabled = true;
		}
		else if (Application.loadedLevelName == OgopogoScene) {
			isOgopogInteractionsTriggerEnabled = true;
		}

	}

	void startTour(){
		//change camera state 
		//cameraNode.GetComponent<CameraController> ().cameraState = CameraController.CameraState.MoveState;

	}

	// Update is called once per frame
	void Update () {

		if (PSMoveExample.curButton != null) {
			if (PSMoveExample.curButton == MoveButton.Square && prevButon != PSMoveExample.curButton) {
				prevButon = PSMoveExample.curButton;
				//Debug.Log ("Trigger Square");
				onPSMoveButtonClick();

			}else if (PSMoveExample.curButton == MoveButton.Triangle && prevButon != PSMoveExample.curButton) {
				prevButon = PSMoveExample.curButton;
				//Debug.Log ("Trigger Circle");
				onPSMoveButtonClick();

			}  else if (PSMoveExample.curButton == MoveButton.Cross && prevButon != PSMoveExample.curButton) {
				prevButon = PSMoveExample.curButton;
				//Debug.Log ("Trigger Cross");
				onPSMoveButtonClick();
			} else if (PSMoveExample.curButton == MoveButton.Circle && prevButon != PSMoveExample.curButton) {
				prevButon = PSMoveExample.curButton;
				//Debug.Log ("Trigger Circle");
				onPSMoveButtonClick();

			} else if (PSMoveExample.curButton == MoveButton.T && prevButon != PSMoveExample.curButton) {
				prevButon = PSMoveExample.curButton;
				onPSMoveTriggerClick();

			}
			else if (PSMoveExample.curButton == MoveButton.None) {
				prevButon = MoveButton.None;

			}
		}
		

		UpdateSeaMonsterStart ();
	}

	public void onPSMoveButtonClick(){

		print ("onPSMoveButtonClick" + isInitialInteractionTriggerEnabled + " curlevel: " + Application.loadedLevelName);
		if (Application.loadedLevelName == TourScene && isInitialInteractionTriggerEnabled == true) {
			isInitialInteractionTriggerEnabled = false;
			OnRightSideOnBoardEvent(this, EventArgs.Empty);
			print ("first if");

		}
		else if(isOgopogAppearanceTriggerEnabled){
			isOgopogAppearanceTriggerEnabled = false;
			cameraNode.GetComponent<CameraController>().triggerOgopogoSwimEvent();
			
		}
		else if(Application.loadedLevelName == OgopogoScene && isOgopogInteractionsTriggerEnabled){
			isOgopogInteractionsTriggerEnabled = false;
			ActivateSeamonsterState();
			
		}

	}

	public void onPSMoveTriggerClick(){
		cameraNode.GetComponent<CameraController> ().Resume();
		isInitialInteractionTriggerEnabled = false;

	}

	public void UpdateSeaMonsterStart() {
		if (gameState == GameState.SeaMonster && seaMonsterSideGoal == FloorSide.NoSide) {
			if (sideTracker.GetFullFloorSide() == FloorSide.Right) {
				seaMonsterSideGoal = FloorSide.Left;
				floorController.SendMessage("RaiseLeft");
				AudioManager.Main.PlayNewSound("BoatTilt");
			} else if (sideTracker.GetFullFloorSide() == FloorSide.Left) {
				seaMonsterSideGoal = FloorSide.Right;
				floorController.SendMessage("RaiseRight");
				AudioManager.Main.PlayNewSound("BoatTilt");
			}
		}
	}

	public void OnLeftSideOnBoardEvent(object sender, EventArgs e){
		if (gameState == GameState.OnBoardLeft) {
			floorController.SendMessage("LevelDown");
			gameState = GameState.Calm;
		}
	}

	public void OnRightSideOnBoardEvent(object sender, EventArgs e){
		if (gameState == GameState.OnBoardRight) {
			gameState = GameState.OnBoardLeft;
			floorController.SendMessage("RaiseLeft");
			AudioManager.Main.PlayNewSound("BoatTilt");

		}
	}

	public void ActivateLeftUpOutsideState() {

		gameState = GameState.LeftUpOutside;
		floorController.SendMessage("RaiseLeft");
		AudioManager.Main.PlayNewSound("BoatTilt");

	}

	public void ActivateRightUpOutsideState() {
		gameState = GameState.RightUpOutside;
		floorController.SendMessage("RaiseRight");
		AudioManager.Main.PlayNewSound("BoatTilt");

	}

	public void ActivateSeamonsterState(){
		print ("ActivateSeamonsterState");
		gameState = GameState.SeaMonster;
		seaMonsterSideGoal = FloorSide.NoSide;
//		if (sideTracker.GetFullFloorSide() != FloorSide.Right) {
//			floorController.SendMessage ("RaiseRight");
//			seaMonsterSideGoal = FloorSide.Right;
//		} else if (sideTracker.GetFullFloorSide() != FloorSide.Left) {
//			floorController.SendMessage ("RaiseLeft");
//			seaMonsterSideGoal = FloorSide.Left;
//		}
	}

    void Resume() {
        cameraNode.GetComponent<CameraController>().Resume();
    }

	void OnLeftSideStartEvent(object sender, EventArgs e) {
		print("STARTED LEFT SIDE");
		switch (gameState) {

			case GameState.LeftUpOutside:
				print("LeftUpOutside");
				floorController.SendMessage("LevelDown");
				gameState = GameState.Calm;
                Resume();
                break;
			case GameState.SeaMonster:
			if (seaMonsterSideGoal == FloorSide.Left) {
					seaMonsterTiltCount++;
					if (seaMonsterTiltCount == seaMonsterTiltGoal) {
						// Spit out boat here
						floorController.SendMessage("LevelDown");
						gameState = GameState.Calm;
					} else {
						
						LeanTween.delayedCall(1f,()=>{
							floorController.SendMessage("RaiseRight");
							AudioManager.Main.PlayNewSound("BoatTilt");

						});
					}

					if (this.StomachHitEvent != null) {
						this.StomachHitEvent(this, EventArgs.Empty);
					}
					seaMonsterSideGoal = FloorSide.Right;
				}
				break;
		}
	}
	
	void OnRightSideStartEvent(object sender, EventArgs e) {
		print("STARTED RIGHT SIDE");
		switch (gameState) {

			case GameState.RightUpOutside:
				floorController.SendMessage("LevelDown");
				gameState = GameState.Calm;
                Resume();
                break;
			case GameState.SeaMonster:
				if (seaMonsterSideGoal == FloorSide.Right) {
					seaMonsterTiltCount++;
					if (seaMonsterTiltCount == seaMonsterTiltGoal) {
						// Spit out boat here
						floorController.SendMessage("LevelDown");
						gameState = GameState.Calm;
					} else {
						LeanTween.delayedCall(1f,()=>{
							floorController.SendMessage("RaiseLeft");
							AudioManager.Main.PlayNewSound("BoatTilt");

						});
						
					}

					if (this.StomachHitEvent != null) {
						this.StomachHitEvent(this, EventArgs.Empty);
					}
					seaMonsterSideGoal = FloorSide.Left;
				}
				break;
		}

	}



}
