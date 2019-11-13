using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurn : MonoBehaviour
{

    public int rotationSpeed;
    public Rigidbody turret;
    public float fieldHeight, fieldWidth;
    public GameObject playground;

    // Start is called before the first frame update
    void Start()
    {
        fieldWidth = playground.transform.lossyScale.x * 10;
        fieldHeight = playground.transform.lossyScale.z * 10;
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
    }
}
