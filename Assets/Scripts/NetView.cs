using UnityEngine;

public class NetView : MonoBehaviour
{
    // 네트워크 ID
    public long netId;

    protected virtual void Start()
    {
        netId = UDPClient.instance.AddNetView(this);
    }

    // 서버에서 응답받는 함수
    public virtual void OnMessage(string message) { }
}
