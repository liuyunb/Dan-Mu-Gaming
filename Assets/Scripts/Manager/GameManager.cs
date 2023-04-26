using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using LitJson;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    private List<Audience> _audiences = new List<Audience>();

    public PlayerData_SO playerData;
    private void Start()
    {
        StartCoroutine(GetDanMuCoroutine());
    }

    public void RegisterPlayer(Audience audience)
    {
        if(audience != null && !_audiences.Contains(audience))
            _audiences.Add(audience);
    }

    public void UnRegisterPlayer(Audience audience)
    {
        if (audience != null)
            _audiences.Remove(audience);
    }

    IEnumerator GetDanMuCoroutine()
    {
        GetDanMu getDanMu = new GetDanMu();
        string sendTime=string.Empty;
        getDanMu.SetRequest();
        string t0 = getDanMu.Response();
        DOb ob0 = JsonMapper.ToObject<DOb>(t0);
        sendTime = ob0.data.room[ob0.data.room.Count - 1].timeline;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            getDanMu.SetRequest();
            string t = getDanMu.Response();
            DOb ob = JsonMapper.ToObject<DOb>(t);
            int index = ob.data.room.Count - 1;
            if (sendTime == ob.data.room[index].timeline)
            {
                
            }
            else
            {
                sendTime = ob.data.room[index].timeline;
                string strs = ob.data.room[index].text;
                bool chinese = Regex.IsMatch(strs, @"^[\u4e00-\u9fa5]{2}$");
                bool move = Regex.IsMatch(strs, @"^[\u4e00-\u9fa5]{2} -?\d+,-?\d+$");
                bool dance = Regex.IsMatch(strs, @"^[\u4e00-\u9fa5]{2} -?\d+$");
                if (chinese)
                {
                    switch (strs)
                    {
                        case "跳舞":PlayerDance(ob.data.room[index],0);
                            break;
                        case "停止" : PlayerStopDance(ob.data.room[index]);
                            break;
                        case "创建" : CreatePlayer(ob.data.room[index]);
                            break;
                        default:Saying(ob.data.room[index]);
                            break;
                    }
                }
                else if (dance && strs.Substring(0, 2) == "跳舞")
                {
                    float num = 0;
                    float.TryParse(strs.Substring(3), out num);
                    PlayerDance(ob.data.room[index],num);
                }
                else if (move && strs.Substring(0, 2) == "移动")
                {
                    float x, y;
                    string str = strs.Substring(3);
                    var strPos = str.Split(',');
                    float.TryParse(strPos[0], out x);
                    float.TryParse(strPos[1], out y);
                    Vector3 pos = new Vector3(x, 0, y);
                    
                    PlayerMove(ob.data.room[index],pos);
                }
                else
                {
                    Saying(ob.data.room[index]);
                }
            }
        }
    }

    public void PlayerDance(Room room,float num)
    {
        var audience = FindAudience(room.uid);
        if (audience != null)
        {
            audience.player.Dance(num);
        }
    }
    
    public void PlayerStopDance(Room room)
    {
        var audience = FindAudience(room.uid);
        if (audience != null)
        {
            audience.player.StopDance();
        }
    }

    public void CreatePlayer(Room room)
    {
        if(FindAudience(room.uid) != null) return;
        float posX = Random.Range(-19, 9);
        float posY = Random.Range(3, 14);
        var pos = new Vector3(posX,0,posY);
        var playerNing = Instantiate(playerData.ningGuang, pos, Quaternion.identity);
        Instantiate(playerData.baChong, pos + new Vector3(2f,0,2f) , Quaternion.identity);
        //z 3 - 14 x 9 -19
        var audience = new Audience()
        {
            player = playerNing,
            uid = room.uid,
            nickname = room.nickname,
            text = room.text
        };
        playerNing.CurAudience = audience;
        _audiences.Add(audience);
    }

    public void PlayerMove(Room room,Vector3 pos)
    {
        var audience = FindAudience(room.uid);
        pos.x = Mathf.Clamp( pos.x,-19, 9);
        pos.z = Mathf.Clamp(pos.z,3, 14);
        if (audience != null)
        {
            audience.player.Walk();
            StartCoroutine(MoveToTarget(audience.player, pos));
        }
    }

    IEnumerator MoveToTarget(Player player,Vector3 pos)
    {
        while (Vector3.Distance(player.transform.position, pos) > 0.1f)
        {
            yield return null;
            var playerPos = player.transform.position;
            player.transform.position = Vector3.Lerp(playerPos, pos, Time.deltaTime * 0.5f);
            player.transform.forward = pos - playerPos;
        }
        player.StopDance();
        player.transform.rotation = player.originRotate;
    }

    public void Saying(Room room)
    {
        var audience = FindAudience(room.uid);
        if (audience != null)
        {
            audience.player.GetComponent<DialogBar>().CreateDialog(room.text);
        }
    }
    
    public Audience FindAudience(int uid)
    {
        foreach (var audience in _audiences)
        {
            if (audience.uid == uid)
                return audience;
        }

        return null;
    }
}
