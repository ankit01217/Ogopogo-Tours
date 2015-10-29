using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SampleSideTrackerEventSubscriber : MonoBehaviour {

    public SideTracker sideTracker;

	// Use this for initialization
	void Start () {
        sideTracker.LeftSideStartEvent += new EventHandler(this.OnLeftSideStartEvent);
        sideTracker.LeftSideEndEvent += new EventHandler(this.OnLeftSideEndEvent);
        sideTracker.RightSideStartEvent += new EventHandler(this.OnRightSideStartEvent);
        sideTracker.RightSideEndEvent += new EventHandler(this.OnRightSideEndEvent);
        sideTracker.NoSideStartEvent += new EventHandler(this.OnNoSideStartEvent);
        sideTracker.NoSideEndEvent += new EventHandler(this.OnNoSideEndEvent);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnLeftSideStartEvent(object sender, EventArgs e) {
        print("STARTED LEFT SIDE");

    }

    void OnLeftSideEndEvent(object sender, EventArgs e) {
        print("ENDED LEFT SIDE");

    }

    void OnRightSideStartEvent(object sender, EventArgs e) {
        print("STARTED RIGHT SIDE");

    }

    void OnRightSideEndEvent(object sender, EventArgs e) {
        print("ENDED RIGHT SIDE");

    }

    void OnNoSideStartEvent(object sender, EventArgs e) {
        print("STARTED NO SIDE");
    }

    void OnNoSideEndEvent(object sender, EventArgs e) {
        print("ENDED NO SIDE");
    }
}
