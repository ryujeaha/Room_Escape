using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QUestion_Effect : MonoBehaviour
{

    [SerializeField] float MoveSpeed;//����ü�ӵ�
    Vector3 targetPos = new Vector3();

    public static bool isCollide = false;//����Ʈ�� �΋H���°��� �Ǵ��ϴ� bool ����.(static�� ���������ν� ��𼭳� ��밡���ϰ� �ϰڴٴ� �ǹ�)

    [SerializeField] ParticleSystem Ps_Effect;//���������� ����Ʈ�� ���� ����.

    public void SetTarget(Vector3 _target)
    {
        targetPos = _target;//����Ʈ�� ��Ʈ�� ��ǥ���� ���� �Ѿ���Ե�
    }
    // Update is called once per frame
    void Update()
    {
        if(targetPos != Vector3.zero)//��ǥ���� ��ġ���� ���������
        {
            if((transform.position - targetPos).sqrMagnitude >= 0.1f)//�ִ��� ��������������� �������Ѵ�.sqrMagnitude = �� �Ÿ����� �Ÿ����� �������� ���´� {
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, MoveSpeed);//(�Ÿ�,������,�ӵ�)?
               //Lerp = �Ÿ����� n���� 1�� ���������� ��� (0.5f�ϰ�� 2���� 1�� �Ÿ��� ��������)
            }
            else//�ִ��� ��������� ��ȯ�� �������� 0.1���� �������.
            {
                Ps_Effect.gameObject.SetActive(true);//���ֱ� (������ ��ƼŬ�� Play on Awake�� �߱⶧���� �����ڸ��� �÷��̵ȴ�)
                Ps_Effect.transform.position = transform.position;//����� ��ġ�� �̵�
                Ps_Effect.Play();//��ƼŬ�� ��������ش�
                isCollide = true;
                targetPos = Vector3.zero;//�������� �����߱⿡ �ʱ�ȭ
                gameObject.SetActive(false);
            }
        }
    }
}
