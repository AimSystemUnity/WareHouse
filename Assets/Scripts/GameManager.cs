using TMPro;
using UnityEngine;

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

    void Start()
    {
        
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
    public void OnClickOnOff()
    {
        // true -> false, false -> true
        isOn = !isOn;

        // isOn 의 값에 따라서 버튼의 text 변환
        string s = isOn ? "Stop" : "Start";
        txtOnOff.SetText(s);
    }
}

