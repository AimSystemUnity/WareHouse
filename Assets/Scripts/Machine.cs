
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Machine : NetView
{
    // 분류 할 물건 type
    public MyObject.EObjectType type;

    // 나무판 Prefab
    public GameObject woodenPrefab;

    // 물건 올려놓고 있는 나무판
    public PaletteWooden wooden;

    // 가득찬 나무판의 부모
    public Transform trFullWoodenParent;

    // 가득찬 wooden 을 옮겨야 하는 창고
    public Transform storage;

    // 분류를 할 수 있는지
    bool isOn;

    protected override void Start()
    {
        base.Start();

        CreateWooden();

        // 컨베이어 벨트 돌아가면 기계도 동작하게
        GameManager.instance.delegateOnOff += OnOff;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        // 서버가 아니면 함수 나가자.
        if (GameManager.instance.isServer == false) return;

        // 만약에 동작이 꺼져있으면 함수를 나가자.
        if (isOn == false) return;

        // 부딪힌 물체가 MyObject 컴포넌트를 가져오자.
        MyObject myObject = other.GetComponentInParent<MyObject>();
        if(myObject != null)
        {
            // 충돌한 물체 type 과 분류 할 물건 type 같다면
            if(myObject.type == type)
            {
                JObject jObject = new JObject();
                jObject["net_id"] = netId;
                jObject["net_type"] = (int)ENetType.NET_ADD_OBJECT;
                //jObject["my_object"] = myObject.ToString();
            }
        }
    }

    public override JObject OnMessage(string message)
    {
        JObject jObject = base.OnMessage(message);

        //// 충돌한 물체를 나무판에 옮기자.
        //bool isFull = wooden.AddObject(myObject);
        //// 만약에 나무판이 가득 찼다면
        //if (isFull)
        //{
        //    // 가득찬 나무판을 trFullWoodenParent 의 자식으로!
        //    wooden.transform.parent = trFullWoodenParent;

        //    // 가득판 나무판 갯수에 따라서 위치를 변경
        //    trFullWoodenParent.localPosition += Vector3.left * 2;

        //    // 로봇에게 wooden 옮기라고 명령
        //    GameManager.instance.FindClosestBot(wooden.transform, storage);

        //    // 새로운 나무판을 만들자.
        //    CreateWooden();
        //}

        return jObject;
    }

    public void OnOff()
    {
        // false -> true, true -> false
        isOn = !isOn;

        // 나에게 붙어있는 Animator 컴포넌트 가져오자.
        Animator[] anims = GetComponentsInChildren<Animator>();
        for(int i = 0; i < anims.Length; i++)
        {
            anims[i].enabled = isOn;
        }
    }
}
