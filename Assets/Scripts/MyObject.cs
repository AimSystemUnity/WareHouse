using UnityEngine;

public class MyObject : MonoBehaviour
{
    // ������Ʈ �̵� �ӷ�
    float speed = 0.5f * 1.95f;
    void Start()
    {
        
    }

    void Update()
    {
        // �� �������� �̵��ϰ� �ʹ�.
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
