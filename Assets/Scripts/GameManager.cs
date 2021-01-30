using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public Action<string> callUI;
    public float countdownTimer = 3;
    public int timer;
    public bool gaming;
    public GameObject[] players;
    public int[] target_list;
    private bool loadTutorial = true;
    public int winnerId;
    class container_player{
        public int pid;
        public container_player(int i){
            pid = i;
        }
    }

    // Start is called before the first frame update
    void Awake(){
        if(GameManager.gameManager == null){
            GameManager.gameManager = this;
        }else{
            Destroy(gameObject);
        }
        loadTutorial = true;
    }
    void Start(){
        initiation();
        set_spawn_location();
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
        
        winnerId = -1;
        callUI?.Invoke("Timer");
        for (timer = (int)countdownTimer; timer > -2; timer--) yield return new WaitForSeconds(1f);
        gaming = true;
    }

   

    [Tooltip("if distance of sapwn position between two player is less than this number ,we will reset the location . ")]
    public float minimum_radial_distance = 5;

    void set_spawn_location(){
        for (int i = 0; i < players.Length; i++)
        {
            bool is_crowded = false;
            do{
                int rand = UnityEngine.Random.Range(0,MapGenerator.mapGenerator.free_position_list.Count);
                players[i].transform.position = MapGenerator.mapGenerator.free_position_list[rand];
                is_crowded = false;
                for (int l = 0; l < i; l++)
                {
                    if(Mathf.Abs((players[i].transform.position-players[l].transform.position).magnitude) <= minimum_radial_distance){
                        is_crowded = true;
                    }
                }
                print("again!!!");
            }while(is_crowded);

        }
    }



    ///////////////////////////////////////////////////////////
    // DO NOT LOOK INTO IT YOU PROBABLY WILL DIE FOR THIS
    ///////////////////////////////////////////////////////////

    void initiation(){
        List<container_player> pl = new List<container_player>();
        target_list = new int[4];
        bool ok = false;
        while(!ok){
            for(int i=0;i<players.Length;i++){
                pl.Add(new container_player(players[i].GetComponent<Move>().playerId));
                print(pl[i].pid);
            }
            for(int i=0;i<players.Length;i++){
                int rand_target = UnityEngine.Random.Range(0,pl.Count);
                print("player "+players[i].GetComponent<Move>().playerId+"'s target is "+pl[rand_target].pid);
                players[i].GetComponent<Move>().targetId = pl[rand_target].pid;
                target_list[i] = pl[rand_target].pid;
                pl.Remove(pl[rand_target]);
            }
            bool check = false;
            for(int i=0;i<players.Length;i++){
                if(players[i].GetComponent<Move>().playerId == target_list[i]){
                    check=true;
                }
            }
            if(check == false){
                ok = true;
            }
        }
    }
    public void win(int winner){
        gaming = false;
        winnerId = winner;
        callUI?.Invoke("GameOver");
    }

}
