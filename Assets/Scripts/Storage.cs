using TMPro;
using UnityEngine;

public class Storage : MonoBehaviour
{
    // 어떤 오브젝트를 쌓을 수 있는지
    public MyObject.EObjectType type;

    // 전체 갯수 
    int total;

    // 전체 갯수 UI
    TMP_Text txt_Total;

    void Start()
    {
        // TMP_Text 컴포넌트 가져오자
        txt_Total = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        
    }

    public void AddCount()
    {
        // 전체 갯수를 4개 증가시키자.
        total += 4;

        // 전체 갯수 UI 갱신
        txt_Total.SetText(total.ToString());
    }
}
