using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manager : MonoBehaviour
{
    [SerializeField] GameObject Go_Dialogue_bar;//보여줄 다이얼로그바 이미지
    [SerializeField] GameObject Go_Dialogue_NameBar;//보여줄 다이얼로그네임바 이미지

    [SerializeField] Text txt_Dialogue;//보여줄 다이얼로그 텍스트
    [SerializeField] Text Txt_Name;//보여줄 네임 텍스트

    Dialogue[] Dialogues;

   bool isDialogue = false;//현재 대화중인지 아닌지 판별
   bool isnext = false; //특정 키 입력 대기(엔터키나 스페이스키를 트루가 되면 누를수 있게만들어 다음대사를 출력할수 있게하는 역할)

    [Header("텍스트 출력 딜레이")]
    [SerializeField] float textDelay;

    int lineCount = 0; //대화 카운트(다음캐릭터가 대화를 출력해야할때 하나씩 올려줘서 다음캐릭터가 대사를 출력하게 하는 역할)
    int contextCount = 0;//대사 카운트(캐릭터마다 하는 대사가 여러개일때 숫자를 초기화하거나 올려서 구별)

    Interaction_Controller theInter;//인터렉션 매니저 참조
    Splash_Manager the_Splash;//스플레쉬 매니저 참조
    Camera_COntroller theCam;//카메라 매니저 참조
    Sprite_Manager theSprite;//스프라이트 매니저참조
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
        if(isDialogue)//대화중인가?
        {
            if(isnext)//다음으로 넘어갈 수 있는가?
            {
                if(Input.GetKeyDown(KeyCode.Space))//스페이스가 눌렸는가?
                {
                    isnext = false;
                    txt_Dialogue.text = "";//초기화
                    if(++contextCount < Dialogues[lineCount].conTexts.Length)//다음 대화 출력(현재 라인카운트의 해당하는 대사의 렝스값보다 적을때만
                    {
                        StartCoroutine(TypeWriter());//코루틴 출력
                    }
                    else//스페이스를 눌러 대사의 랭스값을 넘었을 경우
                    {
                        contextCount = 0;
                        if(++lineCount < Dialogues.Length)//라인카운트를 올리구 그 값이 현재 가져온 대사의 랭스값보다 적을경우 코루틴 실행
                        {
                            StartCoroutine(CameraTargettingType());//대사하는 인물이 바뀔때마다 카메라를 바꿀지 말지 여부결정(코루틴실행)
                        }
                        else//준비된 모든대화가 진행되서 인덱스 값이 랭스값을 넘어버린경우
                        {
                           StartCoroutine(EndDialogue());
                        }
                    }
                   
                }
            }
        }
    }

    public void SHowDialogue(Dialogue[] dialogues)//다이얼로그를 보여주면서 텍스트 초기화
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        Txt_Name.text = "";
        theInter.SettiingUi(false);

        Dialogues = dialogues;
        theCam.CamoriginSetting();//카메라 기본위치 저장
        StartCoroutine(CameraTargettingType());
    }
    IEnumerator CameraTargettingType()
    {
        switch (Dialogues[lineCount].cameratype)
        {
            case CameraType.FaidIn:SettingUI(false); Splash_Manager.isFinish = false;StartCoroutine(the_Splash.FadeIn(false, true));yield return new WaitUntil(() => Splash_Manager.isFinish); break;//모든 작업이 끝나서 이스피니쉬가 트루가될때까지
            case CameraType.FaideOut:SettingUI(false); Splash_Manager.isFinish = false; StartCoroutine(the_Splash.FadeOut(false, true)); yield return new WaitUntil(() => Splash_Manager.isFinish); break;//모든 작업이 끝나서 이스피니쉬가 트루가될때까지
            case CameraType.FlashIn:SettingUI(false); Splash_Manager.isFinish = false; StartCoroutine(the_Splash.FadeIn(true, true)); yield return new WaitUntil(() => Splash_Manager.isFinish); break;//모든 작업이 끝나서 이스피니쉬가 트루가될때까지
            case CameraType.FlashOut:SettingUI(false); Splash_Manager.isFinish = false; StartCoroutine(the_Splash.FadeOut(true, true)); yield return new WaitUntil(() => Splash_Manager.isFinish); break;//모든 작업이 끝나서 이스피니쉬가 트루가될때까지
            case CameraType.ObjectFront: theCam.CameraTargetting(Dialogues[lineCount].tf_target); break;//카메라가 타겟을 추적할떄
            case CameraType.Reset: theCam.CameraTargetting(null, 0.05f, true, false); break;//대화가 끝나지 않고 카메라가 돌아올때
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
        Dialogues = null;//위에것 까지 모두 초기화작업
        isnext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);
        
    }

    void ChangeSprite()
    {
        if(Dialogues[lineCount].tf_target != null)//타겟이 있을 경우에만
        {
            if (Dialogues[lineCount].spritename[contextCount] != "") //이름이 있을경우와 없을경우를 위해 if문(지금은 스프라이트 네임의 대산칸이 비어있지않다면
            {
                StartCoroutine(theSprite.SpriteChangeCoroutine(Dialogues[lineCount].tf_target, Dialogues[lineCount].spritename[contextCount]));//
            }
        }
        
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);
        ChangeSprite();
        //코루틴이 호출 될때마다 현재 대사를 임시변수에 담아서 대체시킬 부분이 있다면 대체해주고 다음으로 넘어갈수 있게 boo값을 바꿔준다음 1초대기
        string t_RePlaceText = Dialogues[lineCount].conTexts[contextCount];//쉼표로 구분하는 CSV파일 형식을 파싱해오기때문에 쉼표를 쓰기위해 지정한 특수문자('')를 쉼표로 치환해주는 변수
        t_RePlaceText = t_RePlaceText.Replace("'", ",");//특정 문자열을 대체시키는 명령어 Replace
        t_RePlaceText = t_RePlaceText.Replace("\\n", "\n");//\n을 문자열로 인식시키기 위해서는 \를 한번더 해준다.

        bool t_white = false, t_Yellow = false;//트루가 되면 해당글자부터 색을 바꾸게하는 역할.
        bool t_ignore = false;//특수문자를 만나면 생략시키기 위한 변수   

        for (int i = 0; i < t_RePlaceText.Length; i++)//받아온 글자갯수만큼 반복
        {
            switch(t_RePlaceText[i])
            {
                case 'ⓦ': t_white = true; t_Yellow = false; t_ignore = true; break;//특정색으로 바꾸라는 지정한 특수기호를 검사중 만나면 해당기호를 생략시키기 위한 불값을 트루로 바꿔주고
                case 'ⓨ': t_white = false; t_Yellow = true; t_ignore = true; break;//지정한 불값중 해당하는것을 트루로 아닌것들은 모두 펄스로 초기화해준다.
                case '①':StartCoroutine(the_Splash.Splash()); Sound_Manager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;//해당 기호에 효과음 출력과 글자 대체변수 설정 스플레쉬(강조효과 출력)
                case '②':StartCoroutine(the_Splash.Splash()); Sound_Manager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
                case '③': Sound_Manager.instance.PlaySound("Emotion3", 1); t_ignore = true; break;
                case '④': Sound_Manager.instance.PlaySound("Emotion4", 1); t_ignore = true; break;
                case '⑤': Sound_Manager.instance.PlaySound("Emotion5", 1); t_ignore = true; break;//5까지는 효과음
                case '⑥': Sound_Manager.instance.PlaySound("Bgm1", 0); t_ignore = true; break;
                case '⑦': Sound_Manager.instance.PlaySound("Bgm2", 0); t_ignore = true; break;//7까지 bgm
                case '⑧': Sound_Manager.instance.SetBgm(0); t_ignore = true; break;//8:BGM정지
                case '⑨': Sound_Manager.instance.SetBgm(1); t_ignore = true; break;//9:BGM 정지해제
                case '⑩': Sound_Manager.instance.SetBgm(2); t_ignore = true; break;//10:음악 아예 멈춤
                case '⑪': Sound_Manager.instance.SetBgm(3); t_ignore = true; break;//11:모든효과음 멈춤
            }

            string t_letter = t_RePlaceText[i].ToString();//가져온 글자를 스트링으로 형변환해서 임시변수에 저장

            if(!t_ignore)//특수기호를 가져올 순서라는 뜻이기에 가져오지 않기위해서 t_ignore가 false일때만
            {
                if(t_white)
                {
                    t_letter = "<color=#ffffff>" + t_letter + "</color>";//브레이크 통해 빠져나온 글자가 해당 조건을 만족하면 해당 글자의 색깔을 추가
                }
                else if(t_Yellow)
                {
                    t_letter = "<color=#FFFF00>" + t_letter + "</color>";//브레이크 통해 빠져나온 글자가 해당 조건을 만족하면 해당 글자의 색깔을 추가
                }
                txt_Dialogue.text += t_letter;//받아온 문장을 한글자씩 대입
            }
            t_ignore = false;//특수기호에서 이그노어가 트루이기때문에 조건문을 모두 건너뛰고 다음글자 부터 색상적용과 출력이 이루어짐.
            yield return new WaitForSeconds(textDelay);//텍스트 딜레이 만큼 대기

        }

        isnext = true;
     

    }

    void SettingUI(bool p_Fleg)//받아오는 파라미터로 관리
    {
        Go_Dialogue_bar.SetActive(p_Fleg);

        if(p_Fleg)//대화창을 보여줘야할때
        {
            if(Dialogues[lineCount].name == "")//받아오는 파일에서 이름에 해당하는 칸이 비어있다면
            {
                Go_Dialogue_NameBar.SetActive(false);
            }
            else//파일에서 이름이 있을경우
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
