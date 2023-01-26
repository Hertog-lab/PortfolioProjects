using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Kart : MonoBehaviour
{
    [Header("Speed Values")]
    public float p_acceleration;
    public Vector3 p_direction = new Vector3(0f,0f,1f);
    public bool p_drifting = false;

    [SerializeField] private float maxSpeedValue;

    [Header("KeyCodes")]
    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    private Transform kartColTrans;
    private Rigidbody rbody;

    private void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody>();
        kartColTrans = gameObject.transform.parent.GetChild(0).transform;
    }

    private void Input()
    {
    	//adjusts the base strength of the rotation
        float driftStrenght = 3;
        
        //Forward movement based on adding to the direction vector
        if (UnityEngine.Input.GetKey(forward))
        { p_direction += p_direction * (0.5f * Time.deltaTime); }
        
        //Drifting/Turning by changing the z with the x and vice versa (plus additional modifiers to give a good feel while driving)
        if (UnityEngine.Input.GetKey(left))
        { p_direction = new Vector3(p_direction.x - (p_direction.z * driftStrenght * Time.deltaTime), 0f, p_direction.z + (p_direction.x * driftStrenght * Time.deltaTime)); }
        if (UnityEngine.Input.GetKey(right))
        { p_direction = new Vector3(p_direction.x + (p_direction.z * driftStrenght * Time.deltaTime), 0f, p_direction.z - (p_direction.x * driftStrenght * Time.deltaTime)); }
		//Adds extra drifting modifiers 
        if (UnityEngine.Input.GetKey(left) || UnityEngine.Input.GetKey(right))
        {
            p_drifting = true;
            p_direction -= p_direction * (0.75f * Time.deltaTime);
            float x = Mathf.Abs(p_direction.x);
            float z = Mathf.Abs(p_direction.z);
            if (x > z) {p_direction.z += p_direction.z * (2.5f * Time.deltaTime);}
            if (x < z) {p_direction.x += p_direction.x * (2.5f * Time.deltaTime);}
		//makes sure the drifting doesnt always happen
        } else {p_drifting = false;}
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Wall bounce achieved by flipping the strongest direction value determined by the normal of the collision object (only works with walls in this case)
        if (collision.transform.GetComponent<Wall>() == true)
        {
            p_direction = Vector3.Reflect(p_direction, collision.GetContact(0).normal);
            p_direction = p_direction * 0.50f;
        }
    }

    private void MovementAppliance()
    {
    	//updates the acceleration used by the camera to predict the kart movement and incase the kart is slow used to update the speed a bit to avoid standstills 
        p_acceleration = p_direction.magnitude;
        //
        if (p_acceleration < 5.3f)
        { p_direction += p_direction * Time.deltaTime; }
        //makes sure the kart rotates the right way
        kartColTrans.rotation = Quaternion.LookRotation(p_direction, Vector3.up);
        //brings the visual part of the kart closer to the "accual" location of the kart
        kartColTrans.position = Vector3.Lerp(kartColTrans.position, transform.position, 0.05f);
        //keeps the speed within the correct values
        p_direction = new Vector3(Mathf.Clamp(p_direction.x, -maxSpeedValue, maxSpeedValue), 0f, Mathf.Clamp(p_direction.z, -maxSpeedValue, maxSpeedValue));
        //applies the movement
        rbody.position += p_direction * Time.deltaTime;
    }

    private void Update()
    {
    //updates input and afterwards the movement
        Input();
        MovementAppliance();
    }
    /*
    
    //[SerializeField] private GameObject camera1;
    //[SerializeField] private GameObject camera2;

    private void Update()
    {
        if (playerControl == true) { Input(); }
        SpeedControll();
        MovementApplicance();
    }

    private void Input()
    {
        //if (UnityEngine.Input.GetKey(KeyCode.F)) { camera1.GetComponent<Camera>().enabled = false; camera2.GetComponent<Camera>().enabled = true; }
      //  else { camera1.GetComponent<Camera>().enabled = true; camera2.GetComponent<Camera>().enabled = false; }
        
        if (UnityEngine.Input.GetAxisRaw("Vertical") > 0f)
        {
            direction += direction * (speedIncrease * 0.1f * Time.deltaTime);
        }
        if      (UnityEngine.Input.GetAxisRaw("Horizontal") > 0f) {Drifting(false); p_drifting = true;}
        else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0f) {Drifting(true ); p_drifting = true;}
        else    
        {
            rotationStrenghtModifier = minRotateStrenght;
            p_drifting = false;
        }
    }

    private void Drifting(bool left)
    {
        float driftStrength = (p_acceleration < 5f) ? (direction.x + direction.z) * 7.5f : 0f;
        rotationStrenghtModifier = Mathf.Clamp(rotationStrenghtModifier += rotationStrenghtModifier * Time.deltaTime,minRotateStrenght,10f);
        direction = (left == false) 
            ? direction = new Vector3(direction.x += driftStrength + (direction.z * rotationStrenghtModifier) * Time.deltaTime, direction.y,direction.z -= (direction.x * rotationStrenghtModifier) * Time.deltaTime) 
            : direction = new Vector3(direction.x -= driftStrength + (direction.z * rotationStrenghtModifier) * Time.deltaTime, direction.y,direction.z += (direction.x * rotationStrenghtModifier) * Time.deltaTime);
        direction = (left == false) 
            ? direction = new Vector3(direction.x -= oldDirection.z * Time.deltaTime, direction.y,direction.z += oldDirection.x * Time.deltaTime) 
            : direction = new Vector3(direction.x += oldDirection.z * Time.deltaTime, direction.y,direction.z -= oldDirection.x * Time.deltaTime);

    }

    private void SpeedControll()
    {
        if (p_acceleration < 5.3f)
        {
            direction += direction * (speedIncrease * 0.1f * Time.deltaTime);
        }
    }
    
    private void MovementApplicance()
    {
        acceleration = new Vector3(direction.x, 0f, direction.z);
        p_acceleration = acceleration.magnitude;
        oldDirection = direction;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.position += acceleration * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        
        if (collision.transform.GetComponent<Wall>() == true)
        {
            direction = Vector3.Reflect(direction, collision.GetContact(0).normal);
        }
        if (collision.transform.GetComponent<MouthHazard>() == true)
        {
            playerControl = false;
            Debug.Log("gobble gobble");
            direction = new Vector3(direction.x += direction.z * Time.deltaTime, direction.y,direction.z -= direction.x * Time.deltaTime);
        }
        
    }
  */
}
