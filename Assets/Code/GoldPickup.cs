using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour {

    public int maxAmount = 10;
    public int minAmount = 3;
    private int amount = 25;
    public GameObject particles;
    public int lifetime = 30;

    private float startTime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerInfo>().Wallet.modifyGold(amount);
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        amount = Random.Range(minAmount, maxAmount);
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
