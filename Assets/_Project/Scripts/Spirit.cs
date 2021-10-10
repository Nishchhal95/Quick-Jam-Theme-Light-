using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{


    public Transform player;
    private Rigidbody rb;
    private Vector3 movement;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0f, angle, 0f);
        direction.Normalize();
        movement = direction;
        moveSpirit(direction);
    }

    void moveSpirit(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
    }
}
