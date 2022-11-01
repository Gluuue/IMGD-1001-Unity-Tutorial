using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Vector2 direction;
    public float moveSpeed = 1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        direction.y = Input.GetAxis("Vertical") * moveSpeed;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }
}
