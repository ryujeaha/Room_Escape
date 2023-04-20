using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;//�����ͺ��̽�(����)�� ����ƽ(���� ����)�� �����ؼ� ��𼭵� ����,������ �����ϰ� �ϴ� ����

    [SerializeField] string csv_FileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>(); //������ �ڷ����� ������� ��ũ��Ʈ ���� �����ؼ� �����͸� ã���� �ְ��ϴ� ��ɾ�

    public static bool isFinish = false;//������ �� �������� �ƴ����� �Ǵ��ϴ� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;//�����ϰ� �ν��Ͱ� ����ִٸ� �����ͺ��̽��� ��������
            Dialogue_Parser theParser = GetComponent<Dialogue_Parser>();
            Dialogue[] dialogues = theParser.Parse(csv_FileName);
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i + 1, dialogues[i]);//i+1 �ϴ� ���� �����Բ� ����
            }
            isFinish = true;
        }
    }

    public Dialogue[] GetDialogue(int _StartNum, int EndNum)//���°���� ���° ��縦 ����������� �� ���� �Լ�?
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for (int i = 0; i < EndNum - _StartNum; i++)
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);//���� ������ ����  ����.
        }
        return dialogueList.ToArray();
    }
}
