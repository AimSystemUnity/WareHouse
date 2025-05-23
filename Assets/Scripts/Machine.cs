
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    // �з� �� ���� type
    public MyObject.EObjectType type;

    // ������ Prefab
    public GameObject woodenPrefab;

    // ���� �÷����� �ִ� ������
    public PaletteWooden wooden;

    // ������ �����ǵ�
    public List<PaletteWooden> fullWoodenList;

    // ������ �������� �θ�
    public Transform trFullWoodenParent;


    void Start()
    {
        CreateWooden();
    }

    void Update()
    {
        
    }

    void CreateWooden()
    {
        // �������� ����� (���� �ڽ�����)
        GameObject go = Instantiate(woodenPrefab, transform);
        // ������ �����ǿ���  PaletteWooden ������Ʈ ��������
        wooden = go.GetComponent<PaletteWooden>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ε��� ��ü�� MyObject ������Ʈ�� ��������.
        MyObject myObject = other.GetComponentInParent<MyObject>();
        if(myObject != null)
        {
            // �浹�� ��ü type �� �з� �� ���� type ���ٸ�
            if(myObject.type == type)
            {
                // �浹�� ��ü�� �����ǿ� �ű���.
                bool isFull = wooden.AddObject(myObject);
                // ���࿡ �������� ���� á�ٸ�
                if(isFull)
                {
                    // ������ ������ woodenList�� �߰�
                    fullWoodenList.Add(wooden);

                    // ������ �������� trFullWoodenParent �� �ڽ�����!
                    wooden.transform.parent = trFullWoodenParent;

                    // ������ ������ ������ ���� ��ġ�� ����
                    trFullWoodenParent.localPosition += Vector3.left * 2;

                    // ���ο� �������� ������.
                    CreateWooden();
                }
            }
        }
    }
}
