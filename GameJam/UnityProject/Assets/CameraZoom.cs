using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Rigidbody2D targetRB;
    private void LateUpdate()
    {
        if (targetRB.velocity.magnitude < 40)
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, (12 + targetRB.velocity.magnitude), 0.001f);
    }
}
