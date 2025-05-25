using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    // MyObject Prefab
    public GameObject myObjectPrefab;

    // 생성시간
    public float createTime = 3;
    // 현재시간
    float currTime;

    long lastProductId;

    void Start()
    {
        UDPClient.instance.onMessage += OnServerMessage;
    }

    void Update()
    {
        if (GameManager.instance.isOn == false) return;
        if (GameManager.instance.isServer == false) return;

        // 시간을 흐르게 하자.
        currTime += Time.deltaTime;
        // 만약에 현재시간이 생성시간보다 커지면
        if(currTime > createTime)
        {
            // 랜덤한 오브젝트 뽑자
            int type = Random.Range(0, (int)MyObject.EObjectType.MAX);
            //CreateMyObject(type);

            JObject jobject = new JObject();
            jobject["CREATE_MYOBJECT"] = type;
            UDPServer.instance.SendData(jobject.ToString());

            // 현재시간 초기화
            currTime = 0;
        }
    }

    void CreateMyObject(int type)
    {
        // MyObject 생성
        GameObject go = Instantiate(myObjectPrefab);
        // 위치를 나의 위치에 놓자.
        go.transform.position = transform.position;

        // MyObject 컴포넌트 가져오자
        MyObject myObject = go.GetComponent<MyObject>();
        // 뽑힌 오브젝트를 전달해서 생성
        myObject.CreateObject(type, ++lastProductId);

        GameManager.instance.AddMyObject(lastProductId, myObject);
    }


    // 서버에서 응답 받는 함수
    void OnServerMessage(string message)
    {
        JObject jobject = JObject.Parse(message);
        string key = "CREATE_MYOBJECT";
        if (jobject.ContainsKey(key))
        {
            CreateMyObject(jobject[key].ToObject<int>());

        }
    }
}
