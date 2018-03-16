using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour {

    public List<Vector3> blocks;
    public GameObject MapPixel;
    public GameObject MapPixelBossRoom;
    public GameObject target;

    public void AddPixel (Vector3 pos)
    {
        bool blockExists = false;

        foreach(Vector3 v in blocks)
        {
            if(v == pos)
            {
                blockExists = true;
            }
        }

        if (!blockExists)
        {
            blocks.Add(pos);
            GameObject newPixel;
            Vector3 newPos = new Vector3(pos.x, pos.z, 0);
            newPixel = Instantiate(MapPixel, transform.position, transform.rotation);
            newPixel.transform.SetParent(this.transform);
            newPixel.transform.position = newPos + transform.position;
            newPixel = null;
        }
    }

    public void AddPixelBossRoom(Vector3 pos)
    {
        bool blockExists = false;

        foreach (Vector3 v in blocks)
        {
            if (v == pos)
            {
                blockExists = true;
            }
        }

        if (!blockExists)
        {
            blocks.Add(pos);
            GameObject newPixel;
            Vector3 newPos = new Vector3(pos.x, pos.z, 0);
            newPixel = Instantiate(MapPixelBossRoom, transform.position, transform.rotation);
            newPixel.transform.SetParent(this.transform);
            newPixel.transform.position = newPos + transform.position;
            newPixel = null;
        }
    }


    public void FixedUpdate()
    {
        if (target)
        {
            Vector3 newPos = new Vector3(-target.transform.position.x, -target.transform.position.z, 0);
            this.transform.position = transform.parent.position + newPos;
        }
    }
}
