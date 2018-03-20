using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapper : MonoBehaviour {

    public GameObject GM;
    public bool useMap = false;
    private void OnCollisionEnter(Collision other)
    {
        if (useMap)
        {
            if (other.gameObject.tag == "Room")
            {
                GM.SendMessage("addMapPixel", other.transform.position);
            }
            else if (other.gameObject.tag == "BossRoom")
            {
                GM.SendMessage("addMapPixelBossRoom", other.transform.position);
            }
        }
    }
}
