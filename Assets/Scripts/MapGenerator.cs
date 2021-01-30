using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Drawing;
using System.IO;
public class MapGenerator : MonoBehaviour
{
    List<string[]> map = new List<string[]>();
    [Tooltip("每一個方格的大小(unity內部單位)")]
    public Vector2 unit_scale;
    public GameObject[] prefab_list;public string[] prefab_name;
    public GameObject outer_map_obj;
    public int width,length;
    void Awake(){
        read_map();
        generate_map();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void read_map(){
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/map_test.csv");
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
                        if((map[i])[j] != "255"){
                            GameObject new_pref = Instantiate(prefab_list[prefab_index]);
                            new_pref.transform.position = new Vector3(0,0,0)+new Vector3(j*unit_scale.y,i*unit_scale.x,0);
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
    // void get_free_coordinate_list(){
    //     for (int x = 0; x < bounds.size.x; x++) {
    //         for (int y = 0; y < bounds.size.y; y++) {
    //             Vector3 pos = new Vector3((offset.x+lower_wall.x + x)*cellsize.x,(offset.y+lower_wall.y + y)*cellsize.y,-5f);
    //             Ray myRay =  Camera.main.ScreenPointToRay(pos);
    //             RaycastHit2D hit = Physics2D.Raycast(myRay.origin, myRay.direction, 10, 2);
    //             if (hit.collider)
    //             {
    //                 print(pos.ToString()+"has object"+hit.collider.gameObject.activeSelf);
    //                 GameObject gm = Instantiate(red);
    //                 gm.transform.position = pos;
    //             }
    //         }
    //     }
    // }

}

