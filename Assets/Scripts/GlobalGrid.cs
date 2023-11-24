using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGrid : MonoBehaviour
{
    [SerializeField] private float _caseSize = 1f;
    [SerializeField] private int _nbCaseX = 100;
    [SerializeField] private int _nbCaseY = 100;


    public static GlobalGrid _instance { get; private set; }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple instances of GlobalGrid!");
        }
        _instance = this;
    }

    public static bool IsInGrid(int i, int j)
    {
        return i >= 0 && i < nbCaseX && j >= 0 && j < nbCaseY;
    }

    public static float caseSize
    {
        get { return _instance._caseSize; }
    }

    public static int nbCaseX
    {
        get { return _instance._nbCaseX; }
    }

    public static int nbCaseY
    {
        get { return _instance._nbCaseY; }
    }

    public static bool GetMouseCase(ref int i, ref int j)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        i = Mathf.FloorToInt(0.5f + mousePos.x / GlobalGrid.caseSize);
        j = Mathf.FloorToInt(0.5f + mousePos.y / GlobalGrid.caseSize);
        return GlobalGrid.IsInGrid(i, j);
    }
}
