#pragma strict
import Phidgets;
import UnityEngine;
import System.Collections;
import UnityEngine.UI;

var showInstructions : boolean;

public var shakeTime:float;
private var motionFloor : Analog;
private var isRaising0 = false;
private var isRaising1 = true;
private var isRaising2 = false;
private var isRaising3 = true;
public var isStormActivated:boolean;


public enum FloorState
{
	Default = 0,
    RaiseLeft = 1,
    RaiseRight = 2,
    LevelUp = 3,
    LevelDown = 4,
    Shake = 5,
    
}


public enum FloorSpeed
{
	Tilt = 1000,
	Normal = 200,
	Storm = 200,
	Shake = 300,
    
}


private var floorState :FloorState;
private var floorSpeed :FloorSpeed;

// the motion floor uses four air bladders, in the corners of the floor, to raise it which are enumerated as follows:
// 2 - front left		3 - front right
// 0 - back left		1 - back right


function Start () {

	// attach the phidget that controls the floor
	motionFloor = new Analog();
    motionFloor.open();
    motionFloor.waitForAttachment(1000);
   
    // lower the floor at start - just in case a previous program left it up
    resetFloor();
    
	//InvokeRepeating("updateFloor",0.05f,0.05f);
    
}


function Update() {
	
	
	 updateFloor();	

}

public function RaiseRight(){
	floorSpeed = FloorSpeed.Tilt;
	floorState = FloorState.RaiseRight;
	
}

public function RaiseLeft(){

	floorSpeed = FloorSpeed.Tilt;
	floorState = FloorState.RaiseLeft;
}

public function LevelUp(){
	floorSpeed = FloorSpeed.Tilt;
	floorState = FloorState.LevelUp;
}

public function LevelDown(){
	floorSpeed = FloorSpeed.Tilt;
	floorState = FloorState.LevelDown;
}

public function Shake(time:float){
	if(floorState != FloorState.Shake)
	{
		floorState = FloorState.Shake;
		Invoke("LevelDown",time);
	}
	else
	{
		resetFloor();
	}
	
}


public function SetStormFlag(flag:boolean){
	isStormActivated = flag;
}

public function LevelHalf(){
	moveAll(5.0f);
}


