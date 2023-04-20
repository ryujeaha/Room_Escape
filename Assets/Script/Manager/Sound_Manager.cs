using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;//������ �̸�
    public AudioClip clip;//���� ����;
}

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;//�ڱ� �ڽ� �ν��Ͻ�ȭ

    [SerializeField] Sound[] Effect_Sound;//ȿ����
    [SerializeField] AudioSource[] Effect_Player;//ȿ���� �÷��̾�

    [SerializeField] Sound[] BGM_Sound;//ȿ����
    [SerializeField] AudioSource BGM_Player;//ȿ���� �÷��̾�

    //[SerializeField] AudioSource voicePlayer;
    private void Awake()
    {
        if(instance == null)//���� �Ŵ����� �� �ڸ��� ����ִ�
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else//�̹� ���� �Ŵ����� ����ִ�
        {
            Destroy(gameObject);//�ٸ� ������ �Ѿ�ٰ� ���� ������ ���� �ߺ����°� �Ǳ⶧���� �׷���� ���� ������ ���� �ı������ش�.
        }
    }

    void PlayBgm(string p_name)//������ �̸� �޾ƿ���(�ش��ϴ� ���� ������� ����)
    {
        for (int i = 0; i < BGM_Sound.Length; i++)
        {
            if(p_name == BGM_Sound[i].name)//��� �� i������ �޾ƿ� �̸��� ���� ���� �ִٸ�
            {
                BGM_Player.clip = BGM_Sound[i].clip;//�÷��̾� Ŭ�����ٰ� ���� �뷡 Ŭ���� �������ְ�
                BGM_Player.Play();//�÷���
                return;
            }
        }
        Debug.LogError(p_name + "�� �ش��ϴ� bgm�� �����ϴ�");//���� ����
    }

     void StopBgm()
    {
        BGM_Player.Stop();//bgm ����
    }

    void PauseBgm()
    {
        BGM_Player.Pause();//bgm �Ͻ����� 
    }

    void UnPauseBgm()
    {
        BGM_Player.UnPause();//bgm �Ͻ����� ����
    }

    void PlayEffectSound(string p_name)
    {
        for (int i = 0; i < Effect_Sound.Length; i++)
        {
            if(p_name == Effect_Sound[i].name)
            {
                for (int j = 0; j < Effect_Player.Length; j++)
                {
                    if (!Effect_Player[j].isPlaying)//���� j��° ȿ���� �÷��̾ ������� �ƴ϶��
                    {
                        Effect_Player[j].clip = Effect_Sound[i].clip;//����ִ� �÷��̾ ���� ���� �־����
                        Effect_Player[j].Play();
                        return;//������ ������ �����ٸ� �ݺ��� �ʿ䰡 �����Ƿ� ����
                    }
                }
                Debug.LogError ("��� ȿ���� �÷��̾ ������Դϴ�");//��� j��° ȿ������ ������� � ���ǹ� ������ �ȵǴ°��
                return;
            }
        }
        Debug.LogError(p_name + "�ش��ϴ� ���尡 �����ϴ�");//�޾ƿ� �̸��� ��ġ�ϴ� �̸��� ���°��
    }

    void StopAllEffectSound()
    {
        for (int i = 0; i < Effect_Player.Length; i++)
        {
            Effect_Player[i].Stop();//��� �÷��̾� ����
        }
    }

    void PlayBoiceSound(string p_name)//���ҽ� �������� ���� �������� ���
    {
        /*
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice" + p_name);
        if(_clip != null)//Ŭ���� ���뷡�� �����ϰ� ������ ��츸
        {
            voicePlayer.clip = _clip;
            voicePlayer.Play();
        }
        else
        {
            Debug.LogError(p_name + "�� �ش��ϴ� ���̽������� �����ϴ�");
        }
    */
    }

    /// p_type : 0 -> BGM ���
    /// p_type : 1 -> ȿ���� ���
    ///p_type : 2 -> ���̽� ���
    public void PlaySound(string p_name, int p_type)//�����ؼ� ���带 Ʋ���ֱ� ���� �����Լ�
    {
        if(p_type == 0)
        {
            PlayBgm(p_name);
        }
        else if (p_type == 1)
        {
            PlayEffectSound(p_name);
        }
        /*
        else
        {
            PlayBoiceSound(p_name);
        }*/
    }
    /// p_type : 0 -> BGM����
    /// p_type : 1 -> BGM ��������
    ///p_type : 2 -> ���� �ƿ� ����
    ///p_type : 3 -> ���ȿ���� ����
    public void SetBgm(int p_type)//bgm���� ���� ���
    {
        if(p_type == 0)
        {
            PauseBgm();
        }
        else if(p_type == 1)
        {
            UnPauseBgm();
        }
        else if(p_type == 2)
        {
            StopBgm();
        }
        else if (p_type == 3)
        {
            StopAllEffectSound();
        }
    }
}
