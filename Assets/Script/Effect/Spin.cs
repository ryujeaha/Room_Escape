using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float spin_Speed;//회전 속도
    [SerializeField] Vector3 spinDir;//회전 방향
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinDir * spin_Speed * Time.deltaTime);//(방향,속도,시간)(스핀다이얼 방향으로,스핀스피드만큼,1초에) 해당스크립트가 붙어있는 객체를
    }
}
