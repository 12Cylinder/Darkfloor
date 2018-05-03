using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float Delay = 5.0f;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time > startTime + Delay)
        {
            Destroy(this.gameObject);
        }
    }
}
