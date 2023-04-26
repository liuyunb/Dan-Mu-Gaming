using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NamaBar : MonoBehaviour
{
    private Camera _cam;

    private Canvas _nameCanvas;

    public Transform namePos;

    private Transform _nameBar;

    private void Start()
    {
        _nameCanvas = GameObject.FindWithTag("NameCanvas").GetComponent<Canvas>();
        _cam = Camera.main;
        _nameBar = Instantiate(GameManager.Instance.playerData.nameBar, _nameCanvas.transform).transform;
        _nameBar.position = namePos.position;
        _nameBar.forward = _cam.transform.forward;
        _nameBar.GetComponent<TextMeshProUGUI>().text = GetComponent<Player>().CurAudience.nickname;
    }

    private void LateUpdate()
    {
        if (_cam != null && _nameBar != null)
        {
            _nameBar.position = namePos.position;
            _nameBar.forward = _cam.transform.forward;
        }
    }
}
