using Newtonsoft.Json.Linq;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public enum EBotState
    {
        IDLE,
        MOVE_TO_OBJECT,
        MOVE_TO_STORAGE,
        MOVE_TO_ORIGIN
    }

    public EBotState currState;

    public Transform storage;
    public Transform origin;
    public Transform moveTarget;
    public int botId;

    NavMeshAgent nav;
    Animator anim;

    void Start()
    {
        UDPClient.instance.onMessage += OnServerMessage;

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

    }

    public IEnumerator Co()
    {
        yield break;
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            
        }
    }

    Vector3 prevPos;
    void Update()
    {
        if(GameManager.instance.isServer)
        {
            if(prevPos != transform.position)
            {
                JObject jobject = new JObject();
                jobject["type"] = "TR_POSITION";
                jobject["x"] = transform.position.x;
                jobject["y"] = transform.position.y;
                jobject["z"] = transform.position.z;
                jobject["botId"] = botId;
                UDPServer.instance.SendData(jobject.ToString());
            }
            
            switch (currState)
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

            prevPos = transform.position;
        }       
        
    }


    public void Carry(Transform wooden)
    {
        if(currState == EBotState.IDLE)
        {
            anim.SetTrigger("WALK");
        }

        currState = EBotState.MOVE_TO_OBJECT;
        moveTarget = wooden;
    }

    

    void UpdateMoveToObject()
    {
        if(Move())
        {
            currState = EBotState.MOVE_TO_STORAGE;

            moveTarget.GetComponentInParent<NavMeshObstacle>().enabled = false;

            moveTarget.parent.parent = transform;

            moveTarget = storage;

            anim.SetTrigger("CARRY");

        }
    }

    void UpdateMoveToStorage()
    {
        if(Move())
        {
            Destroy(GetComponentInChildren<PaletteWooden>().gameObject);

            currState = EBotState.MOVE_TO_ORIGIN;
            moveTarget = origin;
            GameManager.instance.allBots.Add(this);
            anim.SetTrigger("WALK");

        }
    }

    void UpdateMoveToOrigin()
    {
        if(Move())
        {
            currState = EBotState.IDLE;
            anim.SetTrigger("IDLE");
        }
    }

    bool Move()
    {
        Vector3 dir = moveTarget.position - transform.position;

        if (dir.magnitude < 0.1f)
        {
            return true;
        }
        else
        {
            //transform.forward = dir.normalized;
            //transform.position += transform.forward * 5 * Time.deltaTime;
            nav.SetDestination(moveTarget.position);
            return false;
        }
    }

    void OnServerMessage(string message)
    {
        JObject jobject = JObject.Parse(message);
        string key = "type";
        if(jobject.ContainsKey(key))
        {
            string valueKey = jobject[key].ToString();
            if(valueKey == "TR_POSITION")
            {
                if(!GameManager.instance.isServer)
                {
                    if (jobject["botId"].ToObject<int>() == botId)
                    {
                        Vector3 pos = new Vector3(
                        jobject["x"].ToObject<float>(),
                        jobject["y"].ToObject<float>(),
                        jobject["z"].ToObject<float>());
                        transform.position = pos;
                    }                    
                }
            }
        }       
    }
}
