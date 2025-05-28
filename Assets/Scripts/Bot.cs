using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Bot : NetView
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

    // 현재 시간
    float currTime;

    // 이전 위치값
    Vector3 startPos;
    // 도착 위치값
    Vector3 endPos;
    // 이전 회전값
    Quaternion startRot;
    // 도착 회전값
    Quaternion endRot;
    // 걸리 시간
    float timeTaken;

    protected override void Start()
    {
        base.Start();

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

        // 현재 위치를 startPos 설정
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        // 만약에 서버라면
        if(GameManager.instance.isServer)
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

            // 1 초에 30 번 나의 위치를 클라이언트에게 알려주자!
            currTime += Time.deltaTime;
            if(currTime > 1.0f / 10)
            {
                float dist = Vector3.Distance(startPos, transform.position);
                if (dist > 0)
                {
                    JObject jObect = new JObject();
                    jObect["net_id"] = netId;
                    jObect["net_type"] = (int)ENetType.NET_BOT_TRANSFORM;
                    jObect["s_pos"] = JsonUtility.ToJson(startPos);
                    jObect["e_pos"] = JsonUtility.ToJson(transform.position);
                    jObect["s_rot"] = JsonUtility.ToJson(startRot);
                    jObect["e_rot"] = JsonUtility.ToJson(transform.rotation);
                    
                    jObect["time"] = currTime;
                    UDPServer.instance.SendData(jObect.ToString());

                    currTime = 0;
                    // 이전 값을 갱신
                    startPos = transform.position;
                    startRot = transform.rotation;
                }
            }
        }
        else
        {
            // 서버에서 온 데이터로 움직이자.
            if(currTime < timeTaken)
            {
                currTime += Time.deltaTime;
                // currTime 과 timeTaken 비율
                float ratio = currTime / timeTaken;
                if (ratio > 1) ratio = 1;
                transform.position = Vector3.Lerp(startPos, endPos, ratio);
                transform.rotation = Quaternion.Lerp(startRot, endRot, ratio);
            }
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
            SendTrigger("CARRY");
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
            SendTrigger("MOVE");

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
            SendTrigger("IDLE");
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
            //// 방향을 나의 앞방향으로
            //transform.forward = dir.normalized;
            //// 나의 앞방으로 이동하자.
            //transform.position += transform.forward * 10 * Time.deltaTime;

            // 목적지로 이동하자.
            navi.SetDestination(moveTarget.position);

            return false;
        }
    }

    void SendTrigger(string trigger)
    {
        JObject jObject = new JObject();
        jObject["net_id"] = netId;
        jObject["net_type"] = (int)ENetType.NET_SEND_TRIGGER;
        jObject["trigger"] = trigger;

        UDPServer.instance.SendData(jObject.ToString());
    }

    public override JObject OnMessage(string message)
    {
        JObject jObject =  base.OnMessage(message);

        if (jObject["net_type"].ToObject<ENetType>() == ENetType.NET_BOT_TRANSFORM)
        {
            if(GameManager.instance.isServer == false)
            {
                startPos = JsonUtility.FromJson<Vector3>(jObject["s_pos"].ToString());
                endPos = JsonUtility.FromJson<Vector3>(jObject["e_pos"].ToString());
                startRot = JsonUtility.FromJson<Quaternion>(jObject["s_rot"].ToString());
                endRot = JsonUtility.FromJson<Quaternion>(jObject["e_rot"].ToString());
                timeTaken = jObject["time"].ToObject<float>();
                currTime = 0;
            }
        }
        else if(jObject["net_type"].ToObject<ENetType>() == ENetType.NET_SEND_TRIGGER)
        {
            anim.SetTrigger(jObject["trigger"].ToString());
        }

        return jObject;
    }
}
