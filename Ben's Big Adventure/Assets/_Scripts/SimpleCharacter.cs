using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacter : MonoBehaviour {

    //Capsule character
    public CharacterController myController;
    public Transform cameraTransform;
    public float speed = 3f;
    public float gravityStrength = 9.8f;
    public float jumpSpeed = 5f;
    bool canJump = false;
    float verticalVelocity;

    // Update is called once per frame
    void Update () {
        //Sets up character postion
        Vector3 myVector = new Vector3(0, 0, 0);

        //get input from player
        myVector.x = Input.GetAxis("Horizontal");
        myVector.z = Input.GetAxis("Vertical");
        myVector = Vector3.ClampMagnitude(myVector, 1f);

        myVector = myVector * speed * Time.deltaTime;

        //rotate input by direction of camera
        //keeps input relative to camera
        Quaternion inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up));
        myVector = inputRotation * myVector;

        //Adjusts vertical velocity according to gravity
        verticalVelocity = verticalVelocity - gravityStrength*Time.deltaTime;

        //Enables character to jump
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                verticalVelocity += jumpSpeed;
            }
        }

        //adjust character postion according to new calculated velocity
        //jumping/ gravity etc
        myVector.y = verticalVelocity*Time.deltaTime;

        //Use input to move player
        CollisionFlags flags = myController.Move(myVector);

        //if in contact with ground
        if((flags & (CollisionFlags.Sides|CollisionFlags.Below)) != 0)
        {
            canJump = true;
            verticalVelocity = -1f;
        }
        else
        {
            canJump = false;
        }

	}
}
