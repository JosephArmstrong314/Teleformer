using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkParticleSystem : MonoBehaviour
{
    public const int NUM_PARTICLE_TYPES = 4;

    public enum P
    {
        DAMAGE = 0,
        HEALING = 1,
        HOT = 2,
        COLD = 3
    };

    private const int particle_buf_size = 100;

    private const float particle_speed = 6.0f;
    private const float particle_target_offset = 1.0f;
    private const float particle_link_offset = 0.5f;

    public P type;

    public ParticleSystem ps;

    public uint p_accumulator;

    private ParticleSystem.Particle[] particles;
    private List<ParticleSystem.Particle> trigger_list;

    private LinkConnection connection;

    [SerializeField]
    private HealthSystem internal_health_system;

    public void spawn_particles(uint num_particles, Vector3 emitter_pos)
    {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = emitter_pos;
        emitParams.applyShapeToPosition = true;
        ps.Emit(emitParams, (int)num_particles);
    }

    public void add_to_particle_accumulator(uint accumulator_increase, Vector3 emitter_pos)
    {
        p_accumulator += accumulator_increase;
        spawn_particles(p_accumulator >> 3, emitter_pos);
        p_accumulator &= 0b111;
    }

    public void apply_link_path_particle_movement()
    {
        Vector3 link_vec = connection.transform.position - connection.partner.transform.position;
        float link_dist = link_vec.magnitude;
        Vector3 link_vec_norm = link_vec.normalized;
        float offset_coefficient = particle_link_offset * -4.0f;
        float one_over_dist = 1.0f / link_dist;

        int size = ps.GetParticles(particles);

        for (int i = 0; i < size; i++)
        {
            float traversed_already = Vector3.Dot(
                (particles[i].position - connection.partner.transform.position),
                link_vec_norm
            );

            float target_dist = Mathf.Clamp(
                traversed_already + particle_target_offset,
                0,
                link_dist
            );

            Vector3 target = target_dist * link_vec_norm;

            float percent_dist = target_dist * one_over_dist - 0.5f;

            float offset =
                offset_coefficient * (percent_dist) * (percent_dist) + particle_link_offset;

            target += (Vector3)(Vector2.Perpendicular(link_vec_norm)) * offset;

            target += connection.partner.transform.position;

            particles[i].velocity = (target - particles[i].position).normalized * particle_speed;
        }

        ps.SetParticles(particles, size);
    }

    public void apply_direct_particle_movement()
    {
        Vector3 target = transform.position;

        int size = ps.GetParticles(particles);

        for (int i = 0; i < size; i++)
        {
            particles[i].velocity =
                particles[i].velocity * particles[i].remainingLifetime
                + (target - particles[i].position).normalized
                    * particle_speed
                    * (particles[i].startLifetime - particles[i].remainingLifetime);
            particles[i].velocity /= particles[i].startLifetime;
        }

        ps.SetParticles(particles, size);
    }

    void FixedUpdate()
    {
        switch (type)
        {
            case P.DAMAGE:
            case P.HEALING:
                apply_direct_particle_movement();
                break;
            case P.HOT:
            case P.COLD:
                apply_link_path_particle_movement();
                break;
        }
    }

    void Awake()
    {
        particles = new ParticleSystem.Particle[particle_buf_size];
        trigger_list = new List<ParticleSystem.Particle>();
    }

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        connection = transform.parent.GetComponent<LinkConnection>();
    }

    void OnParticleTrigger()
    {
        if (connection == null)
        {
            // TEMP code
            HandleNoConnection();
            return;
        }

        if (connection.currently_linked_connected_object == null)
            return;

        int inside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, trigger_list);

        if (inside == 0)
            return;

        switch (type)
        {
            case P.DAMAGE:
                if (connection.currently_linked_connected_object.health_system == null)
                    break;
                connection.currently_linked_connected_object.health_system.change_health(
                    -(inside << 3)
                );
                break;
            case P.HEALING:
                if (connection.currently_linked_connected_object.health_system == null)
                    break;
                connection.currently_linked_connected_object.health_system.change_health(
                    (inside << 3)
                );
                break;
            case P.HOT:
                if (connection.currently_linked_connected_object.heat_system == null)
                    break;
                connection.currently_linked_connected_object.heat_system.change_heat((inside << 3));
                break;
            case P.COLD:
                if (connection.currently_linked_connected_object.heat_system == null)
                    break;
                connection.currently_linked_connected_object.heat_system.change_heat(
                    -(inside << 3)
                );
                break;
        }

        for (int i = 0; i < inside; i++)
        {
            ParticleSystem.Particle p = trigger_list[i];
            p.remainingLifetime = 0f;
            trigger_list[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, trigger_list);
    }

    // TEMP
    void HandleNoConnection()
    {
        int inside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, trigger_list);

        if (inside == 0)
            return;

        switch (type)
        {
            case P.DAMAGE:
                internal_health_system.change_health(-(inside << 3));
                break;
            case P.HEALING:
                internal_health_system.change_health((inside << 3));
                break;
        }

        for (int i = 0; i < inside; i++)
        {
            ParticleSystem.Particle p = trigger_list[i];
            p.remainingLifetime = 0f;
            trigger_list[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, trigger_list);
    }
}
