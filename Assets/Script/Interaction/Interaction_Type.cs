using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Type : MonoBehaviour
{
    public bool isDoor;//�����ϰ� �ִ� ��ü�� ������ �ƴ��� �Ǻ�
    public bool isobject;//�Ϲ� ��ü �Ǻ�

    [SerializeField] string Interaction_Name;//ũ�ν������ ���ٴ� ��ü�� �̸��� ������ ����
 
    public string GetName()//�̸��� �������� �Լ�
    {
        return Interaction_Name;
    }
}
