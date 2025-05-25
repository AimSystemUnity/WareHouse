
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    // 분류 할 물건 type
    public MyObject.EObjectType type;

    // 나무판 Prefab
    public GameObject woodenPrefab;

    // 물건 올려놓고 있는 나무판
    public PaletteWooden wooden;

    // 가득찬 나무판의 부모
    public Transform trFullWoodenParent;

    public long machineId;

    public Transform botPos;

    void Start()
    {
        UDPClient.instance.onMessage += OnServerMessage;

        GameManager.instance.AddMachine(machineId, this);

        CreateWooden();
    }

    void Update()
    {
        
    }

    void CreateWooden()
    {
        // 나무판을 만든다 (나의 자식으로)
        GameObject go = Instantiate(woodenPrefab, transform);
        // 생성된 나무판에서  PaletteWooden 컴포넌트 가져오기
        wooden = go.GetComponent<PaletteWooden>();

        GameManager.instance.AddWooden(wooden);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.isServer == false) return;

        // 부딪힌 물체가 MyObject 컴포넌트를 가져오자.
        MyObject myObject = other.GetComponentInParent<MyObject>();
        if(myObject != null)
        {
            // 충돌한 물체 type 과 분류 할 물건 type 같다면
            if(myObject.type == type)
            {
                //AddObject(myObject);

                JObject jObject = new JObject();
                jObject["type"] = "ADD_OBJECT"; 
                jObject["productId"] = myObject.productId;
                jObject["machineId"] = machineId;
                UDPServer.instance.SendData(jObject.ToString());
            }
        }
    }

    void AddObject(MyObject myObject)
    {
        // 충돌한 물체를 나무판에 옮기자.
        bool isFull = wooden.AddObject(myObject);
        // 만약에 나무판이 가득 찼다면
        if (isFull)
        {

            // 가득찬 나무판을 trFullWoodenParent 의 자식으로!
            wooden.transform.parent = trFullWoodenParent;

            // 가득판 나무판 갯수에 따라서 위치를 변경
            trFullWoodenParent.localPosition += Vector3.left * 2;

            // 로봇에게 시키기
            GameManager.instance.CarryObject(wooden.botPos);

            // 새로운 나무판을 만들자.
            CreateWooden();
        }
    }

    void OnServerMessage(string message)
    {
        JObject jobject = JObject.Parse(message);
        string key = "type";
        if (jobject.ContainsKey(key) && jobject[key].ToString() == "ADD_OBJECT")
        {
            long _machineId = jobject["machineId"].ToObject<long>();
            if (machineId != _machineId) return;

            Machine machine = GameManager.instance.GetMachine(_machineId);

            long _productId = jobject["productId"].ToObject<long>();
            MyObject myObject = GameManager.instance.GetMyObject(_productId);

            machine.AddObject(myObject);
        }
    }
}
