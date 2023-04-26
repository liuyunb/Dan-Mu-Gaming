using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogBar : MonoBehaviour
{
    private Camera _cam;

    private Canvas _dialogCanvas;

    public Transform dialogPos;

    private Transform _dialogBar;

    public bool isCreate;

    private void Start()
    {
        _dialogCanvas = GameObject.FindWithTag("NameCanvas").GetComponent<Canvas>();
        _cam = Camera.main;
        
    }

    public void CreateDialog(string text)
    {
        if (isCreate && _dialogBar != null)
        {
            _dialogBar.GetComponentInChildren<TextMeshProUGUI>().text = text;
            CancelInvoke("DestoryBar");
            Invoke("DestoryBar",10f);
        }
        else
        {
            _dialogBar = Instantiate(GameManager.Instance.playerData.dialogBar, _dialogCanvas.transform).transform;
            _dialogBar.position = dialogPos.position;
            _dialogBar.forward = _cam.transform.forward;
            _dialogBar.GetComponentInChildren<TextMeshProUGUI>().text = text;
            Invoke("DestoryBar",10f);
            isCreate = true;
        }
    }

    public void DestoryBar()
    {
        Destroy(_dialogBar.gameObject);
        isCreate = false;
    }

    private void LateUpdate()
    {
        if (_cam != null && _dialogBar != null)
        {
            _dialogBar.position = dialogPos.position;
            _dialogBar.forward = -_cam.transform.forward;
        }
    }
}
