using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Manager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;//��������Ʈ�� ��ä�Ǵ� �ӵ�,

    bool CheckSameSprite(SpriteRenderer p_spriteRenderer,Sprite p_sprite)//�ٲٷ��� �̹����� ���ٸ� ���� �ٲ��� �ʱ����� �Լ�
    {
        if(p_spriteRenderer.sprite == p_sprite)//���� ������ �ִ� ��������Ʈ�� �ٲٷ��� ��������Ʈ�� ���ٸ�
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator SpriteChangeCoroutine(Transform p_target,string p_Sprite_Name)//������ ��������Ʈ �̸��� ���������� ������ Ÿ���� ��ġ����
    {
        SpriteRenderer[] t_spriteRenderer = p_target.GetComponentsInChildren<SpriteRenderer>();//��������Ʈ�� �׸����� ��������Ʈ �������� �ʿ��ϹǷ� �������ش�.(�� �̹����� �θ�ü�� ���ӿ�����Ʈ�̹Ƿ� �������� ���� �ڽİ�ü�鿡�Լ� �����´�(InChildren)(������ s)
        Sprite t_Sprite = Resources.Load("Characters/" + p_Sprite_Name, typeof(Sprite)) as Sprite;//���ҽ� �����ȿ� �ִ� ĳ���� �������� �����Ͽ�  �޾ƿ� �̸��� �ش��ϴ� �� �����µ� ��������Ʈ�� ��ȯ�� �����ϸ�(type of) ������ ��������Ʈ�� ��ȯ
        if(!CheckSameSprite(t_spriteRenderer[0],t_Sprite))
        {
            Color t_color = t_spriteRenderer[0].color;//�÷������� �÷�����
            Color t_shadow_color = t_spriteRenderer[1].color;
            t_color.a = 0;
            t_shadow_color.a = 0;
            t_spriteRenderer[0].color = t_color;//������� �����
            t_spriteRenderer[1].color = t_shadow_color;//������� �����

            t_spriteRenderer[0].sprite = t_Sprite;
            t_spriteRenderer[1].sprite = t_Sprite;

            while (t_color.a < 1)
            {
                t_color.a += fadeSpeed;
                t_shadow_color.a += fadeSpeed;
                t_spriteRenderer[0].color = t_color;
                t_spriteRenderer[1].color = t_shadow_color;
                yield return null;
            }
        }
    }
}
