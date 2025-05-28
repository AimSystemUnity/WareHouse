using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class Storage : NetView
{
    // 어떤 오브젝트를 쌓을 수 있는지
    public MyObject.EObjectType type;

    // 전체 갯수 
    int total;

    // 전체 갯수 UI
    TMP_Text txt_Total;

    protected override void Start()
    {
        base.Start();

        // TMP_Text 컴포넌트 가져오자
        txt_Total = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        
    }

    public void AddCount()
    {
        JObject jObject = new JObject();
        jObject["net_id"] = netId;
        jObject["net_type"] = (int)ENetType.NET_ADD_STORAGE_COUNT;
        jObject["count"] = (total += 4);

        UDPServer.instance.SendData(jObject.ToString());        
    }

    public override JObject OnMessage(string message)
    {
        JObject jObject = base.OnMessage(message);

        if (jObject["net_type"].ToObject<ENetType>() == ENetType.NET_ADD_STORAGE_COUNT)
        {
            // 전체 갯수를 4개 증가시키자.
            total = jObject["count"].ToObject<int>();

            // 전체 갯수 UI 갱신
            txt_Total.SetText(total.ToString());
        }

        return jObject;
    }
}
