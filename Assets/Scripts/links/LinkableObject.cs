using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkableObject : MonoBehaviour
{
    public LinkedList<LinkConnection> connections = new LinkedList<LinkConnection>();

    // Links need to be refactored to be based on this
    // public LinkedList<GameObject> link_connections = new LinkedList<GameObject>();

    public bool movable = true;
    public bool magnetic = false;

    public GroundChecker groundChecker = null;

    public HealthSystem health_system;
    public HeatSystem heat_system;

    void Awake()
    {
        groundChecker = GetComponent<GroundChecker>();
        health_system = GetComponent<HealthSystem>();
        heat_system = GetComponent<HeatSystem>();
        if (groundChecker == null && movable)
            Debug.Log("WARNING: Linkable Object Without Ground Checker: " + gameObject.name);
    }

    public bool is_energized()
    {
        for (
            LinkedListNode<LinkConnection> node = connections.First;
            node != null;
            node = node.Next
        )
            if (node.Value.link.energized)
                return true;
        return false;
    }

    public void break_all_links()
    {
        LinkConnection connection_to_delete;
        for (LinkedListNode<LinkConnection> node = connections.First; node != null; )
        {
            connection_to_delete = node.Value;
            node = node.Next;
            connection_to_delete.link.delete_connections();
        }
    }
}
