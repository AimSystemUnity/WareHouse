using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // �ӷ�
    public float speed = 0.5f;

    // ��Ʈ
    public GameObject belt;

    // ��Ʈ ���͸���
    Material matBelt;


    void Start()
    {
        // ��Ʈ ���͸����� ��������.
        MeshRenderer mr = belt.GetComponent<MeshRenderer>();
        matBelt = mr.material;
    }

    void Update()
    {
        if (GameManager.instance.isOn == false) return;

        // ��Ʈ ���͸����� offset ���� ����        
        Vector2 offset = matBelt.mainTextureOffset;
        offset += Vector2.down * speed * Time.deltaTime;
        offset.y %= 1;
        matBelt.mainTextureOffset = offset;
    }
}
