using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


// 유니티 에디터를 처음 실행 했을때 호출되는 클래스 생성
// 이 부분은 NetView.nextNetId 값을 현재 하이어라키 창에 배치되어 있는
// NetView 의 최고 높은 값으로 설정 하기 위함
#if UNITY_EDITOR
[InitializeOnLoad]
public class EditorStartup
{
    static EditorStartup()
    {
        EditorApplication.update += OnEditorLoaded;
    }

    static void OnEditorLoaded()
    {
        EditorApplication.update -= OnEditorLoaded;

        NetView [] views = GameObject.FindObjectsByType<NetView>(FindObjectsSortMode.None);
        foreach(NetView view in views)
        {
            if(NetView.nextNetId < view.netId)
            {
                NetView.nextNetId = view.netId;
                Debug.Log(NetView.nextNetId + "------");
            }
        }
    }
}
#endif

public class NetView : MonoBehaviour
{
    // 네트워크 ID
    public long netId;

    public static long nextNetId;

    

    protected virtual void Start()
    {
        netId = UDPClient.instance.AddNetView(this);
        print(netId + " - " + name);
    }

    // 서버에서 응답받는 함수
    public virtual JObject OnMessage(string message) 
    {
        return JObject.Parse(message);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        
        // 현재 에디터가 Play 중이 아니고,
        // 해당 오브젝트가 Assets 폴더에 있는것이 아니라면
        if (!EditorApplication.isPlaying && !EditorUtility.IsPersistent(this))
        {
            // netId 가 0이 아니면
            if(netId == 0)
            {
                // netId 를 하나 증가시키자.
                netId = ++nextNetId;
            }
        }
#endif
    }
}
