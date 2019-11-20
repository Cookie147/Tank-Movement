using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed;
    public int bounceCount;
    public int maxBounce;
    public Rigidbody shell;
    public GameObject shot;
    public string type = "normal";//to be automatized later

    // Start is called before the first frame update
    void Start()
    {
        shell = GetComponent<Rigidbody>();
        shot = shell.gameObject;
        speed = 8;
        print(shot.transform.rotation.y);
        float x = Mathf.Sin(shell.rotation.y * Mathf.PI / 180) * speed;
        float z = Mathf.Cos(shell.rotation.y * Mathf.PI / 180) * speed;
        //print("x: " + x + ", z: " + z);
        shell.velocity = new Vector3(x, 1, z);
        //print(shell.velocity.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //print(shell.velocity.ToString());
        //transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
        //if (Mathf.Abs(shell.transform.position.x) > 100 || Mathf.Abs(shell.transform.position.z) > 100)
          //  Destroy(shell);
    }

    /**
     * Checks unit collisions and reacts correspondingly:
     * wall: bounce if bounce limit has not yet been reached (destroy this shell otherwise)
     * hay: same as wall for normal type shells, set hay aflame if shell type is 'fast'
     * tank: destroy tank (and shell)
     * mine: do nothing as the mine itself checks for unit collisions and triggers if hit by a shell
     * (shield: destroy shell and weaken shield)
     */
    private void OnTriggerEnter(Collider other)
    {
        //print("trigger occurred");
        if (other.CompareTag("Wall") || (other.CompareTag("Hay") && type == "Normal"))
        {
            ++bounceCount;
            if (bounceCount > maxBounce)
            {
                Destroy(shot);
                return;
            }
            Bounce();
            
        }
        else if(other.CompareTag("Hay"))
        {

        }
        else if(other.CompareTag("Tank"))
        {
            
        }
        else if(other.CompareTag("Shot"))
        {
            //some animation

            Destroy(other);
            Destroy(shot);
        }
    }

    /**
     * bounces off a wall in a mathematically ideal way
     */
    private void Bounce()
    {
        print("gonna bounce");

        Vector3 v = shell.velocity;
        Vector3 w = v;
        shell.velocity = new Vector3(v.x, v.y, -v.z);
        print("before: " + w.ToString() + ", after: " + v.ToString() + ", velocity: " + shell.velocity.ToString());
    }
}
