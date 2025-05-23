using UnityEngine;

public class MyObject : MonoBehaviour
{
    // 오브젝트 이동 속력
    float speed = 0.5f * 1.95f;
    void Start()
    {
        
    }

    void Update()
    {
        // 앞 방향으로 이동하고 싶다.
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
