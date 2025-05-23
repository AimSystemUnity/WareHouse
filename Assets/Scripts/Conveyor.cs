using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // 속력
    public float speed = 0.5f;

    // 벨트
    public GameObject belt;

    // 벨트 매터리얼
    Material matBelt;


    void Start()
    {
        // 벨트 매터리얼을 가져오자.
        MeshRenderer mr = belt.GetComponent<MeshRenderer>();
        matBelt = mr.material;
    }

    void Update()
    {
        if (GameManager.instance.isOn == false) return;

        // 벨트 매터리얼의 offset 값을 변경        
        Vector2 offset = matBelt.mainTextureOffset;
        offset += Vector2.down * speed * Time.deltaTime;
        offset.y %= 1;
        matBelt.mainTextureOffset = offset;
    }
}
