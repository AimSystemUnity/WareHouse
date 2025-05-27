using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 나 자신
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 변수를 Inspector 에 노출 (변수가 public, private 상관없이)
    [SerializeField]
    int number = 10;

    // 컨베이어 벨트 동작 유무 
    //[HideInInspector] // 변수를 Inspector에서 노출되지 않게 하기
    public bool isOn;
    // 컨베이어 벨트 동작 UI (Text)
    public TMP_Text txtOnOff;

    // 모든 Bot 가지는 변수
    public List<Bot> allBot = new List<Bot>();

    // OnOff 가 클릭되어있을 때 호출되는 함수를 가지는 변수
    public Action delegateOnOff;

    public void OnClickOnOff()
    {
        // true -> false, false -> true
        isOn = !isOn;

        // isOn 의 값에 따라서 버튼의 text 변환
        string s = isOn ? "Stop" : "Start";
        txtOnOff.SetText(s);

        if(delegateOnOff != null)
        {
            // delegateOnOff 에 들어있는 함수를 호출
            delegateOnOff();
        }
    }

    public void FindClosestBot(Transform wooden, Transform storage)
    {
        // 현재 최단 거리
        float minDist = float.MaxValue;
        // 현재 최단 거리 idx
        int idx = -1;

        // bot 갯수만큼 거리 비교
        for(int i = 0; i < allBot.Count; i++)
        {
            // 만약에 bot 의 상태가 IDLE 이 아니라면 continue 하자
            if (allBot[i].currState != Bot.EBotState.IDLE) continue;

            // wooden 과 i 번째의 bot 거리
            float dist = Vector3.Distance(wooden.position, allBot[i].transform.position);//(wooden.position - allBot[i].transform.position).magnitude;

            // 만약에 dist 가 현재 최단 거리 보다 작으면 ( i 번째가 더 가깝다)
            if(dist < minDist)
            {
                // 현재 최단 거리 idx 갱신
                idx = i;
                // 현재 최단 거리 갱신
                minDist = dist;
            }
        }

        // 만약에 idx 가 -1이 아니라면 (최단거리 bot 존재)
        if(idx != -1)
        {
            // idx 번째 bot 에게 wooden 위치로 이동해라 (MOVE_TO_OBJECT)
            allBot[idx].ChageMoveToObject(wooden, storage);
        }
    }
}

