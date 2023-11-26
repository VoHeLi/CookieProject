using System;
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

    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private float ballMass;

    private SpriteRenderer pistonSpriteRenderer;
    private SpriteRenderer cableSpriteRenderer;

    private bool isExtending = false;


    private bool isConnected;
    private bool isCableAttached = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (isConnected && !isCableAttached)
        {

            if (isFacingRight)
            {

                GameObject rightPistonCable = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer cableSpriteRenderer = rightPistonCable.AddComponent<SpriteRenderer>();
                cableSpriteRenderer.sprite = pistonRightCable;
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            } else
            {
                // Créez un nouvel objet GameObject vide
                GameObject cableGameObject = new GameObject("CableObject");
                GameObject leftPistonCable = Instantiate(cableGameObject, this.transform);
                Destroy(cableGameObject);

                SpriteRenderer cableSpriteRenderer = leftPistonCable.AddComponent<SpriteRenderer>();
                cableSpriteRenderer.sprite = pistonLeftCable;
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
            
            
        }
        
        if (isCableAttached && !isConnected)
        {
            Destroy(transform.GetChild(0).gameObject);
            isCableAttached = false;
        }

        if (isFacingRight && !isExtending)
        {
            spriteRenderer.sprite = pistonRightDefault;
            spriteRenderer.sortingOrder = 1;
        }
        else if (isFacingLeft && !isExtending)
        {
            spriteRenderer.sprite = pistonLeftDefault;
            spriteRenderer.sortingOrder = 1;
        }

        // Si cable voisins, cable attaché
        Element.TypeElement voisinGauche = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        Element.TypeElement voisinDroite = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());

        if (isFacingLeft && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau)))
        {
            isConnected = true;
        } else if (isFacingRight && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau))) {
            isConnected = true;
        } else
        {
            isConnected = false;
        }

    }

    // TEST
    // TODO : Commencer l'animation pas à un click de la souris mais quand on presse le bouton Start animation
    public void StartAnimation()
    {
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

    public void LaunchBall(Vector2Int ballDirection, float energy)
    {
        StartAnimation();
        ball.bodyType = RigidbodyType2D.Dynamic;
        
        float speed = Mathf.Sqrt(2 * energy / ballMass);
        
        ball.velocity = speed * new Vector2(ballDirection.x, ballDirection.y);

        Debug.Log("test");
    }
}
