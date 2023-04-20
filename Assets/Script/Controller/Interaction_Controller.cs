using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Controller : MonoBehaviour
{
    [SerializeField] Camera cam;//ī�޶� ���� ����.(�������� �� ī�޶�)

    RaycastHit hitInfo;//�������� ���� ��ü�� ������ ������ ����.

    [SerializeField] GameObject go_NomalCrossHair;//�븻 ũ�ν��� ���� ����.
    [SerializeField] GameObject go_Interactive_CrossHair;//��ȣ�ۿ� ũ�ν��� ���� ����.
    [SerializeField] GameObject go_CrossHair;//ũ�ν���� �θ�ü(��������ؼ�)
    [SerializeField] GameObject go_Cursor;//���� �ַο� �θ�ü
    [SerializeField] GameObject go_TargetNameBar;//������ ��� ���ӹ�
    [SerializeField] Text txt_TargetName;//������ �̸��� �ٲ� �ؽ�Ʈ

    bool isContact = false;//��ȣ�ۿ밣�� ��ü���� �ƴ����� ����
   public static bool isInteract = false;//��ȣ�ۿ����̴ٸ� �������� �Һ���

    [SerializeField] ParticleSystem ps_QuestionEffect;//������ ����Ʈ�� ���� ����.

    [SerializeField] Image Img_Interaction;
    [SerializeField] Image Img_interactionEffect;

    Dialogue_Manager theDial;//���̾�α� �Ŵ��� ����

    public void SettiingUi(bool p_Fleg)
    {
        go_CrossHair.SetActive(p_Fleg);
        go_Cursor.SetActive(p_Fleg);
        go_TargetNameBar.SetActive(p_Fleg);

        isInteract = !p_Fleg;//��ȭ�� ������ ��ȣ�ۿ����� �����ٰ� �˷��ִ� ����
    }
    private void Start()
    {
        theDial = FindObjectOfType<Dialogue_Manager>();//���̾��Ű���� �ش��ϴ� ��ũ��Ʈ�� �پ��ִ� ��ü�� ã�Ƽ� �־���´�.(���ɻ� �Ҹ�)
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
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);//���콺�� ��ġ���� ����

        //HitInfo���ٰ� �����ϱ����ؼ� ������ �������� �Լ�(HitInfo�� �����Ų�ٴ� ��)
        if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100))//(������ �߻���ġ,����,�Ÿ�).ScreenPointToRay = ī�޶� ������ ������ ���� �������� 2Dȭ�� ��ǥ�� 3D������� ��ǥ�� ġȯ���ִ� ����.(���콺��ǥ�� ���� 3D��ǥ�� ġȯ)
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
            go_TargetNameBar.SetActive(true);//���� ����
            txt_TargetName.text = hitInfo.transform.GetComponent<Interaction_Type>().GetName();//Ʈ�������� �ִ� ��ũ��Ʈ�� ����� �̸��� �����ͼ� ����
            if (!isContact)
            {
                isContact = true;//�ߺ� ���� ����
                go_NomalCrossHair.SetActive(false);
                go_Interactive_CrossHair.SetActive(true);
                StopCoroutine("Interaction");//�ߺ� �������
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
            isContact = false;//�ߺ� ���� ����
            go_Interactive_CrossHair.SetActive(false);
            go_NomalCrossHair.SetActive(true);
            StopCoroutine("Interaction");//�ߺ� �������
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
                yield return null;//��Ÿ�� ���
            }
        }else
        {
            while(color.a > 0)
            {
                color.a -= 0.1f;
                Img_Interaction.color = color;
                yield return null;//��Ÿ�� ���
            }
        }
    }
    IEnumerator InteractionEffect()
    {
        while(isContact && !isInteract)//ũ�ν���� ��ȣ�ۿ��� ������ ��ü�� �ӹ����� ��ȣ�ۿ����� �ƴҰ��
        {
            Color color = Img_interactionEffect.color;
            color.a = 0.5f;

            Img_interactionEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 t_scale = Img_interactionEffect.transform.localScale;

            while (color.a > 0)
            {
                color.a -= 0.01f;
                Img_interactionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);//������ ���� �����ϱ����� ������ Set�̴� time.deltaTime�� �ǹ̴� 1���� 1�� �ø��ٴ� ��
                Img_interactionEffect.transform.localScale = t_scale;
                yield return null;
            }
            yield return null;
        }
    }
    void ClickLeftBTN()
    {
        if(!isInteract)//��ȣ�ۿ����� �ƴҶ���
        {
            if (Input.GetMouseButtonDown(0))//GetMouseButtonDown���ٰ� 0�� �Ѱ��ְ� �Ǹ� ���콺 ��Ŭ���� �����ϰԵȴ�.
            {
                if (isContact)//��ȣ�ۿ� ��ü�� Ŭ���������.
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
        ps_QuestionEffect.transform.position = cam.transform.position;//���� ��ġ���� �����°�ó�� �����ϱ����ؼ� ���� ī�޶��� ��ġ�� �̵�

        StartCoroutine(WaitCollision());
    }
    IEnumerator WaitCollision()//��ȣ�ۿ� ����Ʈ�� ��� �΋H���� ���� ���̾�α׹ٸ� �������� ����Ű�� �ڷ�ƾ
    {
        yield return new WaitUntil(()=>QUestion_Effect.isCollide);//WaitForSeconds�� ���� �ʴ� ������ ��Ȯ�� ���ʵڿ� �΋H������ �˼� ���⶧���� waitUntill(()=> Ư�� ����),(�����Ҷ����� ���)�� ����Ѵ�.
        QUestion_Effect.isCollide = false;

       

        theDial.SHowDialogue(hitInfo.transform.GetComponent<Interaction_Event>().GetDialogue());
    }
}
