using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurn : MonoBehaviour
{

    public int rotationSpeed;
    public Rigidbody turret;
    public float fieldHeight, fieldWidth;
    public GameObject playground;

    private Camera cam;
    private int floorMask;
    private const float rayLength = 100f;

    // Start is called before the first frame update
    void Start()
    {
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
        
        Vector2 tankOnScreen = cam.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2) cam.ScreenToViewportPoint(mousePos);
        float angle = -90f - Mathf.Atan2(tankOnScreen.y - mouseOnScreen.y, tankOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        //transform.rotation = 
        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        //*/
        /*
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, cam.pixelHeight - mousePos.y, cam.nearClipPlane));

        print(mousePos.ToString() + " ⇒ " + worldPos.ToString());


        *//*
        //get mouse and tank position (mouse position is according to its position on the computer screen, NOT in-game)
        Vector3 mousePos = Input.mousePosition;
        Vector3 tankPos = turret.transform.position;
        //scale mouse position to a useful format and project it onto the in-game plane
        mousePos.y = mousePos.y / Screen.height * fieldHeight;
        mousePos.x = mousePos.x / Screen.width * fieldWidth;
        mousePos.z = mousePos.y;
        mousePos.y = 1.1f;
        //calculate and set the direction the turret should point at
        Vector3 direction = tankPos - mousePos;
        Vector3 a = new Vector3(0, 0, 0);
        if(mousePos.z > tankPos.z)
            a.y = Mathf.Atan(direction.x / direction.z) * 180 / Mathf.PI;
        else
            a.y = 180 + Mathf.Atan(direction.x / direction.z) * 180 / Mathf.PI;
        turret.transform.SetPositionAndRotation(tankPos, Quaternion.Euler(a));
        //*/
    }
}
