using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurn : MonoBehaviour
{

    public int rotationSpeed;
    public Rigidbody turret;
    public float fieldHeight, fieldWidth;
    public GameObject playground;
    public Camera cam;

    private int floorMask;
    private const float rayLength = 100f;

    // Start is called before the first frame update
    void Start()
    {
        playground = GameObject.Find("Play Area");
        fieldWidth = playground.transform.lossyScale.x * 10;
        fieldHeight = playground.transform.lossyScale.z * 10;
        cam = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
    }

    /**
     * Sets the rotation of the turret according to the postion of the mouse
     * the turret will always be turned in the direction where the mouse is
     */
    private void SetRotation()
    {
        if (!Input.mousePresent)
        {
            print("no mouse found!");
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        /*
        //works exactly but doesn't support multiple inputs, e.g. doesn't turn while movement buttons are pressed
        Ray r = cam.ScreenPointToRay(mousePos);

        if(Physics.Raycast(r, out RaycastHit hit, rayLength, floorMask))
        {
            Vector3 direction = hit.point - transform.position;
            //direction.y = 0f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //turret.MoveRotation(Quaternion.LookRotation(direction));
        }
        //*/


        //not exactly the direction the mouse is
        //this could be solved using an arrow that indicates exactly where the shot will go (just let it have the same rotation as the turret)
        
        Vector2 tankOnScreen = cam.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2) cam.ScreenToViewportPoint(mousePos);
        float angle = -90f - Mathf.Atan2(tankOnScreen.y - mouseOnScreen.y, tankOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
    }
}
