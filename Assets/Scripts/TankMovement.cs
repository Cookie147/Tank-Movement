using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        //Tank movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveVertical == 0 && moveHorizontal == 0) return;
        speed /= Mathf.Sqrt(Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));


        transform.Translate(transform.right * -moveHorizontal * Time.deltaTime * speed);
        transform.Translate(transform.forward * moveVertical * Time.deltaTime * speed);

        //Tank rotation
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, newRotation, Time.deltaTime * 8);
        }
    }
}
