using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "playerData",menuName = "Player/PlayerData")]
public class PlayerData_SO : ScriptableObject
{
    [Header("凝光")]
    public Player ningGuang;

    [Header("八重神子")] 
    public Player baChong;

    [Header("头框")] 
    public GameObject nameBar;

    [Header("对话框")] public GameObject dialogBar;

}
