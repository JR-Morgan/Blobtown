using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{

    //maybe should be home factory?

    public GameObject homePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void placeHome(Tile tile)
    {
        Instantiate(homePrefab, tile.transform.position, Quaternion.identity, this.transform);
    }
}
