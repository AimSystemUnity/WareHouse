using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Tilemaps.Tilemap;

public class Bot : NetView
{
    public NavMeshAgent agent;

    float currTime = 0;
    float speed = 0;
    Vector3 prevPos;
    Vector3 targetPos;
    Quaternion prevRot;
    Quaternion targetRot;


    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();

        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && name.Equals("X Bot"))
        {
            if (GameManager.instance.isServer == false) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(1) && name.Equals("X Bot (1)"))
        {
            if (GameManager.instance.isServer == false) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (GameManager.instance.isServer == true)
        {
            currTime += Time.deltaTime;
            if(currTime >= 1.0f / 30)
            {
                float dist = Vector3.Distance(prevPos, transform.position);
                if (dist <= 0) return;
                speed = dist / currTime;
                print(speed);

                JObject jObject = new JObject();
                jObject["net_id"] = netId;
                jObject["net_type"] = (int)ENetType.NET_POSITION;
                jObject["s_pos"] = JsonUtility.ToJson(prevPos);
                jObject["e_pos"] = JsonUtility.ToJson(transform.position);
                jObject["s_rot"] = JsonUtility.ToJson(prevRot);
                jObject["e_rot"] = JsonUtility.ToJson(transform.rotation);

                jObject["time"] = currTime;
                UDPServer.instance.SendData(jObject.ToString());

                prevPos = transform.position;
                prevRot = transform.rotation;
                currTime = 0;
            }
        }
        else
        {
            
                if (currTime < speed)
                {
                    currTime += Time.deltaTime;
                    float fTime = currTime / speed;
                    if (fTime > 1) fTime = 1;
                    transform.position = Vector3.Lerp(prevPos, targetPos, fTime);
                    transform.rotation = Quaternion.Lerp(prevRot, targetRot, fTime);
                    
                }
            
        }
        
    }

    
    public override void OnMessage(string msg)
    {
        base.OnMessage(msg);

        if (GameManager.instance.isServer == true) return;

        JObject jObject = JObject.Parse(msg);
        ENetType type = jObject["net_type"].ToObject<ENetType>();

        if (type == ENetType.NET_POSITION)
        {
            prevPos = JsonUtility.FromJson<Vector3>(jObject["s_pos"].ToString());
            targetPos = JsonUtility.FromJson<Vector3>(jObject["e_pos"].ToString());
            prevRot = JsonUtility.FromJson<Quaternion>(jObject["s_rot"].ToString());
            targetRot = JsonUtility.FromJson<Quaternion>(jObject["e_rot"].ToString());
            speed = jObject["time"].ToObject<float>();
            currTime = 0;
        }
    }
}
