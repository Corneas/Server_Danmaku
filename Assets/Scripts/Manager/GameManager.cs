using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private Player _player = null;

    public Player player => _player ??= FindObjectOfType<Player>();
}
