using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ObjectCreator : NetView
{
    // MyObject Prefab
    public GameObject myObjectPrefab;

    // 생성시간
    public float createTime = 3;
    
    // 현재 돌아가고 있는 코루틴
    Coroutine currCo;

    protected override void Start()
    {
        base.Start();

        GameManager.instance.delegateOnOff += OnOff;        
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
            // 뽑힌 오브젝트를 전달해서 생성
            myObject.CreateObject(type);
        }


        return jObject;
    }
}