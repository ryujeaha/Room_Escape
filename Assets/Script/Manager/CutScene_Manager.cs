using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene_Manager : MonoBehaviour
{
    public static bool isFinished = false;

    Splash_Manager the_splash;//
    Camera_COntroller the_Cam;//�ƽ��� �ҷ����� ���� �ٽ� ī�޶� ���� ��ġ�� ���ƿ������ؼ�
    [SerializeField] Image img_cutScene;//�ƽ� �̹���
        
    // Start is called before the first frame update
    void Start()
    {
        the_splash = FindObjectOfType<Splash_Manager>();
        the_Cam = FindObjectOfType<Camera_COntroller>();
    }
    public bool CheckCutScene()
    {
        return img_cutScene.gameObject.activeSelf;//���� �� ��ü�� Ȱ��ȭ���� �ƴ��� �˷��ִ� ��ɾ�
    }

    public IEnumerator CutsceneCoroutine(string CutsceneName,bool p_isShow)//�ƽ��� �̸��� �ƽ��� ���� ������ ���θ� ������ �Ұ�
    {
        Splash_Manager.isFinish = false;//������ ���� ����Ű�� ���� ���� �ٲ����
        StartCoroutine(the_splash.FadeOut(true, false));
        yield return new WaitUntil(()=> Splash_Manager.isFinish);//�̽� �ǴϽ��� Ʈ�簡 �ɋ����� ���
        if(p_isShow)
        {
            Sprite t_sprite = Resources.Load<Sprite>("CutScenes/" + CutsceneName);
            if (t_sprite != null)//�ùٸ��� �ƽ��� �ҷ���������
            {
                img_cutScene.gameObject.SetActive(true);
                img_cutScene.sprite = t_sprite;//������ ��������Ʈ�� �ٲ��ֱ�
                the_Cam.CameraTargetting(null, 0.1f, true, false);
            }
            else
            {
                Debug.LogError("���� �ش� ��ο���" + CutsceneName + "�ش��ϴ� �̹����� ã���� �����ϴ�");
            }
        }
        else
        {
            img_cutScene.gameObject.SetActive(false);
        }
       

        Splash_Manager.isFinish = false;//������ ���� ����Ű�� ���� ���� �ٲ����
        StartCoroutine(the_splash.FadeIn(true, false));
        yield return new WaitUntil(() => Splash_Manager.isFinish);//�̽� �ǴϽ��� Ʈ�簡 �ɋ����� ���

        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }
}
