using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HeatSystem : MonoBehaviour
{
    public const int max_heat_magnitude = 100;
    public int heat = 0;

    private float heat_transfer_coef = 0.1f;

    public LinkableObject linkable_object;

    public bool is_source = false;
    public bool is_sink = false;

    internal Collider2D[] contacts;
    private int num_contacts;

    [SerializeField]
    private Collider2D my_collider;

    internal SpriteRenderer sprite_renderer;

    private const float time_per_heat_loss = 0.3f;
    private float heat_loss_timer = 0f;

    void Awake()
    {
        linkable_object = GetComponent<LinkableObject>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        contacts = new Collider2D[10];
        change_heat(0);
    }

    public virtual void change_heat(int heat_change)
    {
        if (is_source)
            return;
        heat = Math.Clamp(heat + heat_change, -max_heat_magnitude, max_heat_magnitude);
        if (sprite_renderer != null)
        {
            if (heat > 0)
                sprite_renderer.material.color = Color.Lerp(
                    Color.white,
                    Color.red,
                    (float)(heat) / (float)(max_heat_magnitude)
                );
            else
                sprite_renderer.material.color = Color.Lerp(
                    Color.white,
                    Color.cyan,
                    (float)(-heat) / (float)(max_heat_magnitude)
                );
        }
    }

    public void send_heat_particles(int heat_change, Vector3 pos, LinkConnection connection)
    {
        if (heat_change > 0)
            connection.p_systems[(int)LinkParticleSystem.P.HOT].add_to_particle_accumulator(
                (uint)Math.Abs(heat_change),
                pos
            );
        else
            connection.p_systems[(int)LinkParticleSystem.P.COLD].add_to_particle_accumulator(
                (uint)Math.Abs(heat_change),
                pos
            );
    }

    internal virtual int FixedUpdateChild()
    {
        return 1;
    }

    void FixedUpdate()
    {
        if (is_sink)
            return;

        int heat_change = 0;

        num_contacts = my_collider.GetContacts(contacts);

        HeatSystem other_heat_system;

        for (int i = 0; i < num_contacts; ++i)
        {
            other_heat_system = contacts[i].GetComponent<HeatSystem>();

            if (other_heat_system == null)
                continue;

            int heat_to_send = (int)(
                (heat - other_heat_system.heat) * other_heat_system.heat_transfer_coef
            );

            heat_change -= heat_to_send;

            other_heat_system.change_heat(heat_to_send);
        }

        for (
            LinkedListNode<LinkConnection> node = linkable_object.connections.First;
            node != null;
            node = node.Next
        )
        {
            if (node.Value.partner.currently_linked_connected_object == null)
                continue;

            other_heat_system =
                node.Value.partner.currently_linked_connected_object.GetComponent<HeatSystem>();

            if (other_heat_system == null)
                continue;

            if (other_heat_system.is_source)
                continue;

            int heat_to_send = (int)(
                (heat - other_heat_system.heat) * other_heat_system.heat_transfer_coef
            );

            heat_change -= heat_to_send;

            other_heat_system.send_heat_particles(
                heat_to_send,
                node.Value.transform.position,
                node.Value.partner
            );
        }

        heat_loss_timer += Time.fixedDeltaTime;
        int heat_loss = (int)(heat_loss_timer / time_per_heat_loss);
        heat_loss_timer -= heat_loss * time_per_heat_loss;

        change_heat(heat_change - Math.Sign(heat) * heat_loss * FixedUpdateChild());
    }
}
