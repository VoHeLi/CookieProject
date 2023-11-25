using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilateur : MonoBehaviour
{

    [Header("Sprites ventilateur right")]
    [SerializeField] private Sprite[] ventilateurRightExtention;
    [SerializeField] private Sprite ventilateurRightDefault;
    [SerializeField] private Sprite ventilateurRightCable;

    [Header("Sprites ventilateur left")]
    [SerializeField] private Sprite[] ventilateurLeftExtention;
    [SerializeField] private Sprite ventilateurLeftDefault;
    [SerializeField] private Sprite ventilateurLeftCable;

    [Header("Sprites ventilateur up")]
    [SerializeField] private Sprite[] ventilateurUpExtention;
    [SerializeField] private Sprite ventilateurUpDefault;
    [SerializeField] private Sprite ventilateurUpCable;

    [Header("Sprites ventilateur down")]
    [SerializeField] private Sprite[] ventilateurDownExtention;
    [SerializeField] private Sprite ventilateurDownDefault;
    [SerializeField] private Sprite ventilateurDownCable;

    [Space]
    [Header("Debug")]
    [SerializeField] private bool isFacingRight;
    [SerializeField] private bool isFacingLeft;
    [SerializeField] private bool isFacingUp;
    [SerializeField] private bool isFacingDown;

    [SerializeField] private bool isBlowing;

    private bool isConnected;
    private bool isCableAttached = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        /*SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (isFacingRight)
        {
            spriteRenderer.sprite = ventilateurRightDefault;
            spriteRenderer.sortingOrder = 1;
        }*/

    }
}
