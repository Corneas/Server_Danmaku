using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseComponent : MonoBehaviour
{
    protected Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
}
