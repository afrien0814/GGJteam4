using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Drawing;
using System.IO;
using System;
public class MapGenerator : MonoBehaviour
{
    public class USER_DEFINE_DATA{
        public bool use_user_define_map;
        public string map_name;
        public string item_amounts;
        public float refill_item_time;
    }
    public USER_DEFINE_DATA user_define_data;
    public static MapGenerator mapGenerator;
    public List<string[]> map = new List<string[]>();
    [Tooltip("每一個方格的大小(unity內部單位)")]
    public Vector2 unit_scale;
    [Tooltip("prefab_list內放置牆壁prefab")]
    public GameObject[] prefab_list;
    [Tooltip("牆壁prefab 在的灰階：0～255")]
    public string[] prefab_name;
    [Tooltip("沒有放牆壁的格子")]
    public List<Vector2> free_position_list = new List<Vector2>();
    [Tooltip("讀取地圖時取得的長度及寬度")]
    public int width,length;
    [Tooltip("生成物品prefab列表")]
    public GameObject[] item_prefab_list;
    [Tooltip("生成物件的清單")]
    public List<GameObject> generated_item_list = new List<GameObject>();
    [Tooltip("生成物件的預定數量")]
    public int[] generated_items_amount;
    [Tooltip("重新填滿物件清單的間隔時間")]
    public float refill_timer; float refill_timer_max;
    [Tooltip("有幾張地圖可選?")]
    public int max_amount;
    [Tooltip("指定載入之地圖編號，指定-1為隨機選取")]
    //public int specify_map_id = -1;
    public string specify_map_name = "none";
    public GameObject outer_wall_prefab;
    void Awake(){
        if(mapGenerator == null){
            mapGenerator = this;
        }else{
            Destroy(this.gameObject);
        }   
        // GameManager.gameManager.map_name;
        refill_timer_max = refill_timer;
        
        load_user_define_data();
        if(specify_map_name == "none"){
            read_map(/*GameManager.gameManager.map_name*/"map"+(UnityEngine.Random.Range(0,max_amount) +1 )+".csv");
        }else{
            read_map(/*GameManager.gameManager.map_name*/specify_map_name+".csv");
        }
        
        // data test
        // USER_DEFINE_DATA udd = new USER_DEFINE_DATA();
        // udd.use_user_define_map = false;
        // udd.map_name = "user_map";
        // udd.item_amounts = "4,6,7,4";
        // string jsonInfo = JsonUtility.ToJson(udd,true);
        // File.WriteAllText(Application.streamingAssetsPath+"/user_data.txt", jsonInfo);
        //把字串轉換成user_define_data物件
        
    }
    void Start()
    {
        generate_map();
    }

