using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBeamColision : MonoBehaviour
{
    [SerializeField]
    private GameObject effect;

    void OnCollisionEnter2D(Collision2D col)
    {
        handle_collision(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        handle_collision(col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (
                col.gameObject
                    .GetComponent<PlayerController>()
                    .link_manager.break_all_player_links()
            )
                instantiate_effect(col.transform.position);
            return;
        }

        LinkableObject obj = col.gameObject.GetComponent<LinkableObject>();
        if (obj == null || obj.connections.First == null)
        {
            LinkConnection connection = col.gameObject.GetComponent<LinkConnection>();
            if (connection != null)
            {
                instantiate_effect(col.transform.position);
                connection.link.delete_connections();
            }
            return;
        }
        instantiate_effect(col.transform.position);
        obj.break_all_links();
    }

    void handle_collision(Collision2D col)
    {
        LinkableObject obj = col.gameObject.GetComponent<LinkableObject>();
        if (obj == null || obj.connections.First == null)
            return;
        instantiate_effect(col.transform.position);
        obj.break_all_links();

        Rigidbody2D obj_rigidbody = obj.GetComponent<Rigidbody2D>();
        if (obj_rigidbody != null)
            obj_rigidbody.AddForce(
                (obj_rigidbody.position - (Vector2)transform.position).normalized * 1.2f,
                ForceMode2D.Impulse
            );
    }

    void instantiate_effect(Vector2 point)
    {
        GameObject eff = Instantiate(effect, transform.parent);
        eff.transform.position = point;
        eff.SetActive(true);
    }
}
