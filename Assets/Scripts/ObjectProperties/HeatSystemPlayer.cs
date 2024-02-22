using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HeatSystemPlayer : HeatSystem
{
    private SimpleShader camera_shader;
    private Color hot_shader_color = new Color(1.0f, 0.1215f, 0.0f);
    private Color cold_shader_color = new Color(0.0f, 0.1215f, 1.0f);
    private const int heat_cutoff = 75;
    private const float death_overheat_time = 2.2f;
    private float death_overheat_timer = 0f;
    private const int death_hot_thresh = 75;
    private const int HEAT_LOSS_COEF = 1;

    void Awake()
    {
        linkable_object = GetComponent<LinkableObject>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        contacts = new Collider2D[10];
    }

    void Start()
    {
        camera_shader = GameObject.Find("Camera/Main Camera").GetComponent<SimpleShader>();
        if (camera_shader == null)
            Debug.Log("cant find Camera/Main Camera");
        change_heat(0);
    }

    public override void change_heat(int heat_change)
    {
        heat = Math.Clamp(heat + heat_change, -max_heat_magnitude, max_heat_magnitude);
        if (heat > 0)
        {
            sprite_renderer.material.color = Color.Lerp(
                Color.white,
                Color.red,
                (float)(heat) / (float)(max_heat_magnitude)
            );
        }
        else
        {
            sprite_renderer.material.color = Color.Lerp(
                Color.white,
                Color.cyan,
                (float)(-heat) / (float)(max_heat_magnitude)
            );
        }

        if (heat > heat_cutoff)
        {
            camera_shader.enabled = true;
            camera_shader.material.SetFloat(
                "_VRadius",
                0.85f + 0.15f * (float)(max_heat_magnitude - heat) / (float)(max_heat_magnitude)
            );
            // camera_shader.material.SetFloat(
            //     "_VSoft",
            //     0.5f * (float)(heat) / (float)(max_heat_magnitude)
            // );
            camera_shader.material.SetColor("_VColor", hot_shader_color);
        }
        else if (heat < -heat_cutoff)
        {
            camera_shader.enabled = true;
            camera_shader.material.SetFloat(
                "_VRadius",
                0.85f + 0.15f * (float)(max_heat_magnitude + heat) / (float)(max_heat_magnitude)
            );
            // camera_shader.material.SetFloat(
            //     "_VSoft",
            //     0.5f * (float)(-heat) / (float)(max_heat_magnitude)
            // );
            camera_shader.material.SetColor("_VColor", cold_shader_color);
        }
        else
        {
            camera_shader.enabled = false;
        }
    }

    internal override int FixedUpdateChild()
    {
        if (heat > death_hot_thresh)
        {
            death_overheat_timer += Time.fixedDeltaTime;
            if (death_overheat_timer > death_overheat_time)
            {
                death_overheat_timer = 0f;
                GetComponent<PlayerController>().onDeath();
            }
        }
        else
        {
            death_overheat_timer = 0f;
        }

        return HEAT_LOSS_COEF;
    }
}
