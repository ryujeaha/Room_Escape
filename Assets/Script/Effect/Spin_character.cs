using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin_character : MonoBehaviour
{
   [SerializeField] Transform tf_target;//ĳ���Ͱ� �i�ư� �÷��̾� ��ġ����.

    private void Update()
    {
        if(tf_target != null)//Ÿ�ٺ����� ��ġ������ ����ִٸ�
        {
            Quaternion t_rotation = Quaternion.LookRotation(tf_target.position);
            Vector3 t_Eulur = new Vector3(0, t_rotation.eulerAngles.y, 0);
            transform.eulerAngles = t_Eulur;
        }
    }

}
