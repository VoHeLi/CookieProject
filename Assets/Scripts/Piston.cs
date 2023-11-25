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

    private bool isExtending = false;


    [SerializeField] private bool isConnected;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingRight && !isExtending)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetComponent<SpriteRenderer>().sprite = pistonRightDefault;
        }
        if (isFacingLeft && !isExtending)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetComponent<SpriteRenderer>().sprite = pistonLeftDefault;
        }
        if (isFacingRight && isConnected)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            GetComponent<SpriteRenderer>().sprite = pistonRightCable;
        }
        if (isFacingRight && isConnected)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            GetComponent<SpriteRenderer>().sprite = pistonLeftCable;
        }
    }

    // TEST
    // TODO : Commencer l'animation pas à un click de la souris mais quand on presse le bouton Start animation
    private void OnMouseDown()
    {
        Debug.Log("Click");
        StartCoroutine(startPistonRightAnimation());
        
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
