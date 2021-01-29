using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public float timer = 180;
    public GameObject[] players;
    public int[] target_list;
    class container_player{
        public int pid;
        public container_player(int i){
            pid = i;
        }
    }

    // Start is called before the first frame update
    void Awake(){
        if(gameManager == null){
            gameManager = this;
        }else{
            Destroy(gameManager.gameObject);
        }
        initiation();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                int rand_target = Random.Range(0,pl.Count);
                print("player "+players[i].GetComponent<Move>().playerId+"'s target is "+pl[rand_target].pid);
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
        
    }

}
