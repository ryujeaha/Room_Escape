using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_COntroller : MonoBehaviour
{
    Vector3 originPos;//ī�޶��� ���� ��ġ
    Quaternion originRot;//ī�޶��� ���� ����

    Interaction_Controller theInter;
    Player_Controller thePlayer;

    Coroutine coroutine;//�����ν��� �ڷ�ƾ

    private void Start()
    {
        theInter = FindObjectOfType<Interaction_Controller>();
        thePlayer = FindObjectOfType<Player_Controller>();
    }
    public void CamoriginSetting()
    {
        originPos = transform.position;//���� ��ġ ���   
        originRot = Quaternion.Euler(0, 0, 0);
    }

    public void CameraTargetting(Transform p_Target, float p_camSpeed = 0.1f, bool isReset = false, bool p_isFinish = false)
    {
        if (!isReset)//�������� ����Ʈ�ϰ��
        {
            if (p_Target != null)
            {
                StopAllCoroutines();//������ �����ִ� �ڷ�ƾ �ߴ�.(��������)
              coroutine =  StartCoroutine(CameraTargettingCoroutine(p_Target, p_camSpeed));//�������� �ڷ�ƾ������ �ֱ�
            }
        }
        else//������ �ؾ��Ұ��
        {
            if(coroutine != null)//�������� �ڷ�ƾ�� �ִٸ�
            {
                StopCoroutine(coroutine);
            }
            StartCoroutine(CameraResetCoroutine(p_camSpeed, p_isFinish));
        }
      
        
    }

     IEnumerator CameraTargettingCoroutine(Transform p_Target,float p_camSpeed = 0.05f)//ī�޶� ������ ���,�����ӵ��� �޾ƿ´�
     {
        Vector3 t_TargetPos = p_Target.position;//Ÿ���� ��Ȯ�� ��ġ������ ������ ����
        Vector3 t_targetFrontPos = t_TargetPos + p_Target.forward;//Ÿ���� �ٷξտ��� ī�޶� ���߱����� �ٷξ� ��ǥ ���� �����ش�.(forward.�Ķ�ȭ��ǥ����)
        Vector3 t_Direction = (t_TargetPos - t_targetFrontPos).normalized;//���ⱸ�ϱ� + �Ÿ��� ������� �������� ��ȯ�ϵ��� ��ֶ�������(����ȭ)

        while (transform.position != t_targetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f)//ī�޶� ���������� �����ϰ� �������� ������������ ������������ �ݺ�(LookRotation ������ ��ǥ�� ���̴� ���Ⱚ�� ����)
        {
            transform.position = Vector3.MoveTowards(transform.position, t_targetFrontPos, p_camSpeed);//Ÿ�� ��ġ���� ��� �̵������ִ� ����.(������ġ,��ǥ,�ӵ�)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction), p_camSpeed);//ī�޶� ȸ�������ִ� �ڵ�(������ġ,�������� �ٶ󺸴� ��������,�ӵ�)
            yield return null;
        }
     }

    IEnumerator CameraResetCoroutine(float p_camSpeed = 0.1f,bool p_isFinish = false)//����ȭ�� ���������� �ǵ��ư��� ķ�� �ӵ�
    {
        yield return new WaitForSeconds(0.3f);

        while (transform.position != originPos || Quaternion.Angle(transform.rotation,originRot) >= 0.5f)//ī�޶� ���������� �����ϰ� �������� ������������ ������������ �ݺ�
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, p_camSpeed);//Ÿ�� ��ġ���� ��� �̵������ִ� ����.(������ġ,��ǥ,�ӵ�)
            transform.rotation = Quaternion.Lerp(transform.rotation,originRot, p_camSpeed);//ī�޶� ȸ�������ִ� �ڵ�(������ġ,�������� �ٶ󺸴� ��������,�ӵ�)
            yield return null;
        }
        transform.position = originPos;

        if(p_isFinish)//��� ��ȭ��
        {
            thePlayer.ResetPos();
            theInter.SettiingUi(true);
        }
    }
}
