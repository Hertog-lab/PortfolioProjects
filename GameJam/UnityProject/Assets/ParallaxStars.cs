using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxStars : MonoBehaviour
{
        [SerializeField, Range(-1,1)] float scrollSpeed;

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(InfoManager.instance._currentPlayerVelocity * Time.deltaTime * scrollSpeed);
    }
}
