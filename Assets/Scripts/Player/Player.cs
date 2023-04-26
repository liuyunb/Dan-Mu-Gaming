using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _anim;

    private Audience _curAudience;

    public Quaternion originRotate;

    public Audience CurAudience
    {
        get => _curAudience;
        set => _curAudience = value;
    }

    private bool _isDance;

    private bool _isWalk;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _curAudience = new Audience()
        {
            player = this
        };
        originRotate = transform.rotation;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(_curAudience);
    }

    private void Update()
    {
        SetAnim();
    }

    public void SetAnim()
    {
        _anim.SetBool("Dance",_isDance);
        _anim.SetBool("Walk",_isWalk);
    }

    public void Dance(float num)
    {
        _isDance = true;
        _isWalk = false;
        _anim.SetFloat("DanceNum",num);
    }

    public void StopDance()
    {
        _isDance = false;
        _isWalk = false;
    }

    public void Walk()
    {
        _isWalk = true;
        _isDance = false;
    }
}
