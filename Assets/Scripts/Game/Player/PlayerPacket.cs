using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamagePacket
{
    public int frame;

    public bool isDamaged;

    public override string ToString()
    {
        string str = "";
        str += "frame:" + frame;
        str += "isDamaged:" + isDamaged;
        return str;
    }

};

public class PlayerPacket : MonoBehaviour
{
    KeyData[] syncedKetInputs = new KeyData[2];
    KeyData localKeyInput;

    public KeyData GetLocalPlayerData()
    {
        return localKeyInput;
    }

    public KeyData GetPlayerData(int id)
    {
        return syncedKetInputs[id];
    }

    public void SetPlayerData(int id, KeyData data)
    {
        syncedKetInputs[id] = data;
    }
}
