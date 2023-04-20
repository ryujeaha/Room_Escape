using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QUestion_Effect : MonoBehaviour
{

    [SerializeField] float MoveSpeed;//투사체속도
    Vector3 targetPos = new Vector3();

    public static bool isCollide = false;//이펙트가 부딫혔는가를 판단하는 bool 변수.(static은 정적변수로써 어디서나 사용가능하게 하겠다는 의미)

    [SerializeField] ParticleSystem Ps_Effect;//퍼져나가는 이펙트를 담을 변수.

    public void SetTarget(Vector3 _target)
    {
        targetPos = _target;//이펙트를 터트릴 목표물의 값이 넘어오게됨
    }
    // Update is called once per frame
    void Update()
    {
        if(targetPos != Vector3.zero)//목표물의 위치값이 들어왔을경우
        {
            if((transform.position - targetPos).sqrMagnitude >= 0.1f)//최대한 가까워졌을떄까지 날리게한다.sqrMagnitude = 두 거리간의 거리차의 제곱값이 나온다 {
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, MoveSpeed);//(거리,목적지,속도)?
               //Lerp = 거리차를 n분의 1씩 좁혀나가는 방식 (0.5f일경우 2분의 1씩 거리를 좁혀나감)
            }
            else//최대한 가까워져서 반환된 제곱값이 0.1보다 작을경우.
            {
                Ps_Effect.gameObject.SetActive(true);//켜주기 (날리는 파티클은 Play on Awake를 했기때문에 켜주자마자 플레이된다)
                Ps_Effect.transform.position = transform.position;//재생할 위치로 이동
                Ps_Effect.Play();//파티클을 재생시켜준다
                isCollide = true;
                targetPos = Vector3.zero;//목적지의 도착했기에 초기화
                gameObject.SetActive(false);
            }
        }
    }
}
