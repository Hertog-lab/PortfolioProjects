using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;
    float speed = 180;
    float maxVelocity = 80;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {


        if (InfoManager.instance._scannerIsActive) {
            target = InfoManager.instance._lastPlayerLocation;
            Rotate();
        }
        else {
            Brake();
            target = transform;
        }
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
            rb.AddForce(-rb.velocity * (rb.velocity.magnitude - (maxVelocity - rb.velocity.magnitude)));

    }

    private void Rotate()
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100 * Time.deltaTime);
        Thrust(angle);



        if (angle > 180)
            Brake();

    }

    private void Thrust(float angle)
    {
        rb.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Force);
    }

    private void Brake()
    {
        if (rb.velocity.magnitude > 0)
            rb.AddForce(-rb.velocity * 3 * Time.deltaTime);
    }
}
