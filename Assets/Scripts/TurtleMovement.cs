using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDown)
        {
            Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = temp;
        }
    }
    public bool isDown = false;

    private void OnMouseDown()
    {
        isDown = true;
    }
    private void OnMouseup()
    {
        isDown = false;
    }
}
