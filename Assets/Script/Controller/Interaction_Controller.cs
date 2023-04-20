using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Controller : MonoBehaviour
{
    [SerializeField] Camera cam;//카메라를 담을 변수.(레이저를 쏠 카메라)

    RaycastHit hitInfo;//레이저의 맞은 객체의 정보를 저장할 변수.

    [SerializeField] GameObject go_NomalCrossHair;//노말 크로스헤어를 담을 변수.
    [SerializeField] GameObject go_Interactive_CrossHair;//상호작용 크로스헤어를 담을 변수.
    [SerializeField] GameObject go_CrossHair;//크로스헤어 부모객체(숨기기위해서)
    [SerializeField] GameObject go_Cursor;//숨길 애로우 부모객체
    [SerializeField] GameObject go_TargetNameBar;//툴팁을 띄울 네임바
    [SerializeField] Text txt_TargetName;//툴팁의 이름을 바뀔 텍스트

    bool isContact = false;//상호작용간으 객체인지 아닌지를 구별
   public static bool isInteract = false;//상호작용중이다를 구분해줄 불변수

    [SerializeField] ParticleSystem ps_QuestionEffect;//만들어둔 이펙트를 담을 변수.

    [SerializeField] Image Img_Interaction;
    [SerializeField] Image Img_interactionEffect;

    Dialogue_Manager theDial;//다이얼로그 매니저 참조

    public void SettiingUi(bool p_Fleg)
    {
        go_CrossHair.SetActive(p_Fleg);
        go_Cursor.SetActive(p_Fleg);
        go_TargetNameBar.SetActive(p_Fleg);

        isInteract = !p_Fleg;//대화가 끝나면 상호작용중이 끝났다고 알려주는 역할
    }
    private void Start()
    {
        theDial = FindObjectOfType<Dialogue_Manager>();//하이어라키에서 해당하는 스크립트가 붙어있는 객체를 찾아서 넣어놓는다.(성능상 불리)
    }

    // Update is called once per frame
    void Update()
    {   
        if(!isInteract)
        {
            CheckObject();
            ClickLeftBTN();
        }
       
    }
    void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);//마우스의 위치값을 저장

        //HitInfo에다가 저장하기위해서 밖으로 내보내는 함수(HitInfo의 저장시킨다는 뜻)
        if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100))//(레이저 발사위치,방향,거리).ScreenPointToRay = 카메라 형식의 변수에 하위 문법으로 2D화면 좌표를 3D월드상의 좌표로 치환해주는 문법.(마우스좌표를 실제 3D좌표로 치환)
        {
            Contact();
        }
        else
        {
            NotContact();
        }
    }
    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            go_TargetNameBar.SetActive(true);//툴팁 띄우기
            txt_TargetName.text = hitInfo.transform.GetComponent<Interaction_Type>().GetName();//트랜스폼에 있는 스크립트에 저장된 이름을 가져와서 대입
            if (!isContact)
            {
                isContact = true;//중복 실행 방지
                go_NomalCrossHair.SetActive(false);
                go_Interactive_CrossHair.SetActive(true);
                StopCoroutine("Interaction");//중복 실행방지
                StopCoroutine("InteractionEffect");
                StartCoroutine("Interaction", true);
                StartCoroutine("InteractionEffect");
            }
            
        }else
        {
            NotContact();
        }
    }
    void NotContact()
    {
        if(isContact)
        {
            go_TargetNameBar.SetActive(false);
            isContact = false;//중복 실행 방지
            go_Interactive_CrossHair.SetActive(false);
            go_NomalCrossHair.SetActive(true);
            StopCoroutine("Interaction");//중복 실행방지
            StartCoroutine("Interaction", false);
        }
       
    }

    IEnumerator Interaction(bool p_Appear)
    {
        Color color = Img_Interaction.color;
        if(p_Appear)
        {
            color.a = 0;
            while(color.a < 1)
            {
                color.a += 0.1f;
                Img_Interaction.color = color;
                yield return null;//한타임 대기
            }
        }else
        {
            while(color.a > 0)
            {
                color.a -= 0.1f;
                Img_Interaction.color = color;
                yield return null;//한타임 대기
            }
        }
    }
    IEnumerator InteractionEffect()
    {
        while(isContact && !isInteract)//크로스웨어가 상호작용이 가능한 객체의 머무르고 상호작용중이 아닐경우
        {
            Color color = Img_interactionEffect.color;
            color.a = 0.5f;

            Img_interactionEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 t_scale = Img_interactionEffect.transform.localScale;

            while (color.a > 0)
            {
                color.a -= 0.01f;
                Img_interactionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);//벡터의 값을 수정하기위한 문법은 Set이다 time.deltaTime의 의미는 1초의 1씩 늘린다는 뜻
                Img_interactionEffect.transform.localScale = t_scale;
                yield return null;
            }
            yield return null;
        }
    }
    void ClickLeftBTN()
    {
        if(!isInteract)//상호작용중이 아닐때만
        {
            if (Input.GetMouseButtonDown(0))//GetMouseButtonDown에다가 0을 넘겨주게 되면 마우스 좌클릭을 감지하게된다.
            {
                if (isContact)//상호작용 객체를 클릭했을경우.
                {
                    interect();
                }
            }
        }
    }
    void interect()
    {
        isInteract = true;

        StopCoroutine("Interaction");
        Color color = Img_Interaction.color;
        color.a = 0;
        Img_Interaction.color = color;

        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<QUestion_Effect>().SetTarget(t_targetPos);
        ps_QuestionEffect.transform.position = cam.transform.position;//본인 위치에서 던지는것처럼 연출하기위해서 현재 카메라의 위치로 이동

        StartCoroutine(WaitCollision());
    }
    IEnumerator WaitCollision()//상호작용 이펙트가 대상에 부딫히고 나서 다이얼로그바를 띄우기위해 대기시키는 코루틴
    {
        yield return new WaitUntil(()=>QUestion_Effect.isCollide);//WaitForSeconds를 쓰지 않는 이유는 정확히 몇초뒤에 부딫히는지 알수 없기때문에 waitUntill(()=> 특정 조건),(만족할때까지 대기)를 사용한다.
        QUestion_Effect.isCollide = false;

       

        theDial.SHowDialogue(hitInfo.transform.GetComponent<Interaction_Event>().GetDialogue());
    }
}
