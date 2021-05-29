using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{

    //maybe should be home factory?

    public GameObject homePrefab;
    private List<Building> homes;



    // Start is called before the first frame update
    void Start()
    {
        homes = new List<Building>();
        foreach (Transform child in transform) 
        {
            homes.Add(child.gameObject.GetComponent<Building>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject buildHome(Tile tile)
    {
        GameObject newHome = Instantiate(homePrefab, tile.transform.position, Quaternion.identity, this.transform);
        
        homes.Add(newHome.GetComponent<Building>());
        return newHome;
    }
}
