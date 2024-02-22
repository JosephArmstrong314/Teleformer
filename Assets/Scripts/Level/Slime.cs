using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Hurtable
{
    [SerializeField]
    private BoxCollider2D my_collider;

    private static Vector2 dead_collider_pos = new Vector2(0.06720543f, -0.683046f);
    private static Vector2 dead_collider_size = new Vector2(0.8818951f, 0.4956124f);

    private static Vector2 alive_collider_pos = new Vector2(0.0f, -0.119266f);
    private static Vector2 alive_collider_size = new Vector2(2.001987f, 1.623172f);

    private Animator my_animator;
    private SpriteRenderer my_sprite;

    private float speed = 1.0f;

    [SerializeField]
    private Transform right_point;

    [SerializeField]
    private Transform left_point;

    public bool currently_seeking_right_point = false;

    private Rigidbody2D my_rigidbody;

    public bool dead = false;

    void Start()
    {
        my_collider = GetComponent<BoxCollider2D>();
        my_animator = GetComponent<Animator>();
        my_rigidbody = GetComponent<Rigidbody2D>();
        my_sprite = GetComponent<SpriteRenderer>();
        my_sprite.flipX = !currently_seeking_right_point;
    }

    public override void onHit()
    {
        // my_animator.SetTrigger("Hit");
        my_animator.Play("HitSlime");
    }

    public override void onDeath()
    {
        my_collider.offset = dead_collider_pos;
        my_collider.size = dead_collider_size;
        gameObject.layer = LayerMask.NameToLayer("DeadSlime");
        my_rigidbody.velocity = Vector2.zero;
        // this.enabled = false;
        if (!dead)
            // my_animator.SetTrigger("Death");
            my_animator.Play("DeathSlime");
        dead = true;
    }

    public override void onRevive()
    {
        my_collider.offset = alive_collider_pos;
        my_collider.size = alive_collider_size;
        // this.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        dead = false;
        // my_animator.SetTrigger("Revive");
        my_animator.Play("WalkSlime");
    }

    private void FixedUpdate()
    {
        if (dead)
            return;
        if (currently_seeking_right_point)
        {
            if (my_rigidbody.position.x >= right_point.position.x)
            {
                currently_seeking_right_point = false;
                my_sprite.flipX = !currently_seeking_right_point;
            }
        }
        else
        {
            if (my_rigidbody.position.x <= left_point.position.x)
            {
                currently_seeking_right_point = true;
                my_sprite.flipX = !currently_seeking_right_point;
            }
        }

        my_rigidbody.velocity = Vector2.right * (currently_seeking_right_point ? speed : -speed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!dead && col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().onDeath();
        }
    }
}
