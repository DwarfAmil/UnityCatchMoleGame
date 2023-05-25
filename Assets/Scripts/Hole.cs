using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum MoleState
{
    None,
    Open,
    Idle,
    Close,
    Catch
}

public class Hole : MonoBehaviour
{
    //두더지들의 상태 표기
    public MoleState MS;

    //이미지들(스프라이트) 묶음
    public Texture[] Open_Images;
    public Texture[] Idle_Images;
    public Texture[] Close_Images;
    public Texture[] Catch_Images;
    
    //어떤 두더지인지 체크하기 위한 값.
    public bool GoodMole;
    public int PerGood = 15;
    
    //이미지들의 묶음.
    public Texture[] Open_Images_Good;
    public Texture[] Idle_Images_Good;
    public Texture[] Close_Images_Good;
    public Texture[] Catch_Images_Good;

    //애니메이션 속도 관리 변수들
    public float Ani_Speed;
    public float _now_ani_time;
    int Ani_Count;
    
    //사운드 플레이용.
    public AudioClip Open_Sound;
    public AudioClip Catch_Sound;

    void Update () {
        if (_now_ani_time >= Ani_Speed)
        {
            if (MS == MoleState.Open)
            {
                Open_Ing();
            }
            if (MS == MoleState.Idle)
            {
                Idle_Ing();
            }
            if (MS == MoleState.Close)
            {
                Close_Ing();
            }
            if (MS == MoleState.Catch)
            {
                Catch_Ing();
            }
            _now_ani_time = 0;
        }
        else
        {
            _now_ani_time += Time.deltaTime;
        }
    }
    
    public void Open_On()
    {
        MS = MoleState.Open;
        Ani_Count = 0;
        
        GetComponent<AudioSource>().clip = Open_Sound;
        GetComponent<AudioSource>().Play();
        
        int a = Random.Range(0, 100);
        
        if (a <= PerGood)
        {
            GoodMole = true;
        }
        else
        {
            GoodMole = false;
        }
    }
    
    public void Open_Ing()
    {
        if (GoodMole == false)
        {
            GetComponent<Renderer>().material.mainTexture = Open_Images[Ani_Count];
        }
        else
        {
            GetComponent<Renderer>().material.mainTexture = Open_Images_Good[Ani_Count];
        }
        
        Ani_Count += 1;
        if (Ani_Count >= Open_Images.Length)
        {
            // Open 애니메이션 끝나는 시점
            Idle_On();
        }
    }

    public void Idle_On()
    {
        MS = MoleState.Idle;
        Ani_Count = 0;
    }
    
    public void Idle_Ing()
    {
        if (GoodMole == false)
        {
            GetComponent<Renderer>().material.mainTexture = Idle_Images[Ani_Count];
        }
        else
        {
            GetComponent<Renderer>().material.mainTexture = Idle_Images_Good[Ani_Count];
        }
        
        Ani_Count += 1;
        if (Ani_Count >= Idle_Images.Length)
        {
            // Idle 애니메이션 끝나는 시점
            Close_On();
        }
    }
    
    public void Close_On()
    {
        MS = MoleState.Close;
        Ani_Count = 0;
    }
    
    public void Close_Ing()
    {
        if (GoodMole == false)
        {
            GetComponent<Renderer>().material.mainTexture = Close_Images[Ani_Count];
        }
        else
        {
            GetComponent<Renderer>().material.mainTexture = Close_Images_Good[Ani_Count];
        }
        
        Ani_Count += 1;
        if (Ani_Count >= Close_Images.Length)
        {
            // Close 애니메이션 끝나는 시점
            StartCoroutine("Wait");
        }
    }
    
    public void Catch_On()
    {
        MS = MoleState.Catch;
        Ani_Count = 0;
        
        GetComponent<AudioSource>().clip = Catch_Sound;
        GetComponent<AudioSource>().Play();

    }
    
    public void Catch_Ing()
    {
        if (GoodMole == false)
        {
            GetComponent<Renderer>().material.mainTexture = Catch_Images[Ani_Count];
        }
        else
        {
            GetComponent<Renderer>().material.mainTexture = Catch_Images_Good[Ani_Count];
        }
        
        Ani_Count += 1;
        if (Ani_Count >= Catch_Images.Length)
        {
            StartCoroutine("Wait");
        }
    }
    
    public IEnumerator Wait()
    {
        MS = MoleState.None;
        Ani_Count = 0;
        float wait_Time = Random.Range(0.5f, 4.5f); //랜덤으로 시간 결정
        yield return new WaitForSeconds(wait_Time); //결정된 시간만큼 대기
        Open_On(); //Open_On() 실행
    }

    public void OnMouseDown()
    {
        if (MS == MoleState.Idle || MS == MoleState.Open)
        {
            Catch_On();

            if (GoodMole)
            {
                Debug.Log("Good");
                GameManager.instance.Count_Good++;
            }
            else
            {
                GameManager.instance.Count_Bed++;
            }
        }

        if (MS == MoleState.Close)
        {
            Debug.Log("Miss");
        }
    }
}