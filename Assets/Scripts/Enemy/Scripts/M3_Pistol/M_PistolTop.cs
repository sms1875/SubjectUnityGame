using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PistolTop : MonoBehaviour
{
    public Transform topPistol;
    public Transform leftPistol;
    public Transform rightPistol;

    public Animator topAnim;
    public Animator leftAnim;
    public Animator rightAnim;

    public bool isAimShot;
    public bool isShoot;
    public bool isSpinShot;
    public bool isTurret;
    public bool isDead;
    public bool isRotate;

    public Vector3 destination;

    private Transform target;
    private AudioSource audio;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        audio = GetComponent<AudioSource>();
        audio.Stop();
    }

    private void OnEnable()
    {
       // StartCoroutine("Sound");
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            StopAllCoroutines();
            audio.Stop();
            topAnim.SetBool("IsShoot", false);
            leftAnim.SetBool("IsShoot", false);
            rightAnim.SetBool("IsShoot", false);
            return;
        }
        if (isAimShot)
        {
            LookTarget();
        }
        else if (isSpinShot || isTurret)
        {
            SpinAround();
        }
        else 
        {
            LookForward();
        }
    }

    private void LookForward()
    {
        if (audio.isPlaying)
        {
            audio.Stop();
        }
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination - from), 30 * Time.deltaTime);
        topPistol.rotation = Quaternion.RotateTowards(topPistol.rotation, transform.rotation, 15 * Time.deltaTime);
        leftPistol.rotation = Quaternion.RotateTowards(leftPistol.rotation, transform.rotation, 15 * Time.deltaTime);
        rightPistol.rotation = Quaternion.RotateTowards(rightPistol.rotation, transform.rotation, 15 * Time.deltaTime);
    }

    private void LookTarget()
    {
        if (audio.isPlaying)
        {
            audio.Stop();
        }
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 60 * Time.deltaTime);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < 20)
        {
            if (!isShoot)
            {
                topAnim.speed = 1f;
                leftAnim.speed = 1f;
                rightAnim.speed = 1f;
                StartCoroutine("OnShoot", 5);
            }

            to = new Vector3(target.position.x, target.position.y + 3, target.position.z);

            from = new Vector3(topPistol.position.x, topPistol.position.y, topPistol.position.z);
            topPistol.rotation = Quaternion.RotateTowards(topPistol.rotation, Quaternion.LookRotation(to - from), 20 * Time.deltaTime);

            float topRotationX = topPistol.localEulerAngles.x;

            if (topRotationX >= 35 && topRotationX <= 90)
            {
                topPistol.localRotation = Quaternion.Euler(new Vector3(35, topPistol.localEulerAngles.y, topPistol.localEulerAngles.z));
            }
            else if (topRotationX <= 315 && topRotationX >= 270)
            {
                topPistol.localRotation = Quaternion.Euler(new Vector3(315, topPistol.localEulerAngles.y, topPistol.localEulerAngles.z));
            }

            from = new Vector3(leftPistol.position.x, leftPistol.position.y, leftPistol.position.z);
            leftPistol.rotation = Quaternion.RotateTowards(leftPistol.rotation, Quaternion.LookRotation(to - from), 20 * Time.deltaTime);

            from = new Vector3(rightPistol.position.x, rightPistol.position.y, rightPistol.position.z);
            rightPistol.rotation = Quaternion.RotateTowards(rightPistol.rotation, Quaternion.LookRotation(to - from), 20 * Time.deltaTime);
        }

        else
        {
            topPistol.rotation = Quaternion.RotateTowards(topPistol.rotation, transform.rotation, 20 * Time.deltaTime);
            leftPistol.rotation = Quaternion.RotateTowards(leftPistol.rotation, transform.rotation, 20 * Time.deltaTime);
            rightPistol.rotation = Quaternion.RotateTowards(rightPistol.rotation, transform.rotation, 20 * Time.deltaTime);
        }
    }

    private void SpinAround()
    {
        if (isSpinShot)
        {
            if (!isShoot)
            {
                topAnim.speed = 1.3f;
                leftAnim.speed = 1.3f;
                rightAnim.speed = 1.3f;
                StartCoroutine("OnShoot", 16);
            }
            transform.Rotate(Vector3.up, 45 * Time.deltaTime);

            topPistol.rotation = Quaternion.RotateTowards(topPistol.rotation, Quaternion.LookRotation(target.position - topPistol.position), 20 * Time.deltaTime);
            topPistol.localRotation = Quaternion.Euler(new Vector3(topPistol.localEulerAngles.x, 0, 0));

            float topRotationX = topPistol.localEulerAngles.x;

            if (topRotationX >= 35 && topRotationX <= 90)
            {
                topPistol.localRotation = Quaternion.Euler(new Vector3(35, topPistol.localEulerAngles.y, topPistol.localEulerAngles.z));
            }
            else if (topRotationX <= 315 && topRotationX >= 270)
            {
                topPistol.localRotation = Quaternion.Euler(new Vector3(315, topPistol.localEulerAngles.y, topPistol.localEulerAngles.z));
            }

            leftPistol.rotation = Quaternion.RotateTowards(leftPistol.rotation, Quaternion.LookRotation(target.position - leftPistol.position), 20 * Time.deltaTime);
            leftPistol.localRotation = Quaternion.Euler(new Vector3(leftPistol.localEulerAngles.x, 0, 0));

            rightPistol.rotation = Quaternion.RotateTowards(rightPistol.rotation, Quaternion.LookRotation(target.position - rightPistol.position), 20 * Time.deltaTime);
            rightPistol.localRotation = Quaternion.Euler(new Vector3(rightPistol.localEulerAngles.x, 0, 0));
        }
        else
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
        }
    }

    private IEnumerator OnShoot(float time)
    {
        isShoot = true;
        yield return new WaitForSeconds(0.5f);

        topAnim.SetBool("IsShoot", true);
        leftAnim.SetBool("IsShoot", true);
        rightAnim.SetBool("IsShoot", true);

        yield return new WaitForSeconds(time);

        topAnim.SetBool("IsShoot", false);
        leftAnim.SetBool("IsShoot", false);
        rightAnim.SetBool("IsShoot", false);
    }

    private IEnumerator Sound()
    {
        while (true)
        {
            float y = transform.eulerAngles.y;
            yield return new WaitForSeconds(0.4f);

            if (y - transform.eulerAngles.y > 8f || y - transform.eulerAngles.y < -8f)
            {
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
            else
            {
                audio.Stop();
            }

            yield return null;
        }
    }
}
