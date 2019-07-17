using System;
using System.Collections;
using System.Collections.Generic;
using States;
using UnityEngine;
using Random = UnityEngine.Random;


public class FishController : MonoBehaviour {

    
    public Rigidbody2D rb2d;
    public GameManager gm;
    public bool InWater = false;
    public Transform fishBody;
    public State currentState;

    public float hor;
    public float rot;

    public bool turnAround;

    public float _speed;
    public float _rotateSpeed;
    public float _followDistance;

    public GameObject trail1;
    public GameObject trail2;

    public FishManager fishManager;

    public string color;

    public bool hooked;
    public GameObject hookRef;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        //gameObject.SetActive(false);

        ChangeState(new IdleState(this));

        if (fishManager == null)
            fishManager = FindObjectOfType<FishManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentState != null)
            currentState.OnUpdate();


        //check if behind water
        if (transform.position.y > 3.2f)
        {
            InWater = false;
            trail1.SetActive(false);
            trail2.SetActive(false);
        }
        else
        {
            InWater = true;
            trail1.SetActive(true);
            trail2.SetActive(true);
        }



    }


    void FixedUpdate()
    {
        //check if game is running.
        if (gm.GameStart)
        {
            //move forward
            if (InWater)
            {

                //forward
                rb2d.velocity = (hor * transform.right * _speed);

                if (turnAround)
                {
                    rot = 1;
                }

                //rotation
                if (rot != 0)
                {
                    transform.Rotate(0, 0, rot * -_rotateSpeed);
                }
            }
            else
            {
                //not in water
                var dir = rb2d.velocity;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

        }

    }

    public void ChangeState(State state)
    {
        if (currentState != null)
            currentState.OnExit();

        currentState = state;

        currentState.OnEnter();
    }

    public void FollowMOuse()
    {
        ChangeState(new FollowState(this));
    }

    public void StopFollowingMouse()
    {
        ChangeState(new IdleState(this));
    }

    public class IdleState : State
    {
        private FishController fish;
        float t = 0f;

        public IdleState(FishController fish)
        {
            this.fish = fish;
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            if (!fish.hooked)
            {
                //only forward, no back!
                fish.hor = 1;

                if (fish.turnAround)
                {
                    fish.rot = 1;
                    t += Time.deltaTime;

                    if (t > 0.5f)
                    {
                        fish.turnAround = false;
                        t = 0;
                    }
                }
                else
                {
                    //random direction left and right
                    fish.rot = Random.Range(-1, 2); //left and right
                }
            }
            else
            {
                fish.hor = 0;
                fish.rot = 0;
            }
            
            
        }

        public override void OnExit()
        {

        }
    }

    public class FollowState : State
    {
        private FishController fish;

        public FollowState(FishController fish)
        {
            this.fish = fish;
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            if (!fish.hooked)
            {
                //check how far away the target is
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //reset z axis
                mousePos.z = 0;

                //only forward, no back!
                fish.hor = 1;

                //get direction to mouse
                var dir = mousePos - fish.transform.position;

                //get angle
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                //rotate fish
                fish.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                fish.hor = 0;
                fish.rot = 0;
            }


        }

        public override void OnExit()
        {

        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        
        
        if (coll.gameObject.tag == "Borders" || coll.gameObject.tag == "Sand")
            turnAround = true;

        if (coll.gameObject.tag == "Hook")
        {
            var bobber = coll.gameObject.GetComponentInParent<Bobber>();

            //check if the bobber is the same color as me.
            if (bobber.color == color)
            {
                //hook fish
                bobber.HookFish(this.transform);

                gm.AddTimer(color);
                gm.ShowText(gameObject.transform.position, color);
                

                fishManager.DisperseFish();

            }
            else
            {
                //do nothing?
            }
        }

    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        fishManager.FishFollowed(this);
    }
}
