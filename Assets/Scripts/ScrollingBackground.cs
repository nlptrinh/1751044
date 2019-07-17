using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSize;
    public bool scrolling, paralax;

    private Transform cameraTransform;
    private Transform[] layers;

    private float viewZone = 10f;
    private int leftIndex;
    private int rightIndex;

    public float paralaxSpeed;
    private float lastCameraX;

    private Vector3 startPos;
    private Vector3[] startPosistions;


	// Use this for initialization
	void Start ()
	{
	    startPos = transform.position;
	    cameraTransform = Camera.main.transform;
	    lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        startPosistions = new Vector3[transform.childCount];

	    for (int i = 0; i < transform.childCount; i++)
	    {
	        layers[i] = transform.GetChild(i);
	        startPosistions[i] = layers[i].position;
	    }

	    leftIndex = 0;
	    rightIndex = layers.Length - 1;

	}

    public void ResetPosition()
    {
        transform.position = startPos;

        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i].transform.position = startPosistions[i];
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (paralax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
            
        }

        lastCameraX = cameraTransform.position.x;

        if (scrolling)
	    {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            {
                ScrollLeft();
            }

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
            {
                ScrollRight();
            }
        }

	    
	    

	    
    }



    private void ScrollLeft()
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }

    }

    private void ScrollRight()
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }

    }
}
