using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiColliderDrift : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.GetChild(0).transform.position = gameObject.transform.position;
    }
}
