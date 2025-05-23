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
        if (GameManager.instance.isOn == false) return;

        // �ð��� �帣�� ����.
        currTime += Time.deltaTime;
        // ���࿡ ����ð��� �����ð����� Ŀ����
        if(currTime > createTime)
        {
            // MyObject ����
            GameObject go = Instantiate(myObjectPrefab);
            // ��ġ�� ���� ��ġ�� ����.
            go.transform.position = transform.position;
            // ����ð� �ʱ�ȭ
            currTime = 0;

            // ������ ������Ʈ ����
            int type = Random.Range(0, (int)MyObject.EObjectType.MAX);
            // MyObject ������Ʈ ��������
            MyObject myObject = go.GetComponent<MyObject>();
            // ���� ������Ʈ�� �����ؼ� ����
            myObject.CreateObject(type);
        }
    }
}
