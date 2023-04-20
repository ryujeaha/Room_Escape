using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Splash_Manager : MonoBehaviour
{
    [SerializeField] Image Fade_Img;//���� ȿ���� �����̹���

    [SerializeField] Color FadeColor_White;//���̵� �̹����� �÷��� ������ �÷�����(�� ������ �����ؼ� ���÷���(����)ȿ���� ����)
    [SerializeField] Color FadeColor_Black;//���̵� �̹����� �÷��� ������ �÷�����(�� ������ �����ؼ� ����ȿ���� ����)

    [SerializeField] float FadeSpeed;//�����ӵ�.
    [SerializeField] float FadeSlow_Speed;//������ ������ ��� ���Ǵ� �ӵ�

    public static bool isFinish = false;//������ �����ڿ� ��ȭâ�� �߰��ϱ����� ������ �˷��ֱ����� ����
   
    public IEnumerator Splash()
    {
        isFinish = false;
        StartCoroutine(FadeOut(true, false));
        yield return new WaitUntil(() => isFinish);
        StartCoroutine(FadeIn(true, false));
    }


    public IEnumerator FadeOut(bool isWhite,bool isSlow)//���̵� �ƿ� �ڷ�ƾ(�Ͼ���ΰ� �ƴѰ�?,������ �����Ұ��ΰ� �ƴѰ��� ���� ����)(��ҿ��� �ӵ��� ������ �����ϰ� ����ǥ������ ������ ������ �����Ѵ�)
    {
        Color t_color = (isWhite == true) ? FadeColor_White : FadeColor_Black;
        t_color.a = 0;

        Fade_Img.color = t_color;

        while(t_color.a < 1)//���İ��� ������ ������ ������ �ݺ�
        {
            t_color.a += (isSlow == true) ? FadeSlow_Speed : FadeSpeed;
            Fade_Img.color = t_color;
            yield return null;
        }
        isFinish = true;//��� �۾��� �����ٴ� ��(��ȭâ ��°���)
    }
    public IEnumerator FadeIn(bool isWhite, bool isSlow)//���̵� �ƿ� �ڷ�ƾ(�Ͼ���ΰ� �ƴѰ�?,������ �����Ұ��ΰ� �ƴѰ��� ���� ����)
    {
        Color t_color = (isWhite == true) ? FadeColor_White : FadeColor_Black;
        t_color.a = 1;

        Fade_Img.color = t_color;

        while (t_color.a > 0)//���İ��� ������ ������ ������ �ݺ�
        {
            t_color.a -= (isSlow == true) ? FadeSlow_Speed : FadeSpeed;
            Fade_Img.color = t_color;
            yield return null;
        }
        isFinish = true;//��� �۾��� �����ٴ� ��(��ȭâ ��°���)
    }
}
