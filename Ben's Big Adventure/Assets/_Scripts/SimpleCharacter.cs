using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacter : MonoBehaviour {

    //Capsule character
    public CharacterController myController;
    public Transform cameraTransform;
    public float speed = 3f;
    public float gravityStrength = 9.8f;
    public float jumpSpeed = 3f;
    bool canDoubleJump = false;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 groundedVelocity;
    Vector3 normal;

    // Update is called once per frame
    void Update()
    {
        //Sets up character postion
        Vector3 myVector = new Vector3(0, 0, 0);

        if (myController.isGrounded)
        {
            //get input from player
            myVector.x = Input.GetAxis("Horizontal");
            myVector.z = Input.GetAxis("Vertical");
            myVector = Vector3.ClampMagnitude(myVector, 1f);

            myVector = myVector * speed * Time.deltaTime;

            //rotate input by direction of camera
            //keeps input relative to camera
            Quaternion inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up));
            myVector = inputRotation * myVector;

        }
        else
        {
            myVector = groundedVelocity;
            myVector *= Time.deltaTime;
        }

        //Adjusts vertical velocity according to gravity
        verticalVelocity = verticalVelocity - gravityStrength * Time.deltaTime;

        //set newly calculated vertical velocity
        myVector.y = verticalVelocity * Time.deltaTime;

        //Use input to move player
        CollisionFlags flags = myController.Move(myVector);
        velocity = myVector / Time.deltaTime;
        

        //Enables character to jump
        if (Input.GetButtonDown("Jump"))
        {

            //enables double jump
            if (canDoubleJump)
            {
                //increase vertical velocity
                verticalVelocity += (jumpSpeed / 2);

                //ensures no more than 2 jumps may occur
                canDoubleJump = false;
            }
            if ((flags & CollisionFlags.Below) != 0)
            {
                //set grounded velocity
                groundedVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);

                //smooth movement down ramps
                verticalVelocity = -1f;

                //increase vertical velocity
                verticalVelocity += jumpSpeed;

                //enables a second double jump
                canDoubleJump = true;
            }
            if ( ( ((flags & CollisionFlags.Sides) != 0) & (flags & CollisionFlags.Below) == 0) ) 
            {
                groundedVelocity = Vector3.Reflect(groundedVelocity, normal);
                print("reached");
                //increase vertical velocity
                verticalVelocity += jumpSpeed;
            }
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        normal = hit.normal;
    }


}
    

