using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //public GameObject m_stagePrefabs;   // 스테이지를 등록해 둔다.


    enum State
    {
        GameIn,     //게임 시작 준비.
        Game,       //게임 중.
        GameChanging,//종료직전 연출.
        GameOut,    //게임 종료 준비.
        GameEnd,    //게임 종료.
    };
    State m_state;



    // Start is called before the first frame update
    void Start()
    {
        m_state = State.GameIn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (m_state)
        {
            case State.GameIn:
                UpdateGameIn();
                break;
            case State.Game:
                UpdateGame();
                break;
            case State.GameChanging:
                //UpdateGameChanging();
                break;
            case State.GameOut:
                //UpdateGameOut();
                break;
            case State.GameEnd:
                //UpdateGameEnd();
                break;
        }
    }

    //게임 시작 준비.
    void UpdateGameIn()
    {
        //스테이지 구축.
        /*
        GameObject stage = GameObject.Find("Stage");
        if (stage == null)
        {
            stage = Instantiate(m_stagePrefabs);
            stage.name = "Stage";
            Debug.Log("스테이지출현");

            return;
        }
        */
        //게임 시작으로 전환.
        m_state = State.Game;

        //발사할 수 있게 합니다.
        GameObject[] bars = GameObject.FindGameObjectsWithTag("Bar");
        foreach (GameObject obj in bars)
        {
            BarScript bar = obj.GetComponent<BarScript>();
            bar.SetShotEnable(true);       //발사기능 OFF.
        }
    }


    //게임 중.
    void UpdateGame()
    {

    }


    //스테이지를 바꾸는 연출..
    void UpdateGameChanging()
    {

    }


    //게임 종료 준비.
    void UpdateGameOut()
    {

    }



    //게임 종료라면 true.
    public bool IsEnd()
    {
        return (m_state == State.GameEnd);
    }

}

