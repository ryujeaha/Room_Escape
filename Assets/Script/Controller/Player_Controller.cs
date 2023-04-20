using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] Transform tf_CrossHair;//ũ�ν��� ������ �θ�ü�� ��ġ���� ������ ����.
    [SerializeField] Transform tf_Cam;//���� ������ ī�޶��� ��ġ���� �޾ƿ��⟢ ������ ����(�¿��,Y���� �����̰�,���ϴ�X���� ����������.) 
    [SerializeField] Vector2 Cam_Boundary;//ķ�� ���� (���α�)

    [SerializeField] float sightMoveSpeed;//�¿� ������ �ӵ�.(���� ������ �ణ�� �����̴� �ӵ�)
    [SerializeField] float sightSensivitity; // ���� ������ �ӵ�.
    [SerializeField] float LookLimitX;//���� ���� �� �ִ� X���Ѱ�.
    [SerializeField] float LookLimitY;//���� ���� �� �ִ� Y���Ѱ�.
    float currentAngleX,currentAngleY; //���� ��ŭ ���� ���ȴ����� ������ ����.

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
        originPosY = tf_Cam.localPosition.y;//1�� ���� �ȴ�.
    }
    // Update is called once per frame
    void Update()
    {
        if (!Interaction_Controller.isInteract)//��ȣ�ۿ�ÿ� �������� ��������
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

        if (currentAngleX <= -LookLimitY)//�¿������ ���ױ� ������
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
        if(tf_Cam.localPosition.x >= Cam_Boundary.x)//ī�޶� �����ѿ��������� �������� �ϰų� ���������.(�¿�)
        {
            tf_Cam.localPosition = new Vector3(Cam_Boundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
       else if (tf_Cam.localPosition.x <= -Cam_Boundary.x)//ī�޶� �����ѿ��������� �������� �ϰų� ���������.(�¿�)
        {
            tf_Cam.localPosition = new Vector3(-Cam_Boundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Cam.localPosition.y >= originPosY+ Cam_Boundary.y)//ī�޶� �����ѿ��������� �������� �ϰų� ���������.(����)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY+ Cam_Boundary.y, tf_Cam.localPosition.z);//���� ī�޶� +1��ġ�� �����Ƿ�
        }
       else if (tf_Cam.localPosition.y <= originPosY- Cam_Boundary.y)//ī�޶� �����ѿ��������� �������� �ϰų� ���������.(����)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY - Cam_Boundary.y, tf_Cam.localPosition.z);//���� ī�޶� +1��ġ�� �����Ƿ�
        }
    }

    void KeyViewMoving()
    {
        if(Input.GetAxisRaw("Horizontal") != 0)//����Ű�� �������� ��� (������ 1��ȯ ���� -1��ȯ)
        {
            currentAngleY += sightSensivitity * Input.GetAxisRaw("Horizontal");//��� ���� ����(�����̸� -1�� ���ϱ� �������̸� +1�� ���ؼ�)
            currentAngleY = Mathf.Clamp(currentAngleY, -LookLimitX, LookLimitX);//�������α� ����(Clamp)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + sightMoveSpeed * Input.GetAxisRaw("Horizontal"), tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        if (Input.GetAxisRaw("Vertical") != 0)//����Ű�� �������� ��� (���� 1��ȯ �Ʒ��� -1��ȯ)
        {
            currentAngleX += sightSensivitity * -Input.GetAxisRaw("Vertical");//��� ���� ����(�����̸� -1�� ���ϱ� �������̸� +1�� ���ؼ�)+ �¿����
            currentAngleX = Mathf.Clamp(currentAngleX, -LookLimitY, LookLimitY);//�������α� ����(Clamp)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + sightMoveSpeed * Input.GetAxisRaw("Vertical"), tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    } 

     void ViewMoving()
    {
        //�¿� ��ɱ���
        if(tf_CrossHair.localPosition.x > (Screen.width / 2 - 100) || tf_CrossHair.localPosition.x < (-Screen.width /2 + 100 ))//���� ȭ���� ��ũ���� width,height���� 2���� 1��ŭ�̴� �װͺ��� ũ�ų� ������� �ش� �������� �������� �ȴ�.(100�� ������,�������� ���� �����̰�)
        {
            currentAngleY += (tf_CrossHair.localPosition.x > 0) ? sightSensivitity : -sightSensivitity; //(3�׿�����)���� ũ�ν������ x���� 0���� ũ��(������)�̶�� �����ְ� �ƴ϶�� ���ش�. 
            currentAngleY = Mathf.Clamp(currentAngleY,-LookLimitX,LookLimitX);//�������α� ����(Clamp)

            float t_ApplySpeed = (tf_CrossHair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;//3�׿�����
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_ApplySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);//����
        }
        //���� ��ɱ���
        if (tf_CrossHair.localPosition.y > (Screen.height / 2 - 100) || tf_CrossHair.localPosition.y < (-Screen.height / 2 + 100))//���� ȭ���� ��ũ���� width,height���� 2���� 1��ŭ�̴� �װͺ��� ũ�ų� ������� �ش� �������� �������� �ȴ�.(100�� ������,�������� ���� �����̰�)
        {
            currentAngleX += (tf_CrossHair.localPosition.y > 0) ? -sightSensivitity : sightSensivitity; //(3�׿�����)���� ũ�ν������ Y���� 0���� ũ�� ���ְ� ������ �����ش� (Y���� �����ָ� ���ط� �������⶧����)
            currentAngleX = Mathf.Clamp(currentAngleX, -LookLimitY, LookLimitY);//�������α� ����(Clamp)

            float t_ApplySpeed = (tf_CrossHair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;//3�׿�����
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_ApplySpeed, tf_Cam.localPosition.z);//����
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z); //localEulerAngles ȸ������ �ٷꋚ ���� ����(�����ư��� ��ɱ����� ���� ī�޶� �����̱�)
    }
    void CrossHairMoving()//ũ�ν��� �����̱����� �Լ�.
    {
        //localposition�ڽİ�ü�� �θ�ü�κ��� ��ŭ �������ִ����� ��ǥ���� �ٲ���Ѵ�(�����ǥ)
        tf_CrossHair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2),
                                                 Input.mousePosition.y - (Screen.height / 2));//���콺�� ���������� ũ�ν������ ������ġ�� �� ��ũ�� ���߱����� ���̳��� ����ŭ ���ִ°� ����
        float t_cursorPosX = tf_CrossHair.localPosition.x;
        float t_cursorPosY = tf_CrossHair.localPosition.y;

        t_cursorPosX = Mathf.Clamp(t_cursorPosX, (-Screen.width / 2 + 50), (Screen.width / 2 - 50));//�����ϰ���� ������ �־��ְ� �ּҰ�,�ִ밪�� ���ϸ� �׸�ŭ ���� �����ϴ� ����
        t_cursorPosY = Mathf.Clamp(t_cursorPosY, (-Screen.height / 2 + 50), (Screen.height / 2 - 50));//ũ�ν���� ���� ���α�

        tf_CrossHair.localPosition = new Vector2(t_cursorPosX, t_cursorPosY);//���ѵ� ���������� �����ϼ� �ְ�.
    }
}
