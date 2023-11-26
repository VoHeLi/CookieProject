using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilateur : Element
{ 

    [SerializeField] private AudioSource _windSound;

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
    [SerializeField] private bool isCableAttached = false;
    private bool isBaseAttached = false;
    [SerializeField] private bool isBaseAttachedRight = false;
    [SerializeField] private bool isBaseAttachedLeft = false;

    [SerializeField] private bool isConnectedRight;
    [SerializeField] private bool isConnectedLeft;

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
            GameObject rightPistonCable = Instantiate(new GameObject("Base"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurRightBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }

        if (isFacingLeft && !isBaseAttached)
        {
            // Affichage de la base
            GameObject leftPistonCable = Instantiate(new GameObject("Base"), this.transform);
            SpriteRenderer cableSpriteRenderer = leftPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurLeftBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }

        if (isFacingUp && !isBaseAttached)
        {
            // Affichage de la base
            GameObject upPistonCable = Instantiate(new GameObject("Base"), this.transform);
            SpriteRenderer cableSpriteRenderer = upPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurUpBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }

        if (isFacingDown && !isBaseAttached)
        {
            // Affichage de la base
            GameObject downPistonCable = Instantiate(new GameObject("Base"), this.transform);
            SpriteRenderer cableSpriteRenderer = downPistonCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurDownBase;
            cableSpriteRenderer.sortingOrder = 1;

            isBaseAttached = true;
        }


        // Si cable voisins, cable attache
        Element.TypeElement voisinGauche = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        Element.TypeElement voisinDroite = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());

        if (isFacingLeft && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau) || (voisinDroite == Element.TypeElement.Ventilateur_right) || (voisinDroite == Element.TypeElement.Ventilateur_up) || (voisinDroite == Element.TypeElement.Ventilateur_down) || (voisinDroite == Element.TypeElement.Eolienne_left) || (voisinDroite == Element.TypeElement.Eolienne_up) || (voisinDroite == Element.TypeElement.Eolienne_down)))
        {
            isConnected = true;
        }
        else if (isFacingRight && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau) || (voisinGauche == Element.TypeElement.Ventilateur_left) || (voisinGauche == Element.TypeElement.Ventilateur_up) || (voisinGauche == Element.TypeElement.Ventilateur_down) || (voisinGauche == Element.TypeElement.Eolienne_right) || (voisinGauche == Element.TypeElement.Eolienne_up) || (voisinGauche == Element.TypeElement.Eolienne_down)))
        {
            isConnected = true;
        } else if (isFacingLeft && voisinDroite == Element.TypeElement.None) {
            isConnected = false;
        } else if (isFacingRight && voisinGauche == Element.TypeElement.None)
        {
            isConnected = false;
        }

        if (isFacingUp && !isConnectedLeft && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau) || (voisinGauche == Element.TypeElement.Ventilateur_left) || (voisinGauche == Element.TypeElement.Ventilateur_up) || (voisinGauche == Element.TypeElement.Ventilateur_down) || (voisinGauche == Element.TypeElement.Eolienne_right) || (voisinGauche == Element.TypeElement.Eolienne_up) || (voisinGauche == Element.TypeElement.Eolienne_down)))
        {
            isConnected = true;
            isConnectedRight = true;
        } 
        if (isFacingUp && !isConnectedRight && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau) || (voisinDroite == Element.TypeElement.Ventilateur_right) || (voisinDroite == Element.TypeElement.Ventilateur_up) || (voisinDroite == Element.TypeElement.Ventilateur_down) || (voisinDroite == Element.TypeElement.Eolienne_left) || (voisinDroite == Element.TypeElement.Eolienne_up) || (voisinDroite == Element.TypeElement.Eolienne_down)))
        {
            isConnected = true;
            isConnectedLeft = true;
        } 
        if (isFacingDown && !isConnectedLeft && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau) || (voisinGauche == Element.TypeElement.Ventilateur_left) || (voisinGauche == Element.TypeElement.Ventilateur_up) || (voisinGauche == Element.TypeElement.Ventilateur_down) || (voisinGauche == Element.TypeElement.Eolienne_right) || (voisinGauche == Element.TypeElement.Eolienne_up) || (voisinGauche == Element.TypeElement.Eolienne_down)))
        {
            isConnected = true;
            isConnectedRight = true;
        }
        if (isFacingDown && !isConnectedRight && ((voisinDroite == Element.TypeElement.Cable) || (voisinDroite == Element.TypeElement.Batterie) || (voisinDroite == Element.TypeElement.Poteau) || (voisinDroite == Element.TypeElement.Ventilateur_right) || (voisinDroite == Element.TypeElement.Ventilateur_up) || (voisinDroite == Element.TypeElement.Ventilateur_down) || (voisinDroite == Element.TypeElement.Eolienne_left) || (voisinDroite == Element.TypeElement.Eolienne_up) || (voisinDroite == Element.TypeElement.Eolienne_down)))
        {
            isConnected = true;
            isConnectedLeft = true;
        }

        if (isFacingUp && (voisinGauche == Element.TypeElement.None))
        {
            // isConnected = false;
            isConnectedRight = false;
        }
        if (isFacingUp && (voisinDroite == Element.TypeElement.None))
        {
            // isConnected = false;
            isConnectedLeft = false;
        }
        if (isFacingUp && (voisinDroite == Element.TypeElement.None) && (voisinGauche == Element.TypeElement.None))
        {
            isConnected = false;
        }
        if (isFacingDown && (voisinGauche == Element.TypeElement.None))
        {
            // isConnected = false;
            isConnectedRight = false;
        }
        if (isFacingDown && (voisinDroite == Element.TypeElement.None))
        {
            // isConnected = false;
            isConnectedLeft = false;
        }
        if (isFacingDown && (voisinDroite == Element.TypeElement.None) && (voisinGauche == Element.TypeElement.None))
        {
            isConnected = false;
        }

        if ((isFacingUp || isFacingDown) && (voisinDroite == Element.TypeElement.None) && (voisinGauche == Element.TypeElement.None))
        {
            isConnected = false;
            isConnectedLeft = false;
            isConnectedRight = false;
        }

        // Attachement des cables
        if (isConnected && !isCableAttached)
        {

            if (isFacingRight)
            {
                GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
                cableSpriteRenderer.sprite = ventilateurRightCable;
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
            if (isFacingLeft)
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
                if (isConnectedRight)
                {
                    cableSpriteRenderer.sprite = ventilateurUpCableRight;
                    isBaseAttachedRight = true;
                }
                if (isConnectedLeft)
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
                    isBaseAttachedRight = true;
                }
                if (isConnectedLeft)
                {
                    cableSpriteRenderer.sprite = ventilateurDownCableLeft;
                    isBaseAttachedLeft = true;
                }
                cableSpriteRenderer.sortingOrder = 0;

                isCableAttached = true;
            }
        }

        if (isCableAttached && !isConnected)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Base(Clone)") Destroy(child.gameObject);
            }
            isCableAttached = false;
            isBaseAttachedRight = false;
            isBaseAttachedLeft = false;
        }

        /* if (isFacingUp && (voisinDroite == Element.TypeElement.None) && ((voisinGauche == Element.TypeElement.Cable) || (voisinGauche == Element.TypeElement.Batterie) || (voisinGauche == Element.TypeElement.Poteau)))
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Base(Clone)") Destroy(child.gameObject);
            }

        } */

        if (isFacingUp && isBaseAttachedLeft && isConnectedRight)
        {
            isBaseAttachedLeft = false;
            isBaseAttachedRight = true;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Base(Clone)") Destroy(child.gameObject);
            }
            GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurUpCableRight;
            cableSpriteRenderer.sortingOrder = 0;

        }
        if (isFacingUp && isBaseAttachedRight && isConnectedLeft)
        {
            isBaseAttachedRight = false;
            isBaseAttachedLeft = true;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Base(Clone)") Destroy(child.gameObject);
            }
            GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurUpCableLeft;
            cableSpriteRenderer.sortingOrder = 0;
        }
        if (isFacingDown && isBaseAttachedLeft && isConnectedRight)
        {
            isBaseAttachedLeft = false;
            isBaseAttachedRight = true;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Base(Clone)") Destroy(child.gameObject);
            }
            GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurDownCableRight;
            cableSpriteRenderer.sortingOrder = 0;

        }
        if (isFacingDown && isBaseAttachedRight && isConnectedLeft)
        {
            isBaseAttachedRight = false;
            isBaseAttachedLeft = true;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Base(Clone)") Destroy(child.gameObject);
            }
            GameObject rightVentilateurCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightVentilateurCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = ventilateurDownCableLeft;
            cableSpriteRenderer.sortingOrder = 0;
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
            if (obj.transform.parent == null && obj.name == "CableObject" || obj.transform.parent == null && obj.name == "Base")
            {
                Destroy(obj);
            }
        }


    }




    public void StartAnimation()
    {
        // Debug.Log("Click");
        _windSound?.Play();
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

    public bool getIsConnected()
    {
        return this.isConnected;
    }
    public bool getIsConnectedRight()
    {
        return this.isConnectedRight;
    }
    public bool getIsConnectedLeft()
    {
        return this.isConnectedLeft;
    }


    public override void StopAnimation()
    {
        _windSound.Stop();
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
