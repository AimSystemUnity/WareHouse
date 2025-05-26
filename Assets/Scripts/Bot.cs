using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    // Bot 의 상태
    public enum EBotState
    {
        IDLE,
        MOVE_TO_OBJECT,
        MOVE_TO_STORAGE,
        MOVE_TO_ORIGIN
    }

    // 현재 상태
    public EBotState currState;
    // 현재 목적지
    public Transform moveTarget;
    // wooden Transfrom
    Transform wooden;
    // 창고 Transform
    Transform storage;
    // 초기 Transform
    public Transform origin;

    // nav agent
    NavMeshAgent navi;

    // animator 컴포넌트
    Animator anim;

    // Hip Transform
    public Transform hip;

    void Start()
    {
        // GameManager 에게 나를 알려주자.
        GameManager.instance.allBot.Add(this);
        // 초기 Transform 설정
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        go.name = name + " origin";
        origin = go.transform;

        // nav mesh agent 컴포넌트 가져오자.
        navi = GetComponent<NavMeshAgent>();

        // 자식에 있는 Animator 컴포넌트 가져오자
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch(currState)
        {
            case EBotState.MOVE_TO_OBJECT:
                UpdateMoveToObject();
                break;
            case EBotState.MOVE_TO_STORAGE:
                UpdateMoveToStorage();
                break;
            case EBotState.MOVE_TO_ORIGIN:
                UpdateMoveToOrigin();
                break;
        }
    }

    public void ChageMoveToObject(Transform _wooden, Transform _storage)
    {
        // 현재 상태를 MOVE_TO_OBJECT 로
        currState = EBotState.MOVE_TO_OBJECT;
        // 현재 목적지를 wooden 으로
        moveTarget = wooden = _wooden;
        // 창고 transform
        storage = _storage;
        // MOVE 애니메이션 실행
        anim.SetTrigger("MOVE");
    }

    void UpdateMoveToObject()
    {
        bool goal = Move();

        if (goal)
        {
            // 현재 상태를 MOVE_TO_STORAGE 로
            currState = EBotState.MOVE_TO_STORAGE;
            // wooden을 나의 자식 (wooden 의 부모를 나로한다)
            wooden.parent = hip;
            wooden.localPosition = new Vector3(0, -0.06f, 0.88f);
            wooden.localEulerAngles = new Vector3(356.34f, 176.17f, 358.34f);

            // 현재 목적지를 storage 로!
            moveTarget = storage;
            // CARRY 애니메이션 실행
            anim.SetTrigger("CARRY");
        }        
    }

    void UpdateMoveToStorage()
    {
        bool goal = Move();

        if(goal)
        {
            // 현재 상태를 MOVE_TO_ORIGIN 로
            currState = EBotState.MOVE_TO_ORIGIN;
            // 현재 목적지를 처음 위치로!
            moveTarget = origin;
            // 내가 들고 있는 wooden 삭제
            Destroy(wooden.gameObject);
            // MOVE 애니메이션 실행
            anim.SetTrigger("MOVE");

            // 창고에 쌓인 갯수를 증가시키자.
            storage.GetComponent<Storage>().AddCount();
        }       
    }

    void UpdateMoveToOrigin()
    {
        bool goal = Move();

        if (goal)
        {
            // 현재 상태를 IDLE 로
            currState = EBotState.IDLE;
            // IDLE 애니메이션 실행
            anim.SetTrigger("IDLE");
        }
    }

    bool Move()
    {
        // moveTarget 향하는 방향을 구하자.
        Vector3 dir = moveTarget.position - transform.position;

        // 만약에 dir 의 크기가 0.1 보다 작으면
        if (dir.magnitude < 0.1f)
        {
            return true;
        }
        else
        {
            // 방향을 나의 앞방향으로
            //transform.forward = dir.normalized;
            // 나의 앞방으로 이동하자.
            //transform.position += transform.forward * 10 * Time.deltaTime;

            // 목적지로 이동하자.
            navi.SetDestination(moveTarget.position);

            return false;
        }
    }
}
