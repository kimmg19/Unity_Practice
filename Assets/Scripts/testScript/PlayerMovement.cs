using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;            // 플레이어 애니메이터
    Camera _camera;               // 메인 카메라
    CharacterController controller; // 캐릭터 컨트롤러

    public float speed = 5f;       // 기본 이동 속도
    public float runspeed = 8f;    // 달리기 속도
    public float finalSpeed;       // 최종 이동 속도
    public bool run;               // 달리기 상태 여부

    public bool toggleCameraRotation; // Alt 키를 눌렀을 때 시점 변환 여부
    public float smoothness = 10f;    // 회전 시 부드러움 정도

    void Start()
    {
        animator = this.GetComponent<Animator>();  // 애니메이터 컴포넌트 할당
        _camera = Camera.main;                     // 메인 카메라 할당
        controller = this.GetComponent<CharacterController>();  // 캐릭터 컨트롤러 할당
    }

    void Update()
    {
        // Alt 키를 누르면 시점 변환 활성화, 떼면 비활성화
        toggleCameraRotation = Input.GetKey(KeyCode.LeftAlt);

        // Shift 키를 누르면 달리기 활성화, 떼면 비활성화
        run = Input.GetKey(KeyCode.LeftShift);

        InputMovement();  // 이동 관련 입력 처리

        if(Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("onWeaponAttack");
        }
    }

    void LateUpdate()
    {
        // 시점 변환이 비활성화된 상태에서 플레이어를 메인 카메라의 정면 방향으로 회전
        if (!toggleCameraRotation)
        {
            Vector3 playerLotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerLotate), Time.deltaTime * smoothness);
        }
    }

    void InputMovement()
    {
        finalSpeed = (run) ? runspeed : speed; // Shift 키를 누르면 달리기 속도, 아니면 기본 속도

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // 입력에 따라 이동 방향 계산
        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        // 캐릭터 컨트롤러를 통해 이동
        controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);

        // 이동 속도에 따라 애니메이션 블렌딩
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
