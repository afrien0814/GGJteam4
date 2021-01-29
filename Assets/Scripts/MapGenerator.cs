using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.IO;
public class MapGenerator : MonoBehaviour
{
    class tile_info{

    }
    //List<string[]> map = new List<string[]>();
    //[Tooltip("每一個方格的大小(unity內部單位)")]
    //public Vector2 unit_scale;
    public List<Vector2> empty_grid = new List<Vector2>();
    public GameObject[] prefab_list;//public GameObject outer_map_obj;
    public int[] prefab_amount;
    //public string[] prefab_name;
    //public int width = 0;
    //public int length = 0;
    void Awake(){
        //read_map();
        //generate_map();
        //generate_random();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public GameObject red,green;
    void read_map(){
        // StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/map_test.csv");
        // try {
        //     string line;bool first = true;
        //     while ((line = sr.ReadLine()) != null)
        //     {
        //         string[] row = line.Split(',');
        //         if(first){
        //             width = row.Length;
        //             first = false;
        //         }
        //         if(row.Length == width){
        //             map.Add(row);
        //         }
        //     }
        //     length = map.Count;
        // }
        // catch (UnityException e)
        // {
        //     // Let the user know what went wrong.
        //     print("The file could not be read:");
        //     print(e.Message);
        // }
        Tilemap tilemap = FindObjectOfType<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);bool fisrt = true;
        Grid gr = tilemap.gameObject.GetComponentInParent<Grid>();
        
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(fisrt){
                    //print("x"+tilemap.CellToWorld(new Vector3Int(x,y,0)).x+"y"+tilemap.CellToWorld(new Vector3Int(x,y,0)).y+"x*y"+bounds.size.x*bounds.size.y);
                    print("!!!x"+tilemap.LocalToCell(new Vector3(0,0,0)).x+"y"+tilemap.LocalToCell(new Vector3(0,0,0)).y);
                    fisrt = !fisrt;
                    print(tile);
                }
                if (tile != null) {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    //print("x"+tilemap.CellToLocal(new Vector3Int(x,y,0)).x+"y"+tilemap.CellToLocal(new Vector3Int(x,y,0)).y+"x*y"+bounds.size.x*bounds.size.y);
                    //print("x"+tilemap.CellToLocal(+"y"+tilemap.CellToWorld(new Vector3Int(x,y,0)).y+"x*y"+bounds.size.x*bounds.size.y);
                    GameObject gm = Instantiate(red);
                    gm.transform.position = new Vector3(x*gr.cellSize.x,y*gr.cellSize.y,0);
                } else {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                    //print("x"+tilemap.CellToLocal(new Vector3Int(x,y,0)).x+"y"+tilemap.CellToLocal(new Vector3Int(x,y,0)).y+"x*y"+bounds.size.x*bounds.size.y);
                    empty_grid.Add(new Vector2(x,y));
                }
            }
        }
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                
            }
        }
    }
    void generate_map(){
        // i=第幾行,j=某行第幾個元素
        // for(int i=0;i < map.Count;i++){
        //     for(int j=0;j < map[i].Length;j++){
        //         for(int prefab_index=0;prefab_index<prefab_list.Length;prefab_index++){
        //             if((map[i])[j] == prefab_name[prefab_index]){
        //                 if((map[i])[j] != "0"){
        //                     GameObject new_pref = Instantiate(prefab_list[prefab_index]);
        //                     new_pref.transform.position = new Vector3(0,0,0)+new Vector3(i*unit_scale.x,j*unit_scale.y,0);
        //                 }
        //             }
        //         }
        //     }
        // }

    }
    void generate_random(){
        int rand;
        //i=哪個prefab,j=重複生成次數
        for(int i=0;i < prefab_list.Length;i++){
            for(int j=0;j < prefab_amount[i];j++){
                if(empty_grid.Count!=0){
                    rand = Random.Range(0,empty_grid.Count);
                    GameObject new_pref = Instantiate(prefab_list[i]);
                    new_pref.transform.position = new Vector3(empty_grid[rand].x,empty_grid[rand].y,0);
                }
            }
        }
    }
}

