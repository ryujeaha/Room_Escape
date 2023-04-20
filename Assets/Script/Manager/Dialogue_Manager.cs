using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manager : MonoBehaviour
{
    [SerializeField] GameObject Go_Dialogue_bar;//������ ���̾�α׹� �̹���
    [SerializeField] GameObject Go_Dialogue_NameBar;//������ ���̾�α׳��ӹ� �̹���

    [SerializeField] Text txt_Dialogue;//������ ���̾�α� �ؽ�Ʈ
    [SerializeField] Text Txt_Name;//������ ���� �ؽ�Ʈ

    Dialogue[] Dialogues;

   bool isDialogue = false;//���� ��ȭ������ �ƴ��� �Ǻ�
   bool isnext = false; //Ư�� Ű �Է� ���(����Ű�� �����̽�Ű�� Ʈ�簡 �Ǹ� ������ �ְԸ���� ������縦 ����Ҽ� �ְ��ϴ� ����)

    [Header("�ؽ�Ʈ ��� ������")]
    [SerializeField] float textDelay;

    int lineCount = 0; //��ȭ ī��Ʈ(����ĳ���Ͱ� ��ȭ�� ����ؾ��Ҷ� �ϳ��� �÷��༭ ����ĳ���Ͱ� ��縦 ����ϰ� �ϴ� ����)
    int contextCount = 0;//��� ī��Ʈ(ĳ���͸��� �ϴ� ��簡 �������϶� ���ڸ� �ʱ�ȭ�ϰų� �÷��� ����)

    Interaction_Controller theInter;//���ͷ��� �Ŵ��� ����
    Splash_Manager the_Splash;//���÷��� �Ŵ��� ����
    Camera_COntroller theCam;//ī�޶� �Ŵ��� ����
    Sprite_Manager theSprite;//��������Ʈ �Ŵ�������
    CutScene_Manager theCutScene;

    private void Start()
    {
        Sound_Manager.instance.PlaySound("Bgm1",0);
        theInter = FindObjectOfType<Interaction_Controller>();
        theCam = FindObjectOfType<Camera_COntroller>();
        theSprite = FindObjectOfType<Sprite_Manager>();
        the_Splash = FindObjectOfType<Splash_Manager>();
        theCutScene = FindObjectOfType<CutScene_Manager>();
;    }

    private void Update()
    {
        if(isDialogue)//��ȭ���ΰ�?
        {
            if(isnext)//�������� �Ѿ �� �ִ°�?
            {
                if(Input.GetKeyDown(KeyCode.Space))//�����̽��� ���ȴ°�?
                {
                    isnext = false;
                    txt_Dialogue.text = "";//�ʱ�ȭ
                    if(++contextCount < Dialogues[lineCount].conTexts.Length)//���� ��ȭ ���(���� ����ī��Ʈ�� �ش��ϴ� ����� ���������� ��������
                    {
                        StartCoroutine(TypeWriter());//�ڷ�ƾ ���
                    }
                    else//�����̽��� ���� ����� �������� �Ѿ��� ���
                    {
                        contextCount = 0;
                        if(++lineCount < Dialogues.Length)//����ī��Ʈ�� �ø��� �� ���� ���� ������ ����� ���������� ������� �ڷ�ƾ ����
                        {
                            StartCoroutine(CameraTargettingType());//����ϴ� �ι��� �ٲ𶧸��� ī�޶� �ٲ��� ���� ���ΰ���(�ڷ�ƾ����)
                        }
                        else//�غ�� ����ȭ�� ����Ǽ� �ε��� ���� �������� �Ѿ�������
                        {
                           StartCoroutine(EndDialogue());
                        }
                    }
                   
                }
            }
        }
    }

    public void SHowDialogue(Dialogue[] dialogues)//���̾�α׸� �����ָ鼭 �ؽ�Ʈ �ʱ�ȭ
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        Txt_Name.text = "";
        theInter.SettiingUi(false);

        Dialogues = dialogues;
        theCam.CamoriginSetting();//ī�޶� �⺻��ġ ����
        StartCoroutine(CameraTargettingType());
    }
    IEnumerator CameraTargettingType()
    {
        switch (Dialogues[lineCount].cameratype)
        {
            case CameraType.FaidIn:SettingUI(false); Splash_Manager.isFinish = false;StartCoroutine(the_Splash.FadeIn(false, true));yield return new WaitUntil(() => Splash_Manager.isFinish); break;//��� �۾��� ������ �̽��ǴϽ��� Ʈ�簡�ɶ�����
            case CameraType.FaideOut:SettingUI(false); Splash_Manager.isFinish = false; StartCoroutine(the_Splash.FadeOut(false, true)); yield return new WaitUntil(() => Splash_Manager.isFinish); break;//��� �۾��� ������ �̽��ǴϽ��� Ʈ�簡�ɶ�����
            case CameraType.FlashIn:SettingUI(false); Splash_Manager.isFinish = false; StartCoroutine(the_Splash.FadeIn(true, true)); yield return new WaitUntil(() => Splash_Manager.isFinish); break;//��� �۾��� ������ �̽��ǴϽ��� Ʈ�簡�ɶ�����
            case CameraType.FlashOut:SettingUI(false); Splash_Manager.isFinish = false; StartCoroutine(the_Splash.FadeOut(true, true)); yield return new WaitUntil(() => Splash_Manager.isFinish); break;//��� �۾��� ������ �̽��ǴϽ��� Ʈ�簡�ɶ�����
            case CameraType.ObjectFront: theCam.CameraTargetting(Dialogues[lineCount].tf_target); break;//ī�޶� Ÿ���� �����ҋ�
            case CameraType.Reset: theCam.CameraTargetting(null, 0.05f, true, false); break;//��ȭ�� ������ �ʰ� ī�޶� ���ƿö�
            case CameraType.ShowCutScene: SettingUI(false); CutScene_Manager.isFinished = false; StartCoroutine(theCutScene.CutsceneCoroutine(Dialogues[lineCount].spritename[contextCount], true)); yield return new WaitUntil(() => CutScene_Manager.isFinished); break;
            case CameraType.HideCutScene: SettingUI(false); CutScene_Manager.isFinished = false; StartCoroutine(theCutScene.CutsceneCoroutine(null,false)); yield return new WaitUntil(() => CutScene_Manager.isFinished);theCam.CameraTargetting(Dialogues[lineCount].tf_target); break;
        }
        StartCoroutine(TypeWriter());
    }
    IEnumerator EndDialogue()
    {
        SettingUI(false);
        if (theCutScene.CheckCutScene())
        {
            CutScene_Manager.isFinished = false;
           StartCoroutine(theCutScene.CutsceneCoroutine(null, false)); 
           yield return new WaitUntil(() => CutScene_Manager.isFinished);
        }
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        Dialogues = null;//������ ���� ��� �ʱ�ȭ�۾�
        isnext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);
        
    }

    void ChangeSprite()
    {
        if(Dialogues[lineCount].tf_target != null)//Ÿ���� ���� ��쿡��
        {
            if (Dialogues[lineCount].spritename[contextCount] != "") //�̸��� �������� ������츦 ���� if��(������ ��������Ʈ ������ ���ĭ�� ��������ʴٸ�
            {
                StartCoroutine(theSprite.SpriteChangeCoroutine(Dialogues[lineCount].tf_target, Dialogues[lineCount].spritename[contextCount]));//
            }
        }
        
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);
        ChangeSprite();
        //�ڷ�ƾ�� ȣ�� �ɶ����� ���� ��縦 �ӽú����� ��Ƽ� ��ü��ų �κ��� �ִٸ� ��ü���ְ� �������� �Ѿ�� �ְ� boo���� �ٲ��ش��� 1�ʴ��
        string t_RePlaceText = Dialogues[lineCount].conTexts[contextCount];//��ǥ�� �����ϴ� CSV���� ������ �Ľ��ؿ��⶧���� ��ǥ�� �������� ������ Ư������('')�� ��ǥ�� ġȯ���ִ� ����
        t_RePlaceText = t_RePlaceText.Replace("'", ",");//Ư�� ���ڿ��� ��ü��Ű�� ��ɾ� Replace
        t_RePlaceText = t_RePlaceText.Replace("\\n", "\n");//\n�� ���ڿ��� �νĽ�Ű�� ���ؼ��� \�� �ѹ��� ���ش�.

        bool t_white = false, t_Yellow = false;//Ʈ�簡 �Ǹ� �ش���ں��� ���� �ٲٰ��ϴ� ����.
        bool t_ignore = false;//Ư�����ڸ� ������ ������Ű�� ���� ����   

        for (int i = 0; i < t_RePlaceText.Length; i++)//�޾ƿ� ���ڰ�����ŭ �ݺ�
        {
            switch(t_RePlaceText[i])
            {
                case '��': t_white = true; t_Yellow = false; t_ignore = true; break;//Ư�������� �ٲٶ�� ������ Ư����ȣ�� �˻��� ������ �ش��ȣ�� ������Ű�� ���� �Ұ��� Ʈ��� �ٲ��ְ�
                case '��': t_white = false; t_Yellow = true; t_ignore = true; break;//������ �Ұ��� �ش��ϴ°��� Ʈ��� �ƴѰ͵��� ��� �޽��� �ʱ�ȭ���ش�.
                case '��':StartCoroutine(the_Splash.Splash()); Sound_Manager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;//�ش� ��ȣ�� ȿ���� ��°� ���� ��ü���� ���� ���÷���(����ȿ�� ���)
                case '��':StartCoroutine(the_Splash.Splash()); Sound_Manager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
                case '��': Sound_Manager.instance.PlaySound("Emotion3", 1); t_ignore = true; break;
                case '��': Sound_Manager.instance.PlaySound("Emotion4", 1); t_ignore = true; break;
                case '��': Sound_Manager.instance.PlaySound("Emotion5", 1); t_ignore = true; break;//5������ ȿ����
                case '��': Sound_Manager.instance.PlaySound("Bgm1", 0); t_ignore = true; break;
                case '��': Sound_Manager.instance.PlaySound("Bgm2", 0); t_ignore = true; break;//7���� bgm
                case '��': Sound_Manager.instance.SetBgm(0); t_ignore = true; break;//8:BGM����
                case '��': Sound_Manager.instance.SetBgm(1); t_ignore = true; break;//9:BGM ��������
                case '��': Sound_Manager.instance.SetBgm(2); t_ignore = true; break;//10:���� �ƿ� ����
                case '��': Sound_Manager.instance.SetBgm(3); t_ignore = true; break;//11:���ȿ���� ����
            }

            string t_letter = t_RePlaceText[i].ToString();//������ ���ڸ� ��Ʈ������ ����ȯ�ؼ� �ӽú����� ����

            if(!t_ignore)//Ư����ȣ�� ������ ������� ���̱⿡ �������� �ʱ����ؼ� t_ignore�� false�϶���
            {
                if(t_white)
                {
                    t_letter = "<color=#ffffff>" + t_letter + "</color>";//�극��ũ ���� �������� ���ڰ� �ش� ������ �����ϸ� �ش� ������ ������ �߰�
                }
                else if(t_Yellow)
                {
                    t_letter = "<color=#FFFF00>" + t_letter + "</color>";//�극��ũ ���� �������� ���ڰ� �ش� ������ �����ϸ� �ش� ������ ������ �߰�
                }
                txt_Dialogue.text += t_letter;//�޾ƿ� ������ �ѱ��ھ� ����
            }
            t_ignore = false;//Ư����ȣ���� �̱׳� Ʈ���̱⶧���� ���ǹ��� ��� �ǳʶٰ� �������� ���� ��������� ����� �̷����.
            yield return new WaitForSeconds(textDelay);//�ؽ�Ʈ ������ ��ŭ ���

        }

        isnext = true;
     

    }

    void SettingUI(bool p_Fleg)//�޾ƿ��� �Ķ���ͷ� ����
    {
        Go_Dialogue_bar.SetActive(p_Fleg);

        if(p_Fleg)//��ȭâ�� ��������Ҷ�
        {
            if(Dialogues[lineCount].name == "")//�޾ƿ��� ���Ͽ��� �̸��� �ش��ϴ� ĭ�� ����ִٸ�
            {
                Go_Dialogue_NameBar.SetActive(false);
            }
            else//���Ͽ��� �̸��� �������
            {
                Go_Dialogue_NameBar.SetActive(true);
                Txt_Name.text = Dialogues[lineCount].name;
            }
        }
        else
        {
            Go_Dialogue_NameBar.SetActive(false);
        }

        
    }
}
