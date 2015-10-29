using UnityEngine;
using System.Collections;

public class CallParentScale : MonoBehaviour {

    public void doCallParentScale() {
        OgopogoScaler scaler = GameObject.FindObjectOfType<OgopogoScaler>();
        scaler.TriggerScale();
    }
}
