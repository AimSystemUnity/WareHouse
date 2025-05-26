using UnityEngine;

public class MyObject : MonoBehaviour
{
    // 오브젝트 종류 정의
    public enum EObjectType
    {
        BARREL,
        BOXWOODEN,
        CONE,
        MAX
    }

    // 오브젝트 외형들
    public GameObject[] objectPrefabs;

    // 나의 오브젝트 종류
    public EObjectType type;

    // 오브젝트 이동 속력
    float speed = 0.5f * 1.95f;


    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.isOn == false) return;

        // 앞 방향으로 이동하고 싶다.
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void CreateObject(int objType)
    {
        // objType 번째의 오브젝트를 생성 
        GameObject go = Instantiate(objectPrefabs[objType], transform);

        // go.transform.parent = null;

        // objType 을 type 에 설정
        type = (EObjectType)objType;
    }
}