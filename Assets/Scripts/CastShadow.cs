using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastShadow : MonoBehaviour
{
    public float range;
    public float degree;
    public LayerMask layer;
    public Image test;
    public Camera cam;
   

    RaycastHit2D hit;

    Texture2D tmp;
    Vector2 pixel_pos = new Vector2(0,0);
    private void Update()
    {
        tmp = new Texture2D(960, 540);
        for (int i=0; i<360 / degree; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(degree * i), Mathf.Sin(degree * i));
            hit = Physics2D.Raycast(this.transform.position, dir, range, layer, 0, 0);

            pixel_pos = cam.WorldToScreenPoint(this.transform.position) / 100;
            for(int j=0; j<5; j++)
            {
                for(int k=0; k<5; k++)
                {
                    tmp.SetPixel((int)pixel_pos.x + k, (int)pixel_pos.y + j, new Color(0, 0, 0, 0));
                }
            }

            /*
            if(hit)
            {
                for(int j=0; j<960;j++)
                {
                    pixel_pos = cam.WorldToScreenPoint(hit.point + dir * j) / 100 + new Vector3(480, 270);
                    if (pixel_pos.x > tmp.width || pixel_pos.y > tmp.height)
                    {
                        break;
                    }
                    tmp.SetPixel((int)pixel_pos.x, (int)pixel_pos.y, new Color(0,0,0,0));
                }
                Debug.DrawRay(hit.point, dir * range, Color.yellow);
            }
            */
        }

        tmp.Apply();
        test.sprite = Sprite.Create(tmp,new Rect(0.0f, 0.0f, tmp.width, tmp.height), new Vector2(0.5f, 0.5f));
    }
}
