using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f;

    private void FixedUpdate()
    {
        transform.position += Vector3.right * Time.fixedDeltaTime * speed;
    }
}
