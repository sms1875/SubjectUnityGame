using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_FireTop : MonoBehaviour
{
    public Transform leftFlame;
    public Transform rightFlame;
    public Transform leftCockPitFlame;
    public Transform rightCockPitFlame;

    public Animator leftAnim;
    public Animator rightAnim;
    public Animator leftCockPitAnim;
    public Animator rightCockPitAnim;

    public Transform target;

    public bool isDead;
    public bool isShoot;
    public bool isFireZone;
    public bool isFireZoneReady;
    private int shootCnt = 0;
    private float currentTime = 0f;
    private float attackRateTime = 10f;

    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        //StartCoroutine("Sound");
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            if (isShoot)
            {
                isShoot = false;
                foreach(RobotFire robotFire in GetComponentsInChildren<RobotFire>())
                {
                    robotFire.OnStop();
                }
            }
            return;
        }
        if (!isFireZone)
        {
            LookTarget();
            Shoot();
        }
    }

    private void LookTarget()
    {
        if (audio.isPlaying)
        {
            audio.Stop();
        }
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 45 * Time.deltaTime);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < 30)
        {
            from = new Vector3(leftFlame.position.x, leftFlame.position.y, leftFlame.position.z);
            leftFlame.rotation = Quaternion.RotateTowards(leftFlame.rotation, Quaternion.LookRotation(to - from), 20 * Time.deltaTime);

            from = new Vector3(rightFlame.position.x, rightFlame.position.y, rightFlame.position.z);
            rightFlame.rotation = Quaternion.RotateTowards(rightFlame.rotation, Quaternion.LookRotation(to - from), 20 * Time.deltaTime);
        }

        leftCockPitFlame.rotation = Quaternion.RotateTowards(leftCockPitFlame.rotation, Quaternion.LookRotation(target.position - leftCockPitFlame.position), 20 * Time.deltaTime);
        leftCockPitFlame.localRotation = Quaternion.Euler(new Vector3(leftCockPitFlame.localEulerAngles.x, 0, 0));

        rightCockPitFlame.rotation = Quaternion.RotateTowards(rightCockPitFlame.rotation, Quaternion.LookRotation(target.position - rightCockPitFlame.position), 20 * Time.deltaTime);
        rightCockPitFlame.localRotation = Quaternion.Euler(new Vector3(rightCockPitFlame.localEulerAngles.x, 0, 0));
    }

    private void Shoot()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= attackRateTime)
        {
            currentTime = 0;

            isShoot = true;
            shootCnt++;
            if (shootCnt >= 3)
            {
                isFireZoneReady = true;
            }

            leftAnim.SetBool("IsShoot", true);
            rightAnim.SetBool("IsShoot", true);
            leftCockPitAnim.SetBool("IsShoot", true);
            rightCockPitAnim.SetBool("IsShoot", true);

            Invoke("AnimIsShootFalse", 6.5f);
        }
    }

    public void OnFireZone()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
        shootCnt = 0;
        isFireZoneReady = false;
        transform.Rotate(Vector3.up, 245 * Time.deltaTime);
        leftFlame.rotation = Quaternion.RotateTowards(leftFlame.rotation, transform.rotation, 20 * Time.deltaTime);
        rightFlame.rotation = Quaternion.RotateTowards(rightFlame.rotation, transform.rotation, 20 * Time.deltaTime);
        leftCockPitFlame.rotation = Quaternion.RotateTowards(leftCockPitFlame.rotation, transform.rotation, 20 * Time.deltaTime);
        rightCockPitFlame.rotation = Quaternion.RotateTowards(rightCockPitFlame.rotation, transform.rotation, 20 * Time.deltaTime);
    }

    private void AnimIsShootFalse()
    {
        leftAnim.SetBool("IsShoot", false);
        rightAnim.SetBool("IsShoot", false);
        leftCockPitAnim.SetBool("IsShoot", false);
        rightCockPitAnim.SetBool("IsShoot", false);
        isShoot = false;
    }

    private IEnumerator Sound()
    {
        while (true)
        {
            float y = transform.eulerAngles.y;
            yield return new WaitForSeconds(0.1f);

            if (y - transform.eulerAngles.y > 1f || y - transform.eulerAngles.y < -1f)
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
