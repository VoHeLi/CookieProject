using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
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
        //TODO : Add other elements
    }
    public const int ELEMENT_TYPE_COUNT = 9;


    public TypeElement type = TypeElement.None;
}
