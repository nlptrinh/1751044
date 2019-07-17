using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{

    public GameObject bobber;

    public float bobHeight;

    public GameObject fish;
    public bool hooked = false;

    public string color;

    public GameObject hook;


    public Rigidbody2D rb2d;

    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void moveUpAndDown()
    {
        LeanTween.moveY(bobber,transform.position.y + bobHeight, 1).setEase(LeanTweenType.pingPong).setOnComplete(
            () =>
            {
                LeanTween.moveY(bobber, transform.position.y - bobHeight, 1)
                    .setEase(LeanTweenType.linear)
                    .setLoopPingPong(-1);
            });
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bobber")
        {
            if (!LeanTween.isTweening(bobber))
            {
                moveUpAndDown();
            }
        }
    }

    public void HookFish(Transform fishHooked)
    {
        //hide fish
        fishHooked.gameObject.SetActive(false);

        //hide bobber
        gameObject.SetActive(false);
        
    }


    public void RaiseHook()
    {
        LeanTween.moveY(hook, transform.position.y, 1).setOnComplete(() =>
        {
            //give points?

           


            //hide bobber

        });
    }

    public void LowerHook()
    {
        LeanTween.moveY(hook, transform.position.y - 4.7f, 1);
    }


}
