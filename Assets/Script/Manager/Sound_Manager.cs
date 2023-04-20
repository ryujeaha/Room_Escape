using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;//음악의 이름
    public AudioClip clip;//음악 파일;
}

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;//자기 자신 인스턴스화

    [SerializeField] Sound[] Effect_Sound;//효과음
    [SerializeField] AudioSource[] Effect_Player;//효과음 플레이어

    [SerializeField] Sound[] BGM_Sound;//효과음
    [SerializeField] AudioSource BGM_Player;//효과음 플레이어

    //[SerializeField] AudioSource voicePlayer;
    private void Awake()
    {
        if(instance == null)//사운드 매니저가 들어갈 자리가 비어있다
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else//이미 사운드 매니저가 들어있다
        {
            Destroy(gameObject);//다른 씬으로 넘어갔다가 원래 씬으로 오면 중복상태가 되기때문에 그럴경우 새로 생성된 쪽을 파괴시켜준다.
        }
    }

    void PlayBgm(string p_name)//음악의 이름 받아오기(해당하는 음악 재생으로 추정)
    {
        for (int i = 0; i < BGM_Sound.Length; i++)
        {
            if(p_name == BGM_Sound[i].name)//브금 사운에 i번쨰의 받아온 이름과 같은 곡이 있다면
            {
                BGM_Player.clip = BGM_Sound[i].clip;//플레이어 클립에다가 현재 노래 클립을 저장해주고
                BGM_Player.Play();//플레이
                return;
            }
        }
        Debug.LogError(p_name + "에 해당하는 bgm이 없습니당");//에러 띄우기
    }

     void StopBgm()
    {
        BGM_Player.Stop();//bgm 중지
    }

    void PauseBgm()
    {
        BGM_Player.Pause();//bgm 일시정지 
    }

    void UnPauseBgm()
    {
        BGM_Player.UnPause();//bgm 일시정지 해제
    }

    void PlayEffectSound(string p_name)
    {
        for (int i = 0; i < Effect_Sound.Length; i++)
        {
            if(p_name == Effect_Sound[i].name)
            {
                for (int j = 0; j < Effect_Player.Length; j++)
                {
                    if (!Effect_Player[j].isPlaying)//현재 j번째 효과음 플레이어가 사용중이 아니라면
                    {
                        Effect_Player[j].clip = Effect_Sound[i].clip;//비어있는 플레이어에 현재 음악 넣어놓기
                        Effect_Player[j].Play();
                        return;//음악이 들어오고 재생됬다면 반복할 필요가 없으므로 리턴
                    }
                }
                Debug.LogError ("모든 효과음 플레이어가 사용중입니다");//모든 j번째 효과음이 사용중이 어서 조건문 진입이 안되는경우
                return;
            }
        }
        Debug.LogError(p_name + "해당하는 사운드가 없습니다");//받아온 이름과 일치하는 이름이 없는경우
    }

    void StopAllEffectSound()
    {
        for (int i = 0; i < Effect_Player.Length; i++)
        {
            Effect_Player[i].Stop();//모든 플레이어 정지
        }
    }

    void PlayBoiceSound(string p_name)//리소스 폴더에서 직접 가져오는 방식
    {
        /*
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice" + p_name);
        if(_clip != null)//클립에 ㄴ노래가 원할하게 들어가있을 경우만
        {
            voicePlayer.clip = _clip;
            voicePlayer.Play();
        }
        else
        {
            Debug.LogError(p_name + "에 해당하는 보이스파일이 없습니다");
        }
    */
    }

    /// p_type : 0 -> BGM 재생
    /// p_type : 1 -> 효과음 재생
    ///p_type : 2 -> 보이스 재생
    public void PlaySound(string p_name, int p_type)//구별해서 사운드를 틀어주기 위한 종합함수
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
    /// p_type : 0 -> BGM정지
    /// p_type : 1 -> BGM 정지해제
    ///p_type : 2 -> 음악 아예 멈춤
    ///p_type : 3 -> 모든효과음 멈춤
    public void SetBgm(int p_type)//bgm음악 정지 기능
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
