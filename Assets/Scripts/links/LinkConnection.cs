using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkConnection : MonoBehaviour
{
    private Color sprite_color_normal = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color line_color_normal = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color line_color_disabled_normal = new Color(1.0f, 1.0f, 1.0f, 0.6f);

    private Color sprite_color_magnetic = new Color(1f, 0.92f, 0.016f, 1.0f);
    private Color line_color_magnetic = new Color(1f, 0.92f, 0.016f, 1.0f);
    private Color line_color_disabled_magnetic = new Color(1f, 0.92f, 0.016f, 0.6f);

    private Color sprite_color;
    private Color line_color;
    private Color line_color_disabled;

    public LinkConnection partner;
    public Link link;

    public LinkableObject currently_linked_connected_object;
    public LinkedListNode<LinkConnection> connected_object_connection_list_reference;

    public LineRenderer line;

    // [SerializeField]
    public LinkParticleSystem[] p_systems;

    void Start()
    {
        p_systems = GetComponentsInChildren<LinkParticleSystem>();
    }

    public void Init(Link set_link, LinkConnection set_partner, bool enable_line)
    {
        set_color_normal();
        GetComponent<SpriteRenderer>().color = sprite_color;
        line = GetComponent<LineRenderer>();
        line.startColor = line.endColor = line_color_disabled;
        link = set_link;
        partner = set_partner;
        this.enabled = true;
        line.enabled = enable_line;
        setSpriteEnabled(true);
    }

    public void set_color_normal()
    {
        sprite_color = sprite_color_normal;
        line_color = line_color_normal;
        line_color_disabled = line_color_disabled_normal;
    }

    public void set_color_magnetic()
    {
        sprite_color = sprite_color_magnetic;
        line_color = line_color_magnetic;
        line_color_disabled = line_color_disabled_magnetic;
    }

    public LinkableObject GetConnectedLinkableObject()
    {
        if (transform.parent == link.shooter_tip)
            return transform.parent.parent.GetComponent<LinkableObject>();
        else
            return transform.parent.GetComponent<LinkableObject>();
    }

    public void set_currently_connected_linked_object(LinkableObject connected_object)
    {
        connected_object_connection_list_reference = connected_object.connections.AddFirst(this);
        currently_linked_connected_object = connected_object;

        for (int i = 0; i < LinkParticleSystem.NUM_PARTICLE_TYPES; ++i)
            p_systems[i].p_accumulator = 0;
    }

    public void forget_currently_connected_linked_object()
    {
        if (
            currently_linked_connected_object != null
            && connected_object_connection_list_reference != null
        )
            currently_linked_connected_object.connections.Remove(
                connected_object_connection_list_reference
            );
        currently_linked_connected_object = null;
    }

    void OnDestroy()
    {
        forget_currently_connected_linked_object();
    }

    public void setSpriteEnabled(bool enabled)
    {
        GetComponent<SpriteRenderer>().enabled = enabled;
    }

    void Update()
    {
        if (!line.enabled)
            return;

        if (link.energized)
            line.startColor = line.endColor = line_color;
        else
            line.startColor = line.endColor = line_color_disabled;

        line.SetPositions(new Vector3[] { transform.position, partner.transform.position });
    }
}
