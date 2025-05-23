using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    // MyObject Prefab
    public GameObject myObjectPrefab;

    // �����ð�
    public float createTime = 3;
    // ����ð�
    float currTime;

    void Start()
    {
        
    }

    void Update()
    {
        // �ð��� �帣�� ����.
        currTime += Time.deltaTime;
        // ���࿡ ����ð��� �����ð����� Ŀ����
        if(currTime > createTime)
        {
            // MyObject ����
            GameObject myObject = Instantiate(myObjectPrefab);
            // ��ġ�� ���� ��ġ�� ����.
            myObject.transform.position = transform.position;
            // ����ð� �ʱ�ȭ
            currTime = 0;
        }
    }
}
