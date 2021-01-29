using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Tooltip("The prefab to make visible when mousing over (if any). Leave blank if nothing should be displayed when mousing over.")]
    public GameObject ActivatedObject;
    [Tooltip("Whether the object can be interacted with more than once.")]
    public bool IsOneShot = false;
    [Tooltip("Events to execute once the object is interacted with.")]
    public UnityEvent OnClick;

    private PlayerController _player;
    private bool _isMouseOver = false;
    private bool _isActive = true;
    private Camera _mainCam;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _mainCam = Camera.main;
        if (ActivatedObject != null)
        {
            ActivatedObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isActive == true)
        {
            RaycastHit[] cameraHits;
            Ray cameraRay = _mainCam.ScreenPointToRay(Input.mousePosition);
            if ((cameraHits = Physics.RaycastAll(cameraRay)) != null)
            {
                for (int hitIndex = 0; hitIndex < cameraHits.Length; hitIndex++)
                {
                    if (cameraHits[hitIndex].collider.gameObject == gameObject)
                    {
                        _isMouseOver = true;
                        break;
                    }
                    else
                    {
                        _isMouseOver = false;
                    }
                }
            }

            if (ActivatedObject != null)
            {
                ActivatedObject.SetActive(_isMouseOver);
            }

            if (_isMouseOver == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _player.SetInteractableRouting(this);
                }
            }
        }
        else
        {
            _isMouseOver = false;
            if (ActivatedObject != null && ActivatedObject.activeInHierarchy == true)
            {
                ActivatedObject.SetActive(false);
            }
        }
    }

    public void SetState(bool state) => _isActive = state;
}
