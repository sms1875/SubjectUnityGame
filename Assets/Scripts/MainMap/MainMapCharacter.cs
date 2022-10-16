using UnityEngine;
using System.Collections;

public class MainMapCharacter : MonoBehaviour
{

	private Animator anim;
	private CharacterController controller;

	public float speed = 6.0f;
	public float turnSpeed = 400.0f;
	private Vector3 moveDirection = Vector3.zero;
	public float gravity = 20.0f;
	public bool isPlayerWalking = false;
	public Transform target;

	public static MainMapCharacter instance;
    private void Awake()
    {
		instance = this;
    }

    void Start()
	{
		controller = GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
		transform.position = MapManager.instance.Player.position;//위치 추가
	}

	void Update()
	{

		if (isPlayerWalking)
		{
			float step = speed * Time.deltaTime;
			anim.SetInteger("AnimationPar", 1);
			transform.LookAt(target);
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		}
		else
		{
			anim.SetInteger("AnimationPar", 0);
		}

		if (controller.isGrounded)
		{
			moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
		}
		/*
		float turn = Input.GetAxis("Horizontal");
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;*/

	}
	public void checkPlayerWalk()
	{
		isPlayerWalking = GameObject.Find("MapManager").GetComponent<MapManager>().isPlayerWalk;
	}
}