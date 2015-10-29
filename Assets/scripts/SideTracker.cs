using UnityEngine;
using System.Collections;
using System;

public enum FloorSide{
    Left,
    Right,
    NoSide
};

public enum Platform{
	PSMove,
	MakeyMakey
};

public class SideTracker : MonoBehaviour {

	public Platform platform;
	public float PSMoveXRange, PSMoveYRange;
    public event EventHandler LeftSideStartEvent;
    public event EventHandler LeftSideEndEvent;
    public event EventHandler RightSideStartEvent;
    public event EventHandler RightSideEndEvent;
    public event EventHandler NoSideStartEvent;
    public event EventHandler NoSideEndEvent;

	public event EventHandler RightSideOnBoardEvent;
	public event EventHandler LeftSideOnBoardEvent;

    private KeyCode leftKey = KeyCode.LeftArrow;
    private KeyCode rightKey = KeyCode.RightArrow;

    public FloorSide floorSide { get; private set; }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

		FloorSide oldFloorSide = this.floorSide;
		if (platform == Platform.MakeyMakey) {
			if (Input.GetKey(leftKey) && !Input.GetKey(rightKey)) {
				this.floorSide = FloorSide.Left;
				
			} else if (Input.GetKey(rightKey) && !Input.GetKey(leftKey)) {
				this.floorSide = FloorSide.Right;
				
			} else {
				this.floorSide = FloorSide.NoSide;
				
			}


			if (Input.GetKeyDown (leftKey)) {
				if (this.LeftSideOnBoardEvent != null) {
					this.LeftSideOnBoardEvent(this, EventArgs.Empty);
				}
			}
			
			if (Input.GetKeyDown (rightKey)) {
				if (this.RightSideOnBoardEvent != null) {
					this.RightSideOnBoardEvent(this, EventArgs.Empty);
				}
			}

		} 
		else if (platform == Platform.PSMove) 
		{
			int rightCount = 0;
			int leftCount = 0;
			int totCount = 0;
			for(int i=0; i<PSMoveInput.MAX_MOVE_NUM; i++)
			{
				
				MoveController moveController = PSMoveInput.MoveControllers[i];
				if(moveController.Connected) {
					totCount++;
					MoveData moveData = moveController.Data;
					if(moveData.Position.x >= 0){
						rightCount++;
					}
					else{
						leftCount++;
					}
					
				}
			}
			
			print("totCount :" + totCount + " left: "+leftCount + " right: "+rightCount);
			
			if (leftCount == totCount)  {
				this.floorSide = FloorSide.Left;
				
			} else if (rightCount == totCount) {
				this.floorSide = FloorSide.Right;
				
			} else {
				this.floorSide = FloorSide.NoSide;
			}


			if (leftCount == 1) {
				if (this.LeftSideOnBoardEvent != null) {
					this.LeftSideOnBoardEvent(this, EventArgs.Empty);
				}
			}
			
			if (rightCount == 1) {
				if (this.RightSideOnBoardEvent != null) {
					this.RightSideOnBoardEvent(this, EventArgs.Empty);
				}
			}
		}
        


		fireFloorSideEvents(this.floorSide, oldFloorSide);
	}

    void fireFloorSideEvents(FloorSide newFloorSide, FloorSide oldFloorSide) {
        if (newFloorSide != oldFloorSide) {
            switch (oldFloorSide) {
                case FloorSide.Left:
                    if (this.LeftSideEndEvent != null) {
                        this.LeftSideEndEvent(this, EventArgs.Empty);
                    }                
                    break;
                case FloorSide.Right:
                    if (this.RightSideEndEvent != null) {
                        this.RightSideEndEvent(this, EventArgs.Empty);
                    }
                    break;
                case FloorSide.NoSide:
                    if (this.NoSideEndEvent != null) {
                        this.NoSideEndEvent(this, EventArgs.Empty);
                    }
                    break;
            }
            switch (newFloorSide) {
                case FloorSide.Left:
                    if (this.LeftSideStartEvent != null) {
                        this.LeftSideStartEvent(this, EventArgs.Empty);
                    }
                    break;
                case FloorSide.Right:
                    if (this.RightSideStartEvent != null) {
                        this.RightSideStartEvent(this, EventArgs.Empty);
                    }
                    break;
                case FloorSide.NoSide:
                    if (this.NoSideStartEvent != null) {
                        this.NoSideStartEvent(this, EventArgs.Empty);
                    }
                    break;
            }
        }
    }

	public FloorSide GetFullFloorSide() {
		if (platform == Platform.MakeyMakey) {
			if (Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow)) {
				return FloorSide.Right;
			} else if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
				return FloorSide.Left;
			} else {
				return FloorSide.NoSide;
			}
		} else { // PSMove
			int rightCount = 0;
			int leftCount = 0;
			int totCount = 0;
			for (int i=0; i<PSMoveInput.MAX_MOVE_NUM; i++) {
				
				MoveController moveController = PSMoveInput.MoveControllers [i];
				if (moveController.Connected) {
					totCount++;
					MoveData moveData = moveController.Data;
					if (moveData.Position.x >= 0) {
						rightCount++;
					} else {
						leftCount++;
					}
					
				}
			}
			if (rightCount == totCount) {
				return FloorSide.Right;
			} else if (leftCount == totCount) {
				return FloorSide.Left;
			} else {
				return FloorSide.NoSide;
			}
		}
	}
}
