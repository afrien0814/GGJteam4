using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Drawing;
using System.IO;
public class MapGenerator : MonoBehaviour
{
    public static MapGenerator mapGenerator;
    public List<string[]> map = new List<string[]>();
    [Tooltip("每一個方格的大小(unity內部單位)")]
    public Vector2 unit_scale;
    public GameObject[] prefab_list;public string[] prefab_name;
    public List<Vector2> free_position_list = new List<Vector2>();
    public int width,length;

    public GameObject[] item_list;
    void Awake(){
        if(mapGenerator == null){
            mapGenerator = this;
        }else{
            Destroy(this.gameObject);
        }   
        // GameManager.gameManager.map_name;
        read_map(/*GameManager.gameManager.map_name*/"map3.csv");
        generate_map();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void read_map(string map_name){
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + map_name);
        try {
            string line;bool first = true;
            while ((line = sr.ReadLine()) != null)
            {
                string[] row = line.Split(',');
                if(first){
                    width = row.Length;
                    first = false;
                }
                if(row.Length == width){
                    map.Add(row);
                }
            }
            length = map.Count;
        }
        catch (UnityException e)
        {
            print("The file could not be read:");
            print(e.Message);
        }
    }
    
    void generate_map(){
        //i=第幾行,j=某行第幾個元素
        for(int i=0;i < map.Count;i++){
            for(int j=0;j < map[i].Length;j++){
                for(int prefab_index=0;prefab_index<prefab_list.Length;prefab_index++){
                    if((map[i])[j] == prefab_name[prefab_index]){
                        if((map[i])[j] != prefab_name[0]){//prefab_name[0] => blank
                            GameObject new_pref = Instantiate(prefab_list[prefab_index]);
                            new_pref.transform.position = new Vector3(0,0,0)+new Vector3(j*unit_scale.y,i*unit_scale.x,0);
                        }else{
                            free_position_list.Add(new Vector2(j*unit_scale.y,i*unit_scale.x));
                        }
                    }
                }
            }
        }
    }
    void generate_random(){
        // int rand;
        // //i=哪個prefab,j=重複生成次數
        // for(int i=0;i < prefab_list.Length;i++){
        //     for(int j=0;j < prefab_amount[i];j++){
        //         if(empty_grid.Count!=0){
        //             rand = Random.Range(0,empty_grid.Count);
        //             GameObject new_pref = Instantiate(prefab_list[i]);
        //             new_pref.transform.position = new Vector3(empty_grid[rand].x,empty_grid[rand].y,0);
        //         }
        //     }
        // }
    }
    public GameObject red , detector;
    public  void get_free_coordinate_list(){
        print(map[0].Length + " / " + map.Count);
        for (int x = 0; x < map[0].Length; x++) {
            for (int y = 0; y < map.Count; y++) {
                //Ray myRay =  new Ray(new Vector3(unit_scale.x*x,unit_scale.y*y,-5),Vector3.back);
                //Ray2D myRay = new Ray2D(new Vector2(unit_scale.x*x,unit_scale.y*y),new Vector2(100,100));
                //Ray myRay = Camera.main.ScreenPointToRay(new Vector3(unit_scale.x*x,unit_scale.y*y,-2));
                //print("ray  "+myRay.origin + " /dir " + myRay.direction);
                //RaycastHit2D hit = Physics2D.Raycast(myRay.origin, myRay.direction, 10, -1);
                //RaycastHit2D[] hit = Physics2D.RaycastAll(myRay.origin, myRay.direction);
                //if (hit.Length != 0)
                //{
                    //print(new Vector3(unit_scale.x*x,unit_scale.y*y,0).ToString()+"has object"+hit[0].collider.gameObject.activeSelf);
                    //GameObject gm = Instantiate(red);
                    //gm.transform.position = new Vector3(unit_scale.x*x,unit_scale.y*y,0);
                //}

            }
        }
    }

}

