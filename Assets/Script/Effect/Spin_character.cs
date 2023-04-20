using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin_character : MonoBehaviour
{
   [SerializeField] Transform tf_target;//캐릭터가 쫒아갈 플레이어 위치정보.

    private void Update()
    {
        if(tf_target != null)//타겟변수의 위치정보가 담겨있다면
        {
            Quaternion t_rotation = Quaternion.LookRotation(tf_target.position);
            Vector3 t_Eulur = new Vector3(0, t_rotation.eulerAngles.y, 0);
            transform.eulerAngles = t_Eulur;
        }
    }

}
