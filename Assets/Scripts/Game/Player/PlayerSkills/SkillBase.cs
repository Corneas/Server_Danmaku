using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected Player player = null;

    [SerializeField]
    protected GameObject bulletPrefab = null;

    public void SetPlayer(Player _player)
    {
        player = _player;
    }

    public abstract void Fire();
}
