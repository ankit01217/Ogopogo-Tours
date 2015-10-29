using UnityEngine;
using System.Collections;
using System;

public class OgopogoScaler : MonoBehaviour {

    public GameObject CameraNode;

    // Use this for initialization
    void Start() {
        CameraController cameraController = CameraNode.GetComponent<CameraController>();
        cameraController.OgopogoScaleEvent += new EventHandler(this.OnOgopogoScaleEvent);
        cameraController.OgopogoSwimEvent += new EventHandler(this.OnOgopogoSwimEvent);
    }

    void OnOgopogoScaleEvent(object sender, EventArgs e) {
        GetComponent<Animator>().SetTrigger("shouldScale");
    }

    void OnOgopogoSwimEvent(object sender, EventArgs e) {
        transform.FindChild("Ogopogo_RaiseLowerAnim").GetComponent<Animator>().SetTrigger("shouldOpenAndClose");
    }

    public void TriggerScale() {
        CameraNode.GetComponent<CameraController>().triggerOgopogoScaleEvent();
    }
}
