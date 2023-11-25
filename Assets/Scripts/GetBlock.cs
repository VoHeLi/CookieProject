using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetBlock : MonoBehaviour
{
    
    public static GetBlock instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of GetBlock!");
        }

        instance = this;
    }


    [SerializeField] 
    private static Tilemap _ground;

    [SerializeField] 
    private static Tilemap _gates;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

    public static bool CanBePlacedOn(int xtile, int ytile)
    {

        return (GlobalGrid.IsInGrid(xtile, ytile) && 
                !_ground.HasTile(new Vector3Int(xtile, ytile)) && 
                !_gates.HasTile(new Vector3Int(xtile, ytile)) && 
                (_ground.HasTile(new Vector3Int(xtile, ytile + 1)) || 
                 _ground.HasTile(new Vector3Int(xtile, ytile - 1))));
            
    }
    
}
