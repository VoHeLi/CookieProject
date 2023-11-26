using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilateur : Element
{ 

    [Header("Sprites ventilateur right")]
    [SerializeField] private Sprite[] ventilateurRightAnimation;
    [SerializeField] private Sprite ventilateurRightDefault;
    [SerializeField] private Sprite ventilateurRightBase;
    [SerializeField] private Sprite ventilateurRightCable;

    [Header("Sprites ventilateur left")]
    [SerializeField] private Sprite[] ventilateurLeftAnimation;
    [SerializeField] private Sprite ventilateurLeftDefault;
    [SerializeField] private Sprite ventilateurLeftBase;
    [SerializeField] private Sprite ventilateurLeftCable;

    [Header("Sprites ventilateur up")]
    [SerializeField] private Sprite[] ventilateurUpAnimation;
    [SerializeField] private Sprite ventilateurUpDefault;
    [SerializeField] private Sprite ventilateurUpBase;
    [SerializeField] private Sprite ventilateurUpCableRight;
    [SerializeField] private Sprite ventilateurUpCableLeft;

    [Header("Sprites ventilateur down")]
    [SerializeField] private Sprite[] ventilateurDownAnimation;
    [SerializeField] private Sprite ventilateurDownDefault;
    [SerializeField] private Sprite ventilateurDownBase;
    [SerializeField] private Sprite ventilateurDownCableRight;
    [SerializeField] private Sprite ventilateurDownCableLeft;

    [Space]
    [Header("Debug")]
    [SerializeField] private bool isFacingRight;
    [SerializeField] private bool isFacingLeft;
    [SerializeField] private bool isFacingUp;
    [SerializeField] private bool isFacingDown;

    [SerializeField] private bool isBlowing;

    [SerializeField] private bool isConnected = false;
    private bool isCableAttached = false;
    private bool isBaseAttached = false;
    private bool isBaseAttachedRight = false;
    private bool isBaseAttachedLeft = false;

    private bool isConnectedRight;
    private bool isConnectedLeft;

    public bool stopAnimation = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Affichage des bases
        if (isFacingRight && !isBaseAttached)
        {
            // Affichage de la base
            GameObject rightPistonCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurRightBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }

        if (isFacingLeft && !isBaseAttached)
        {
            // Affichage de la base
            GameObject leftPistonCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = leftPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurLeftBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }

        if (isFacingUp && !isBaseAttached)
        {
            // Affichage de la base
            GameObject upPistonCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = upPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurUpBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }

        if (isFacingDown && !isBaseAttached)
        {
            // Affichage de la base
            GameObject downPistonCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = downPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurDownBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }


        // Si cable voisins, cable attache
        Element.TypeElement voisinGauche = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        Element.TypeElement voisinDroite = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());

        if (isFacingLeft && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau)))
        {
            isConnected = true;
        }
        else if (isFacingRight && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau)))
        {
            isConnected = true;
        }
        else if (isFacingLeft || isFacingRight)
        {
            isConnected = false;
        }

        if (isFacingUp && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau)))
        {
            isConnected = true;
            isConnectedRight = true;
        } 
        else if (isFacingUp && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau)))
        {
            isConnected = true;
            isConnectedLeft = true;
        } 
        else if (isFacingDown && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau)))
        {
            isConnected = true;
            isConnectedRight = true;
        }
        else if (isFacingDown && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau)))
        {
            isConnected = true;
            isConnectedLeft = true;
        } else if (isFacingUp || isFacingDown)
        {
            isConnected = false;
            isConnectedLeft = false;
            isConnectedRight = false;
        }

        // Attachement des cables
        if (isConnected && (!isCableAttached || !isBaseAttachedRight || !isBaseAttachedLeft)) {

            if (isFacingRight && !isCableAttached)
            {
                GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
                cableSpriteRenderer.sprite = ventilateurRightCable;
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
            if (isFacingLeft && !isCableAttached)
            {
                GameObject leftVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer cableSpriteRenderer = leftVentilateurCable.AddComponent<SpriteRenderer>();
                cableSpriteRenderer.sprite = ventilateurLeftCable;
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
            if (isFacingUp)
            {
                GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
                if (isConnectedRight && !isBaseAttachedRight)
                {
                    cableSpriteRenderer.sprite = ventilateurUpCableRight;
                    isBaseAttachedRight = true;
                }
                if (isConnectedLeft && !isBaseAttachedLeft)
                {
                    cableSpriteRenderer.sprite = ventilateurUpCableLeft;
                    isBaseAttachedLeft = true;
                }
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
            if (isFacingDown)
            {
                GameObject downVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer cableSpriteRenderer = downVentilateurCable.AddComponent<SpriteRenderer>();
                if (isConnectedRight)
                {
                    cableSpriteRenderer.sprite = ventilateurDownCableRight;
                }
                if (isConnectedLeft)
                {
                    cableSpriteRenderer.sprite = ventilateurDownCableLeft;
                }
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
        }

        if (isCableAttached && !isConnected)
        {
            Destroy(transform.GetChild(0).gameObject);
            isCableAttached = false;
            isBaseAttachedRight = false;
            isBaseAttachedLeft = false;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (isFacingRight && !isBlowing)
        {
            spriteRenderer.sprite = ventilateurRightDefault;
            spriteRenderer.sortingOrder = 2;
        }
        if (isFacingLeft && !isBlowing)
        {
            spriteRenderer.sprite = ventilateurLeftDefault;
            spriteRenderer.sortingOrder = 2;
        }
        if (isFacingUp && !isBlowing)
        {
            spriteRenderer.sprite = ventilateurUpDefault;
            spriteRenderer.sortingOrder = 2;
        }
        if (isFacingDown && !isBlowing)
        {
            spriteRenderer.sprite = ventilateurDownDefault;
            spriteRenderer.sortingOrder = 2;
        }

        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();


        foreach (var obj in allGameObjects)
        {
            if (obj.transform.parent == null && obj.name == "CableObject")
            {
                Destroy(obj);
            }
        }


    }




    public void StartAnimation()
    {
        // Debug.Log("Click");

        if (isFacingRight)
        {
            StartCoroutine(startVentRightAnimation());
        }
        else if (isFacingLeft)
        {
            StartCoroutine(startVentLeftAnimation());
        }
        else if (isFacingUp)
        {
            StartCoroutine(startVentUpAnimation());
        }
        else if (isFacingDown)
        {
            StartCoroutine(startVentDownAnimation());
        }
    }


    public override void StopAnimation()
    {
        stopAnimation = true;
    }


    // Coroutines de lancement d'animation de tournoiement
    IEnumerator startVentRightAnimation()
    {
        stopAnimation = false;
        isBlowing = true;
        int i = 0;
        while(true)
        {
            if (i >= ventilateurRightAnimation.Length)
            {
                i = 0;
            }

            GetComponent<SpriteRenderer>().sprite = ventilateurRightAnimation[i];
            i++;


            if (stopAnimation)
            {
                GetComponent<SpriteRenderer>().sprite = ventilateurRightDefault;
                break;
            }

            yield return new WaitForSeconds(.1f);

        }

        isBlowing = false;
    }
    IEnumerator startVentLeftAnimation()
    {
        stopAnimation = false;
        isBlowing = true;
        int i = 0;
        while (true)
        {
            if (i >= ventilateurLeftAnimation.Length)
            {
                i = 0;
            }

            GetComponent<SpriteRenderer>().sprite = ventilateurLeftAnimation[i];
            i++;


            if (stopAnimation)
            {
                GetComponent<SpriteRenderer>().sprite = ventilateurLeftDefault;
                break;
            }

            yield return new WaitForSeconds(.1f);

        }

        isBlowing = false;
    }
    IEnumerator startVentUpAnimation()
    {
        stopAnimation = false;
        isBlowing = true;
        int i = 0;
        while (true)
        {
            if (i >= ventilateurUpAnimation.Length)
            {
                i = 0;
            }

            GetComponent<SpriteRenderer>().sprite = ventilateurUpAnimation[i];
            i++;


            if (stopAnimation)
            {
                GetComponent<SpriteRenderer>().sprite = ventilateurUpDefault;
                break;
            }

            yield return new WaitForSeconds(.1f);

        }

        isBlowing = false;
    }
    IEnumerator startVentDownAnimation()
    {
        stopAnimation = false;
        isBlowing = true;
        int i = 0;
        while (true)
        {
            if (i >= ventilateurDownAnimation.Length)
            {
                i = 0;
            }

            GetComponent<SpriteRenderer>().sprite = ventilateurDownAnimation[i];
            i++;


            if (stopAnimation)
            {
                GetComponent<SpriteRenderer>().sprite = ventilateurDownDefault;
                break;
            }

            yield return new WaitForSeconds(.1f);

        }

        isBlowing = false;
    }
}
