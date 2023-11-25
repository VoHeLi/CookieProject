using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{

    [Header("Sprites piston right")]
    [SerializeField] private Sprite[] pistonRightExtention;
    [SerializeField] private Sprite pistonRightDefault;
    [SerializeField] private Sprite pistonRightCable;

    [Space]
    [Header("Sprites piston left")]
    [SerializeField] private Sprite[] pistonLeftExtention;
    [SerializeField] private Sprite pistonLeftDefault;
    [SerializeField] private Sprite pistonLeftCable;

    [Space]
    [Header("Debug")]
    [SerializeField] private bool isFacingRight;
    [SerializeField] private bool isFacingLeft;

    private SpriteRenderer pistonSpriteRenderer;
    private SpriteRenderer cableSpriteRenderer;

    private bool isExtending = false;


    [SerializeField] private bool isConnected;



    // Start is called before the first frame update
    void Start()
    {
        pistonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (isConnected)
        {

            spriteRenderer.sprite = pistonRightCable;
            spriteRenderer.sortingOrder = 1;
        }
        else
        {

            if (isFacingRight && !isExtending)
            {
                spriteRenderer.sprite = pistonRightDefault;
            }
            else if (isFacingLeft && !isExtending)
            {
                spriteRenderer.sprite = pistonLeftDefault;
            }

            spriteRenderer.sortingOrder = 0;
        }
    }

    // TEST
    // TODO : Commencer l'animation pas à un click de la souris mais quand on presse le bouton Start animation
    private void OnMouseDown()
    {
        Debug.Log("Click");
        
        if (isFacingLeft)
        {
            StartCoroutine(startPistonLeftAnimation());
        } else
        {
            StartCoroutine(startPistonRightAnimation());
        }
    }

    IEnumerator startPistonRightAnimation()
    {
        isExtending = true;
        for (int i = 0; i < pistonRightExtention.Length; i++)
        {
            GetComponent<SpriteRenderer>().sprite = pistonRightExtention[i];
            yield return new WaitForSeconds(.1f);
        }
        isExtending = false;
    }

    IEnumerator startPistonLeftAnimation()
    {
        isExtending = true;
        for (int i = 0; i < pistonLeftExtention.Length; i++)
        {
            GetComponent<SpriteRenderer>().sprite = pistonLeftExtention[i];
            yield return new WaitForSeconds(.1f);
        }
        isExtending = false;
    }
}
