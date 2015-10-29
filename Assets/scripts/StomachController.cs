using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class StomachController : MonoBehaviour {

	public GameController gameController;
	public int seaMonsterTiltGoal;
	public float maxStomachOffset;
	public string[] monsterRoarSounds;
	public string[] monsterPainSounds;
	public GameObject floorController;
	private Animator anim;
	private int currentHitCount = 0;

	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator> ();
		floorController = GameObject.FindGameObjectWithTag ("FloorController");

		gameController.StomachHitEvent += new EventHandler(this.TriggerStomachHit);
		AudioManager.Main.PlayNewSound ("monster inhale", true);
		AudioManager.Main.PlayNewSound ("belly", true,0.1f);

	}

	void TriggerStomachHit(object sender, EventArgs e){
		if (currentHitCount < seaMonsterTiltGoal)
		{
			LeanTween.delayedCall(0.1f,()=>{
				anim.SetTrigger ("StomachHit");
				AudioManager.Main.PlayNewSound (monsterPainSounds[UnityEngine.Random.Range(0,monsterPainSounds.Length)]);

			});

			LeanTween.delayedCall(1.3f,()=>{
				RaiseWaterLevel();
			});
			currentHitCount++;

		}
	}

	void ActivateSeamonsterState(){
		AudioManager.Main.PlayNewSound ("GettingOutMusic",true,0.7f);
		gameController.ActivateSeamonsterState ();
	}

	void RaiseWaterLevel(){

			float offset = (float) maxStomachOffset / seaMonsterTiltGoal;
			float newPosY = gameObject.transform.position.y + offset;
			LeanTween.moveY(gameObject,newPosY,1.5f).setEase(LeanTweenType.easeSpring).setOnComplete(() => {
				print("curHitCount : "+ currentHitCount);
				
				if(currentHitCount == seaMonsterTiltGoal)
				{	
					AudioManager.Main.PlayNewSound ("MonsterSplit");
					Invoke("StartEndScene",0.5f);
					
				}
			});
		
	}

	void StartEndScene(){

		floorController.SendMessage("Shake",4f);
		DontDestroyOnLoad (floorController);
		LeanTween.delayedCall(1.5f,()=>{
			floorController.SendMessage("LevelUp");
		});

		Fader.LoadLevel("Splitout" ,1f,1f,Color.black);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			TriggerStomachHit(this,EventArgs.Empty);
		}
	}
}
