using UnityEngine;

public class Storage : MonoBehaviour
{
    // 어떤 오브젝트를 쌓을 수 있는지
    public MyObject.EObjectType type;

    // 쌓인 갯수 
    public int total;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddCount()
    {
        // 전체 갯수를 4개 증가시키자.
        total += 4;

        print(type.ToString() + " : " + total);
    }
}
