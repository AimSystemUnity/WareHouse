using Newtonsoft.Json.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class NetView : MonoBehaviour
{
    // 네트워크 ID
    public long netId;

    static long nextNetId;

    protected virtual void Start()
    {
        netId = UDPClient.instance.AddNetView(this);
        print(name + " : " + netId);
    }

    // 서버에서 응답받는 함수
    public virtual JObject OnMessage(string message)
    {
        return JObject.Parse(message);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying && !EditorUtility.IsPersistent(this))
        {

            if(netId == 0)
            {
                netId =  ++nextNetId;
                print(name + " : " + netId);
            }
        }
#endif
    }
}
