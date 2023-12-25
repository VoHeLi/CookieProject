using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{

    private int xPos;
    private int yPos;

    private void Start()
    {
        // Debug.Log("Je suis positione2 en " + (getXPos(), getYPos()));
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
        Piston_right = 14,
        Sponge_cornerBottomLeft = 15,
        Sponge_cornerBottomRight = 16,
        Sponge_cornerTopLeft = 17,
        Sponge_cornerTopRight = 18,
        Antipiston_left = 19,
        Antipiston_right = 20,

        //TODO : Add other elements
    }
    public const int ELEMENT_TYPE_COUNT = 21;

    public enum ElementToSelect
    {
        None = 0,
        Cable = 1,
        Poteau = 2,
        Ventilateur = 3,
        Eolienne = 4,
        Piston = 5,
        Antipiston = 6,
        SpongeCorner = 7
    }
    public const int ELEMENT_TO_SELECT_COUNT = 8;

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

    public virtual void StopAnimation()
    {

    }
}
