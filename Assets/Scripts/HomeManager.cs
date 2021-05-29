using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{

    //maybe should be home factory?

    public GameObject homePrefab;
    private List<GameObject> homes;

    // Start is called before the first frame update
    void Start()
    {
        homes = new List<GameObject>();
        foreach (Transform child in transform) 
        {
            homes.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void placeHome(Tile tile)
    {
        GameObject newHome = Instantiate(homePrefab, tile.transform.position, Quaternion.identity, this.transform);
        homes.Add(newHome);
    }
}
