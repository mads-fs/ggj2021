using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float Offset = 1.25f;
    public Transform target;

    private Vector3 _offset;

    private void Start() {
        _offset = (transform.position - target.position) * Offset;
    }

    // Update is called once per frame
    void Update() {
        transform.position = target.position + _offset;
    }
}
