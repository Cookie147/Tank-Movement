using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed = 8;
    public int bounceCount;
    public int maxBounce = 1;
    public Rigidbody shell;
    public string type = "normal";//to be automatized later

    // Start is called before the first frame update
    void Start()
    {
        shell.velocity = new Vector3(Mathf.Sin(shell.transform.rotation.y) * speed, 0, Mathf.Cos(shell.transform.rotation.y) * speed);
        print(shell.velocity.ToString());
    }

    // Update is called once per frame
    void Update()
    {
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
        print("trigger occurred");
        if (other.CompareTag("Wall") || (other.CompareTag("Hay") && type == "Normal"))
        {
            //++bounceCount;
            if (bounceCount > maxBounce)
            {
                Destroy(shell);
                return;
            }
            print("gonna bounce");
            Vector3 v = shell.velocity;
            Vector3 w = v;
            v.z = -v.z;
            shell.velocity.Set(v.x, v.y, v.z);
            print("before: " + v.ToString() + ", after: " + w.ToString());
        }
        else if(other.CompareTag("Hay"))
        {

        }
        else if(other.CompareTag("Tank"))
        {
            
        }
    }

    /**
     * bounces off a wall in a mathematically ideal way
     */
    private void Bounce()
    {
    }
}
