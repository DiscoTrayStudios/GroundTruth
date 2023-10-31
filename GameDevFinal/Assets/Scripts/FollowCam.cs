using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject sprite;
    public float boundaryPercent;
    public float easing;
    private float lBound;
    private float rBound;
    private float uBound;
    private float dBound;

    private float size;

    void Start() {
        lBound = boundaryPercent * Camera.main.pixelWidth;
        rBound = Camera.main.pixelWidth - lBound;
        dBound = boundaryPercent * Camera.main.pixelHeight;
        uBound = Camera.main.pixelHeight - dBound;
        size = gameObject.GetComponent<Camera>().orthographicSize;
    }

    void FixedUpdate() {
        if (sprite & !GameManager.Instance.GetPlayerBusy())
        {
            Vector3 spriteLoc = Camera.main.WorldToScreenPoint(sprite.transform.position);
            Vector3 pos = transform.position;

            if (spriteLoc.x < lBound)
            {
                pos.x -= lBound - spriteLoc.x;
            }
            else if (spriteLoc.x > rBound)
            {
                pos.x += spriteLoc.x - rBound;
            }

            if (spriteLoc.y < dBound)
            {
                pos.y -= dBound - spriteLoc.y;
            }
            else if (spriteLoc.y > uBound)
            {
                pos.y += spriteLoc.y - uBound;
            }

            pos = Vector3.Lerp(transform.position, pos, easing);

            transform.position = pos;
        }
    }
    
    void Update() {
        
    }
}
