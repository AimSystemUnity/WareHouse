using System.Collections;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    // MyObject Prefab
    public GameObject myObjectPrefab;

    // 생성시간
    public float createTime = 3;
    
    // 현재 돌아가고 있는 코루틴
    Coroutine currCo;

    void Start()
    {
        GameManager.instance.delegateOnOff += OnOff;
    }

    void OnOff()
    {
        if (GameManager.instance.isOn == true)
        {
            currCo = StartCoroutine(CoCreateMyObject());
        }
        else
        {
            StopCoroutine(currCo);
        }
    }

    IEnumerator CoCreateMyObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(createTime);

            // MyObject 생성
            GameObject go = Instantiate(myObjectPrefab);
            // 위치를 나의 위치에 놓자.
            go.transform.position = transform.position;        

            // 랜덤한 오브젝트 뽑자
            int type = Random.Range(0, (int)MyObject.EObjectType.MAX);
            // MyObject 컴포넌트 가져오자
            MyObject myObject = go.GetComponent<MyObject>();
            // 뽑힌 오브젝트를 전달해서 생성
            myObject.CreateObject(type);
        }
    }
}