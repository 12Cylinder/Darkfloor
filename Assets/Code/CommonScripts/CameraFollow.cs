using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public float dampening;
    public Vector3 offset;

    private Vector3 wantedPosition;
    private Vector3 currentPosition;

    public void FixedUpdate()
    {
        wantedPosition = target.transform.position;

        currentPosition = Vector3.Lerp(currentPosition, wantedPosition, dampening);

        this.transform.position = currentPosition + offset;
    }
}
