using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMan : CombatEvent
{
    private float health = 1000f;
    private float maxHealth = 1000f;

    private bool isDie;
    private bool isGround;
    private bool isSetAngle;
    private float spinTime = 0f;
    private float angle;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isDie)
        {
            return;
        }
        if (isGround)
        {
            Move();
        }
        else
        {
            if (spinTime >= 3)
            {
                isGround = true;
                isSetAngle = false;
            }
            spinTime += Time.fixedDeltaTime;
        }
    }

    private void Move()
    {
        if (!isSetAngle)
        {
            int exponent = Random.Range(0, 2);
            angle = Random.Range(0, 130);
            angle *= Mathf.Pow(-1, exponent);
            isSetAngle = true;
            spinTime = 0;
        }

        if (spinTime <= 1)
        {
            transform.Rotate(transform.up * angle * Time.fixedDeltaTime);
            spinTime += Time.fixedDeltaTime;
        }

        else
        {
            Debug.Log("Jump");
            rigid.AddForce((transform.forward + Vector3.up) * 5f, ForceMode.Impulse);
            spinTime = 0;
            isGround = false;
            isSetAngle = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            isGround = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0)
        {
            return;
        }
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            isDie = true;

            Clear();
        }
    }
}
