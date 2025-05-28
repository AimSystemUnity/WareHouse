using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ObjectCreator : NetView
{
    // 나 자신
    public static ObjectCreator instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // MyObject Prefab
    public GameObject myObjectPrefab;

    // 생성시간
    public float createTime = 3;
    
    // 현재 돌아가고 있는 코루틴
    Coroutine currCo;

    // 모든 오브젝를 가지고 있는 변수
    Dictionary<long, MyObject> allMyObject = new Dictionary<long, MyObject>();
    // 마지막 오브젝트 id 
    long myObjectId;

    protected override void Start()
    {
        base.Start();

        GameManager.instance.delegateOnOff += OnOff;        
    }

    public long AddObject(MyObject myObject)
    {
        myObjectId++;

        allMyObject[myObjectId] = myObject;

        return myObjectId;
    }

    public MyObject GetObject(long id)
    {
        return allMyObject[id];
    }

    void OnOff()
    {
        // 만약에 서버가 아니라면 함수 나가자.
        if (GameManager.instance.isServer == false) return;

        if (GameManager.instance.isOn == true)
        {
            currCo = StartCoroutine(CoCreateMyObject());
        }
        else
        {
            StopCoroutine(currCo);
        }
    }

    IEnumerator CoCreateMyObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(createTime);

            // 랜덤한 오브젝트 뽑자
            int type = Random.Range(0, (int)MyObject.EObjectType.MAX);

            JObject jObject = new JObject();
            jObject["net_id"] = netId;
            jObject["net_type"] = (int)ENetType.NET_CREATE_MY_OBJECT;
            jObject["obj_type"] = type;

            UDPServer.instance.SendData(jObject.ToString());
        }
    }

    public override JObject OnMessage(string message)
    {
        JObject jObject = base.OnMessage(message);

        if (jObject["net_type"].ToObject<ENetType>() == ENetType.NET_CREATE_MY_OBJECT)
        {
            // MyObject 생성
            GameObject go = Instantiate(myObjectPrefab);
            // 위치를 나의 위치에 놓자.
            go.transform.position = transform.position;

            // 랜덤한 오브젝트 뽑자
            int type = jObject["obj_type"].ToObject<int>();
            // MyObject 컴포넌트 가져오자
            MyObject myObject = go.GetComponent<MyObject>();

            // myObject 를 allMyObject 에 추가
            long id = AddObject(myObject);

            // 뽑힌 오브젝트를 전달해서 생성
            myObject.CreateObject(type, id);
        }


        return jObject;
    }
}