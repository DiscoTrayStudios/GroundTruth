using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{   

    private float camerasize;

    private Vector3 camerapos;

    private float zoomsize = 3;
    private bool followCam = false;

    public void ZoomIn(Vector3 position){
        Vector3 zoompos = new Vector3(position.x, position.y + .8f, transform.position.z);
        print(zoompos);
        camerapos = transform.position;
        StartCoroutine(ZoomToPos(zoomsize, zoompos, 1));
    }
    public void UnZoom(){
        followCam = true;
        StartCoroutine(ZoomToPos(camerasize, camerapos, 1));
    }

    private IEnumerator ZoomToPos(float size, Vector3 pos, float duration){
        float time = 0f;
        Vector3 initialPosition = transform.position;
        float initialSize = gameObject.GetComponent<Camera>().orthographicSize;
        while(time < duration){
            transform.position = Vector3.Lerp(initialPosition, pos, time/duration);
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(initialSize, size, time/duration); 
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
        gameObject.GetComponent<Camera>().orthographicSize = size;
        // time = 0f;
        if (followCam) { Camera.main.GetComponent<FollowCam>().enabled = true; }
        //StopAllCoroutines();

    }
    // Start is called before the first frame update
    void Start()
    {
        camerasize = gameObject.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
