using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float time_limit = 5f;
    private float time_elapsed = 0f;
    private LinkConnection connection;

    [SerializeField]
    private LayerMask what_is_solid_to_link_projectiles;

    private bool collided = false;

    public void Init(float angle, LinkConnection set_connection)
    {
        Rigidbody2D self = gameObject.GetComponent<Rigidbody2D>();
        self.simulated = true;
        self.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        this.enabled = true;
        connection = set_connection;
    }

    void FixedUpdate()
    {
        time_elapsed += Time.fixedDeltaTime;
        if (time_elapsed > time_limit)
            connection.link.delete_connections();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collided)
            return;

        if (((0b1 << other.gameObject.layer) & what_is_solid_to_link_projectiles.value) == 0)
            return;

        // Debug.Log("col: " + other.gameObject.name);

        if (other.transform == connection.partner.transform.parent)
        {
            connection.link.delete_connections();
            return;
        }

        LinkableObject hit = other.gameObject.GetComponent<LinkableObject>();

        if (hit == null)
        {
            connection.link.delete_connections();
            return;
        }

        connection.transform.parent = hit.transform;
        if (hit.movable)
            connection.transform.position = hit.transform.position;

        connection.link.link();

        // Debug.Log("destroy col: " + other.gameObject.name);

        collided = true;

        Destroy(this.gameObject);
    }
}
