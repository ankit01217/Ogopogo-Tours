using UnityEngine;
using System.Collections;
using System;

public class FlockMember : MonoBehaviour {

    public GameObject CameraNode;
    public GameObject model;

    private const float timeToDisappear = 5.0f;

    // Use this for initialization
    void Start() {
        model.GetComponent<SkinnedMeshRenderer>().enabled = false;
        CameraController cameraController = CameraNode.GetComponent<CameraController>();
        cameraController.FlockSwoopEvent += new EventHandler(this.OnFlockSwoopEvent);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnFlockSwoopEvent(object sender, EventArgs e) {
        model.GetComponent<SkinnedMeshRenderer>().enabled = true;
        GetComponent<Animator>().SetTrigger("shouldSwoop");
        StartCoroutine(DisappearAfterWait());
    }

    IEnumerator DisappearAfterWait() {
        yield return new WaitForSeconds(timeToDisappear);
        model.GetComponent<SkinnedMeshRenderer>().enabled = false;
    }
}