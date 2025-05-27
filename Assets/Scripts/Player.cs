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

    // 마우스 클릭으로 체크하고 싶은 Layer
    public LayerMask layerMask;
    void Update()
    {
        // 만약에 왼쪽 마우스를 누르면
        if(Input.GetMouseButtonDown(0))
        {
            // cube 위치에서 cube 앞방향으로 설정된 Ray 를 만든다.
            //Ray ray = new Ray(transform.position, transform.forward);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Ray 이용해서 Raycast 실행
            // 만약에 어딘가에 부딪혔다면           

            RaycastHit hit;
            // Physics.SphereCast
            if(Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
            {
                print(hit.collider.name);

                // 만약에 부딪힌 물체가 Pc.008 이면
                if(hit.collider.name.Contains("Pc.008"))
                {
                    // 부모의 컴포넌트 중 Machine 가져오자.
                    Machine machine = hit.collider.GetComponentInParent<Machine>();
                    // 가져온 컴포넌트의 기능 중 OnOff 함수 실행
                    machine.OnOff();
                }
            }
        }


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
