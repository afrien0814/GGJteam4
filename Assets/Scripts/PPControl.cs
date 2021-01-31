using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPControl : MonoBehaviour
{
    public PostProcessProfile ppp ;
    public bool start_gray = false;
    ColorGrading cgd;
    public float gray_timer;float max_t;
    // Start is called before the first frame update
    void Start()
    {
        cgd = ppp.GetSetting<ColorGrading>();
        max_t = gray_timer;
        
    }

    // Update is called once per frame
    void Update()
    {
        turn_to_gray();
    }

    public void turn_to_gray(){
        if(start_gray ){
            if(gray_timer>0){
                gray_timer -= Time.deltaTime;
                float progress = (max_t-gray_timer)/max_t;
                cgd.mixerBlueOutBlueIn.value = 100 - 50*progress;
                cgd.mixerBlueOutGreenIn.value = 50*progress;
                cgd.mixerBlueOutRedIn.value = 50*progress;

                cgd.mixerGreenOutBlueIn.value = 50*progress;
                cgd.mixerGreenOutGreenIn.value = 100 - 50*progress;
                cgd.mixerGreenOutRedIn.value = 50*progress;

                cgd.mixerRedOutBlueIn.value = 50*progress;
                cgd.mixerRedOutGreenIn.value = 50*progress;
                cgd.mixerRedOutRedIn.value = 100 - 50*progress;
            }else{
                start_gray = false;
            }
        }
    }

    public void return_normal_color(){
        cgd.mixerBlueOutBlueIn.value = 100;
        cgd.mixerBlueOutGreenIn.value = 0;
        cgd.mixerBlueOutRedIn.value = 0;

        cgd.mixerGreenOutBlueIn.value = 0;
        cgd.mixerGreenOutGreenIn.value = 100;
        cgd.mixerGreenOutRedIn.value = 0;

        cgd.mixerRedOutBlueIn.value = 0;
        cgd.mixerRedOutGreenIn.value = 0;
        cgd.mixerRedOutRedIn.value = 100;
    }
}
