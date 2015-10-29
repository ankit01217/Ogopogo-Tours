using UnityEngine;
using System.Collections;
using System;

public class SingleGoose : MonoBehaviour {

    public GameObject CameraNode;
    public GameObject model;

    // Use this for initialization
    void Start() {
        model.GetComponent<SkinnedMeshRenderer>().enabled = false;
        CameraController cameraController = CameraNode.GetComponent<CameraController>();
        cameraController.FirstGooseSwoopEvent += new EventHandler(this.OnFirstGooseSwoopEvent);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnFirstGooseSwoopEvent(object sender, EventArgs e) {
        AudioManager.Main.PlayNewSound("goose fly", true);

        model.GetComponent<SkinnedMeshRenderer>().enabled = true;
        print("ON FIRST GOOSE SWOOP CALLBACK");
        GetComponent<Animator>().SetTrigger("shouldSwoop");
    }
}