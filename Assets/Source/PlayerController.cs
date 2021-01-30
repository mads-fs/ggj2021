using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public float DistanceThreshold = 1f;
    private NavMeshAgent _agent;
    private Camera _mainCamera;
    private bool _isDestinationUse = false;
    private Interactable _useObject = null;
    private bool _isActive = true;
    private bool _playerIsSuspended = false;
    private Queue<float> _suspensions;

    [Header("Debug")]
    [SerializeField]
    private float _distanceToDestination = 0f;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _mainCamera = Camera.main;
        _suspensions = new Queue<float>();
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    _agent.destination = hit.point;
                    if (_useObject != null)
                    {
                        if (_useObject.GetIsMouseOver() == false)
                        {
                            CancelInteractableRouting();
                        }
                    }
                }
            }

            _distanceToDestination = Vector3.Distance(transform.position, _agent.destination);

            if (_isDestinationUse == true)
            {
                if (_distanceToDestination < DistanceThreshold)
                {
                    _useObject.OnClick?.Invoke();
                    _isDestinationUse = false;
                    if (_useObject.IsOneShot == true)
                    {
                        _useObject.SetState(false);
                    }
                    _useObject = null;
                }
            }
        }
    }

    /// <summary>
    /// Use this to set whether the player can interact with the world or the <see cref="PlayerController"/> at all.
    /// </summary>
    /// <param name="state">The new state. Default is true.</param>
    public void SetPlayerState(bool state) => _isActive = state;

    public void SetInteractableRouting(Interactable endInteractable)
    {
        _isDestinationUse = true;
        _useObject = endInteractable;
    }

    public void CancelInteractableRouting()
    {
        _isDestinationUse = false;
        _useObject = null;
    }

    public void PrintMessage(string value) => Debug.Log(value);

    #region Suspension
    public void SuspendPlayerForSeconds(float seconds) => QueueSuspension(seconds);

    private void QueueSuspension(float seconds)
    {
        if (_playerIsSuspended == true)
        {
            _suspensions.Enqueue(seconds);
        }
        else
        {
            if (_suspensions.Count == 0)
            {
                _suspensions.Enqueue(seconds);
                StartNextSuspension();
            }
        }
    }
    private void StartNextSuspension()
    {
        if (_suspensions.Count > 0)
        {
            Debug.LogWarning($"Staring Suspension for {_suspensions.Peek()} seconds.");
            StartCoroutine(SuspendPlayer(_suspensions.Dequeue(), StartNextSuspension));
        }
        else
        {
            _playerIsSuspended = false;
            _isActive = true;
        }
    }

    private IEnumerator SuspendPlayer(float seconds, Action callback = null)
    {
        _isActive = false;
        _playerIsSuspended = true;
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
        yield return null;
    }
    #endregion
}
