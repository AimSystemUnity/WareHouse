using UnityEngine;

public class NetView : MonoBehaviour
{
    public long netId;

    protected virtual void Start()
    {
        netId = UDPClient.instance.AddNetView(this);
    }


    public virtual void OnMessage(string msg) { }
}
