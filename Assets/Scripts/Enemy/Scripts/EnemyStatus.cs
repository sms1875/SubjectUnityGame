using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]
    private float maxHealthPoint = 100f;
    [SerializeField]
    private float healthPoint = 100f;
    [SerializeField]
    private float attackRange = 2f;
    [SerializeField]
    private float attackDamage = 1f;
    [SerializeField]
    private float attackRate = 5f;
    [SerializeField]
    private float attackHoldingTime = 0f;
    [SerializeField]
    private float targetRecognitionRange = 8f;
    [SerializeField]
    private float pursuitLimitRange = 15f;
    [SerializeField]
    private float walkSpeed = 3f;
    [SerializeField]
    private float runSpeed = 5f;
    [SerializeField]
    private float moveTime = 2f;
    [SerializeField]
    private float jumpForce = 3f;
    [SerializeField]
    private float spinSpeed = 4f;
    [SerializeField]
    private float viewAngle = 120f;
    [SerializeField]
    private float defence = 0;

    public float MaxHealthPoint
    {
        set => maxHealthPoint = value;
        get => maxHealthPoint;
    }
    public float HealthPoint
    {
        set => healthPoint = value;
        get => healthPoint;
    }
    public float AttackRange
    {
        set => attackRange = value;
        get => attackRange;
    }
    public float AttackDamage
    {
        set => attackDamage = value;
        get => attackDamage;
    }
    public float AttackRate
    {
        set => attackRate = value;
        get => attackRate;
    }
    public float AttackHoldingTime
    {
        set => attackHoldingTime = value;
        get => attackHoldingTime;
    }
    public float TargetRecognitionRange
    {
        set => targetRecognitionRange = value;
        get => targetRecognitionRange;
    }
    public float PursuitLimitRange
    {
        set => pursuitLimitRange = value;
        get => pursuitLimitRange;
    }
    public float WalkSpeed
    {
        set => walkSpeed = value;
        get => walkSpeed;
    }
    public float RunSpeed
    {
        set => runSpeed = value;
        get => runSpeed;
    }
    public float MoveTime
    {
        set => moveTime = value;
        get => moveTime;
    }
    public float JumpForce
    {
        set => jumpForce = value;
        get => jumpForce;
    }
    public float SpinSpeed
    {
        set => spinSpeed = value;
        get => spinSpeed;
    }
    public float ViewAngle
    {
        set => viewAngle = value;
        get => viewAngle;
    }

    public float Defence
    {
        set => defence = value;
        get => defence;
    }
}

