using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed = 8;
    public int bounceCount;
    public int maxBounce = 1;
    public Rigidbody shell;
    public string type = "normal";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);

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
        if(other.tag == "wall" || (other.tag == "hay" && type == "normal"))
        {
            ++bounceCount;
            if (bounceCount > maxBounce)
            {
                Destroy(shell);
                return;
            }
            Bounce();
        }
        else if(other.tag == "hay")
        {

        }
        else if(other.tag == "tank")
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