function updateFloor(){
	var maxCutoff = ((isStormActivated == true) ? 8.0f : 9.0f);
	var minCutoff = 0.1f;
	var minVoltage = 0.0f;
	var maxVoltage = 10.0f;
	var offset0 = 0.0f;
	var offset1 = 0.0f;
	var offset2 = 0.0f;
	var offset3 = 0.0f;
	
	switch(floorState)
	{
		
		case FloorState.RaiseLeft:
			
			maxVoltage = 10.0f;
			minVoltage = 0.0f;
			if(getVoltage(0) < maxCutoff || getVoltage(2) < maxCutoff){
				moveOne(0,maxCutoff);
				moveOne(2,maxCutoff);
			}
			if(getVoltage(1) > minCutoff || getVoltage(3) > minCutoff){
				moveOne(1,minCutoff);
				moveOne(3,minCutoff);
			}
				
			floorSpeed  = ((isStormActivated == true) ? FloorSpeed.Storm : FloorSpeed.Normal);
		
			if(isRaising0 == true && getVoltage(0) >= maxVoltage){ isRaising0 = false;}
			if(isRaising0 == false && getVoltage(0) <= maxCutoff){ isRaising0 = true;}
			
			isRaising2 = isRaising0;
			
			if(isRaising3 == true && getVoltage(3) >= minCutoff){ isRaising3 = false;}
			if(isRaising3 == false && getVoltage(3) <= minVoltage){ isRaising3 = true;}
			
			isRaising1 = isRaising3;
			
			offset0 = (isRaising0 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset1 = (isRaising1 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset2 = (isRaising2 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset3 = (isRaising3 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			
			moveOne(0,Mathf.Clamp((getVoltage(0) + offset0),minVoltage, maxVoltage));
			moveOne(1,Mathf.Clamp((getVoltage(1) + offset1),minVoltage, maxVoltage));
			moveOne(2,Mathf.Clamp((getVoltage(2) + offset2),minVoltage, maxVoltage));
			moveOne(3,Mathf.Clamp((getVoltage(3) + offset3),minVoltage, maxVoltage));
			
			
		break;
		case FloorState.RaiseRight:
		
			maxVoltage = 10.0f;
			minVoltage = 0.0f;
			if(getVoltage(1) < maxCutoff || getVoltage(3) < maxCutoff){
				moveOne(1,maxCutoff);
				moveOne(3,maxCutoff);
			}
			if(getVoltage(0) > minCutoff || getVoltage(2) > minCutoff){
				moveOne(0,minCutoff);
				moveOne(2,minCutoff);
			}
				
			floorSpeed  = ((isStormActivated == true) ? FloorSpeed.Storm : FloorSpeed.Normal);
		
			if(isRaising1 == true && getVoltage(1) >= maxVoltage){ isRaising1 = false;}
			if(isRaising1 == false && getVoltage(1) <= maxCutoff){ isRaising1 = true;}
			
			isRaising3 = isRaising1;
			
			if(isRaising0 == true && getVoltage(0) >= minCutoff){ isRaising0 = false;}
			if(isRaising0 == false && getVoltage(0) <= minVoltage){ isRaising0 = true;}
			
			isRaising2 = isRaising0;
			
			offset0 = (isRaising0 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset1 = (isRaising1 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset2 = (isRaising2 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset3 = (isRaising3 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			
			moveOne(0,Mathf.Clamp((getVoltage(0) + offset0),minVoltage, maxVoltage));
			moveOne(1,Mathf.Clamp((getVoltage(1) + offset1),minVoltage, maxVoltage));
			moveOne(2,Mathf.Clamp((getVoltage(2) + offset2),minVoltage, maxVoltage));
			moveOne(3,Mathf.Clamp((getVoltage(3) + offset3),minVoltage, maxVoltage));
				
		break;
		case FloorState.LevelUp:
			
			maxVoltage = 10.0f;
			minVoltage = maxVoltage - 1.0f;
			
			floorSpeed  = ((isStormActivated == true) ? FloorSpeed.Storm : FloorSpeed.Normal);
		
			if(isRaising1 == true && getVoltage(1) >= maxVoltage){ isRaising1 = false;}
			if(isRaising1 == false && getVoltage(1) <= minVoltage){ isRaising1 = true;}
			
			if(isRaising3 == true && getVoltage(3) >= maxVoltage){ isRaising3 = false;}
			if(isRaising3 == false && getVoltage(3) <= minVoltage){ isRaising3 = true;}
			
			isRaising0 = isRaising2 = !isRaising1;
			
			offset0 = (isRaising0 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset1 = (isRaising1 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset2 = (isRaising2 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset3 = (isRaising3 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			
			moveOne(0,Mathf.Clamp((getVoltage(0) + offset0),minVoltage, maxVoltage));
			moveOne(1,Mathf.Clamp((getVoltage(1) + offset1),minVoltage, maxVoltage));
			moveOne(2,Mathf.Clamp((getVoltage(2) + offset2),minVoltage, maxVoltage));
			moveOne(3,Mathf.Clamp((getVoltage(3) + offset3),minVoltage, maxVoltage));
			
		break;
		
		case FloorState.Shake:
			
			var shakeSpeed = floorSpeed.Shake.GetHashCode();
			
			if(isRaising1 == true && getVoltage(1) >= 10.0f){ isRaising1 = false;}
			if(isRaising1 == false && getVoltage(1) <= 0.0f){ isRaising1 = true;}
			
			if(isRaising2 == true && getVoltage(2) >= 10.0f){ isRaising2 = false;}
			if(isRaising2 == false && getVoltage(2) <= 0.0f){ isRaising2 = true;}
			
			isRaising0 = isRaising3 = !isRaising1;
			
			offset0 = (isRaising0 == true) ? Time.deltaTime * shakeSpeed : -(Time.deltaTime * shakeSpeed);
			offset1 = (isRaising1 == true) ? Time.deltaTime * shakeSpeed : -(Time.deltaTime * shakeSpeed);
			offset2 = (isRaising2 == true) ? Time.deltaTime * shakeSpeed : -(Time.deltaTime * shakeSpeed);
			offset3 = (isRaising3 == true) ? Time.deltaTime * shakeSpeed : -(Time.deltaTime * shakeSpeed);
			
			moveOne(0,Mathf.Clamp((getVoltage(0) + offset0),0.0f, 10.0f));
			moveOne(1,Mathf.Clamp((getVoltage(1) + offset1),0.0f, 10.0f));
			moveOne(2,Mathf.Clamp((getVoltage(2) + offset2),0.0f, 10.0f));
			moveOne(3,Mathf.Clamp((getVoltage(3) + offset3),0.0f, 10.0f));
			
		break;
		case FloorState.Default:
		case FloorState.LevelDown:
			
			if(isStormActivated == true)
			{
				minVoltage = 1.5f;
				maxVoltage = 5.0f;
			}
			else
			{
				minVoltage = 2.5f;
				maxVoltage = 5.0f;
			}
			
			
			
			floorSpeed  = ((isStormActivated == true) ? FloorSpeed.Storm : FloorSpeed.Normal);
		
			if(isRaising1 == true && getVoltage(1) >= maxVoltage){ isRaising1 = false;}
			if(isRaising1 == false && getVoltage(1) <= minVoltage){ isRaising1 = true;}
			
			if(isRaising3 == true && getVoltage(3) >= maxVoltage){ isRaising3 = false;}
			if(isRaising3 == false && getVoltage(3) <= minVoltage){ isRaising3 = true;}
			
			isRaising0 = isRaising2 = !isRaising1;
			
			offset0 = (isRaising0 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset1 = (isRaising1 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset2 = (isRaising2 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			offset3 = (isRaising3 == true) ? Time.deltaTime * floorSpeed.GetHashCode() : -(Time.deltaTime * floorSpeed.GetHashCode());
			
			moveOne(0,Mathf.Clamp((getVoltage(0) + offset0),minVoltage, maxVoltage));
			moveOne(1,Mathf.Clamp((getVoltage(1) + offset1),minVoltage, maxVoltage));
			moveOne(2,Mathf.Clamp((getVoltage(2) + offset2),minVoltage, maxVoltage));
			moveOne(3,Mathf.Clamp((getVoltage(3) + offset3),minVoltage, maxVoltage));
			
			
			
		break;
	
	}

}



function OnDisable () {
	// drop the floor
    Debug.Log("resetting floor in OnDisable");
    resetFloor();
    motionFloor.close();	// if you don't close the phidget then this phidget hangs
}

/*
function OnGUI() {
	var buttonW : int  = 150;
	var buttonH : int  = 50;

	
	if (showInstructions) {
		
		GUI.BeginGroup(Rect(1420,1200,400,300));
		  
		GUI.Box (Rect (0,0,400,300), "Floor Controls");
	   // utility controls
		if (GUI.Button(Rect(20,20,buttonW,buttonH), "Enable")) {
			// enable the floor for use and automatically lower the floor
			enable();
			lowerFloor();
		}
		if (GUI.Button(Rect(20,80,buttonW,buttonH), "Disable")) {
			// automatically lower the floor and then disable it from use
			lowerFloor();
			disable();
		}
	   // full floor controls
		if (GUI.Button(Rect(200,20,buttonW,buttonH), "Lower")) {
			// lower the motion floor
			lowerFloor();
		}
		if (GUI.Button(Rect(200,80,buttonW,buttonH), "Raise to Half")) {
			// raise the motion floor to half
			moveAll(5.0F);
		}
		if (GUI.Button(Rect(200,140,buttonW,buttonH), "Raise to Full")) {
			// raise the motion floor to its full height
			moveAll(10.0F);
		}
		

		GUI.EndGroup();
	}
}
*/

function enable (){
	// the air bladders have to be enabled before they can be used
	for (var i : int = 0; i < 4; i++) {
		motionFloor.outputs[i].Enabled = true;
	}
}

function disable (){
	// when done using the floor, and after it has been lowered, disable the bladders
	for (var i : int = 0; i < 4; i++) {
		motionFloor.outputs[i].Enabled = false;
	}
}

function resetFloor() {
	// lowers the floor *** for safty, should be called at the start and end of all games ***
	floorState = FloorState.Default;
    floorSpeed = (isStormActivated == true) ? FloorSpeed.Storm : FloorSpeed.Normal;
	enable();
    lowerFloor();
   
}

function getVoltage(index : int): float {
	// return the current voltage level of the provided air bladder
	return motionFloor.outputs[index].Voltage;
}

function lowerFloor (){
	// lower the floor by setting all bladders voltages to zero - should be used at the start and end of games
	for (var i : int = 0; i < 4; i++) {
		motionFloor.outputs[i].Voltage = 0.0F;
	}
}

function moveOne (index : int, voltage : float){
	// set the voltage (translates to floor height) of the provided bladder to the provided voltage
	// voltage = 0.0F is fully lowered
	// voltage = 5.0F is raised halfway
	// voltage = 10.0F is raised fully
	motionFloor.outputs[index].Voltage = voltage;
}

function moveAll (voltage : float){
	// for all four bladders set the provided voltage (translates to height) in a range of 0.0F - 10.0F
	for (var i : int = 0; i < 4; i++) {
		motionFloor.outputs[i].Voltage = voltage;
	}
}
	
/*	
function raiseFront(voltage : float) {
	// raise the bladders on the front side of the floor to the provided voltage and fully lower the bladders on the back side
	motionFloor.outputs[2].Voltage = voltage;
	motionFloor.outputs[3].Voltage = voltage;
	motionFloor.outputs[0].Voltage = 0.0F;
	motionFloor.outputs[1].Voltage = 0.0F;
}

function raiseBack(voltage : float) {
	// raise the bladders on the back side of the floor to the provided voltage and fully lower the bladders on the front side
	motionFloor.outputs[0].Voltage = voltage;
	motionFloor.outputs[1].Voltage = voltage;
	motionFloor.outputs[2].Voltage = 0.0F;
	motionFloor.outputs[3].Voltage = 0.0F;
}

function raiseRight(voltage : float) {
	// raise the bladders on the right side of the floor to the provided voltage and fully lower the bladders on the left side
	motionFloor.outputs[1].Voltage = voltage;
	motionFloor.outputs[3].Voltage = voltage;
	motionFloor.outputs[0].Voltage = 0.0F;
	motionFloor.outputs[2].Voltage = 0.0F;
}

function raiseLeft(voltage : float) {
	// raise the bladders on the left side of the floor to the provided voltage and fully lower the bladders on the right side
	motionFloor.outputs[0].Voltage = voltage;
	motionFloor.outputs[2].Voltage = voltage;
	motionFloor.outputs[1].Voltage = 0.0F;
	motionFloor.outputs[3].Voltage = 0.0F;
}*/



