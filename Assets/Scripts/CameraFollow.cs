using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField, Tooltip("The target which the camera should follow")]
    private Transform target;

    [Header("Tweaks")]
    [SerializeField, Range(0.05f, 0.5f)]
    private float smoothTime = 0.25f;

    private Vector3 _camOffset;
    private Vector3 _velocity;

    private void Start()
    {
        transform.position = target.position;
        _camOffset = transform.position - target.position;
        _velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        var targetPosition = target.position + _camOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
