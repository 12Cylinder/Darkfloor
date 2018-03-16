using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public enum PlayerSetting
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    public PlayerSetting Player;

    public float walkSpeed;
    public float turnSpeed;

    private string playerSuffix;
    private Rigidbody rb;
    private Vector3 LookDirection = new Vector3();
    private Vector3 lastLookDirection = new Vector3();


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        switch (Player)
        {
            case PlayerSetting.Player1:
                playerSuffix = "P1";
                break;
            case PlayerSetting.Player2:
                playerSuffix = "P2";
                break;
        }

        LookDirection = rb.position;
    }


    private void FixedUpdate()
    {
        #region forward viewing only
        Vector3 newPosition;
        newPosition = new Vector3(Input.GetAxis("Horizontal" + playerSuffix), 0, -Input.GetAxis("Vertical" + playerSuffix)).normalized * walkSpeed;
        rb.MovePosition(rb.position + newPosition);

        if (Mathf.Abs(Input.GetAxis("HorizontalLook" + playerSuffix)) >= .75 || Mathf.Abs(Input.GetAxis("VerticalLook" + playerSuffix)) >= .75)
        {
            LookDirection = rb.position + new Vector3(-Input.GetAxis("HorizontalLook" + playerSuffix), 0, -Input.GetAxis("VerticalLook" + playerSuffix));
            Quaternion lookRot = Quaternion.LookRotation(rb.position - LookDirection);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, lookRot, turnSpeed);
        }
        #endregion

        #region relative viewing
        /*Vector3 newRotation;
        newRotation = new Vector3(0, Input.GetAxis("HorizontalLook" + playerSuffix) * turnSpeed, 0);
        transform.Rotate(newRotation);


        Vector3 newPosition;
        Vector3 fwd = Vector3.forward;
        newPosition = new Vector3(Input.GetAxis("Horizontal" + playerSuffix), 0, -Input.GetAxis("Vertical" + playerSuffix) * Vector3.forward.z).normalized * walkSpeed;
        newPosition = transform.TransformDirection(newPosition);
        rb.MovePosition(transform.position + newPosition);*/
        #endregion

    }
}
