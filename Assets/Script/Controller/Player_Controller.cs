using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] Transform tf_CrossHair;//크로스헤어를 움직일 부모객체의 위치값을 저장할 변수.
    [SerializeField] Transform tf_Cam;//고개를 움직일 카메라의 위치값을 받아오기윟 저장할 변수(좌우는,Y값을 움직이고,상하는X값을 움직여야함.) 
    [SerializeField] Vector2 Cam_Boundary;//캠의 영역 (가두기)

    [SerializeField] float sightMoveSpeed;//좌우 움직임 속도.(고개를 돌릴떄 약간식 움직이는 속도)
    [SerializeField] float sightSensivitity; // 고개의 움직임 속도.
    [SerializeField] float LookLimitX;//고개를 돌릴 수 있는 X값한계.
    [SerializeField] float LookLimitY;//고개를 돌릴 수 있는 Y값한계.
    float currentAngleX,currentAngleY; //현재 얼만큼 고개를 돌렸는지를 저장할 변수.

    [SerializeField] GameObject go_NotCamHigh;
    [SerializeField] GameObject go_NotCamDown;
    [SerializeField] GameObject go_NotCamRight;
    [SerializeField] GameObject go_NotCamLeft;

    float originPosY;

    public void ResetPos()
    {
        currentAngleX = 0;
        currentAngleY = 0;
    }
    private void Start()
    {
        originPosY = tf_Cam.localPosition.y;//1이 들어가게 된다.
    }
    // Update is called once per frame
    void Update()
    {
        if (!Interaction_Controller.isInteract)//상호작용시에 움직임을 막기위함
        {
            CrossHairMoving();
            ViewMoving();
            KeyViewMoving();
            CameraLimit();
            NotCamUI();
        }
       
    }
    
    void NotCamUI()
    {
        go_NotCamHigh.SetActive(false);
        go_NotCamDown.SetActive(false);
        go_NotCamRight.SetActive(false);
        go_NotCamLeft.SetActive(false);

        if(currentAngleY >=  LookLimitX)
        {
            go_NotCamRight.SetActive(true);
        }
        else if(currentAngleY <= -LookLimitX)
        {
            go_NotCamLeft.SetActive(true);
        }

        if (currentAngleX <= -LookLimitY)//좌우반전을 시켰기 때문에
        {
            go_NotCamHigh.SetActive(true);
        }
        else if (currentAngleX >= LookLimitY)
        {
            go_NotCamDown.SetActive(true);
        }
    }

    void CameraLimit()
    {
        if(tf_Cam.localPosition.x >= Cam_Boundary.x)//카메라가 지정한영역밖으로 나가려고 하거나 나갔을경우.(좌우)
        {
            tf_Cam.localPosition = new Vector3(Cam_Boundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
       else if (tf_Cam.localPosition.x <= -Cam_Boundary.x)//카메라가 지정한영역밖으로 나가려고 하거나 나갔을경우.(좌우)
        {
            tf_Cam.localPosition = new Vector3(-Cam_Boundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Cam.localPosition.y >= originPosY+ Cam_Boundary.y)//카메라가 지정한영역밖으로 나가려고 하거나 나갔을경우.(상하)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY+ Cam_Boundary.y, tf_Cam.localPosition.z);//현재 카메라가 +1위치에 있으므로
        }
       else if (tf_Cam.localPosition.y <= originPosY- Cam_Boundary.y)//카메라가 지정한영역밖으로 나가려고 하거나 나갔을경우.(상하)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY - Cam_Boundary.y, tf_Cam.localPosition.z);//현재 카메라가 +1위치에 있으므로
        }
    }

    void KeyViewMoving()
    {
        if(Input.GetAxisRaw("Horizontal") != 0)//방향키가 눌러졌을 경우 (오른쪽 1반환 왼쪽 -1반환)
        {
            currentAngleY += sightSensivitity * Input.GetAxisRaw("Horizontal");//양수 음수 구분(왼쪽이면 -1을 곱하구 오른쪽이면 +1을 곱해서)
            currentAngleY = Mathf.Clamp(currentAngleY, -LookLimitX, LookLimitX);//범위가두기 구현(Clamp)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + sightMoveSpeed * Input.GetAxisRaw("Horizontal"), tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        if (Input.GetAxisRaw("Vertical") != 0)//방향키가 눌러졌을 경우 (위쪽 1반환 아랫쪽 -1반환)
        {
            currentAngleX += sightSensivitity * -Input.GetAxisRaw("Vertical");//양수 음수 구분(왼쪽이면 -1을 곱하구 오른쪽이면 +1을 곱해서)+ 좌우반전
            currentAngleX = Mathf.Clamp(currentAngleX, -LookLimitY, LookLimitY);//범위가두기 구현(Clamp)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + sightMoveSpeed * Input.GetAxisRaw("Vertical"), tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    } 

     void ViewMoving()
    {
        //좌우 기능구현
        if(tf_CrossHair.localPosition.x > (Screen.width / 2 - 100) || tf_CrossHair.localPosition.x < (-Screen.width /2 + 100 ))//현재 화면이 스크린의 width,height값의 2분의 1만큼이니 그것보다 크거나 작을경우 해당 방향으로 나간것이 된다.(100은 여유값,적당히만 가도 움직이게)
        {
            currentAngleY += (tf_CrossHair.localPosition.x > 0) ? sightSensivitity : -sightSensivitity; //(3항연산자)현재 크로스헤어의 x값이 0보다 크다(오른쪽)이라면 더해주고 아니라면 빼준다. 
            currentAngleY = Mathf.Clamp(currentAngleY,-LookLimitX,LookLimitX);//범위가두기 구현(Clamp)

            float t_ApplySpeed = (tf_CrossHair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;//3항연산자
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_ApplySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);//대입
        }
        //상하 기능구현
        if (tf_CrossHair.localPosition.y > (Screen.height / 2 - 100) || tf_CrossHair.localPosition.y < (-Screen.height / 2 + 100))//현재 화면이 스크린의 width,height값의 2분의 1만큼이니 그것보다 크거나 작을경우 해당 방향으로 나간것이 된다.(100은 여유값,적당히만 가도 움직이게)
        {
            currentAngleX += (tf_CrossHair.localPosition.y > 0) ? -sightSensivitity : sightSensivitity; //(3항연산자)현재 크로스헤어의 Y값이 0보다 크면 뺴주고 작으면 더해준다 (Y값은 더해주면 아해로 내려가기때문에)
            currentAngleX = Mathf.Clamp(currentAngleX, -LookLimitY, LookLimitY);//범위가두기 구현(Clamp)

            float t_ApplySpeed = (tf_CrossHair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;//3항연산자
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_ApplySpeed, tf_Cam.localPosition.z);//대입
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z); //localEulerAngles 회전값을 다룰떄 쓰는 문법(고개돌아가는 기능구현을 위한 카메라 움직이기)
    }
    void CrossHairMoving()//크로스헤어를 움직이기위한 함수.
    {
        //localposition자식객체는 부모객체로부터 얼만큼 떨어져있는지로 좌표값이 바뀌게한다(상대좌표)
        tf_CrossHair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2),
                                                 Input.mousePosition.y - (Screen.height / 2));//마우스의 시작지점과 크로스헤어의 시작위치가 그 싱크를 맞추기위해 차이나는 값만큼 빼주는것 같다
        float t_cursorPosX = tf_CrossHair.localPosition.x;
        float t_cursorPosY = tf_CrossHair.localPosition.y;

        t_cursorPosX = Mathf.Clamp(t_cursorPosX, (-Screen.width / 2 + 50), (Screen.width / 2 - 50));//제어하고싶은 변수를 넣어주고 최소값,최대값을 정하면 그만큼 값을 제한하는 문법
        t_cursorPosY = Mathf.Clamp(t_cursorPosY, (-Screen.height / 2 + 50), (Screen.height / 2 - 50));//크로스헤어 범위 가두기

        tf_CrossHair.localPosition = new Vector2(t_cursorPosX, t_cursorPosY);//제한된 범위에서만 움직일수 있게.
    }
}
