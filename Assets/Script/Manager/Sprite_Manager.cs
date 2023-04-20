using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Manager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;//스프라이트가 교채되는 속도,

    bool CheckSameSprite(SpriteRenderer p_spriteRenderer,Sprite p_sprite)//바꾸려는 이미지가 같다면 굳이 바꾸지 않기위한 함수
    {
        if(p_spriteRenderer.sprite == p_sprite)//현재 가지고 있는 스프라이트와 바꾸려는 스프라이트가 같다면
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator SpriteChangeCoroutine(Transform p_target,string p_Sprite_Name)//가져올 스프라이트 이름과 렌더러들을 가져올 타겟의 위치정보
    {
        SpriteRenderer[] t_spriteRenderer = p_target.GetComponentsInChildren<SpriteRenderer>();//스프라이트를 그리려면 스프라이트 렌더러가 필요하므로 가져와준다.(단 이미지의 부모객체는 게임오브젝트이므로 렌더러가 없어 자식객체들에게서 가져온다(InChildren)(복수형 s)
        Sprite t_Sprite = Resources.Load("Characters/" + p_Sprite_Name, typeof(Sprite)) as Sprite;//리소스 폴더안에 있는 캐릭터 폴더에서 접근하여  받아온 이름에 해당하는 걸 가져온뒤 스프라이트로 변환이 가능하면(type of) 강제로 스프라이트로 변환
        if(!CheckSameSprite(t_spriteRenderer[0],t_Sprite))
        {
            Color t_color = t_spriteRenderer[0].color;//컬러변수에 컬러대입
            Color t_shadow_color = t_spriteRenderer[1].color;
            t_color.a = 0;
            t_shadow_color.a = 0;
            t_spriteRenderer[0].color = t_color;//사라지게 만들기
            t_spriteRenderer[1].color = t_shadow_color;//사라지게 만들기

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
