using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleElementManager : MonoBehaviour
{
    public static GrilleElementManager instance;

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
    [SerializeField] public List<int> inventory/* = new List<int> { 0, 0, 0, 0, 0, 0 ,0}*/ ;
    private AudioSource _placementSound;
    private AudioSource _removeSound;

    [HideInInspector] public Element.TypeElement[,] elementMaps;
    [HideInInspector] public GameObject[,] elementObjects;

    public Vector2Int sourcePosition = new Vector2Int(-1000, -1000);


    private Element.TypeElement currentPlacingElement = Element.TypeElement.None;
    private GameObject currentPlacingElementObject = null;
    private int[] curentElementPossibilities;

    private Dictionary<int, int[]> relation_Element_Selection = new Dictionary<int, int[]>();
    private Dictionary<int[], int> elementIndexMemory = new Dictionary<int[], int>();
    public bool isDictionnariesCompleted = false;

    public Dictionary<int, int[]> getRelationElementSelection()
    {
        return relation_Element_Selection;
    }

    public void completeDictionnaries()
    {
        int[] possibilities_1 = new int[] { 1 };
        int[] possibilities_2 = new int[] { 6 };
        int[] possibilities_3 = new int[] { 2, 3, 4, 5 };
        int[] possibilities_4 = new int[] { 9, 10, 11, 12 };
        int[] possibilities_5 = new int[] { 13, 14 };
        int[] possibilities_6 = new int[] { 19, 20 };
        int[] possibilities_7 = new int[] { 15, 16, 17, 18 };

        relation_Element_Selection.Add(1, possibilities_1);
        relation_Element_Selection.Add(2, possibilities_2);
        relation_Element_Selection.Add(3, possibilities_3);
        relation_Element_Selection.Add(4, possibilities_4);
        relation_Element_Selection.Add(5, possibilities_5);
        relation_Element_Selection.Add(6, possibilities_6);
        relation_Element_Selection.Add(7, possibilities_7);

        // Pour mémoriser dans quel état on était lors de la précédente sélection de l'objet
        elementIndexMemory.Add(possibilities_1, 0);
        elementIndexMemory.Add(possibilities_2, 0);
        elementIndexMemory.Add(possibilities_3, 0);
        elementIndexMemory.Add(possibilities_4, 0);
        elementIndexMemory.Add(possibilities_5, 0);
        elementIndexMemory.Add(possibilities_6, 0);
        elementIndexMemory.Add(possibilities_7, 0);

        //Debug.Log("Dictionnaires complets");
        isDictionnariesCompleted = true;
    }    

    public int reverseDictionnaries(int index)
    {
        foreach(KeyValuePair<int, int[]> element in relation_Element_Selection)
        {
           
            foreach (int value in element.Value)
            {
                //  Debug.Log(index + " != " + value + " : " + element.Key);
                if (index == value) return element.Key;
            }
            
        }

        Debug.Log("index " + index + " was not found");
        return -1;
    }



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

        _removeSound = GameObject.Find("RemoveSound").GetComponent<AudioSource>();
        _placementSound = GameObject.Find("PlacementSound").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (UI.instance.isRunning)
        {
            if (currentPlacingElementObject != null)
            {
                currentPlacingElement = Element.TypeElement.None;
                Destroy(currentPlacingElementObject);
            }
            return;
        }

        //current Placing Element at good position
        if (currentPlacingElement != Element.TypeElement.None)
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
            int i = 0, j = 0;
            if (!GlobalGrid.GetMouseCase(ref i, ref j) || !GetBlock.instance.CanBePlacedOn(i, j, flags))
            {
                  currentPlacingElementObject.transform.position = new Vector3(-1000, -1000, 0);
            }

            else currentPlacingElementObject.transform.position = new Vector3(i * GlobalGrid.caseSize, j * GlobalGrid.caseSize, 0);
        }

        // Changer le type de currentElement choisi
        if (currentPlacingElement != Element.TypeElement.None && Input.GetKeyDown(KeyCode.R))
        {
            increaseCurrentElementIndex();
        }

        //DEBUG
        if (currentPlacingElement != Element.TypeElement.None && (Input.GetMouseButtonDown(0)))
        {
            EndObjectPlacement();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RemoveElementFromCase();
        }
    }

    

    public void LoadInitialElements()
    {

        elementMaps[initialSetup[0].x, initialSetup[0].y] = Element.TypeElement.Batterie;
        elementMaps[initialSetup[1].x, initialSetup[1].y] = Element.TypeElement.TargetBattery;
        sourcePosition = initialSetup[0];

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

    public void UpdateElementObject(int i, int j, bool addToi = true)
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

            if (addToi)
            {
                addToInventory((int)elementObjects[i, j].GetComponent<Element>().type);
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

        GameObject elementObject = Instantiate(elementPrefabs[(int)elementMaps[i, j]], new Vector3(i * GlobalGrid.caseSize, j * GlobalGrid.caseSize, 0), elementPrefabs[(int)elementMaps[i, j]].transform.rotation);
        elementObject.GetComponent<Element>().setXPos(i);
        elementObject.GetComponent<Element>().setYPos(j);
        Debug.Log("Position de l'element en positon " + (i, j));
        elementObject.transform.parent = transform;
        elementObjects[i, j] = elementObject;

        /*
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
        */
    }

    private void BeginElementPlacement()
    {
        if (UI.instance.isRunning) return;

            Debug.Log("Begin element placement : " + currentPlacingElement.ToString());
        if (currentPlacingElementObject != null)
        {
            Destroy(currentPlacingElementObject);
        }

        if (currentPlacingElement == Element.TypeElement.None) return;

        currentPlacingElementObject = Instantiate(elementPrefabs[(int)currentPlacingElement], Vector3.zero, elementPrefabs[(int)currentPlacingElement].transform.rotation);
    }
    private void EndObjectPlacement()
    {
        int i = 0, j = 0;
        if (!GlobalGrid.GetMouseCase(ref i, ref j)) return;

        if (UI.instance.isRunning) return;

        //Check if there is a battery or a target battery and return if there is
        if (elementMaps[i,j] == Element.TypeElement.Batterie || elementMaps[i,j] == Element.TypeElement.TargetBattery)
        {
            return;
        }


		if (elementMaps[i, j] == Element.TypeElement.Batterie || elementMaps[i, j] == Element.TypeElement.TargetBattery) return;
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

        if (!RemoveFromInventory((int)currentPlacingElement)) return;

        elementMaps[i, j] = currentPlacingElement;

        if(_placementSound != null) _placementSound.Play(0);

        UpdateElementObject(i, j);
    }
    private void RemoveElementFromCase()
    {
        if (UI.instance.isRunning) return;

        int i = 0, j = 0;
        if (!GlobalGrid.GetMouseCase(ref i, ref j)) return;

        //Check if there is a battery or a target battery and return if there is
        if (elementMaps[i, j] == Element.TypeElement.Batterie || elementMaps[i, j] == Element.TypeElement.TargetBattery)
        {
            return;
        }

        if (elementMaps[i, j] == Element.TypeElement.Poteau && GetElementTypeAtPosition(i, j+1) == Element.TypeElement.Poteau) return;

        Debug.Log("Remove element from case");
        //addToInventory((int)elementMaps[i, j]);
        elementMaps[i, j] = Element.TypeElement.None;
        if(_removeSound != null) _removeSound.Play(0);
        UpdateElementObject(i, j);
    }

    public Element.TypeElement GetElementTypeAtPosition(int i, int j)
    {
        //Check if in grid
        if (!GlobalGrid.IsInGrid(i, j)) return Element.TypeElement.None;

        return elementMaps[i, j];
    }

    public GameObject GetObjetAtPosition(int i, int j)
    {
        //Check if in grid
        if (!GlobalGrid.IsInGrid(i, j)) return null;

        return elementObjects[i, j];
    }

    public void setCurrentPlacingElement(int i)
    {
        curentElementPossibilities = relation_Element_Selection[i];
        currentPlacingElement = (Element.TypeElement)curentElementPossibilities[elementIndexMemory[curentElementPossibilities]];
        BeginElementPlacement();
    }

    public void increaseCurrentElementIndex()
    {
        elementIndexMemory[curentElementPossibilities]++;
        if (curentElementPossibilities.Length <= elementIndexMemory[curentElementPossibilities])
        {
            elementIndexMemory[curentElementPossibilities] = 0;
        }

        currentPlacingElement = (Element.TypeElement)curentElementPossibilities[elementIndexMemory[curentElementPossibilities]];
        BeginElementPlacement();
    }

    public void addToInventory(int gridValue)
    {
        if (gridValue == 0) return;
        int inventoryId = reverseDictionnaries(gridValue);
        inventory[inventoryId-1]++;
        UI.instance.updateCount(inventoryId - 1);
        Debug.Log("on m'apelle pour incrementer" + inventoryId);
    }

    public bool RemoveFromInventory(int gridValue)
    {
        // return true and decrement if you can remove from inv, false and do nothing  if not 
        int inventoryId = reverseDictionnaries(gridValue) - 1;
        if (inventoryId == -1) return true;
        if (inventory[inventoryId] < 1) return false;
        inventory[inventoryId]--;
        Debug.Log("From remove");
        UI.instance.updateCount(inventoryId);
        return true;
    }
    
}
