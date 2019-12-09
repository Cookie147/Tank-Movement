using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    //public Rigidbody rb;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
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
        int h = 0;
        int v = 0;
        float newSpeed = speed; 
        if (moveHorizontal != 0) h = 1;
        if (moveVertical != 0) v = 1;
        if (h == 0 && v == 0) return;
        newSpeed /= Mathf.Sqrt(h + v);


        transform.Translate(transform.right * -moveHorizontal * Time.deltaTime * newSpeed);
        transform.Translate(transform.forward * moveVertical * Time.deltaTime * newSpeed);

        //Tank rotation
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
        }
    }
}
