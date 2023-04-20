using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType//열거형은 무조건 콤마로 끝나야 한다
{
    ObjectFront,//카메라가 오브젝트 앞으로 가는것
    Reset,//카메라가 다시 원래 형태로 돌아오는것
    FaideOut,//페이드 아웃(화면 암전)
    FaidIn,//페이드 인(화면 암전 해제)
    FlashOut,//플레시아웃(번쩍이는 효과)
    FlashIn,//플레시인번쩍이는 효과해제?
    ShowCutScene,//컷씬을 보여주는것
    HideCutScene,//컷신을 숨기는것
}


[System.Serializable]//커스텀 클래스는 인스펙터창에서 수정하기위해서는 이 명령어가 필요하다
public class Dialogue 
{
    [Header("카메라가 타겟팅할 대상")]
    public CameraType cameratype;
    public Transform tf_target;

    //[HideInInspector]
    [Tooltip("대사를 하는 캐릭터의 이름")]//인스펙터 창에서 해당 변수의 역할을 띄우기 위한 명령어
    public string name;//대사를 하는 캐릭터의 이름

    //[HideInInspector]
    [Tooltip("대사내용")]//인스펙터 창에서 해당 변수의 역할을 띄우기 위한 명령어
    public string[] conTexts;//대사의 내용(한 캐릭터가 여러번 말할 수 있으므로 배열로 선언)

    [HideInInspector]
    public string[] spritename;
}

[System.Serializable]//인터렉션 이벤트 스크립트에서 선언할때 인스펙터창에서 띄우기 위한 역할
public class DialogueEvent
{
    public string name;//현재 이벤트가 어떠한 이벤트인지 구별하기위한 이름변수(예 식당이벤트,집 이벤트)

    public Vector2 line;//X부터 Y까지의 대사를 추출해내기 위한 변수
    public Dialogue[] dialogues;//한명이 말하는 것이 아닌 다른 등장인물들의 다이얼로그로써도 사용하기위해 배열로 선언
}
