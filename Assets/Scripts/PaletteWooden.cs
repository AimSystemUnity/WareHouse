using UnityEngine;

public class PaletteWooden : MonoBehaviour
{
    // ������ �ö󰡾��� ��ġ
    public Transform[] pos;

    // �ö� ������ ����
    int count;
    
    public bool AddObject(MyObject obj)
    {
        // obj ��Ȱ��ȭ
        obj.enabled = false;
        // ������ �θ� �����ǿ� �߰��� ��ġ ������Ʈ�� count ��°�� ����
        obj.transform.parent = pos[count];
        // ������ ������ġ�� zero �� ����
        obj.transform.localPosition = Vector3.zero;
        // ���� �ö� ���� �ϳ� ����
        count++;

        // ������ ������ ��ġ ���� �� ��ȯ
        return count == pos.Length;
    }
}
