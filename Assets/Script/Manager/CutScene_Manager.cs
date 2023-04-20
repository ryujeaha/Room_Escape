using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene_Manager : MonoBehaviour
{
    public static bool isFinished = false;

    Splash_Manager the_splash;//
    Camera_COntroller the_Cam;//컷신을 불러오고 나서 다시 카메라를 원래 위치로 돌아오기위해서
    [SerializeField] Image img_cutScene;//컷신 이미지
        
    // Start is called before the first frame update
    void Start()
    {
        the_splash = FindObjectOfType<Splash_Manager>();
        the_Cam = FindObjectOfType<Camera_COntroller>();
    }
    public bool CheckCutScene()
    {
        return img_cutScene.gameObject.activeSelf;//현재 이 객체가 활성화인지 아닌지 알려주는 명령어
    }

    public IEnumerator CutsceneCoroutine(string CutsceneName,bool p_isShow)//컷신의 이름과 컷신을 띄우고 보여줄 여부를 결정할 불값
    {
        Splash_Manager.isFinish = false;//끝날때 까지 대기시키기 위해 변수 바꿔놓기
        StartCoroutine(the_splash.FadeOut(true, false));
        yield return new WaitUntil(()=> Splash_Manager.isFinish);//이스 피니쉬가 트루가 될떄까지 대기
        if(p_isShow)
        {
            Sprite t_sprite = Resources.Load<Sprite>("CutScenes/" + CutsceneName);
            if (t_sprite != null)//올바르게 컷신을 불러왔을때만
            {
                img_cutScene.gameObject.SetActive(true);
                img_cutScene.sprite = t_sprite;//저장한 스프라이트를 바꿔주기
                the_Cam.CameraTargetting(null, 0.1f, true, false);
            }
            else
            {
                Debug.LogError("오류 해당 경로에서" + CutsceneName + "해당하는 이미지를 찾을수 없습니다");
            }
        }
        else
        {
            img_cutScene.gameObject.SetActive(false);
        }
       

        Splash_Manager.isFinish = false;//끝날때 까지 대기시키기 위해 변수 바꿔놓기
        StartCoroutine(the_splash.FadeIn(true, false));
        yield return new WaitUntil(() => Splash_Manager.isFinish);//이스 피니쉬가 트루가 될떄까지 대기

        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }
}
