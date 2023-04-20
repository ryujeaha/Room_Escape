using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Event : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogueEvent;

    public Dialogue[] GetDialogue()
    {
        DialogueEvent t_dialogue = new DialogueEvent();
        t_dialogue.dialogues = DatabaseManager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y);//���Ͱ��� �÷��̱⶧���� ��Ʈ�� ����ȯ

        for (int i = 0; i < dialogueEvent.dialogues.Length; i++)
        {
            t_dialogue.dialogues[i].tf_target = dialogueEvent.dialogues[i].tf_target;//�޾ƿ��� �������� �ΰ����� ���ϹǷ� �̸� �־�Q�� ���� �޾ƿ��� �ٽ� �����ü��ְ� ġȯ
            t_dialogue.dialogues[i].cameratype = dialogueEvent.dialogues[i].cameratype;
        }

        dialogueEvent.dialogues = t_dialogue.dialogues;//�ٽ� ���� ������ �̵�
        return dialogueEvent.dialogues;
    }
}
