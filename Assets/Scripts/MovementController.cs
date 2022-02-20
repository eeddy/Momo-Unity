using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed;
    public float upwardsForce;
    public Rigidbody2D rb;
    private Vector2 velocity;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            rb.velocity = Vector3.zero;
            velocity = new Vector2(-speed, 0);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            rb.velocity = Vector3.zero;
            velocity = new Vector2(speed, 0);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(new Vector2(0,1) * upwardsForce);
        }
    }
}
