using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;

    private Vector3 _offset;

    private void Start() {
        _offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update() {
        transform.position = target.position + _offset;
    }
}
