using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float spin_Speed;//ȸ�� �ӵ�
    [SerializeField] Vector3 spinDir;//ȸ�� ����
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinDir * spin_Speed * Time.deltaTime);//(����,�ӵ�,�ð�)(���ɴ��̾� ��������,���ɽ��ǵ常ŭ,1�ʿ�) �ش罺ũ��Ʈ�� �پ��ִ� ��ü��
    }
}
