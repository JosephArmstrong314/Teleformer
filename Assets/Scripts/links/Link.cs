using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link
{
    internal GameObject projectile_obj = null;
    internal GameObject connection_obj = null;

    public bool linked = false;
    public bool energized = false;

    internal FixedJoint2D joint = null;
    internal SpringJoint2D magnet_joint = null;

    public LinkedListNode<Link> manager_node;
    public int player_index = 0xFFFF;

    internal LinkConnection connection_1 = null;
    internal LinkConnection connection_2 = null;

    internal bool is_connection_2_on_shooter = false;
    public Transform shooter_tip = null;

    public void Init(LinkedListNode<Link> node)
    {
        manager_node = node;

        // if (projectile_obj == null)
        projectile_obj = GameObject.Find("LinkProjectile");

        // if (connection_obj == null)
        connection_obj = GameObject.Find("LinkConnection");
    }

    public void Init(LinkedListNode<Link> node, Transform set_shooter_tip)
    {
        Init(node);
        shooter_tip = set_shooter_tip;
    }

    internal void spawn_joint()
    {
        if (connection_1.currently_linked_connected_object.movable)
        {
            joint =
                connection_1.currently_linked_connected_object.gameObject.AddComponent<FixedJoint2D>();
            if (connection_2.currently_linked_connected_object.movable)
                joint.connectedBody =
                    connection_2.currently_linked_connected_object.GetComponent<Rigidbody2D>();
            joint.enableCollision = false;
        }
        else if (connection_2.currently_linked_connected_object.movable)
        {
            joint =
                connection_2.currently_linked_connected_object.gameObject.AddComponent<FixedJoint2D>();
            joint.enableCollision = false;
        }
    }

    internal void spawn_magnet()
    {
        if (connection_1.currently_linked_connected_object.movable)
        {
            magnet_joint =
                connection_1.currently_linked_connected_object.gameObject.AddComponent<SpringJoint2D>();
            if (connection_2.currently_linked_connected_object.movable)
                magnet_joint.connectedBody =
                    connection_2.currently_linked_connected_object.GetComponent<Rigidbody2D>();
            magnet_joint.enableCollision = true;
            magnet_joint.autoConfigureDistance = false;
            magnet_joint.breakForce = Mathf.Infinity;
            magnet_joint.breakTorque = Mathf.Infinity;
            // magnet_joint.distance = Vector2.Distance(
            //     connection_1.transform.position,
            //     connection_2.transform.position
            // );
            magnet_joint.distance = 0f;
            magnet_joint.frequency = 0.55f;
        }
        else if (connection_2.currently_linked_connected_object.movable)
        {
            magnet_joint =
                connection_2.currently_linked_connected_object.gameObject.AddComponent<SpringJoint2D>();
            magnet_joint.enableCollision = true;
            magnet_joint.autoConfigureDistance = false;
            magnet_joint.breakForce = Mathf.Infinity;
            magnet_joint.breakTorque = Mathf.Infinity;
            // magnet_joint.distance = Vector2.Distance(
            //     connection_1.transform.position,
            //     connection_2.transform.position
            // );
            magnet_joint.distance = 0f;
            magnet_joint.frequency = 0.55f;
        }
    }

    // public override void spawnParticles(ParticleSystem ps, Vector2 emitter_pos)
    // {
    //     if (energized && num_movable > 0)
    //     {
    //         float new_num = (((FixedJoint2D)joint).reactionForce.magnitude);
    //         new_num *= source.rigidbody.velocity.magnitude + sink.rigidbody.velocity.magnitude;
    //         force_accumulator += (int)new_num;

    //         int num_particles = (force_accumulator / force_per_particle);

    //         var emitParams = new ParticleSystem.EmitParams();
    //         emitParams.position = emitter_pos;
    //         ps.Emit(emitParams, num_particles);
    //         force_accumulator %= force_per_particle;
    //     }
    // }

    internal void remove_joint()
    {
        if (joint != null)
            UnityEngine.Object.Destroy(joint);
    }

    public void energize()
    {
        if (energized || !linked)
            return;

        spawn_joint();

        energized = true;
    }

    public void unenergize()
    {
        if (!energized)
            return;

        remove_joint();

        energized = false;
    }

    internal void unlink()
    {
        unenergize();

        if (magnet_joint != null)
            UnityEngine.Object.Destroy(magnet_joint);

        if (connection_1 != null)
        {
            connection_1.forget_currently_connected_linked_object();
            connection_1.set_color_normal();
        }

        if (connection_2 != null)
        {
            connection_2.forget_currently_connected_linked_object();
            connection_2.set_color_normal();
        }

        linked = false;
    }

    public bool link()
    {
        unlink();

        if (connection_1 == null || connection_2 == null)
            return false;

        LinkableObject obj1 = connection_1.GetConnectedLinkableObject();

        if (obj1 == null)
            return false;

        LinkableObject obj2 = connection_2.GetConnectedLinkableObject();

        if (obj2 == null)
            return false;

        if (obj1 == obj2)
            return false;

        connection_1.set_currently_connected_linked_object(obj1);
        connection_2.set_currently_connected_linked_object(obj2);

        if (obj1.magnetic || obj2.magnetic)
        {
            connection_1.set_color_magnetic();
            connection_2.set_color_magnetic();

            spawn_magnet();
        }

        return (linked = true);
    }

    internal void instantiate_connections(
        Transform obj_trans1,
        Transform obj_trans2,
        bool set_is_connection_2_on_shooter
    )
    {
        connection_1 = GameObject.Instantiate(connection_obj).GetComponent<LinkConnection>();
        connection_1.transform.parent = obj_trans1;
        connection_1.transform.position = obj_trans1.position;

        connection_2 = GameObject.Instantiate(connection_obj).GetComponent<LinkConnection>();
        connection_2.transform.parent = obj_trans2;
        connection_2.transform.position = obj_trans2.position;

        connection_1.Init(this, connection_2, true);
        connection_2.Init(this, connection_1, false);

        if (set_is_connection_2_on_shooter)
        {
            connection_2.setSpriteEnabled(false);
            is_connection_2_on_shooter = true;
        }
    }

    public void add_to_gun_tip()
    {
        if (is_connection_2_on_shooter && connection_2.transform.parent != shooter_tip)
        {
            connection_2.transform.parent = shooter_tip;
            connection_2.transform.position = shooter_tip.position;
            connection_2.setSpriteEnabled(false);
        }
    }

    public void remove_from_gun_tip()
    {
        if (is_connection_2_on_shooter && connection_2.transform.parent == shooter_tip)
        {
            connection_2.transform.parent = shooter_tip.parent;
            connection_2.transform.position = shooter_tip.parent.position;
            connection_2.setSpriteEnabled(true);
        }
    }

    public bool delete_connections()
    {
        bool result = false;
        unlink();
        LinkProjectile projectile_test;

        if (connection_1 != null)
        {
            projectile_test =
                connection_1.transform.parent.gameObject.GetComponent<LinkProjectile>();
            if (projectile_test != null)
                UnityEngine.Object.Destroy(projectile_test.gameObject);
            UnityEngine.Object.Destroy(connection_1.gameObject);
            connection_1 = null;
            result = true;
        }

        if (connection_2 != null)
        {
            projectile_test =
                connection_2.transform.parent.gameObject.GetComponent<LinkProjectile>();
            if (projectile_test != null)
                UnityEngine.Object.Destroy(projectile_test.gameObject);
            UnityEngine.Object.Destroy(connection_2.gameObject);
            connection_1 = null;
            result = true;
        }

        is_connection_2_on_shooter = false;

        return result;
    }

    public void spawnNoFire(LinkableObject obj1, LinkableObject obj2)
    {
        delete_connections();
        instantiate_connections(obj1.transform, obj2.transform, false);
        link();
    }

    public void fireConnection(float angle)
    {
        unlink();

        GameObject shot = GameObject.Instantiate(projectile_obj);
        shot.transform.position = shooter_tip.position;

        if (!is_connection_2_on_shooter)
        {
            delete_connections();
            instantiate_connections(shot.transform, shooter_tip, true);
            shot.GetComponent<LinkProjectile>().Init(angle, connection_1);
        }
        else
        {
            connection_2.transform.parent = shot.transform;
            connection_2.transform.position = shot.transform.position;
            connection_2.setSpriteEnabled(true);
            is_connection_2_on_shooter = false;
            shot.GetComponent<LinkProjectile>().Init(angle, connection_2);
        }
    }
}
