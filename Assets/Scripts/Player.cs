using UnityEngine;

public class Player : MonoBehaviour
{
    // ������ �� �ִ�?
    bool canMove;

    // �̵� �ӷ�
    public float moveSpeed = 5;

    // ȸ�� �ӷ�
    public float rotSpeed = 200;

    // ȸ�� ��
    float rotX;
    float rotY;

    void Start()
    {
        // ó�� ������ rotX, rotY �� ����
        rotX = transform.eulerAngles.x;
        rotY = transform.eulerAngles.y;
    }

    void Update()
    {
        // ���࿡ canMove �� false �� �Լ� ������.
        if (CanMove() == false) return;

        ChangeMoveSpeed();
        Move();
        Rotate();
    }

    bool CanMove()
    {
        // ���࿡ ���콺 ������ ��ư�� ������
        if(Input.GetMouseButtonDown(1))
        {
            // canMove �� true
            canMove = true;
        }
        // ���࿡ ���콺 ������ ��ư�� ����
        else if(Input.GetMouseButtonUp(1))
        {
            // canMove �� false
            canMove = false;
        }
        return canMove;
    }

    void ChangeMoveSpeed()
    {
        // ���콺 �� ������ ���� �޾ƿ���.
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        // �� ������ moveSpeed ���� ��������.
        moveSpeed += wheel;
    }

    void Move()
    {
        // W, A, S, D Ű�� �Է��� ����.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �Է� ���� ������ ������ ������.
        Vector3 dir = transform.right * h + transform.forward * v;

        // ���� �������� ��������.
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void Rotate()
    {
        // ���콺 �����Ӱ��� �޾ƿ���.
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // ȸ�� ���� ����
        rotX += my * rotSpeed * Time.deltaTime;
        rotY += mx * rotSpeed * Time.deltaTime;

        // ������Ʈ�� ȸ�� ������ ����
        transform.eulerAngles = new Vector3(-rotX, rotY, 0); 

    }
}
