using UnityEngine;

public class Player : MonoBehaviour
{
    // 움직일 수 있니?
    bool canMove;

    // 이동 속력
    public float moveSpeed = 5;

    // 회전 속력
    public float rotSpeed = 200;

    // 회전 값
    float rotX;
    float rotY;

    void Start()
    {
        // 처음 각도를 rotX, rotY 에 설정
        rotX = transform.eulerAngles.x;
        rotY = transform.eulerAngles.y;
    }

    void Update()
    {
        // 만약에 canMove 가 false 면 함수 나가자.
        if (CanMove() == false) return;

        ChangeMoveSpeed();
        Move();
        Rotate();   
    }

    bool CanMove()
    {
        // 만약에 마우스 오른쪽 버튼을 누르면
        if(Input.GetMouseButtonDown(1))
        {
            // canMove 를 true
            canMove = true;
        }
        // 만약에 마우스 오른쪽 버튼을 떼면
        else if(Input.GetMouseButtonUp(1))
        {
            // canMove 를 false
            canMove = false;
        }
        return canMove;
    }

    void ChangeMoveSpeed()
    {
        // 마우스 휠 돌리는 값을 받아오자.
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        // 그 값으로 moveSpeed 값을 변경하자.
        moveSpeed += wheel;
    }

    void Move()
    {
        // W, A, S, D 키의 입력을 받자.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 입력 받은 값으로 방향을 구하자.
        Vector3 dir = transform.right * h + transform.forward * v;

        // 구한 방향으로 움직이자.
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void Rotate()
    {
        // 마우스 움직임값을 받아오자.
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 회전 값을 누적
        rotX += my * rotSpeed * Time.deltaTime;
        rotY += mx * rotSpeed * Time.deltaTime;

        // 오브젝트를 회전 값으로 설정
        transform.eulerAngles = new Vector3(-rotX, rotY, 0); 

    }
}
