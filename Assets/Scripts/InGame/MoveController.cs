using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{

    //속도관련 설정
    private float movementSpeed;
    [SerializeField] private float walkSpeed = 5f, runSpeed = 7f, crouchSpeed = 3f;
    [SerializeField] private float runBuildUpSpeed = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpSpeed = 4F;
    [SerializeField] private float dashSpeed=2f;
    [SerializeField] private float dashTime=0.15f;
    [SerializeField] private float dashDelay = 1f;

    public bool isDash = true;

    //키설정
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashkey = KeyCode.LeftControl;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;

    //캐릭터 설정
    private float originalHight, crouchHight; //앉을 때 높이

    private CharacterController charController;

    //카메라설정
    private Vector3 moveDir = Vector3.zero;
    private Camera cam;
    [SerializeField] private GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        charController = PlayerController.instance.charController;
        cam = FindObjectOfType<Camera>();
        originalHight = charController.height; //CharacterController 높이 저장
        crouchHight = originalHight * 3 / 5; //앉은키 설정
    }

    private void FixedUpdate()
    {
        if(PlayerController.instance.IsStunned)
        {
            return;
        }
        Crouch();
        SetMovementSpeed();
        Move();
        CameraRotation();
    }

    private void Crouch()//앉기 설정
    {
        if (Input.GetKey(crouchKey) && charController.isGrounded && !PlayerController.instance.isRun) 
        {
            movementSpeed = crouchSpeed;
            charController.height = crouchHight;
            PlayerController.instance.isCrouch=true;
        }
        if (Input.GetKeyUp(crouchKey) && PlayerController.instance.isCrouch)
        {
            charController.height = originalHight;
            PlayerController.instance.isCrouch = false;
        }
    }

    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey) && !PlayerController.instance.isCrouch && PlayerController.instance.isWalk) //달리는속도
        {
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
            PlayerController.instance.isRun = true;
        }
        else if (!PlayerController.instance.isCrouch)//걷는속도
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
            PlayerController.instance.isRun = false;
        }
    }


    private void Move()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");


        if (charController.isGrounded)
        {
            if (Input.GetKeyDown(dashkey) &&(vertInput != 0 || horizInput != 0) && PlayerController.instance.currentDashGauge >= 20f && isDash)//이동중일때만 대쉬 발동
            {
                StartCoroutine(Dash());
            }
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= movementSpeed;

            if (Input.GetKey(jumpKey) && !PlayerController.instance.isGround)//점프 설정
                moveDir.y = jumpSpeed;

        }
        moveDir.y -= gravity * Time.deltaTime;
        charController.Move(moveDir * Time.deltaTime);
   
        if ((vertInput != 0 || horizInput != 0) && !PlayerController.instance.isRun && charController.isGrounded)
        {
            PlayerController.instance.isWalk = true;
        }
        else if (vertInput == 0 && horizInput == 0)
        {
            PlayerController.instance.isWalk = false;
        }

    }

    private IEnumerator Dash()
    {
        isDash = false;
        float startTime = Time.time;
        float FOV = cam.fieldOfView;
        PlayerController.instance.currentDashGauge -= 20f;//사용시 게이지 감소
        dashEffect.SetActive(true);
        while (Time.time < startTime + dashTime)
        {
            charController.Move(moveDir * dashSpeed * Time.deltaTime);
            //cam.fieldOfView+=Time.deltaTime*10;
            yield return null;
        }
        dashEffect.SetActive(false);
        //cam.fieldOfView =FOV;
        yield return new WaitForSeconds(dashDelay);
        isDash = true;
    }

    private void CameraRotation()//캐릭터 좌우회전
    {
        float mouseX = Input.GetAxis("Mouse X") * DataManager.instance.mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

}
