using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    // MyObject Prefab
    public GameObject myObjectPrefab;

    // 생성시간
    public float createTime = 3;
    // 현재시간
    float currTime;

    void Start()
    {
        
    }

    void Update()
    {
        // 시간을 흐르게 하자.
        currTime += Time.deltaTime;
        // 만약에 현재시간이 생성시간보다 커지면
        if(currTime > createTime)
        {
            // MyObject 생성
            GameObject myObject = Instantiate(myObjectPrefab);
            // 위치를 나의 위치에 놓자.
            myObject.transform.position = transform.position;
            // 현재시간 초기화
            currTime = 0;
        }
    }
}
