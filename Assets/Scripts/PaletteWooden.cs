using UnityEngine;

public class PaletteWooden : MonoBehaviour
{
    // 물건이 올라가야할 위치
    public Transform[] pos;

    // 올라간 물건의 갯수
    int count;
    
    public bool AddObject(MyObject obj)
    {
        // obj 비활성화
        obj.enabled = false;
        // 물건의 부모를 나무판에 추가한 위치 오브젝트의 count 번째로 설정
        obj.transform.parent = pos[count];
        // 물건의 로컬위치를 zero 로 설정
        obj.transform.localPosition = Vector3.zero;
        // 물건 올라간 갯수 하나 증가
        count++;

        // 생성된 갯수와 위치 갯수 비교 반환
        return count == pos.Length;
    }
}