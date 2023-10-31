using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{   

    private float camerasize;

    private Vector3 camerapos;

    private float zoomsize = 3;

    public void ZoomIn(Vector3 position){
        Vector3 zoompos = new Vector3(position.x, position.y + .8f, transform.position.z);
        print(zoompos);
        camerapos = transform.position;
        StartCoroutine(ZoomToPos(zoomsize, zoompos, 5));
    }
    public void UnZoom(){
        StartCoroutine(ZoomToPos(camerasize, camerapos, 4));
    }

    private IEnumerator ZoomToPos(float size, Vector3 pos, float duration){
        float time = 0f;
        while(transform.position != pos & gameObject.GetComponent<Camera>().orthographicSize != size){
            transform.position = Vector3.Lerp(transform.position, pos, time/duration);
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, size, time/duration); 
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
        gameObject.GetComponent<Camera>().orthographicSize = size;
        StopAllCoroutines();

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
