using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float movementSpeed;
    private Rigidbody2D rb2d;

    public Vector3 direction = Vector3.zero;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = direction * movementSpeed * Time.fixedDeltaTime;
        rb2d.MovePosition(transform.position + movementVector);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthManager>().OnDamageEvent.Invoke(damage, TeamFlag.Enemy, transform);
            Destroy(gameObject);
        }
    }
}
