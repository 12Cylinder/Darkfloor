using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int player = 1;
    public float walkSpeed;
    public float turnSpeed;
    public float targetingSpeed;
    public GameObject[] attacks;
    public float attackRange = 10;

    private string playerSuffix;
    private Rigidbody rb;
    private Vector3 LookDirection = new Vector3();
    private Vector3 lastLookDirection = new Vector3();

    private bool targetingMode = false;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        LookDirection = rb.position;
    }


    private void FixedUpdate()
    {
        #region forward viewing only
        if (!targetingMode)
        {
            Vector3 newPosition;
            newPosition = new Vector3(Input.GetAxis("HorizontalP" + player), 0, -Input.GetAxis("VerticalP" + player)).normalized * walkSpeed;
            rb.MovePosition(rb.position + newPosition);

            if (Mathf.Abs(Input.GetAxis("HorizontalP" + player)) >= .1 || Mathf.Abs(Input.GetAxis("VerticalP" + player)) >= .1)
            {
                LookDirection = rb.position + new Vector3(-Input.GetAxis("HorizontalP" + player), 0, Input.GetAxis("VerticalP" + player)).normalized;
                Quaternion lookRot = Quaternion.LookRotation(rb.position - LookDirection);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, lookRot, turnSpeed);
            }
        }
        else
        {
            GetComponent<CrosshairControl>().CrosshairInstanced.transform.position += new Vector3(Input.GetAxis("HorizontalP" + player), 0, -Input.GetAxis("VerticalP" + player)).normalized * targetingSpeed;
        }
        
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown("joystick " + player + " button 3"))
        {
            if (!targetingMode)
            {
                targetingMode = true;
                GetComponent<CrosshairControl>().startTargetting();
            }
            else
            {
                if (Vector3.Distance(transform.position, GetComponent<CrosshairControl>().CrosshairInstanced.transform.position) < attackRange)
                {
                    targetingMode = false;
                    GetComponent<CrosshairControl>().doAttack(attacks[0]);
                }
            }
        }

        if (targetingMode)
        {
            if(Vector3.Distance(transform.position, GetComponent<CrosshairControl>().CrosshairInstanced.transform.position) > attackRange)
            {
                GetComponent<CrosshairControl>().CrosshairInstanced.GetComponentInChildren<Renderer>().material.SetColor(0, Color.red);
            }
            else
            {
                GetComponent<CrosshairControl>().CrosshairInstanced.GetComponentInChildren<Renderer>().material.SetColor(0, Color.green);
            }
        }
    }
}
