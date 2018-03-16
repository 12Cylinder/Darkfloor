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
            if (other.gameObject.GetComponent<PlayerController>().Player == PlayerController.PlayerSetting.Player1)
            {
                if (Input.GetButton("ActionP1"))
                {
                    openDoor();
                }
            }

            if (other.gameObject.GetComponent<PlayerController>().Player == PlayerController.PlayerSetting.Player2)
            {
                if (Input.GetButton("ActionP2"))
                {
                    openDoor();
                }
            }

            if (other.gameObject.GetComponent<PlayerController>().Player == PlayerController.PlayerSetting.Player3)
            {
                if (Input.GetButton("ActionP3"))
                {
                    openDoor();
                }
            }

            if (other.gameObject.GetComponent<PlayerController>().Player == PlayerController.PlayerSetting.Player4)
            {
                if (Input.GetButton("ActionP4"))
                {
                    openDoor();
                }
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
