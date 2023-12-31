using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiPiston : MonoBehaviour
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
                // Cr�ez un nouvel objet GameObject vide
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

        // Si cable voisins, cable attach�
        if (isFacingLeft && GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos()) == Element.TypeElement.Cable)
        {
            isConnected = true;
        } else if (isFacingRight && GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos()) == Element.TypeElement.Cable) {
            isConnected = true;
        } else
        {
            isConnected = false;
        }

    }

    // TEST
    // TODO : Commencer l'animation pas � un click de la souris mais quand on presse le bouton Start animation
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("Truc toucher piston.");
        if (collision.collider.gameObject.tag == "Ball") //COUILLE
        {
            // Debug.Log("Baballe toucher piston.");

            Vector2 idealDir = isFacingLeft ? Vector2.right : Vector2.left;

            float speed = Vector2.Dot(collision.rigidbody.velocity, idealDir);

            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.gameObject.transform.position = Vector3.zero;
            //Destroy(collision.collider.gameObject);

            float energy = 0.5f * ballMass * speed * speed;

            EnergyResolver.instance.ResolveLevelPart(GrilleElementManager.instance, GlobalGrid.GetGridPosition(transform.position), 1.0f); //TODO
        }
    }
}
