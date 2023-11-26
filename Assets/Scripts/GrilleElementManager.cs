using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleElementManager : MonoBehaviour
{
    public static GrilleElementManager instance ;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of GrilleElementManager!");
        }
        instance = this;
    }

    [SerializeField] private GameObject[] elementPrefabs;
    [SerializeField] private List<Vector2Int> initialSetup;
    [SerializeField] public List<int> inventory;

    [HideInInspector] public Element.TypeElement[,] elementMaps;
    [HideInInspector] public GameObject[,] elementObjects;

    public Vector2Int sourcePosition = new Vector2Int(-1000, -1000);


    private Element.TypeElement currentPlacingElement = Element.TypeElement.None;
    private GameObject currentPlacingElementObject = null;

    void Start()
    {
        elementMaps = new Element.TypeElement[GlobalGrid.nbCaseX, GlobalGrid.nbCaseY];
        for(int i = 0; i < GlobalGrid.nbCaseX; i++)
        {
            for(int j = 0; j < GlobalGrid.nbCaseY; j++)
            {
                elementMaps[i, j] = Element.TypeElement.None;
            }
        }

        elementObjects = new GameObject[GlobalGrid.nbCaseX, GlobalGrid.nbCaseY];

        LoadInitialElements();
    }

    private void Update()
    {
        //current Placing Element at good position
        if (currentPlacingElement != Element.TypeElement.None)
        {
            int flags = 0;
            if (currentPlacingElement == Element.TypeElement.Poteau)
            {
                flags = 2;
            }
            else if (currentPlacingElement == Element.TypeElement.Ventilateur_up || currentPlacingElement == Element.TypeElement.Ventilateur_down)
            {
                flags = 1;
            }
            int i = 0, j = 0;
            if (!GlobalGrid.GetMouseCase(ref i, ref j) || !GetBlock.instance.CanBePlacedOn(i, j, flags))
            {
                  currentPlacingElementObject.transform.position = new Vector3(-1000, -1000, 0);
            }

            else currentPlacingElementObject.transform.position = new Vector3(i * GlobalGrid.caseSize, j * GlobalGrid.caseSize, 0);
        }

        //DEBUG
        if (currentPlacingElement != Element.TypeElement.None && Input.GetMouseButtonDown(0))
        {
            EndObjectPlacement();
        }

        KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Z, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };
        for(int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                currentPlacingElement = (Element.TypeElement)i;
                BeginElementPlacement();
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentPlacingElement = (Element.TypeElement)14;
            BeginElementPlacement();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentPlacingElement = (Element.TypeElement)13;
            BeginElementPlacement();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RemoveElementFromCase();
        }
    }

    

    private void LoadInitialElements()
    {

        elementMaps[initialSetup[0].x, initialSetup[0].y] = Element.TypeElement.Batterie;
        elementMaps[initialSetup[1].x, initialSetup[1].y] = Element.TypeElement.TargetBattery;

        UpdateAllElementObjects();
        //TODO : Load initial elements from level data

        //TEMP_TEST
        /*elementMaps[0,1] = Element.TypeElement.Batterie;
        elementMaps[1,1] = Element.TypeElement.Cable;
        elementMaps[2,1] = Element.TypeElement.Cable;
        elementMaps[3, 1] = Element.TypeElement.Ventilateur_right;
        elementMaps[5, 1] = Element.TypeElement.Ventilateur_left;
        elementMaps[6, 1] = Element.TypeElement.Poteau;

        UpdateAllElementObjects();   */
    }

    void UpdateAllElementObjects()
    {
        for(int i = 0; i < GlobalGrid.nbCaseX; i++)
        {
            for(int j = 0; j < GlobalGrid.nbCaseY; j++)
            {
                UpdateElementObject(i, j);
            }
        }
    }

    private void UpdateElementObject(int i, int j)
    {
        if (elementObjects[i, j] != null)
        {
            if (elementObjects[i, j].GetComponent<Element>().type == elementMaps[i, j]) //TODO create Element class
            {
                return;
            }

            if(i == sourcePosition.x && j == sourcePosition.y)
            {
                sourcePosition = new Vector2Int(-1000, -1000);
                Debug.Log("Source position changed : " + sourcePosition.ToString());
            }

            Destroy(elementObjects[i, j]);
            elementObjects[i, j] = null;

            // rajouter l'ajout dans l'inventaire
        }

        if (elementMaps[i, j] == Element.TypeElement.None)
        {
            return;
        }

        if (elementPrefabs[(int)elementMaps[i, j]] == null)
        {
            Debug.LogError("Element prefab is null!");
            return;
        }

        if (elementPrefabs[(int)elementMaps[i, j]].GetComponent<Element>() == null)
        {
            Debug.LogError("Element prefab has no Element component!");
            return;
        }

        GameObject elementObject = Instantiate(elementPrefabs[(int)elementMaps[i, j]], new Vector3(i * GlobalGrid.caseSize, j * GlobalGrid.caseSize, 0), Quaternion.identity);
        elementObject.GetComponent<Element>().setXPos(i);
        elementObject.GetComponent<Element>().setYPos(j);
        Debug.Log("Position de l'�l�ment en positon " + (i, j));
        elementObject.transform.parent = transform;
        elementObjects[i, j] = elementObject;

        //enlever l'element dans l'inventaire

        if (elementMaps[i, j] == Element.TypeElement.Batterie)
        {
            if(GlobalGrid.IsInGrid(sourcePosition.x, sourcePosition.y))
            {
                elementMaps[sourcePosition.x, sourcePosition.y] = Element.TypeElement.None;
                Destroy(elementObjects[sourcePosition.x, sourcePosition.y]);
                elementObjects[sourcePosition.x, sourcePosition.y] = null;
            }
            
            sourcePosition = new Vector2Int(i, j);
            Debug.Log("Source position changed : " + sourcePosition.ToString());
        }
    }

    private void BeginElementPlacement()
    {
        
        
        Debug.Log("Begin element placement : " + currentPlacingElement.ToString());
        if (currentPlacingElementObject != null)
        {
            Destroy(currentPlacingElementObject);

        }

        if (currentPlacingElement == Element.TypeElement.None) return;

        currentPlacingElementObject = Instantiate(elementPrefabs[(int)currentPlacingElement], Vector3.zero, Quaternion.identity);
    }
    private void EndObjectPlacement()
    {
        int i = 0, j = 0;
        if (!GlobalGrid.GetMouseCase(ref i, ref j)) return;

        int flags = 0;
        if (currentPlacingElement == Element.TypeElement.Poteau)
        {
            flags = 2;
        }
        else if (currentPlacingElement == Element.TypeElement.Eolienne_up || currentPlacingElement == Element.TypeElement.Ventilateur_down)
        {
            flags = 1;
        }

        if (!GetBlock.instance.CanBePlacedOn(i,j, flags)) return;
        elementMaps[i, j] = currentPlacingElement;
        UpdateElementObject(i, j);
    }
    private void RemoveElementFromCase()
    {
        Debug.Log("Remove element from case");

        int i = 0, j = 0;
        if (!GlobalGrid.GetMouseCase(ref i, ref j)) return;

        if (elementMaps[i, j] == Element.TypeElement.Poteau && GetElementTypeAtPosition(i, j+1) == Element.TypeElement.Poteau) return;

        elementMaps[i, j] = Element.TypeElement.None;
        UpdateElementObject(i, j);
    }

    public Element.TypeElement GetElementTypeAtPosition(int i, int j)
    {
        //Check if in grid
        if (!GlobalGrid.IsInGrid(i, j)) return Element.TypeElement.None;

        return elementMaps[i, j];
    }

    public void setCurrentPlacingElement(int i)
    {
        switch (i)
        {
            case 1: // Cable
                currentPlacingElement = (Element.TypeElement)1;
                break;

            case 2: // Ventilateur
                currentPlacingElement = (Element.TypeElement)2;
                break;

            case 3: // Poteau
                currentPlacingElement = (Element.TypeElement)6;
                break;

            case 4: // Eolienne
                currentPlacingElement = (Element.TypeElement)9;
                break;

            case 5: // Piston
                currentPlacingElement = (Element.TypeElement)13;
                break;

            default:
                Debug.Log("Incorrect Element");
                break;
        }
        BeginElementPlacement();
    }
}
