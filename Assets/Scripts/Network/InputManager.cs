using UnityEngine;
using System.Collections;

public struct KeyData
{
    public int frame;

    public bool isDamaged;
    public bool isDead;
    public bool inputSpace;     // 기본 총알 발사
    public bool inputShift;     // 이동속도 감소
    public bool inputSkill1;    // 1번스킬
    public bool inputSkill2;    // 2번스킬

    public float horizontal;
    public float vertical;

    public override string ToString()
    {
        string str = "";
        str += "frame:" + frame;
        str += "horizontal : " + horizontal;
        str += "vertical : " + vertical;
        return str;
    }

};

public struct InputData
{   
	public int 			count;		// 데이터 수. 
	public int			flag;		// 접속 종료 플래그.
    public KeyData[] keyData;       // 키 입력 정보
};


public class InputManager : MonoSingleton<InputManager> {

    //MouseData[] m_syncedInputs = new MouseData[2]; //동기화된 입력값.
    //MouseData m_localInput; //현재 입력값(이 값을 송신시킨다).

    KeyData[] syncedKetInputs = new KeyData[2];
    KeyData localKeyInput;


    //void FixedUpdate() 
    private void Update()
    {
        localKeyInput.horizontal = Input.GetAxisRaw("Horizontal");
        localKeyInput.vertical = Input.GetAxisRaw("Vertical");

        localKeyInput.inputSpace = Input.GetKey(KeyCode.Space);
        localKeyInput.inputSkill1 = Input.GetKey(KeyCode.Alpha1);
        localKeyInput.inputSkill2 = Input.GetKey(KeyCode.Alpha2);

        localKeyInput.inputShift = Input.GetKey(KeyCode.LeftShift);

        //if (localKeyInput.horizontal != 0 || localKeyInput.vertical != 0)
        //    localKeyInput.isInput = true;
        //else
        //    localKeyInput.isInput = false;
    }

    public KeyData GetLocalKeyData()
    {
        return localKeyInput;
    }

    public KeyData GetKeyData(int id)
    {
        return syncedKetInputs[id];
    }

    public void SetInputData(int id, KeyData data)
    {
        syncedKetInputs[id] = data;
    }
}

