using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
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

    private void Start()
    {
        UDPClient.instance.onMessage += OnServerMessage;

        GameObject storage = GameObject.Find("Garage");
        for (int i = 0; i < 10; i++)
        {
            GameObject origin = new GameObject();
            origin.transform.position = botPos.position + Vector3.right * i * 2;

            GameObject go = Instantiate(botPrefab);
            go.transform.position = origin.transform.position;

            Bot bot = go.GetComponent<Bot>();
            allBots.Add(bot);
            bot.botId = i;
            bot.storage = storage.transform;
            bot.origin = origin.transform;
        }

    }

    private void OnDestroy()
    {
        UDPClient.instance.onMessage -= OnServerMessage;
    }

    // 변수를 Inspector 에 노출 (변수가 public, private 상관없이)
    [SerializeField]
    int number = 10;

    

    // 컨베이어 벨트 동작 유무 
    //[HideInInspector] // 변수를 Inspector에서 노출되지 않게 하기
    public bool isOn;
    // 컨베이어 벨트 동작 UI (Text)
    public TMP_Text txtOnOff;
    public Button btnOnOff;
    public void OnClickOnOff()
    {
        //ChangeValueOnOff(!isOn);

        JObject jObject = new JObject();
        jObject["CONVEYOR_ON"] = !isOn;
        UDPServer.instance.SendData(jObject.ToString());        
    }

    void ChangeValueOnOff(bool on)
    {
        // true -> false, false -> true
        isOn = on;

        // isOn 의 값에 따라서 버튼의 text 변환
        string s = isOn ? "Stop" : "Start";
        txtOnOff.SetText(s);
    }

    public bool isServer;
    public Toggle toggleServer;
    public Toggle toggleClient;
    public void OnToggleServer(bool isToggle)
    {
        UDPServer.instance.gameObject.SetActive(true);
        UDPClient.instance.gameObject.SetActive(true);

        toggleServer.interactable = toggleClient.interactable = false;
        btnOnOff.interactable = isServer = true;

        for(int i = 0; i < allBots.Count; i++)
        {
            allBots[i].StartCoroutine(allBots[i].Co());
        }
    }

    public void OnToggleClient(bool isToggle)
    {
        UDPClient.instance.gameObject.SetActive(true);
        toggleServer.interactable = toggleClient.interactable = false;
    }

    Dictionary<long, MyObject> dicMyObject = new Dictionary<long, MyObject>();
    public void AddMyObject(long productId, MyObject myObject)
    {
        dicMyObject[productId] = myObject;
    }

    public MyObject GetMyObject(long productId)
    {
        return dicMyObject[productId];
    }

    Dictionary<long, Machine> dicMachine = new Dictionary<long, Machine>();
    public void AddMachine(long machineId, Machine machine)
    {
        dicMachine[machineId] = machine;
    }
    public Machine GetMachine(long machineId)
    {
        return dicMachine[machineId];
    }

    public Transform botPos;
    public List<Bot> allBots = new List<Bot>();
    public GameObject botPrefab;

    public void CarryObject(Transform wooden)
    {
        if (!isServer) return;

        float minDist = float.MaxValue;
        int idx = -1;
        for(int i = 0; i < allBots.Count; i++)
        {
            if (allBots[i].currState == Bot.EBotState.MOVE_TO_OBJECT ||
                allBots[i].currState == Bot.EBotState.MOVE_TO_STORAGE) continue;

            float dist = Vector3.Distance(wooden.position, allBots[i].transform.position);
            if(dist < minDist)
            {
                idx = i;
                minDist = dist;
            }
        }

        if(idx != -1)
        {
            //allBots.RemoveAt(idx);
            allBots[idx].Carry(wooden);

            //JObject jobject = new JObject();
            //jobject["type"] = "CARRY";
            //jobject["bot"] = idx;
            //jobject["wooden"] = wooden.GetComponentInParent<PaletteWooden>().woodenId;

            //UDPServer.instance.SendData(jobject.ToString());
        }
    }
    Dictionary<long, PaletteWooden> dicWooden = new Dictionary<long, PaletteWooden>();
    long lastWoodenIdx = 0;
    public void AddWooden(PaletteWooden wooden)
    {
        dicWooden[lastWoodenIdx] = wooden;
        wooden.woodenId = lastWoodenIdx;

        lastWoodenIdx++;
    }
    public PaletteWooden GetWooden(long woodenId)
    {
        return dicWooden[woodenId];
    }


    // 서버에서 응답 받는 함수
    void OnServerMessage(string message)
    {
        JObject jobject = JObject.Parse(message);
        string key = "CONVEYOR_ON";
        if(jobject.ContainsKey(key))
        {
            ChangeValueOnOff(jobject[key].ToObject<bool>());
        }

        key = "type";
        if(jobject.ContainsKey("type"))
        {
            string valueKey = jobject[key].ToString();
            if(valueKey == "CARRY")
            {
                int botId = jobject["bot"].ToObject<int>();
                long woodenId = jobject["wooden"].ToObject<long>();
                allBots[botId].Carry(dicWooden[woodenId].botPos);
            }
        }
    }
}
