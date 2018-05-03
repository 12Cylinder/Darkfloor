using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DFCore;
public class AOE_Attack : MonoBehaviour {

    public float range = 3;
    public int damage = 100;
    public float delay = 5;
    public DamageTypes Type;

    public List<GameObject> hits;
    public GameObject effect;
    private float startTime;

    private void Start()
    {
        Instantiate(effect, transform.position, effect.transform.rotation);
        startTime = Time.time;
        GetComponent<CapsuleCollider>().radius = range;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if (!hits.Contains(other.gameObject))
            {
                hits.Add(other.gameObject);
            }
        }
    }

    private void Update()
    {
        if(Time.time > startTime + delay)
        {
            doDamage();
        }
    }

    private void doDamage()
    {
        foreach(GameObject GO in hits)
        {
            if(GO != null)
            GO.GetComponent<Health>().takeDamage(damage, Type);
        }

        Destroy(this.gameObject);
    }
}
