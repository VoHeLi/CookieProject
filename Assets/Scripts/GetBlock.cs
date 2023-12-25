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
    
    [SerializeField] private Tilemap _ground;

    [SerializeField] private Tilemap _sky;
    
    [SerializeField] private Tilemap _gates;
    
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
                // Debug.Log("Can be placed on? " + CanBePlacedOn(xtile,ytile,0));
            }
            
        }
    }

    //flags : default = 0, 1 = can be placed on roof, 2 = poteau
    public bool CanBePlacedOn(int xtile, int ytile, int flags)
    {

        //Debug.Log(xtile + " : " + ytile);

        return (GlobalGrid.IsInGrid(xtile, ytile) && 
                !_ground.HasTile(new Vector3Int(xtile, ytile)) && 
                !_gates.HasTile(new Vector3Int(xtile, ytile)) && 
                    (
                        (_ground.HasTile(new Vector3Int(xtile, ytile + 1)) && flags == 1) || 
                        (_ground.HasTile(new Vector3Int(xtile, ytile - 1)) && flags != 1) || 
                        (flags == 2 && GrilleElementManager.instance.GetElementTypeAtPosition(xtile, ytile-1) == Element.TypeElement.Poteau)         
                     )
                    )
                    ;
            
    }

    public bool CanWindGoThrought(int xTile, int yTile)
    {
        return (GlobalGrid.IsInGrid(xTile, yTile) &&
                (_gates.HasTile(new Vector3Int(xTile, yTile)) ||
                 _sky.HasTile(new Vector3Int(xTile, yTile))) &&
                !_ground.HasTile(new Vector3Int(xTile, yTile))) ;
    }
    
}
