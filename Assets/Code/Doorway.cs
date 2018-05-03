using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour {

    public Animator anim;
    public bool isOpen;
    public bool showNotice;
    public GameObject notice;
    public bool unlocked = false;


    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            showNotice = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            showNotice = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            showNotice = true;
        }

        if (!isOpen && other.gameObject.tag == "Player")
        {
            if (Input.GetKey("joystick "+other.GetComponent<PlayerController>().player+" button 2"))
            {
                openDoor();
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void Update()
    {
        if (notice)
        {
            notice.SetActive(!isOpen && showNotice && unlocked);
        }
    }

    void openDoor()
    {
        if (unlocked)
        {
            isOpen = true;
            anim.SetBool("isOpen", true);
        }
    }
}
