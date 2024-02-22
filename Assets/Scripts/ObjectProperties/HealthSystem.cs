using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int max_health = 100;
    public int health;

    [SerializeField]
    private UnityEngine.UI.Slider health_bar;

    public LinkableObject linkable_object;

    [SerializeField]
    public LinkParticleSystem ps_damage_self;

    [SerializeField]
    public LinkParticleSystem ps_healing_self;

    public Hurtable self_hurtable;

    void Awake()
    {
        linkable_object = GetComponent<LinkableObject>();
        self_hurtable = GetComponent<Hurtable>();
        health = max_health;
        health_bar.maxValue = max_health;
        health_bar.value = health;
    }

    public void change_health(int change)
    {
        if (gameObject.name.Equals("C Slime1"))
            Debug.Log(change);
        if (health == 0 && change > 0)
            self_hurtable.onRevive();
        health = Math.Clamp(health + change, 0, max_health);
        health_bar.value = health;
        if (health == 0)
            self_hurtable.onDeath();
        else if (change < 0)
            self_hurtable.onHit();
    }

    public void do_damage(int damage)
    {
        int count = linkable_object.connections.Count + 1;
        int floored_damage_per = damage / count;
        int remainder = damage - floored_damage_per * count;

        remainder = send_health_particles(ps_damage_self, floored_damage_per, remainder, count--);

        for (
            LinkedListNode<LinkConnection> node = linkable_object.connections.First;
            node != null;
            node = node.Next
        )
        {
            remainder = send_health_particles(
                node.Value.partner.p_systems[(int)LinkParticleSystem.P.DAMAGE],
                floored_damage_per + 1,
                remainder,
                count--
            );
        }
    }

    public void do_healing(int healing)
    {
        int count = linkable_object.connections.Count + 1;
        int floored_healing_per = healing / count;
        int remainder = healing - floored_healing_per * count;

        remainder = send_health_particles(ps_healing_self, floored_healing_per, remainder, count--);

        for (
            LinkedListNode<LinkConnection> node = linkable_object.connections.First;
            node != null;
            node = node.Next
        )
        {
            remainder = send_health_particles(
                node.Value.partner.p_systems[(int)LinkParticleSystem.P.HEALING],
                floored_healing_per,
                remainder,
                count--
            );
        }
    }

    int send_health_particles(
        LinkParticleSystem ps,
        int floored,
        int remaining,
        int count_sinks_left
    )
    {
        System.Random r = new System.Random();

        if (remaining > 0 && r.Next(0, count_sinks_left) < remaining)
        {
            floored += 1;
            remaining -= 1;
        }

        ps.add_to_particle_accumulator((uint)floored, transform.position);

        return remaining;
    }

    // void FixedUpdate()
    // {
    //     if (Input.GetButtonDown("Energize"))
    //         do_damage(10);
    //     if (Input.GetButtonDown("AimStop"))
    //         do_healing(10);
    // }
}
