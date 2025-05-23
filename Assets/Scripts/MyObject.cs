using UnityEngine;

public class MyObject : MonoBehaviour
{
    // ������Ʈ ���� ����
    public enum EObjectType
    {
        BARREL,
        BOXWOODEN,
        CONE,
        MAX
    }

    // ������Ʈ ������
    public GameObject[] objectPrefabs;

    // ���� ������Ʈ ����
    public EObjectType type;

    // ������Ʈ �̵� �ӷ�
    float speed = 0.5f * 1.95f;


    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.isOn == false) return;

        // �� �������� �̵��ϰ� �ʹ�.
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void CreateObject(int objType)
    {
        // objType ��°�� ������Ʈ�� ���� 
        GameObject go = Instantiate(objectPrefabs[objType], transform);

        // go.transform.parent = null;

        // objType �� type �� ����
        type = (EObjectType)objType;
    }
}
