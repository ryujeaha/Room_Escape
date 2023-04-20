using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_COntroller : MonoBehaviour
{
    Vector3 originPos;//카메라의 원래 위치
    Quaternion originRot;//카메라의 원래 방향

    Interaction_Controller theInter;
    Player_Controller thePlayer;

    Coroutine coroutine;//변수로써의 코루틴

    private void Start()
    {
        theInter = FindObjectOfType<Interaction_Controller>();
        thePlayer = FindObjectOfType<Player_Controller>();
    }
    public void CamoriginSetting()
    {
        originPos = transform.position;//원래 위치 기억   
        originRot = Quaternion.Euler(0, 0, 0);
    }

    public void CameraTargetting(Transform p_Target, float p_camSpeed = 0.1f, bool isReset = false, bool p_isFinish = false)
    {
        if (!isReset)//열거형이 프론트일경우
        {
            if (p_Target != null)
            {
                StopAllCoroutines();//기존에 돌고있던 코루틴 중단.(오류방지)
              coroutine =  StartCoroutine(CameraTargettingCoroutine(p_Target, p_camSpeed));//실행중인 코루틴변수에 넣기
            }
        }
        else//리셋을 해야할경우
        {
            if(coroutine != null)//실행중인 코루틴이 있다면
            {
                StopCoroutine(coroutine);
            }
            StartCoroutine(CameraResetCoroutine(p_camSpeed, p_isFinish));
        }
      
        
    }

     IEnumerator CameraTargettingCoroutine(Transform p_Target,float p_camSpeed = 0.05f)//카메라가 추적할 대상,추적속도를 받아온다
     {
        Vector3 t_TargetPos = p_Target.position;//타겟의 정확한 위치정보를 변수에 저장
        Vector3 t_targetFrontPos = t_TargetPos + p_Target.forward;//타켓의 바로앞에서 카메라를 멈추기위해 바로앞 좌표 값을 구해준다.(forward.파란화살표기준)
        Vector3 t_Direction = (t_TargetPos - t_targetFrontPos).normalized;//방향구하기 + 거리에 상관없이 같은값을 반환하도록 노멀라이지드(정규화)

        while (transform.position != t_targetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f)//카메라가 목적지까지 도달하고 각도차가 지정범위까지 좁혀질때까지 반복(LookRotation 지정한 목표가 보이는 방향값을 리턴)
        {
            transform.position = Vector3.MoveTowards(transform.position, t_targetFrontPos, p_camSpeed);//타겟 위치까지 계속 이동시켜주는 문법.(시작위치,목표,속도)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction), p_camSpeed);//카메라를 회전시켜주는 코드(시작위치,목적지를 바라보는 방향으로,속도)
            yield return null;
        }
     }

    IEnumerator CameraResetCoroutine(float p_camSpeed = 0.1f,bool p_isFinish = false)//모든대화가 끝났는지와 되돌아가는 캠의 속도
    {
        yield return new WaitForSeconds(0.3f);

        while (transform.position != originPos || Quaternion.Angle(transform.rotation,originRot) >= 0.5f)//카메라가 목적지까지 도달하고 각도차가 지정범위까지 좁혀질때까지 반복
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, p_camSpeed);//타겟 위치까지 계속 이동시켜주는 문법.(시작위치,목표,속도)
            transform.rotation = Quaternion.Lerp(transform.rotation,originRot, p_camSpeed);//카메라를 회전시켜주는 코드(시작위치,목적지를 바라보는 방향으로,속도)
            yield return null;
        }
        transform.position = originPos;

        if(p_isFinish)//모든 대화끝
        {
            thePlayer.ResetPos();
            theInter.SettiingUi(true);
        }
    }
}
