using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : TriggerWithMask
{
    private LinkableObject self_linkable_obj;
    private Collider2D object_collider;
    private static int moving_platform_counter = 0;
    private int my_counter = 0;
    private Collider2D current_moving_platform;
    private bool moving_platform_checker_dirty = true;

    public bool isActive()
    {
        if (dirty)
        {
            dirty = false;
            active = false; // this placement is necessary for recursion
            // contacts_size = my_collider.GetContacts(filter, partners);
            for (int i = 0; i < contacts_size; ++i)
                if (partners[i].gameObject != gameObject)
                {
                    GroundChecker partner_checker = partners[i].GetComponent<GroundChecker>();
                    if (partner_checker == null || partner_checker.isActive())
                        return active = true;

                    LinkableObject partner_linkable = partners[i].GetComponent<LinkableObject>();

                    if (partner_linkable == null)
                        continue;

                    for (
                        LinkedListNode<LinkConnection> node = partner_linkable.connections.First;
                        node != null;
                        node = node.Next
                    )
                    {
                        partner_checker = node.Value
                            .partner
                            .currently_linked_connected_object
                            .groundChecker;

                        if (partner_checker == null || partner_checker.isActive())
                            return active = true;
                    }
                }
        }
        return active;
    }

    public Transform get_a_contact()
    {
        for (int i = 0; i < contacts_size; ++i)
        {
            if (partners[i].gameObject == gameObject)
                continue;
            return partners[i].transform;
        }
        return null;
    }

    void FixedUpdate()
    {
        dirty = true;
        contacts_size = my_collider.GetContacts(filter, partners);
        moving_platform_checker_dirty = true;
        transform.parent = null;

        if (object_collider.IsTouchingLayers(LayerMask.GetMask("Wall")))
            return;

        moving_platform_counter += 1;

        Collider2D moving_platform = get_moving_platform();

        if (moving_platform != null)
            transform.parent = moving_platform.transform;
    }

    private enum Results
    {
        CONTINUE,
        RETURN_NULL,
        GO_ON
    }

    private Results _check_for_platform(ref Collider2D platform, GroundChecker partner_checker)
    {
        if (partner_checker == null)
            return Results.CONTINUE;

        Collider2D partner_platform = partner_checker.get_moving_platform();
        // Debug.Log(
        //     gameObject.name
        //         + " chk "
        //         + partner_checker.gameObject.name
        //         + " "
        //         + (partner_platform != null ? partner_platform.gameObject.name : "Null")
        // );

        if (partner_platform != null)
        {
            if (partner_platform == platform)
                return Results.CONTINUE;
            if (platform == null)
            {
                platform = partner_platform;
                return Results.CONTINUE;
            }
            else
                return Results.RETURN_NULL;
        }

        return Results.GO_ON;
    }

    public Collider2D get_moving_platform()
    {
        if (moving_platform_counter != my_counter)
        {
            moving_platform_checker_dirty = true;
            my_counter = moving_platform_counter;
            current_moving_platform = null;
        }

        // Debug.Log(gameObject.name + "  cal" + (moving_platform_checker_dirty ? "dirty" : "l"));
        if (moving_platform_checker_dirty)
        {
            moving_platform_checker_dirty = false;
            current_moving_platform = null; // this placement is necessary for recursion
            Collider2D platform = null;
            for (int i = 0; i < contacts_size; ++i)
                if (partners[i].gameObject != gameObject)
                {
                    if (partners[i] == platform)
                        continue;

                    if (partners[i].gameObject.layer == LayerMask.NameToLayer("MovingPlatform"))
                        if (platform == null)
                        {
                            platform = partners[i];
                            continue;
                        }
                        else
                        {
                            // Debug.Log(
                            //     gameObject.name + " " + partners[i].gameObject.name + "  1111"
                            // );
                            return null;
                        }

                    Results r1 = _check_for_platform(
                        ref platform,
                        partners[i].GetComponent<GroundChecker>()
                    );
                    if (r1 == Results.CONTINUE)
                        continue;
                    if (r1 == Results.RETURN_NULL)
                        return null;

                    LinkableObject partner_linkable = partners[i].GetComponent<LinkableObject>();

                    if (partner_linkable == null)
                        continue;

                    for (
                        LinkedListNode<LinkConnection> node = partner_linkable.connections.First;
                        node != null;
                        node = node.Next
                    )
                    {
                        if (!node.Value.link.energized)
                            continue;

                        Results r2 = _check_for_platform(
                            ref platform,
                            node.Value.partner.currently_linked_connected_object.groundChecker
                        );
                        if (r2 == Results.CONTINUE)
                            continue;
                        if (r2 == Results.RETURN_NULL)
                            return null;
                    }
                }
            if (self_linkable_obj != null)
            {
                for (
                    LinkedListNode<LinkConnection> node = self_linkable_obj.connections.First;
                    node != null;
                    node = node.Next
                )
                {
                    if (!node.Value.link.energized)
                        continue;

                    Results r1 = _check_for_platform(
                        ref platform,
                        node.Value.partner.currently_linked_connected_object.groundChecker
                    );
                    if (r1 == Results.CONTINUE)
                        continue;
                    if (r1 == Results.RETURN_NULL)
                        return null;

                    for (
                        LinkedListNode<LinkConnection> node2 = node.Value
                            .partner
                            .currently_linked_connected_object
                            .connections
                            .First;
                        node2 != null;
                        node2 = node2.Next
                    )
                    {
                        if (node2.Value.partner == node.Value)
                            continue;

                        if (!node2.Value.link.energized)
                            continue;

                        Results r2 = _check_for_platform(
                            ref platform,
                            node2.Value.partner.currently_linked_connected_object.groundChecker
                        );
                        if (r1 == Results.CONTINUE)
                            continue;
                        if (r1 == Results.RETURN_NULL)
                            return null;
                    }
                }
            }
            // Debug.Log(
            //     gameObject.name + " " + (platform != null ? platform.gameObject.name : "Null")
            // );
            current_moving_platform = platform;
        }
        // Debug.Log(
        //     gameObject.name
        //         + " full "
        //         + (
        //             current_moving_platform != null
        //                 ? current_moving_platform.gameObject.name
        //                 : "Null"
        //         )
        // );
        return current_moving_platform;
    }

    void Start()
    {
        self_linkable_obj = GetComponent<LinkableObject>();
        object_collider = GetComponent<Collider2D>();

        filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Ground", "MovingPlatform", "Default", "Ice");
    }
}
