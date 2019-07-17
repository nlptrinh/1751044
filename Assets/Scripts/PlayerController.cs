 using System.Collections;
 using System.Collections.Generic;
 using System.IO;
 using UnityEngine;

 public class PlayerController : MonoBehaviour {

     public float moveSpeed = 5f;
     public float rotateSpeed = 5f;
     public float minRotation = -60f;
     public float maxRotation = 60f;

     public bool IsRising = false;

     public Rigidbody2D rb2d;
     public GameManager gm;

     public AudioSource audioSource;
     public bool InWater = false;

     public float pitchIncrease = 0.1f;
     public float maxPitch = 2.7f;
     public float minPitch = 1.7f;

     public Transform fishBody;

     public GameObject trail1;
     public GameObject trail2;

     // Use this for initialization
     void Start()
     {
         rb2d = GetComponent<Rigidbody2D>();
         gm = FindObjectOfType<GameManager>();
         //gameObject.SetActive(false);
     }

     // Update is called once per frame
     void Update()
     {

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

             var hor = Input.GetAxisRaw("Vertical");

             var rot = Input.GetAxisRaw("Horizontal");

             //move forward
             if (InWater)
             {
                 rb2d.velocity = (hor * transform.right * moveSpeed);


                 if (rot != 0)
                 {
                     Debug.Log("rotating");

                     transform.Rotate(0, 0, rot * -rotateSpeed);
                 }

             }
             else
             {
                 //not in water
                 var dir = rb2d.velocity;
                 var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                 transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
             }
             //move up or down

         }

     }

     void OnTriggerEnter2D(Collider2D coll)
     {
        
     }
 }
