using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class M_MissileLeg : MidBoss3
{
    public M_MissileTop top;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        nav.SetDestination(CalculateWanderPosition());
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            top.isDead = true;
            return;
        }
        Move();
    }

    private void Move()
    {
        float distance = Vector3.Distance(transform.position, nav.destination);
        if (distance < 5f)
        {
            nav.SetDestination(CalculateWanderPosition());
        }
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 20f;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        Vector3 rangePosition;
        Vector3 rangeScale;


        rangePosition = Vector3.zero;
        rangeScale = Vector3.one * 70f;


        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = transform.position.y;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }
}
