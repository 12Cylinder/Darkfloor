using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public int amount = 25;
    public GameObject particles;
    public int lifetime = 30;

    private float startTime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("takeHeal", amount);
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time > startTime + lifetime)
        {
            Destroy(gameObject);
        }
    }
}
