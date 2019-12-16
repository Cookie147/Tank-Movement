using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public AudioSource movementAudio;
    public AudioClip movingClip;
    public AudioClip idleClip;

    private const int IDLE = 0;
    private const int DRIVING = 1;
    private int state = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
        rb.velocity = Vector3.zero;
    }

    /*
     * makes the tank move according to the WASD or arrow keys input. 
     * also sets sounds according to the tank's movement state
     */
    private void Movement()
    {
        //Tank movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        //set the Tank's rotation
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveDirection != Vector3.zero)
        {
            rb.freezeRotation = false;
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
        }
        else
        {
            rb.freezeRotation = true;
        }


        int h = 0;
        int v = 0;
        float newSpeed = speed; 
        if (moveHorizontal != 0) h = 1;
        if (moveVertical != 0) v = 1;
        if (h == 0 && v == 0)
        {
            if(state == DRIVING)
            {
                state = IDLE;
                movementAudio.clip = idleClip;
                movementAudio.pitch = Random.Range(1 - 0.2f, 1 + 0.1f);
                movementAudio.Play();
            }
            return;
        }
        if(state == IDLE)
        {
            state = DRIVING;
            movementAudio.clip = movingClip;
            movementAudio.pitch = Random.Range(1 - 0.1f, 1 + 0.2f);
            movementAudio.Play();
        }
        newSpeed /= Mathf.Sqrt(h + v);


        transform.Translate(transform.right * -moveHorizontal * Time.deltaTime * newSpeed);
        transform.Translate(transform.forward * moveVertical * Time.deltaTime * newSpeed);

    }
}
