using UnityEngine;
using System.Collections;

public class OgopogoController : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

	}

	public void startOgopogoEating(){
		anim.SetTrigger ("StartEatingAnim");

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.O)){
			startOgopogoEating();
		}
	}
}
