using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Ready,
    Play,
    End
}
public class GameManager : MonoBehaviour {
    public GameState GS; //게임매니져의 상태관리.
    public Hole[] Holes; //구멍 스크립트들(배열).
    public float LimitTime; //게임 제한시간.
    public Text TimeText; //게임 제한시간을 표기하기 위한 Text.
    public int Count_Bed; //나쁜두더지를 잡은 횟수.
    public int Count_Good; //착한 두더지를 잡은 횟수.
    public GameObject mainPanel;
    public GameObject FinishGUI; //결과화면을 보여주기 위한 오브젝트.
    public Text countBedText;
    public Text countGoodText;
    public Text Final_Count_Bed; //결과화면에서 나쁜 두더지를 잡은 숫자를 보여 줄 Text.
    public Text Final_Count_Good; //결과화면에서 착한 두더지를 잡은 숫자를 보여 줄 Text.
    public Text Final_Score; //결과화면에서 착한 두더지를 잡은 숫자를 보여 줄 Text.
    public AudioClip ReadySound; //레디...하는 경우에 플레이 할 사운드.
    public AudioClip GoSound; //고! 하 는경우에 플레이 할 사운드.
    public AudioClip FinishSound; //끝나고 결과화면이 나올 경우에 플레이 할 사운드.

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainPanel.SetActive(true);
        GetComponent<AudioSource>().clip = ReadySound;
        GetComponent<AudioSource>().Play();
    }
    
    public void GO(){
        GS = GameState.Play;

        for (int i = 0; i < Holes.Length; i++)
        {
            Holes[i].GetComponent<Hole>().StartCoroutine("Wait");
        }
        
        GetComponent<AudioSource>().clip = GoSound;
        GetComponent<AudioSource>().Play();
    }
    
    void Update()
    {
        if (GS == GameState.Play)
        {
            LimitTime -= Time.deltaTime;
            if (LimitTime <= 0)
            {
                LimitTime = 0;
                //게임이 끝나는 시점.
                End();
            }
        }
        TimeText.text = string.Format("{0:N2}", LimitTime);
        countBedText.text = Count_Bed.ToString();
        countGoodText.text = Count_Good.ToString();
    }
    
    void End(){
        GS = GameState.End;
        Final_Count_Bed.text = string.Format("{0}",Count_Bed);
        Final_Count_Good.text = string.Format("{0}",Count_Good);
        Final_Score.text = string.Format("{0}",Count_Bed*100-Count_Good*1000);
        FinishGUI.gameObject.SetActive(true);
        GetComponent<AudioSource>().clip = FinishSound;
        GetComponent<AudioSource>().Play();
    }

    public void OnStartBtn()
    {
        mainPanel.SetActive(false);
        GS = GameState.Play;
    }
}