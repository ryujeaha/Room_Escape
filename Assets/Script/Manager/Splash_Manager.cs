using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Splash_Manager : MonoBehaviour
{
    [SerializeField] Image Fade_Img;//암전 효과의 사용될이미지

    [SerializeField] Color FadeColor_White;//페이드 이미지의 컬러를 변경할 컬러변수(이 변수를 변경해서 스플레쉬(섬광)효과를 연출)
    [SerializeField] Color FadeColor_Black;//페이드 이미지의 컬러를 변경할 컬러변수(이 변수를 변경해서 암전효과를 연출)

    [SerializeField] float FadeSpeed;//암전속도.
    [SerializeField] float FadeSlow_Speed;//느리게 암전할 경우 사용되는 속도

    public static bool isFinish = false;//암전이 끝난뒤에 대화창을 뜨게하기위한 시점을 알려주기위한 변수
   
    public IEnumerator Splash()
    {
        isFinish = false;
        StartCoroutine(FadeOut(true, false));
        yield return new WaitUntil(() => isFinish);
        StartCoroutine(FadeIn(true, false));
    }


    public IEnumerator FadeOut(bool isWhite,bool isSlow)//페이드 아웃 코루틴(하얀색인가 아닌가?,느리게 암전할것인가 아닌가를 위한 변수)(평소에는 속도를 느리게 연출하고 감정표현같은 연출은 빠르게 연출한다)
    {
        Color t_color = (isWhite == true) ? FadeColor_White : FadeColor_Black;
        t_color.a = 0;

        Fade_Img.color = t_color;

        while(t_color.a < 1)//알파값이 완전히 켜지기 전까지 반복
        {
            t_color.a += (isSlow == true) ? FadeSlow_Speed : FadeSpeed;
            Fade_Img.color = t_color;
            yield return null;
        }
        isFinish = true;//모든 작업이 끝났다는 뜻(대화창 출력가능)
    }
    public IEnumerator FadeIn(bool isWhite, bool isSlow)//페이드 아웃 코루틴(하얀색인가 아닌가?,느리게 암전할것인가 아닌가를 위한 변수)
    {
        Color t_color = (isWhite == true) ? FadeColor_White : FadeColor_Black;
        t_color.a = 1;

        Fade_Img.color = t_color;

        while (t_color.a > 0)//알파값이 완전히 꺼지기 전까지 반복
        {
            t_color.a -= (isSlow == true) ? FadeSlow_Speed : FadeSpeed;
            Fade_Img.color = t_color;
            yield return null;
        }
        isFinish = true;//모든 작업이 끝났다는 뜻(대화창 출력가능)
    }
}
