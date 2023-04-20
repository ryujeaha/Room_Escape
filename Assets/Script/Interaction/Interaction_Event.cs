using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Event : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogueEvent;

    public Dialogue[] GetDialogue()
    {
        DialogueEvent t_dialogue = new DialogueEvent();
        t_dialogue.dialogues = DatabaseManager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y);//벡터값은 플롯이기때문에 인트로 형변환

        for (int i = 0; i < dialogueEvent.dialogues.Length; i++)
        {
            t_dialogue.dialogues[i].tf_target = dialogueEvent.dialogues[i].tf_target;//받아오는 과정에서 널값으로 변하므로 미리 넣어놯던 것을 받아온후 다시 가져올수있게 치환
            t_dialogue.dialogues[i].cameratype = dialogueEvent.dialogues[i].cameratype;
        }

        dialogueEvent.dialogues = t_dialogue.dialogues;//다시 원래 변수로 이동
        return dialogueEvent.dialogues;
    }
}
