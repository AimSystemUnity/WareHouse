
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    // 분류 할 물건 type
    public MyObject.EObjectType type;

    // 나무판 Prefab
    public GameObject woodenPrefab;

    // 물건 올려놓고 있는 나무판
    public PaletteWooden wooden;

    // 가득찬 나무판들
    public List<PaletteWooden> fullWoodenList;

    // 가득찬 나무판의 부모
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
        // 나무판을 만든다 (나의 자식으로)
        GameObject go = Instantiate(woodenPrefab, transform);
        // 생성된 나무판에서  PaletteWooden 컴포넌트 가져오기
        wooden = go.GetComponent<PaletteWooden>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 물체가 MyObject 컴포넌트를 가져오자.
        MyObject myObject = other.GetComponentInParent<MyObject>();
        if(myObject != null)
        {
            // 충돌한 물체 type 과 분류 할 물건 type 같다면
            if(myObject.type == type)
            {
                // 충돌한 물체를 나무판에 옮기자.
                bool isFull = wooden.AddObject(myObject);
                // 만약에 나무판이 가득 찼다면
                if(isFull)
                {
                    // 가득찬 나무판 woodenList에 추가
                    fullWoodenList.Add(wooden);

                    // 가득찬 나무판을 trFullWoodenParent 의 자식으로!
                    wooden.transform.parent = trFullWoodenParent;

                    // 가득판 나무판 갯수에 따라서 위치를 변경
                    trFullWoodenParent.localPosition += Vector3.left * 2;

                    // 새로운 나무판을 만들자.
                    CreateWooden();
                }
            }
        }
    }
}
