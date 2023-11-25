using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{

    private int xPos;
    private int yPos;

    private void Start()
    {
        Debug.Log("Je suis positioné2 en " + (getXPos(), getYPos()));
    }

    public enum TypeElement
    {
        None = 0,
        Cable = 1,
        Ventilateur_left = 2,
        Ventilateur_right = 3,
        Ventilateur_up = 4,
        Ventilateur_down = 5,
        Poteau = 6,
        Batterie = 7,
        TargetBattery = 8,
        Eolienne_left = 9,
        Eolienne_right = 10,
        Eolienne_up = 11,
        Eolienne_down = 12,
        Piston_left = 13,
        Piston_right = 14
        //TODO : Add other elements
    }
    public const int ELEMENT_TYPE_COUNT = 15;

    public enum ElementToSelect
    {
        None = 0,
        Cable = 1,
        Ventilateur = 2,
        Poteau = 3,
        Eolienne = 4,
        Piston = 5
        //TODO : Add other elements
    }
    public const int ELEMENT_TO_SELECT_COUNT = 6;

    public TypeElement type = TypeElement.None;

    public int getXPos()
    {
        return this.xPos;
    }
    public int getYPos()
    {
        return this.yPos;
    }
    public void setXPos(int x)
    {
        this.xPos = x;
    }
    public void setYPos(int y)
    {
        this.yPos = y;
    }
}