    void Update()
    {
        refill_map_item();
    }
    void load_user_define_data(){
        user_define_data = JsonUtility.FromJson<USER_DEFINE_DATA>(File.ReadAllText(Application.streamingAssetsPath+"/user_data.txt"));
        if(user_define_data.use_user_define_map){
            specify_map_name = user_define_data.map_name;
            refill_timer = user_define_data.refill_item_time;//Int32.Parse(input)
            string[] strs = user_define_data.item_amounts.Split(',');
            for (int i = 0; i < item_prefab_list.Length; i++)
            {
                generated_items_amount[i]= Int32.Parse(strs[i]);
            }
        }
        // print(user_define_data.item_amounts);
        // print(user_define_data.use_user_define_map);
        // print(user_define_data.map_name);
        // print(user_define_data.refill_item_time);
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
                            new_pref.transform.SetParent(this.gameObject.transform);
                        }else{
                            free_position_list.Add(new Vector2(j*unit_scale.y,i*unit_scale.x));
                        }
                    }
                }
            }
        }
        //outer wall thickness up //
        int thickness = 2;
        for(int k=0;k < thickness;k++){
            for(int i = 0 - thickness;i<map.Count +thickness;i++){
                GameObject new_pref = Instantiate(outer_wall_prefab);
                new_pref.transform.position = new Vector3(0,0,0)+new Vector3((-k-1)*unit_scale.y,(i)*unit_scale.x,0);
                new_pref.transform.SetParent(this.gameObject.transform);
                new_pref = Instantiate(outer_wall_prefab);
                new_pref.transform.position = new Vector3(0,0,0)+new Vector3((map[0].Length+k)*unit_scale.y,(i)*unit_scale.x,0);
                new_pref.transform.SetParent(this.gameObject.transform);
            }
        }
        for(int k=0;k < thickness;k++){
            for(int i = 0;i<map[0].Length;i++){
                GameObject new_pref = Instantiate(outer_wall_prefab);
                new_pref.transform.position = new Vector3(0,0,0)+new Vector3((i)*unit_scale.y,(-k-1)*unit_scale.x,0);
                new_pref.transform.SetParent(this.gameObject.transform);
                new_pref = Instantiate(outer_wall_prefab);
                new_pref.transform.position = new Vector3(0,0,0)+new Vector3((i)*unit_scale.y,(map.Count+k)*unit_scale.x,0);
                new_pref.transform.SetParent(this.gameObject.transform);
            }
        }
    }
    public bool has_generated_item =false;
    public void init_generate_items(){
        for (int i = 0; i < generated_items_amount.Length; i++)
        {
            for (int j = 0; j < generated_items_amount[i]; j++)
            {
                generate_random_element(item_prefab_list[i]);
            }
        }
        has_generated_item = true;
    }
    int index;
    void refill_map_item(){
        if(refill_timer <= 0 && has_generated_item){
            refill_timer = refill_timer_max;
            for (int i = 0; i < generated_items_amount.Length; i++)
            {
                for (int j = 0; j < generated_items_amount[i]; j++)
                {
                    index = 0;
                    for (int k = 0; k < i; k++)
                    {
                        index += generated_items_amount[k];
                    }
                    index += j;
                    if(generated_item_list[index] == null){
                        generate_random_element(item_prefab_list[i]);
                    }
                }
            }
        }else{
            refill_timer -= Time.deltaTime;
        }
        
    }

    public void generate_random_element(GameObject gene_item){
        int rand = UnityEngine.Random.Range(0,free_position_list.Count);
        bool check;int iter = 0;
        Vector3 gene_pos;
        do{
            gene_pos = new Vector3(free_position_list[rand].x,free_position_list[rand].y,0);
            check = true;
            iter++;
            for (int i = 0; i < GameManager.gameManager.players.Length; i++)
            {
                if((GameManager.gameManager.players[i].transform.position-gene_pos).magnitude <= GameManager.gameManager.minimum_radial_distance){
                    check = false;
                }
            }
            for (int i = 0; i < generated_item_list.Count; i++)
            {
                if(generated_item_list[i] != null){
                    if((generated_item_list[i].transform.position-gene_pos).magnitude <= GameManager.gameManager.minimum_radial_distance){
                        check = false;
                    }
                }
            }
        }while(!check && iter <500 ); // max => 500 times
        
        GameObject new_pref = Instantiate(gene_item);
        new_pref.transform.position = gene_pos;
        generated_item_list.Add(new_pref);
    }
    // public GameObject red , detector;
    // public  void get_free_coordinate_list(){
    //     print(map[0].Length + " / " + map.Count);
    //     for (int x = 0; x < map[0].Length; x++) {
    //         for (int y = 0; y < map.Count; y++) {
    //             //Ray myRay =  new Ray(new Vector3(unit_scale.x*x,unit_scale.y*y,-5),Vector3.back);
    //             //Ray2D myRay = new Ray2D(new Vector2(unit_scale.x*x,unit_scale.y*y),new Vector2(100,100));
    //             //Ray myRay = Camera.main.ScreenPointToRay(new Vector3(unit_scale.x*x,unit_scale.y*y,-2));
    //             //print("ray  "+myRay.origin + " /dir " + myRay.direction);
    //             //RaycastHit2D hit = Physics2D.Raycast(myRay.origin, myRay.direction, 10, -1);
    //             //RaycastHit2D[] hit = Physics2D.RaycastAll(myRay.origin, myRay.direction);
    //             //if (hit.Length != 0)
    //             //{
    //                 //print(new Vector3(unit_scale.x*x,unit_scale.y*y,0).ToString()+"has object"+hit[0].collider.gameObject.activeSelf);
    //                 //GameObject gm = Instantiate(red);
    //                 //gm.transform.position = new Vector3(unit_scale.x*x,unit_scale.y*y,0);
    //             //}
    //         }
    //     }
    // }
    public Vector3 world_position_to_grid_index(Vector3 position){
        return new Vector3(sub_get_grid_num_x(0,position.x,unit_scale.x),sub_get_grid_num_y(0,position.y,unit_scale.y),0);
    }
    int sub_get_grid_num_y(float deviation,float position,float scale){// for a specified dimansion
        float min = 9999;int int_index = -1; //deviation => deviation from origin(world)
        for (int i = 0; i < map.Count; i++) //position => world position
        {                                   // scale => unit_scale
            if( min > Mathf.Abs(position - (scale*i + deviation)) ){ 
                min = Mathf.Abs(position - (scale*i + deviation));
                int_index = i;
            }
        }
        return int_index;
    }
    int sub_get_grid_num_x(float deviation,float position,float scale){// for a specified dimansion
        float min = 9999;int int_index = -1; //deviation => deviation from origin(world)
        for (int i = 0; i < map[0].Length; i++) //position => world position
        {                                   // scale => unit_scale
            if( min > Mathf.Abs(position - (scale*i + deviation)) ){ 
                min = Mathf.Abs(position - (scale*i + deviation));
                int_index = i;
            }
        }
        return int_index;
    }
}

