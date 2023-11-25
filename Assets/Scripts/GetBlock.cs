using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetBlock : MonoBehaviour
{
    private Plane _plane;
    
    
    
    [SerializeField] 
    private Tilemap _ground;

    [SerializeField] private Tilemap _grates;
    // Start is called before the first frame update
    void Start()
    {
        _plane = new Plane(new Vector3(0, 0, 1), new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            int xtile = 0, ytile = 0;
            if (GlobalGrid.GetMouseCase(ref xtile,ref ytile))
            {
                Debug.Log("Can be placed on? " + CanBePlacedOn(xtile,ytile));
            }
            
        }
    }

    public bool CanBePlacedOn(int xtile, int ytile)
    {

        return (GlobalGrid.IsInGrid(xtile, ytile) && 
                !(_ground.HasTile(new Vector3Int(xtile, ytile))) && 
                !(_grates.HasTile(new Vector3Int(xtile, ytile))) && 
                (_ground.HasTile(new Vector3Int(xtile, ytile + 1)) || 
                 _ground.HasTile(new Vector3Int(xtile, ytile - 1))));
            
    }
    
}
