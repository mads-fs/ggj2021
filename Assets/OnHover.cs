using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OnHover : MonoBehaviour {
    public Material offMaterial;
    public Material onMaterial;

    public float speed;
    public Transform creditsTransform;

    public UnityEvent onClick;

    private MeshRenderer _renderer;
    private void Start() {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Update() {
        creditsTransform.position = creditsTransform.position + Vector3.up * speed;
    }

    private void OnMouseExit() {
        _renderer.material = offMaterial;
    }

    private void OnMouseEnter() {
        _renderer.material = onMaterial;
    }

    private void OnMouseDown() {
        onClick.Invoke();
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }
    
    public void RollCredits(float newSpeed) {
        speed = newSpeed;
    }
}
