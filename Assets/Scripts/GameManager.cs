using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager gameManager;
    public Action<string> callUI;
    public Action<int, int> itemUpdate;
    public Action callInit;
    public float countdownTimer = 3;
    [HideInInspector]
    public int timer;
    public bool gaming;
    public Move[] players = new Move[4];
    [HideInInspector]
    public int[] target_list, chaser_list, item_list;
    private bool loadTutorial = true;
    public int winnerId;

    // Start is called before the first frame update
    void Awake(){
        if(GameManager.gameManager == null){
            GameManager.gameManager = this;
        }else{
            Destroy(gameObject);
        }
        loadTutorial = true;
    }

    public void LoadGame()
    {
        if (loadTutorial)
        {
            callUI?.Invoke("HowToPlay");
            loadTutorial = false;
        }
        else StartCoroutine(GameStart());
    }

    public IEnumerator GameStart()
    {
        initiation();
        set_spawn_location();
        item_list = new int[4];
        yield return new WaitForSeconds(0.1f);
        winnerId = -1;
        callUI?.Invoke("Timer");
        callUI?.Invoke("PlayerUI");
        callInit?.Invoke();
        for (timer = (int)countdownTimer; timer > -2; timer--) yield return new WaitForSeconds(1f);
        MapGenerator.mapGenerator.init_generate_items();
        gaming = true;
    }

   public void ItemManage(int pId, int iId)
    {
        //Debug.Log("player " + pId + " gets " + iId + (Move.item_type)iId);
        item_list[pId - 1] = iId;
        itemUpdate?.Invoke(pId, iId);
    }

    [Tooltip("if distance of sapwn position between two player is less than this number ,we will reset the location . ")]
    public float minimum_radial_distance = 5;

    void set_spawn_location(){
        print("set_spawn_location");
        for (int i = 0; i < players.Length; i++)
        {
            bool is_crowded = false;
            do{
                int rand = UnityEngine.Random.Range(0,MapGenerator.mapGenerator.free_position_list.Count);
                players[i].transform.position = new Vector3(MapGenerator.mapGenerator.free_position_list[rand].x,MapGenerator.mapGenerator.free_position_list[rand].y,0);
                //print(MapGenerator.mapGenerator.free_position_list[rand]);
                //print(players[i].transform.position);
                is_crowded = false;
                for (int l = 0; l < i; l++)
                {
                    if(Mathf.Abs((players[i].gameObject.transform.position-players[l].transform.position).magnitude) <= minimum_radial_distance){
                        is_crowded = true;
                    }
                }
                //print("again!!!");
            }while(is_crowded);

        }
    }

    void initiation()
    {
        List<int> ps = new List<int>(), pool = new List<int>();
        target_list = new int[4];chaser_list = new int[4];
        for (int i = 0; i < 4; i++) pool.Add(i);
        for(int i = 0; i < 4; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count - 1);
            ps.Add(pool[index]);
            pool.Remove(pool[index]);
        }
        //Debug.Log("" + ps[0] + ps[1] + ps[2] + ps[3]);
        for (int i = 0; i < 4; i++)
        {
            players[ps[i]].targetId = target_list[ps[i]] = ps[(i + 1) % 4] + 1;
            players[ps[i]].chaserId = chaser_list[ps[i]] = ps[(i + 3) % 4] + 1;
            //Debug.Log("player " + (ps[i] + 1) + "'s target is " + target_list[ps[i]] + ", and chaser is " + chaser_list[ps[i]]);
        }   

    }

    public void win(int winner){
        gaming = false;
        winnerId = winner;
        callUI?.Invoke("GameOver");
    }

}
