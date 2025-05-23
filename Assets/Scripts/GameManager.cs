using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �� �ڽ�
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

    // ������ Inspector �� ���� (������ public, private �������)
    [SerializeField]
    int number = 10;

    // �����̾� ��Ʈ ���� ���� 
    //[HideInInspector] // ������ Inspector���� ������� �ʰ� �ϱ�
    public bool isOn;
    // �����̾� ��Ʈ ���� UI (Text)
    public TMP_Text txtOnOff;
    public void OnClickOnOff()
    {
        // true -> false, false -> true
        isOn = !isOn;

        // isOn �� ���� ���� ��ư�� text ��ȯ
        string s = isOn ? "Stop" : "Start";
        txtOnOff.SetText(s);
    }
}
