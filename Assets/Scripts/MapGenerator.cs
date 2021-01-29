using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class MapGenerator : MonoBehaviour
{
    List<string[]> map = new List<string[]>();
    [Tooltip("每一個方格的大小(unity內部單位)")]
    public Vector2 unit_scale;
    public GameObject[] prefab_list;public GameObject outer_map_obj;
    public string[] prefab_name;
    public int width = 0;
    public int length = 0;
    void Start()
    {
        read_map();
        generate_map();
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
            // Let the user know what went wrong.
            print("The file could not be read:");
            print(e.Message);
        }
    }
    void generate_map(){
        // i=第幾行,j=某行第幾個元素
        for(int i=0;i < map.Count;i++){
            for(int j=0;j < map[i].Length;j++){
                for(int prefab_index=0;prefab_index<prefab_list.Length;prefab_index++){
                    if((map[i])[j] == prefab_name[prefab_index]){
                        if((map[i])[j] != "0"){
                            GameObject new_pref = Instantiate(prefab_list[prefab_index]);
                            new_pref.transform.position = new Vector3(0,0,0)+new Vector3(i*unit_scale.x,j*unit_scale.y,0);
                        }
                    }
                }
            }
        }
        generate_outer_wall();

    }
    void generate_outer_wall(){
        for(int i=0;i<0;i++){

        }
    }
}
