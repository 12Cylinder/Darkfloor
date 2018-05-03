using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairControl : MonoBehaviour {

    public GameObject CrosshairPrefab;

    public GameObject CrosshairInstanced;

    public void startTargetting()
    {
        CrosshairInstanced = Instantiate(CrosshairPrefab, transform.position - new Vector3(0, 1, 0), CrosshairPrefab.transform.rotation);
    }

    public void doAttack(GameObject attack)
    {
        Instantiate(attack, CrosshairInstanced.transform.position, attack.transform.rotation);
        Destroy(CrosshairInstanced);
        CrosshairInstanced = null;
    }
}
