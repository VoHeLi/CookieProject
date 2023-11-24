using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleElementManager : MonoBehaviour
{
    

    [SerializeField] private GameObject[] elementPrefabs;

    [HideInInspector] public Element.TypeElement[,] elementMaps;
    [HideInInspector] public GameObject[,] elementObjects;

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

    private void LoadInitialElements()
    {
        //TODO : Load initial elements from level data

        //TEMP_TEST
        elementMaps[0,1] = Element.TypeElement.Batterie;
        elementMaps[1,1] = Element.TypeElement.Cable;
        elementMaps[2,1] = Element.TypeElement.Cable;
        elementMaps[3, 1] = Element.TypeElement.Ventilateur_right;
        elementMaps[5, 1] = Element.TypeElement.Ventilateur_left;
        elementMaps[6, 1] = Element.TypeElement.Poteau;

        UpdateAllElementObject();   
    }

    void UpdateAllElementObject()
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
            Destroy(elementObjects[i, j]);
            elementObjects[i, j] = null;
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
        elementObject.transform.parent = transform;
    }
}
