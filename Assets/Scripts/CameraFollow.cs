using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float dampTime = 0.15f;
     private Vector3 velocity = Vector3.zero;
     public Transform target;
    private Camera camera;
    public GameManager gm;
    public float maxHeight;
    public float minHeight;

    void Start()
    {
        camera = GetComponent<Camera>();
        if (gm == null)
            gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
     void LateUpdate () 
     {
         if (gm.GameStart)
         {
            if (target)
            {
                Vector3 point = camera.WorldToViewportPoint(target.position);
                Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = transform.position + delta;

                /*if (destination.y > maxHeight)
                    destination.y = maxHeight;

                if (destination.y < minHeight)
                    destination.y = minHeight;*/

                destination.y = transform.position.y;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
            else
            {
                target = FindObjectOfType<PlayerController>().transform;
            }
        }
         
     
     }
}
