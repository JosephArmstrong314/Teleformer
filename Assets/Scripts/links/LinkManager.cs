using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{
    public Link[] player_links = new Link[5];
    public LinkedList<Link> player_energizable_links = new LinkedList<Link>();
    public LinkedList<Link> non_player_energizable_links = new LinkedList<Link>();
    public bool player_energized = false;

    private PlayerController player;

    private LinkedListNode<Link> new_link<T>(bool player_energizable)
        where T : Link, new()
    {
        Link link = new T();
        LinkedListNode<Link> node;

        if (player_energizable)
            node = player_energizable_links.AddLast(link);
        else
            node = non_player_energizable_links.AddLast(link);
        return node;
    }

    public Link spawn_link<T>(bool player_energizable)
        where T : Link, new()
    {
        LinkedListNode<Link> node = new_link<T>(player_energizable);

        Link link = node.Value;

        link.Init(node);

        return link;
    }

    public Link spawn_link<T>(bool player_energizable, Transform shooter_tip)
        where T : Link, new()
    {
        LinkedListNode<Link> node = new_link<T>(player_energizable);

        Link link = node.Value;

        link.Init(node, shooter_tip);

        return link;
    }

    public Link spawn_link<T>(int player_index, PlayerController player)
        where T : Link, new()
    {
        Link link = spawn_link<T>(true, player.link_gun_transform);
        player_links[player_index] = link;
        link.player_index = player_index;
        return link;
    }

    // public void delete_link(Link link)
    // {
    //     link.manager_node.List.Remove(link.manager_node);
    //     if (link.player_index != 0xFFFF)
    //         player_links[link.player_index] = null;
    //     link.unlink();
    // }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        spawn_link<Link>(0, player);
        // spawn_link<Link>(1, player);
    }

    void FixedUpdate()
    {
        if (player_energized)
        {
            for (
                LinkedListNode<Link> node = player_energizable_links.First;
                node != null;
                node = node.Next
            )
                node.Value.energize();

            // player_mana_expenditure_timer += Time.fixedDeltaTime;
            // if (player_mana_expenditure_timer >= player_mana_expenditure_time)
            // {
            //     player_mana_expenditure_timer -= player_mana_expenditure_time;
            //     for (
            //         LinkedListNode<Link> node = player_energizable_links.First;
            //         node != null;
            //         node = node.Next
            //     )
            //     {
            //         node.Value.send_mana(player.transform.position, 16);
            //     }
            // }
        }
        else

            for (
                LinkedListNode<Link> node = player_energizable_links.First;
                node != null;
                node = node.Next
            )
                node.Value.unenergize();
    }

    public bool break_all_player_links()
    {
        bool result = false;
        for (
            LinkedListNode<Link> node = player_energizable_links.First;
            node != null;
            node = node.Next
        )
            result = result || node.Value.delete_connections();

        return result;
    }
}
