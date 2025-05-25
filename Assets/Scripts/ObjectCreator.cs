using Newtonsoft.Json.Linq;
using UnityEngine;

public class ObjectCreator : NetView
{
    // MyObject Prefab
    public GameObject myObjectPrefab;

    // 생성시간
    public float createTime = 3;
    // 현재시간
    float currTime;


    void Update()
    {
        if (GameManager.instance.isOn == false) return;
        if (GameManager.instance.isServer == false) return;

        // 시간을 흐르게 하자.
        currTime += Time.deltaTime;
        // 만약에 현재시간이 생성시간보다 커지면
        if(currTime > createTime)
        {
            // 현재시간 초기화
            currTime = 0;
            // 랜덤한 오브젝트 뽑자
            int objType = Random.Range(0, (int)MyObject.EObjectType.MAX);

            JObject jObject = new JObject();
            jObject["net_id"] = netId;
            jObject["net_type"] = (int)ENetType.NET_CREATE_MY_OBJECT;
            jObject["obj_type"] = objType;

            UDPServer.instance.SendData(jObject.ToString());
        }
    }

    public override void OnMessage(string msg)
    {
        base.OnMessage(msg);

        JObject jObject = JObject.Parse(msg);
        ENetType type = jObject["net_type"].ToObject<ENetType>();

        if (type == ENetType.NET_CREATE_MY_OBJECT)
        {
            int objType = jObject["obj_type"].ToObject<int>();

            // MyObject 생성
            GameObject go = Instantiate(myObjectPrefab);
            // 위치를 나의 위치에 놓자.
            go.transform.position = transform.position;
            
            // MyObject 컴포넌트 가져오자
            MyObject myObject = go.GetComponent<MyObject>();
            // 뽑힌 오브젝트를 전달해서 생성
            myObject.CreateObject(objType);
        }
    }
}
