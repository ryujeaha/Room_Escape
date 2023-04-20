using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Type : MonoBehaviour
{
    public bool isDoor;//조사하고 있는 객체가 문인지 아닌지 판별
    public bool isobject;//일반 객체 판별

    [SerializeField] string Interaction_Name;//크로스웨어로 갖다댄 객체의 이름을 저장할 변수
 
    public string GetName()//이름을 가져오는 함수
    {
        return Interaction_Name;
    }
}
