using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1Movement : MonoBehaviour
{

    public int speed;
    public int slerpSpeed;
    public Rigidbody rb;
    public Rigidbody turret;
    public GameObject playerTank;
    public AudioSource movementAudio;
    public AudioClip movingClip;
    public AudioClip idleClip;

    private const int IDLE = 0;
    private const int DRIVING = 1;
    private int state = 0;
    private int length, direction;
    private float oldAngle;
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
        AdjustTurretRotation();
    }

    private void RandomMove()
    {
        if (wait)
        {
            if(state == DRIVING)
            {
                state = IDLE;
                movementAudio.clip = idleClip;
                movementAudio.pitch = Random.Range(1 - 0.2f, 1 + 0.1f);
                movementAudio.Play();
            }
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
                wait = false;
            return;
        }
        if (!moving)
        {
            if (Random.Range(0, 3) < 2)
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
        //set the sound
        if (state == IDLE)
        {
            state = DRIVING;
            movementAudio.clip = movingClip;
            movementAudio.pitch = Random.Range(1 - 0.1f, 1 + 0.2f);
            movementAudio.Play();
        }
    }

    /*
     * generates a new movement in the following way:
     * 
     * @var length will contain the length of the intended way
     * @var direction will contain the direction of the intended way (where direction * 45° will give you the angle in a clockwise manner, starting with 0 at the top)
     * @var moving is set to true so we dont calculate a new movement every frame
     * @var startPos contains the information where the current movement started, so we can calculate the distance travalled and compare it to length
     */
    private void NewMovement()
    {
        length = Random.Range(0, 4) + 2;//could add a function for more functionality
        direction = (direction + Random.Range(-2, 2) + 7) % 8;//same here
        startPos = transform.position;
        moving = true;
        //print("length: " + length + ", dir: " + direction);
    }

    /*
     * moves the tank in the mannar explained above (newMovement)
     * the magnitude of the (imaginary) speed vector will always be "speed" units
     */
    private void Move()
    {
        int moveHorizontal = GetHorizontal();
        int moveVertical = GetVertical();
        float newSpeed = speed;
        //if they are both 0 we don't have any movement at the moment
        if (moveHorizontal == 0 && moveVertical == 0) return;
        //if both directions are non-zero, we are moving diagonally. As we want the magnitude of the speed vector to be "speed" all the time, we need to divide by sqrt(2)
        if (moveHorizontal != 0 && moveVertical != 0) newSpeed /= Mathf.Sqrt(2f);

        //move
        transform.Translate(transform.right * -moveHorizontal * Time.deltaTime * newSpeed);
        transform.Translate(transform.forward * moveVertical * Time.deltaTime * newSpeed);

        //Tank rotation
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, newRotation, Time.deltaTime * slerpSpeed);
        }

        //check if it has moved at least "length" units, if so then set moving to false so that a new movement will be generated
        if ((transform.position - startPos).magnitude >= length)
        {
            moving = false;
            //waitTimer = 0.5f;
            //wait = true;
        }
    }
    
    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Hay"))
        {
            /*
             * IMPORTANT: test wall hits in "OnCollisionStay", not Enter
             * with "Enter" it can happen that the tank will get a new direction that leads into the wall again. Since it 
             * already touches the wall, this won't trigger a new
             * collision, thus the tank will just stay there and try to move into the wall.
             * 
             * if a wall is hit but it is not in the direction the tank wants to move, that does not pose a problem, so test if the wall
             * ahead is the same as the one we hit.
             */

            if(Physics.Raycast(transform.position, Quaternion.Euler(new Vector3(0, direction * 45)) * Vector3.forward, out RaycastHit hit))
            {
                if(hit.transform.gameObject == collision.gameObject)
                {
                    moving = false;
                }
            }
        }
    }

    /*
     * adjusts the turret rotation according to the rotation of the tank made when changing direction. 
     * we want to have the turret rotate individually, e.g. rotation of the tank should not affect the rotation of the turret
     * 
     * note that this probably isn't the most elegant solution to this problem, but it works. Feel free to find another way
     */
    private void AdjustTurretRotation()
    {
        float newAngle = rb.transform.rotation.eulerAngles.y;
        Vector3 newRotation = new Vector3(0, oldAngle - newAngle, 0);
        turret.transform.rotation *= Quaternion.Euler(newRotation);
        oldAngle = newAngle;
    }

    /*
     * converts direction ∈ (0, 7) to the horizontal component needed to create a vector that points to the wanted direction
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
     * converts direction ∈ (0, 7) to the vertical component needed to create a vector that points to the wanted direction
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
