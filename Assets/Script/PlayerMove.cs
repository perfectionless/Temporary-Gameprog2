using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float ascendSpeed = 5f;
    public float descendSpeed = 5f;

    private Vector3 movement;

    // Update is called once per frame

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate horizontal movement
        movement = transform.right * horizontal + transform.forward * vertical;
        movement *= speed;

        // Handle ascending and descending
        if (Input.GetButton("Jump"))
        {
            movement.y = ascendSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            movement.y = -descendSpeed;
        }
        else
        {
            movement.y = 0f;
        }

        // Move the player
        controller.Move(movement * Time.deltaTime);
    }
}
