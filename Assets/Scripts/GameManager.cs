using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class GameManager : NetView
{
    // 나 자신
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        
    }

    // 변수를 Inspector 에 노출 (변수가 public, private 상관없이)
    [SerializeField]
    int number = 10;

    // 컨베이어 벨트 동작 유무 
    //[HideInInspector] // 변수를 Inspector에서 노출되지 않게 하기
    public bool isOn;
    // 컨베이어 벨트 동작 UI (Text)
    public TMP_Text txtOnOff;

    public bool isServer;


    public void OnClickOnOff()
    {
        if (isServer == false) return;

        JObject jObject = new JObject();
        jObject["net_id"] = netId;
        jObject["net_type"] = (int)ENetType.NET_CONVEYOR_IS_ON;
        jObject["is_on"] = !isOn;

        UDPServer.instance.SendData(jObject.ToString());
    }

    public void ToggleServer(bool on)
    {
        UDPServer.instance.gameObject.SetActive(true);
        UDPClient.instance.gameObject.SetActive(true);
        isServer = true;
    }
    public void ToggleClient(bool on)
    {
        UDPClient.instance.gameObject.SetActive(true);
    }

    public override void OnMessage(string msg)
    {
        base.OnMessage(msg);

        JObject jObject = JObject.Parse(msg);
        ENetType type = jObject["net_type"].ToObject<ENetType>();

        if(type == ENetType.NET_CONVEYOR_IS_ON)
        {
            // true -> false, false -> true
            isOn = jObject["is_on"].ToObject<bool>();

            // isOn 의 값에 따라서 버튼의 text 변환
            string s = isOn ? "Stop" : "Start";
            txtOnOff.SetText(s);
        }
    }
}
