using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class UseOnTrigger : MonoBehaviour
{
    [Tooltip("The prefab to make visible when mousing over (if any). Leave blank if nothing should be displayed when mousing over.")]
    public GameObject ActivatedObject;
    public bool IsOneShot = true;
    public bool NeedsLineOfSight = false;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private bool _isMouseOver = false;
    private bool _isActive = true;
    private Camera _mainCam;

    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (_isActive)
        {
            if (ActivatedObject != null)
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
                ActivatedObject.SetActive(_isMouseOver);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (NeedsLineOfSight == true)
            {
                Vector3 playerPos = other.gameObject.transform.position;
                float distance = Vector3.Distance(playerPos, gameObject.transform.position);
                RaycastHit[] hits = Physics.RaycastAll(playerPos, gameObject.transform.position - playerPos, distance);
                if (hits.Length > 0)
                {
                    if (hits[0].collider.gameObject == gameObject)
                    {
                        OnEnter?.Invoke();
                        if (IsOneShot == true)
                        {
                            GetComponent<SphereCollider>().enabled = false;
                        }
                    }
                }
            }
            else
            {
                OnEnter?.Invoke();
                if (IsOneShot == true)
                {
                    GetComponent<SphereCollider>().enabled = false;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (NeedsLineOfSight == true)
            {
                Vector3 playerPos = other.gameObject.transform.position;
                float distance = Vector3.Distance(playerPos, gameObject.transform.position);
                RaycastHit[] hits = Physics.RaycastAll(playerPos, gameObject.transform.position - playerPos, distance);
                if (hits.Length > 0)
                {
                    if (hits[0].collider.gameObject == gameObject)
                    {
                        OnExit?.Invoke();
                        if (IsOneShot == true)
                        {
                            GetComponent<SphereCollider>().enabled = false;
                        }
                    }
                }
            }
            else
            {
                OnExit?.Invoke();
                if (IsOneShot == true)
                {
                    GetComponent<SphereCollider>().enabled = false;
                }
            }
        }
    }
}
