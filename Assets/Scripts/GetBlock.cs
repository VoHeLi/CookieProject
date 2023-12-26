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

    //flags : default = 0, 1 = can be placed on roof, 2 = poteau, 3 = can be placed on grille
    // 0 : 
    // 1 : ventillo vertico
    // 2 : poteau
    // 3 : éponge
    public bool CanBePlacedOn(int xtile, int ytile, Element.TypeElement currentPlacingElement)
    {
        int flags = 0;
        if (currentPlacingElement == Element.TypeElement.Poteau)
        {
            flags = 2;
        }
        else if (currentPlacingElement == Element.TypeElement.Eolienne_up || currentPlacingElement == Element.TypeElement.Ventilateur_down)
        {
            flags = 1;
        }
        else if (currentPlacingElement == Element.TypeElement.Sponge_cornerTopLeft
            || currentPlacingElement == Element.TypeElement.Sponge_cornerTopRight
            || currentPlacingElement == Element.TypeElement.Sponge_cornerBottomLeft
            || currentPlacingElement == Element.TypeElement.Sponge_cornerBottomRight)
        {
            flags = 3;
        }

        // valable pour tous, le spot doit etre dans la grille et libre
        bool isInGrid = GlobalGrid.IsInGrid(xtile, ytile);
        bool isFree = !_ground.HasTile(new Vector3Int(xtile, ytile))
            && !_gates.HasTile(new Vector3Int(xtile, ytile))
            && GrilleElementManager.instance.GetElementTypeAtPosition(xtile, ytile) != Element.TypeElement.Batterie
            && GrilleElementManager.instance.GetElementTypeAtPosition(xtile, ytile) != Element.TypeElement.TargetBattery;

        if (isFree && isInGrid)
        {
            switch (flags)
            {
                case 0:
                    // default, a juste besoin d'un block de terre en dessous
                    return _ground.HasTile(new Vector3Int(xtile, ytile - 1));
                case 1:
                    // ventilo vert, a juste besoin d'un block de terre au dessus
                    return _ground.HasTile(new Vector3Int(xtile, ytile + 1));
                case 2:
                    // poteau, soit a un block de terre, soit un autre poteau
                    bool groundUnder = _ground.HasTile(new Vector3Int(xtile, ytile - 1));
                    bool poolUnder = GrilleElementManager.instance.GetElementTypeAtPosition(xtile, ytile - 1) == Element.TypeElement.Poteau;
                    return groundUnder || poolUnder;
                case 3:
                    // eponge, a besoin d'avoir au moins un point d'accroche en fonction de sa rotation
                    bool[] anchor = { false, false, false, false };
                    anchor[0] = _ground.HasTile(new Vector3Int(xtile, ytile + 1)) || _gates.HasTile(new Vector3Int(xtile, ytile + 1));
                    anchor[1] = _ground.HasTile(new Vector3Int(xtile + 1, ytile)) || _gates.HasTile(new Vector3Int(xtile + 1, ytile));
                    anchor[2] = _ground.HasTile(new Vector3Int(xtile, ytile - 1)) || _gates.HasTile(new Vector3Int(xtile, ytile - 1));
                    anchor[3] = _ground.HasTile(new Vector3Int(xtile - 1, ytile)) || _gates.HasTile(new Vector3Int(xtile - 1, ytile));
                    // regarde en fonction de la rotation
                    switch (currentPlacingElement)
                    {
                        case Element.TypeElement.Sponge_cornerTopLeft:
                            return anchor[0] || anchor[3];
                        case Element.TypeElement.Sponge_cornerTopRight:
                            return anchor[1] || anchor[0];
                        case Element.TypeElement.Sponge_cornerBottomRight:
                            return anchor[2] || anchor[1];
                        case Element.TypeElement.Sponge_cornerBottomLeft:
                            return anchor[3] || anchor[2];
                    }
                    break;
            }
        }
        return false;
    }

    public bool CanWindGoThrought(int xTile, int yTile)
    {
        return (GlobalGrid.IsInGrid(xTile, yTile) &&
                (_gates.HasTile(new Vector3Int(xTile, yTile)) ||
                 _sky.HasTile(new Vector3Int(xTile, yTile))) &&
                !_ground.HasTile(new Vector3Int(xTile, yTile))) ;
    }
    
}
