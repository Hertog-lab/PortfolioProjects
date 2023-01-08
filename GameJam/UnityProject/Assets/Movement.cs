using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    float x, y;

    float thrust;
    float maxThrust;
    float maxBoost;
    float maxVelocity = 70;
    float turnSpeed;

    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> collisionClips = new List<AudioClip>();
    [SerializeField] AudioSource thrusterAudio;
    [SerializeField] AudioSource boostAudio;
    [SerializeField] AudioSource collisionAudio;
    Rigidbody2D rb;

    bool boostIsActive;

    Vector2 direction;
    Vector2 oldDirection;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<CameraController>().target = transform;
        Camera.main.GetComponent<CameraZoom>().targetRB = rb;
        thrust = 500;
        maxThrust = thrust;
        maxBoost = thrust * 2;
        turnSpeed = 2;

    }

    private void Update()
    {

        if (InfoManager.instance._connectedSatalites == InfoManager.instance._disconectedSatalites && Input.GetKey(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        InfoManager.instance._currentPlayerVelocity = rb.velocity;
        if (thrusterAudio.volume < 1)
            thrusterAudio.volume += 0.1f;

        ReadInputs();

        if (!InfoManager.instance._scannerIsActive) {
            Thrust();
        }
        else {
            InfoManager.instance._lastPlayerLocation = transform;
            Brake();
        }

        ApplyBoost();
        RotateShip();
        LimitVelocity();

        if (boostIsActive && x != 0 || y != 0)
            boostAudio.volume = 1;
        
        else
            boostAudio.volume = 0.1f;

    }

    private void Brake()
    {
        if (rb.velocity.magnitude > 0)
            rb.AddForce(-rb.velocity * 100 * Time.deltaTime);
    }

    private void ReadInputs()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            boostIsActive = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            boostIsActive = false;

        if (Input.GetKey(KeyCode.Space)) {
            InfoManager.instance._scannerIsActive = true;
            thrusterAudio.pitch = 0.6f;
        }
        else {
            InfoManager.instance._scannerIsActive = false;
            thrusterAudio.pitch = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            turnSpeed = 0.8f;
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            turnSpeed = 2;
        }

    }

    private void RotateShip()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

    }

    private void Thrust()
    {
        thrust += Time.deltaTime;
        rb.AddForce(transform.up * thrust * y * Time.deltaTime);
        rb.AddForce(transform.right * thrust * x * Time.deltaTime);
    }

    private void ApplyBoost()
    {
        if (boostIsActive) {
            thrust += Time.deltaTime * 25;
            thrust = Mathf.Clamp(thrust, 0, maxBoost);
        }
        else
            thrust = Mathf.Clamp(thrust, 0, maxThrust);
    }
    private void LimitVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
            rb.AddForce(-rb.velocity * (rb.velocity.magnitude - maxVelocity));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int clip = UnityEngine.Random.Range(0, collisionClips.Count - 1);
        if (!collisionAudio.isPlaying)
            collisionAudio.PlayOneShot(collisionClips[clip]);
        if (collision.transform.GetComponent<EnemyMovement>()) {
            InfoManager.instance._hitPoints--;
            if (InfoManager.instance._hitPoints < 1)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
