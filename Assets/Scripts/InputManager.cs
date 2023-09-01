using UnityEngine;
using System.Collections;

public struct KeyData
{
    public int frame;

    public bool isInput;
    public bool inputSkill1;
    public bool inputSkill2;

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


public class InputManager : MonoBehaviour {

    //MouseData[] m_syncedInputs = new MouseData[2]; //동기화된 입력값.
    //MouseData m_localInput; //현재 입력값(이 값을 송신시킨다).

    KeyData[] syncedKetInputs = new KeyData[2];
    KeyData localKeyInput;
    

    void FixedUpdate() 
    {
        localKeyInput.horizontal = Input.GetAxisRaw("Horizontal");
        localKeyInput.vertical = Input.GetAxisRaw("Vertical");

        localKeyInput.inputSkill1 = Input.GetKeyDown(KeyCode.Alpha1);
        localKeyInput.inputSkill2 = Input.GetKeyDown(KeyCode.Alpha2);

        if (localKeyInput.horizontal != 0 && localKeyInput.vertical != 0)
            localKeyInput.isInput = true;
        else
            localKeyInput.isInput = false;
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

