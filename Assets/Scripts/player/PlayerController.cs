using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Added for SFX
    private bool playSFX = true;

    // Constants
    public float max_run_speed = 5f;
    public float min_run_speed = 1f;
    public float stopping_acc_multiplier = 1.5f;
    public float turning_acc_multiplier = 1.5f;
    public float max_speed_gnd_slow_acc_multiplier = 0.8f;
    public float max_speed_air_slow_acc_multiplier = 0.2f;

    public float acc_x = 40f;

    private float ice_acc_x = 2f;

    public float jump_vel = 10f;

    public float gravity_force = 90.0f;

    public float zero_out_vel_deadband = 0.8f;

    public float max_y_up_vel_to_be_grounded = 1.5f;

    public float jump_end_dwn_vel = -0.3f;

    public float jump_straight_pseudo_gravity = 45f;

    public float low_jump_vel_boost = 3.0f;

    public float wall_slide_velocity = -5.5f;
    public float wall_slide_acc = -1.0f;

    // User control constants
    public float coyote_time = 0.15f;

    public float early_jump_time = 0.08f;

    public float jump_straight_time_max = 0.5f;
    public float jump_straight_time_min = 0.1f;

    public float jump_again_time = 0.08f;

    // physics variables
    public Rigidbody2D rigidbody;

    // state data variables
    public bool isGrounded = false;
    public bool isWallSliding = false;
    public bool isJumping = false;
    public bool isFacingRight = true;
    public bool isOneLinkGrounded = false;
    public float coyote_timer = 0f;
    public float jump_straight_timer = 10000f;
    public bool isOnIce = false;

    public int next_vel_func_conditions;
    public PlayerXMovementDecisionTree.NextXVelFunctionIndicator move_x_func;

    // input variables
    public bool input_jump_kick = false;
    public bool input_jump_hold = false;
    public bool input_jump_hold_prev = false;
    public float early_jump_timer = 0f;
    public float jump_again_timer = 0f;

    public enum InputDirectionX
    {
        Left = 0,
        Stop = 1,
        Right = 2
    };

    public InputDirectionX input_direction_x = InputDirectionX.Stop;

    public float input_x;

    public LinkManager link_manager;

    public enum Facing
    {
        NR = 0,
        UR = 45,
        UN = 90,
        UL = 135,
        NL = 180,
        DL = 225,
        DN = 270,
        DR = 315
    };

    public int facing = (int)Facing.NR;
    public int prev_facing = (int)Facing.NR;

    // Events

    public UnityEvent OnLandEvent;

    // Other Objects

    [SerializeField]
    private Collider2D my_collider;
    private int ice_mask;

    [SerializeField]
    private LayerMask what_is_ground;

    [SerializeField]
    private LayerMask what_is_slidable_wall;

    [SerializeField]
    private LayerMask what_is_solid;

    [SerializeField]
    private GroundChecker groundChecker;

    [SerializeField]
    private Transform wall_checker;
    public Vector2 temp_wall_checker_size = new Vector2(0.51f, 0.95f);

    [SerializeField]
    private LinkableObject player_linkable_object;

    public Transform link_gun_transform;

    public Vector2 jump_counter_impulse = new Vector2(0f, -10f);

    // Methods

    private void setSpriteFacing(bool setFacingRight)
    {
        isFacingRight = setFacingRight;

        Vector3 scale = transform.localScale;
        scale.x = (setFacingRight ? 1 : -1) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    // TEMP TEMP TEMP TEMP TEMP

    // public void updateLinkedObjectsGrounded()
    // {
    //     isOneLinkGrounded = false;
    //     for (
    //         LinkedListNode<Link> node = player_linkable_object.links.First;
    //         node != null;
    //         node = node.Next
    //     )
    //     {
    //         if (!node.Value.energized)
    //             continue;
    //         if (node.Value.source == player_linkable_object)
    //         {
    //             if (node.Value.sink.groundChecker.isActive())
    //             {
    //                 isOneLinkGrounded = true;
    //                 return;
    //             }
    //         }
    //         else
    //         {
    //             if (node.Value.source.groundChecker.isActive())
    //             {
    //                 isOneLinkGrounded = true;
    //                 return;
    //             }
    //         }
    //     }
    // }

    public void updateSpriteFacing(float x_joy_input)
    {
        if (isFacingRight && x_joy_input < 0)
        {
            setSpriteFacing(false);
        }
        else if (!isFacingRight && x_joy_input > 0)
        {
            setSpriteFacing(true);
        }
    }

    private void updateIsGroundedAndIsWallSliding()
    {
        bool wasGrounded = isGrounded;

        isGrounded = false;
        isWallSliding = false;
        isOnIce = false;

        if (!wasGrounded && rigidbody.velocity.y >= max_y_up_vel_to_be_grounded)
            return;

        if (isJumping && jump_straight_timer < jump_straight_time_min)
            return;

        // Check for ground

        if (groundChecker.isActive())
        {
            isGrounded = true;

            if (jump_again_timer > 0f)
                jump_again_timer -= Time.fixedDeltaTime;
            if (!wasGrounded)
            {
                OnLandEvent.Invoke();
                // rigidbody.velocity *= new Vector2(1.0f, 0.1f);
                jump_again_timer = jump_again_time;
            }

            isOnIce = my_collider.IsTouchingLayers(ice_mask);
        }

        // Check for wall

        Collider2D[] colliders_touching_wall_check = Physics2D.OverlapBoxAll(
            wall_checker.position,
            temp_wall_checker_size,
            0.0f,
            what_is_solid
        );

        bool is_touching_solid_object = false;

        for (int i = 0; i < colliders_touching_wall_check.Length; i++)
            if (colliders_touching_wall_check[i].gameObject != gameObject)
            {
                if (
                    isWallSliding == false
                    && ~(
                        colliders_touching_wall_check[i].gameObject.layer
                        & what_is_slidable_wall.value
                    ) != 0
                )
                    isWallSliding = true;

                is_touching_solid_object = true;
            }

        if (is_touching_solid_object)
            return;
    }

    private float nextVelStoppedX(float input, float current_vel)
    {
        return 0f;
    }

    private float nextVelIce(float input, float current_vel)
    {
        float velocity_direction = Mathf.Sign(current_vel);

        current_vel -= velocity_direction * ice_acc_x * Time.fixedDeltaTime;

        if (current_vel * velocity_direction <= 0f)
            return 0f;

        return current_vel;
    }

    private float nextVelStartUpX(float input, float current_vel)
    {
        float input_direction = Mathf.Sign(input);

        current_vel += input_direction * acc_x * Time.fixedDeltaTime;

        if (current_vel * input_direction > max_run_speed)
            return max_run_speed * input_direction;

        if (current_vel * input_direction < min_run_speed)
            return min_run_speed * input_direction;

        return current_vel;
    }

    private float nextVelMaxSpeedX_gnd(float input, float current_vel)
    {
        if (jump_again_timer > 0f)
            return nextVelMaxSpeedX_air(input, current_vel);

        float input_direction = Mathf.Sign(input);

        current_vel -=
            input_direction * acc_x * Time.fixedDeltaTime * max_speed_gnd_slow_acc_multiplier;

        if (Mathf.Abs(current_vel) <= max_run_speed)
            return max_run_speed * input_direction;

        return current_vel;
    }

    private float nextVelMaxSpeedX_air(float input, float current_vel)
    {
        float input_direction = Mathf.Sign(input);

        current_vel -=
            input_direction * acc_x * Time.fixedDeltaTime * max_speed_air_slow_acc_multiplier;

        if (Mathf.Abs(current_vel) <= max_run_speed)
            return max_run_speed * input_direction;

        return current_vel;
    }

    private float nextVelStoppingX(float input, float current_vel)
    {
        float velocity_direction = Mathf.Sign(current_vel);

        current_vel -= velocity_direction * acc_x * Time.fixedDeltaTime * stopping_acc_multiplier;

        if (current_vel * velocity_direction <= 0f)
            return 0f;

        return current_vel;
    }

    private float nextVelTurningX(float input, float current_vel)
    {
        return current_vel
            + turning_acc_multiplier * Mathf.Sign(input) * acc_x * Time.fixedDeltaTime;
    }

    private void updateNextVelXFuncConditions()
    {
        int new_conditions = 0b0;

        new_conditions |=
            (int)PlayerXMovementDecisionTree.Cnd.IS_GROUNDED * Convert.ToInt32(isGrounded);
        new_conditions |=
            (int)PlayerXMovementDecisionTree.Cnd.INPUT_IS_STOP
            * Convert.ToInt32(
                input_direction_x == InputDirectionX.Stop || (isOneLinkGrounded && !isGrounded)
            );
        new_conditions |=
            (int)PlayerXMovementDecisionTree.Cnd.VEL_IS_ZERO
            * Convert.ToInt32(Mathf.Abs(rigidbody.velocity.x) < zero_out_vel_deadband);
        new_conditions |=
            (int)PlayerXMovementDecisionTree.Cnd.INPUT_VEL_SAME_DIRECTION
            * Convert.ToInt32(rigidbody.velocity.x * input_x > 0f);
        new_conditions |=
            (int)PlayerXMovementDecisionTree.Cnd.V_OVER_MAX
            * Convert.ToInt32(Mathf.Abs(rigidbody.velocity.x) >= max_run_speed - 0.01f);
        new_conditions |= (int)PlayerXMovementDecisionTree.Cnd.IS_ON_ICE * Convert.ToInt32(isOnIce);

        next_vel_func_conditions = new_conditions;
    }

    private float runNextVelXFunc_for_testing_only(float input, float current_vel)
    {
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STOPPED
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STOPPED;
            return nextVelStoppedX(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STARTUP
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STARTUP;
            return nextVelStartUpX(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.MAXSPEED_AIR
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.MAXSPEED_AIR;
            return nextVelMaxSpeedX_air(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.MAXSPEED_GND
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.MAXSPEED_GND;
            return nextVelMaxSpeedX_gnd(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STOPPING_AIR
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STOPPING_AIR;
            return nextVelStoppingX(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STOPPING_GND
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.STOPPING_GND;
            return nextVelStoppingX(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.TURNING
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.TURNING;
            return nextVelTurningX(input, current_vel);
        }
        if (
            PlayerXMovementDecisionTree.checkConditionsMatchFunc(
                next_vel_func_conditions,
                PlayerXMovementDecisionTree.NextXVelFunctionIndicator.ICE
            )
        )
        {
            move_x_func = PlayerXMovementDecisionTree.NextXVelFunctionIndicator.ICE;
            return nextVelIce(input, current_vel);
        }
        return 0f;
    }

    private void updateXMovement()
    {
        updateNextVelXFuncConditions();

        float next_velocity = runNextVelXFunc_for_testing_only(input_x, rigidbody.velocity.x);
        rigidbody.velocity = new Vector2(next_velocity, rigidbody.velocity.y);
    }

    private void ApplyForceToAll() { }

    private void doJumpKick()
    {
        isGrounded = false;
        input_jump_kick = false;
        isJumping = true;
        early_jump_timer = 0f;
        coyote_timer = 0f;
        jump_straight_timer = 0f;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jump_vel);

        // Added for SFX
        if (playSFX == true)
        {
            playSFX = false;
            SoundManager.Instance.PlayClipByName("JumpSFX");
        }
    }

    private void updateJump()
    {
        if (!isGrounded)
            return;

        coyote_timer = coyote_time;

        if (input_jump_kick && jump_again_timer <= 0f)
        {
            doJumpKick();
        }
        else
        {
            isJumping = false;

            // Added for SFX
            playSFX = true;
        }
    }

    private void updateYMovement()
    {
        if (isJumping)
        {
            jump_straight_timer += Time.fixedDeltaTime;
            if (
                ((jump_straight_timer < jump_straight_time_min) || input_jump_hold)
                && jump_straight_timer < jump_straight_time_max
            )
                rigidbody.velocity = new Vector2(
                    rigidbody.velocity.x,
                    jump_vel
                        - jump_straight_timer * jump_straight_timer * jump_straight_pseudo_gravity
                );
            else
            {
                isJumping = false;
                rigidbody.velocity = new Vector2(
                    rigidbody.velocity.x,
                    jump_end_dwn_vel
                        + low_jump_vel_boost * (jump_straight_time_max - jump_straight_timer)
                );

                // Added for SFX
                playSFX = true;
            }
        }
        else
        {
            if (!isWallSliding)
            {
                rigidbody.AddForce(Vector2.down * gravity_force);
            }
            else
            {
                if (rigidbody.velocity.y <= wall_slide_velocity)
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, wall_slide_velocity);
                else
                    rigidbody.velocity += new Vector2(0, wall_slide_acc);
            }

            if (coyote_timer > 0f)
            {
                if (input_jump_kick)
                    doJumpKick();
                else
                    coyote_timer -= Time.fixedDeltaTime;
            }
        }
    }

    private void updateMovementControl()
    {
        updateXMovement();

        updateJump();

        updateYMovement();

        input_jump_hold_prev = input_jump_hold;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        link_manager = GameObject.Find("LinkManager").GetComponent<LinkManager>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        ice_mask = LayerMask.GetMask("Ice");
    }

    private void FixedUpdate()
    {
        updateIsGroundedAndIsWallSliding();
        // updateLinkedObjectsGrounded();

        updateMovementControl();
    }

    public void onDeath()
    {
        Debug.Log("Player Death");
        Time.timeScale = 0;
        GameObject
            .FindWithTag("Canvas")
            .transform.Find("Game Over Menu")
            .gameObject.SetActive(true);
    }
}
