using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator anim;
    public float speed = 2;
    public float maxSpeed = 4;
    private float currentSpeed;
    public float timeToMax = 1;
    public float rotationSpeed = 100;

    private float currentTime = 0;

// Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

// Update is called once per frame
    void Update()
    {
// Axes
        float turning = Input.GetAxis("Horizontal");
        float moving = Input.GetAxis("Vertical");
// If idle and turning
        if (moving == 0)
        {
            if (Input.GetKeyDown("right"))
                anim.SetTrigger("turnRight");
            if (Input.GetKeyDown("left"))
                anim.SetTrigger("turnLeft");
        } // Moving the character

        if (moving > 0)
        {
            transform.Translate(Vector3.forward * moving * currentSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * turning * rotationSpeed * Time.deltaTime);
            anim.SetBool("isWalking", true);
        }

        if (moving == 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isWalkingBackwards", false);
        }

        if (moving < 0)
        {
            transform.Translate(Vector3.forward * moving * speed / 2.0f * Time.deltaTime);
            transform.Rotate(-Vector3.up * turning * rotationSpeed * Time.deltaTime);
            anim.SetBool("isWalkingBackwards", true);
        }

// Jumping
        if (Input.GetButtonDown("Jump") && moving >= 0)
            anim.SetTrigger("Jump");
// Speed increment
        if (currentTime == 0 && moving > 0)
        {
            currentSpeed = speed;
            currentTime = Time.time;
        }

        if (moving > 0 && currentSpeed < maxSpeed)
        {
            currentTime += Time.deltaTime;
            currentSpeed += timeToMax * Time.deltaTime;
        }

        if (moving == 0)
        {
            currentTime = 0;
            if (currentSpeed > speed)
                currentSpeed -= timeToMax * Time.deltaTime * 2.0f;
        }

        anim.SetFloat("Speed", currentSpeed);
    }
}