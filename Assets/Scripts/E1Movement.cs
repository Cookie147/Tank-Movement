using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1Movement : MonoBehaviour
{

    public int speed;
    public Rigidbody rb;
    public GameObject playerTank;

    private int length, direction;
    private float waitTimer;
    private bool moving = false;
    private bool wait = false;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RandomMove();
    }

    private void RandomMove()
    {
        if (wait)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
                wait = false;
            return;
        }
        if (!moving)
        {
            if (Random.Range(0, 2) < 2)
            {
                NewMovement();
            }
            else
            {
                waitTimer = Random.Range(0, 2f) + 0.5f;
                wait = true;
            }
            //or maybe something else (like wait for some time)
        }
        Move();
    }

    private void NewMovement()
    {
        length = Random.Range(0, 4) + 2;//could add a function for more functionality
        direction = (direction + Random.Range(-2, 2) + 7) % 8;//same here
        startPos = transform.position;
        moving = true;
        //print("length: " + length + ", dir: " + direction);
    }

    private void Move()
    {
        int moveHorizontal = GetHorizontal();
        int moveVertical = GetVertical();
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
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, newRotation, Time.deltaTime * 8);
        }

        if ((transform.position - startPos).magnitude >= length)
        {
            moving = false;
            //waitTimer = 0.5f;
            //wait = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Hay"))
        {
            moving = false;
        }
    }

    /*
     * converts direction ∈ (0, 7) to the horizontal component
     * 
     * @returns -1, 0 or 1
     */
    private int GetHorizontal()
    {
        switch(direction)
        {
            case 1:
            case 2:
            case 3:
                return 1;
            case 5:
            case 6:
            case 7:
                return -1;
            default:
                return 0;
        }
    }
    /*
     * converts direction ∈ (0, 7) to the vertical component
     * 
     * @returns -1, 0 or 1
     */
    private int GetVertical()
    {
        switch (direction)
        {
            case 0:
            case 1:
            case 7:
                return 1;
            case 3:
            case 4:
            case 5:
                return -1;
            default:
                return 0;
        }
    }
}
