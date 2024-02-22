using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerInput : MonoBehaviour
{
    public float input_joy_deadband = 0.5f;

    public float facing_x_threshold = 0.5f;
    public float facing_y_threshold = 0.5f;

    private static readonly KeyCode[] link_swap_keys =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6
    };

    public PlayerController player;

    public LinkableObject test1;
    public LinkableObject test2;
    public int active_link_index = 0;

    private GameObject[] outlines;
    private GameObject[] wires;

    private void Start()
    {
        outlines = GameObject.FindGameObjectsWithTag("Outline");
        wires = GameObject.FindGameObjectsWithTag("Wire");
        stopShowingOutlines();
    }

    private void showOutlines()
    {
        Debug.Log("showOutlines");
        foreach (GameObject outline in outlines)
            outline.SetActive(true);
        foreach (GameObject wire in wires)
            wire.SetActive(true);
    }

    private void stopShowingOutlines()
    {
        Debug.Log("stopShowingOutlines");
        foreach (GameObject outline in outlines)
            outline.SetActive(false);
        foreach (GameObject wire in wires)
            wire.SetActive(false);
    }

    public Animator animator;

    private void Update()
    {
        // Horizontal Input
        float input_x = (Input.GetKey(GameManager.Instance.left) ? -1f : 0f) + (Input.GetKey(GameManager.Instance.right) ? 1f : 0f);
        float input_y = (Input.GetKey(GameManager.Instance.down) ? -1f : 0f) + (Input.GetKey(GameManager.Instance.up) ? 1f : 0f);

        if (Mathf.Abs(input_x) < input_joy_deadband)
            input_x = 0f;

        if (Mathf.Abs(input_y) < input_joy_deadband)
            input_y = 0f;

        int new_facing_x,
            new_facing_y;

        if (input_x >= facing_x_threshold)
            new_facing_x = 0;
        else if (input_x <= -facing_x_threshold)
            new_facing_x = 180;
        else
            new_facing_x = 0xFFFF;

        if (input_y >= facing_y_threshold)
            new_facing_y = 90;
        else if (input_y <= -facing_y_threshold)
            new_facing_y = 270;
        else
            new_facing_y = new_facing_x;

        if (new_facing_x != 0xFFFF)
        {
            if (new_facing_y >= 270 && new_facing_x == 0)
                player.facing = (360 + new_facing_y) / 2;
            else
                player.facing = (new_facing_x + new_facing_y) / 2;
            player.prev_facing = new_facing_x;
        }
        else if (new_facing_y != new_facing_x)
            player.facing = new_facing_y;
        else
        {
            player.facing = player.prev_facing;
        }

        player.updateSpriteFacing(input_x);

        if (Input.GetKeyDown(GameManager.Instance.Lock))
            showOutlines();
        else if (Input.GetKeyUp(GameManager.Instance.Lock))
            stopShowingOutlines();

        if (Input.GetKey(GameManager.Instance.Lock))
            input_x = 0f;

        animations(Mathf.Abs(input_x), player.isJumping);

        if (input_x == 0f)
        {
            player.input_x = 0f;
            player.input_direction_x = PlayerController.InputDirectionX.Stop;
        }
        else
        {
            player.input_x = input_x;
            if (input_x < 0f)
            {
                player.input_direction_x = PlayerController.InputDirectionX.Left;
            }
            else
            {
                player.input_direction_x = PlayerController.InputDirectionX.Right;
            }
        }

        // Jump Input

        if (Input.GetKeyDown(GameManager.Instance.jump))
        {
            player.early_jump_timer = player.early_jump_time;
            player.input_jump_kick = true;
        }
        else if (player.input_jump_kick)
        {
            player.early_jump_timer -= Time.deltaTime;
            if (player.early_jump_timer <= 0f)
                player.input_jump_kick = false;
        }

        player.input_jump_hold = Input.GetKey(GameManager.Instance.jump);

        player.link_gun_transform.SetLocalPositionAndRotation(
            (
                new Vector3(
                    Mathf.Cos(player.facing * Mathf.PI / 180)
                        * (player.isFacingRight ? 1.0f : -1.0f),
                    Mathf.Sin(player.facing * Mathf.PI / 180),
                    0
                )
            ) * 0.4f,
            Quaternion.Euler(0, 0, player.facing)
        ); // TEMP

        if (Input.GetKey(GameManager.Instance.energize))
        {
            player.link_manager.player_energized = true;
        }
        else
        {
            player.link_manager.player_energized = false;
        }

        if (Input.GetKeyDown(GameManager.Instance.shoot))
        {
            player.link_manager.player_links[active_link_index].fireConnection(
                player.facing * Mathf.PI / 180.0f
            );
        }

        // if (Input.GetKeyDown(KeyCode.BackQuote))
        // {
        //     player.link_manager.player_links[active_link_index].flip();
        // }

        for (int i = 0; i < 1; ++i)
            if (Input.GetKeyDown(link_swap_keys[i]))
            {
                player.link_manager.player_links[active_link_index].remove_from_gun_tip();
                active_link_index = i;
                player.link_manager.player_links[active_link_index].add_to_gun_tip();
            }

        // if (Input.GetKeyDown(link_swap_keys[4]))
        // {
        //     player.link_manager.player_links[active_link_index].remove_from_gun_tip();
        //     active_link_index = 4;
        //     ((XYLink)player.link_manager.player_links[active_link_index]).add_to_gun_tip(
        //         XYLink.Axis.X
        //     );
        // }

        // if (Input.GetKeyDown(link_swap_keys[5]))
        // {
        //     player.link_manager.player_links[active_link_index].remove_from_gun_tip();
        //     active_link_index = 4;
        //     ((XYLink)player.link_manager.player_links[active_link_index]).add_to_gun_tip(
        //         XYLink.Axis.Y
        //     );
        // }
    }

    public void animations(float speed, bool jump)
    {
        if (jump)
        {
            animator.SetBool("Jump", true);
        }
        else if (speed > 0.5f)
        {
            animator.SetBool("Jump", false);
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("Jump", false);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    // public void animations(float speed, bool jump)
    // {
    //     if (jump)
    //     {
    //         animator.SetBool("Jump", true);
    //     }
    //     else if (speed > 0.5f)
    //     {
    //         animator.SetBool("Jump", false);
    //         animator.SetFloat("Speed", 1);
    //     }
    //     else
    //     {
    //         animator.SetFloat("Speed", 0);
    //         animator.SetBool("Jump", false);
    //     }
    // }

    // public void OnLanding()
    // {
    //     animator.SetBool("Jump", false);
    // }
}
