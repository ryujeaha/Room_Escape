using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;//데이터베이스(본인)을 스태틱(정적 변수)로 선언해서 어디서든 공유,접근이 가능하게 하는 역할

    [SerializeField] string csv_FileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>(); //지정한 자료형의 방식으로 스크립트 등의 접근해서 데이터를 찾을수 있게하는 명령어

    public static bool isFinish = false;//저장이 다 끝났는지 아닌지를 판단하는 변수

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;//시작하고 인스터가 비어있다면 데이터베이스를 대입해줌
            Dialogue_Parser theParser = GetComponent<Dialogue_Parser>();
            Dialogue[] dialogues = theParser.Parse(csv_FileName);
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i + 1, dialogues[i]);//i+1 하는 이유 선생님께 질문
            }
            isFinish = true;
        }
    }

    public Dialogue[] GetDialogue(int _StartNum, int EndNum)//몇번째부터 몇번째 대사를 가져오고싶을 때 쓰는 함수?
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for (int i = 0; i < EndNum - _StartNum; i++)
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);//지금 포문의 역할  질문.
        }
        return dialogueList.ToArray();
    }
}
