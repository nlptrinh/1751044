using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RedFishSettings
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public float followDistance = 5f;

}

[Serializable]
public class BlueFishSettings
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public float followDistance = 5f;

}

[Serializable]
public class GreenFishSettings
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public float followDistance = 5f;

}

public class FishManager : MonoBehaviour
{

    public GameObject[] fishPrefabs;
    public GameObject[] bobberPrefabs;
    public Transform[] spawnPoints;

    public float fishTimer;
    public float bobberTimer;
    private float timer;
    private float timer2;

    public bool CanSpawnFish = false;
    public bool CanSpawnBobbers = false;

    public bool FishFollowing;

    public List<FishController> fishFollowingList = new List<FishController>();

    public float bobberSpawnMinX;
    public float bobberSpawnMaxX;

    public float bobberSpawnMinY;
    public float bobberSpawnMaxY;

    public List<GameObject> spawnedBobbers = new List<GameObject>();
    public List<GameObject> spawnedFish = new List<GameObject>();

    public Text popupText;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //check if we have fish following the mouse
	    if (FishFollowing)
	    {
            //check if we right click
	        if (Input.GetMouseButtonDown(1))
	        {
                DisperseFish();
	        }
	    }

        List<String> colors = new List<string>
        {
            "Blue",
            "Green",
            "Orange",
            "Purple",
            "Red"
        };

	    var newColor = colors.OrderBy(x => Guid.NewGuid()).FirstOrDefault();


	    if (CanSpawnFish)
	    {
	        timer += Time.deltaTime;

	        if (timer > fishTimer)
	        {
                SpawnFish(newColor);
	            timer = 0;
	        }
	    }

        if (CanSpawnBobbers)
        {
            timer2 += Time.deltaTime;

            if (timer2 > bobberTimer)
            {
                SpawnBobber(newColor);
                timer2 = 0;
            }
        }
    }

    public void DisperseFish()
    {
        if (fishFollowingList.Any())
        {
            foreach (var fish in fishFollowingList)
            {
                fish.StopFollowingMouse();
                fish.turnAround = true;
            }
            fishFollowingList.Clear();
            FishFollowing = false;
        }
    }

    public void SpawnFish(string color)
    {
        if (spawnPoints.Length > 0)
        {
            //pick a random spawn point
            var spawnPoint = spawnPoints.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            //pick a random fish
            //instantiate fish at spawn point
            var fish = GetPooledFish(color);
            fish.SetActive(true);
            fish.transform.position = spawnPoint.position;

        }
        else
        {
            Debug.Log("No spawn points.");
        }
    }

    public void SpawnBobber(string color)
    {
        var bobber = GetPooledBobber(color);
        bobber.SetActive(true);

        var newXPos = UnityEngine.Random.Range(bobberSpawnMinX, bobberSpawnMaxX);
        var newYPos = UnityEngine.Random.Range(bobberSpawnMinY, bobberSpawnMaxY);

        bobber.transform.position = new Vector3(newXPos,newYPos,0);
        
    }

    public void FishFollowed(FishController fish)
    {
        //check if we already have a fish following the mouse
        if (FishFollowing)
        {
            //get reference to oldest fish following the mouse
            var oldFish = fishFollowingList.First();

            //tell this fish to stop following the mouse
            oldFish.StopFollowingMouse();

            //remove it from the list
            fishFollowingList.Remove(oldFish);

        }

        //add the new fish to the list
        fishFollowingList.Add(fish);

        //tell the new fish to follow the mouse
        fish.FollowMOuse();

        FishFollowing = true;
    }


    public GameObject GetPooledBobber( string color)
    {
        if (spawnedBobbers.Any(x => !x.activeInHierarchy && x.GetComponent<Bobber>().color == color))
        {
            return spawnedBobbers.FirstOrDefault(x => !x.activeInHierarchy && x.GetComponent<Bobber>().color == color);
        }
        else
        {
            var obj = Instantiate(bobberPrefabs.FirstOrDefault(x=>x.GetComponent<Bobber>().color == color));
            obj.SetActive(false);
            spawnedBobbers.Add(obj);
            return obj;
        }
    }

    public GameObject GetPooledFish(string color)
    {
        if (spawnedFish.Any(x => !x.activeInHierarchy && x.GetComponent<FishController>().color == color))
        {
            return spawnedFish.FirstOrDefault(x => !x.activeInHierarchy && x.GetComponent<FishController>().color == color);
        }
        else
        {
            var obj = Instantiate(fishPrefabs.Where(x=>x.GetComponent<FishController>().color == color).FirstOrDefault());
            obj.SetActive(false);
            spawnedFish.Add(obj);
            return obj;
        }
    }


    public void ClearPools()
    {
        foreach (var fish in spawnedFish.Where(x=>x.activeInHierarchy))
        {
            fish.SetActive(false);
        }

        foreach (var bobber in spawnedBobbers.Where(x => x.activeInHierarchy))
        {
            bobber.SetActive(false);
        }
    }

}
