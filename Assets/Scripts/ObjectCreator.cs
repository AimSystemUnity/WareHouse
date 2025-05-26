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
        if (GameManager.instance.isOn == false) return;

        // 시간을 흐르게 하자.
        currTime += Time.deltaTime;
        // 만약에 현재시간이 생성시간보다 커지면
        if(currTime > createTime)
        {
            // MyObject 생성
            GameObject go = Instantiate(myObjectPrefab);
            // 위치를 나의 위치에 놓자.
            go.transform.position = transform.position;
            // 현재시간 초기화
            currTime = 0;

            // 랜덤한 오브젝트 뽑자
            int type = Random.Range(0, (int)MyObject.EObjectType.MAX);
            // MyObject 컴포넌트 가져오자
            MyObject myObject = go.GetComponent<MyObject>();
            // 뽑힌 오브젝트를 전달해서 생성
            myObject.CreateObject(type);
        }
    }
}