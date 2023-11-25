using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilateur : MonoBehaviour
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

    [SerializeField] private bool isConnected;
    private bool isCableAttached = false;
    private bool isBaseAttached = false;

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

    }


    // TODO : lancer les animations quand l'electricité ou le vent passe dans les pales
    private void OnMouseDown()
    {
        Debug.Log("Click");

        if (isFacingRight && isConnected)
        {
            StartCoroutine(startVentRightAnimation());
        }
        else if (isFacingLeft && isConnected)
        {
            StartCoroutine(startVentLeftAnimation());
        }
        else if (isFacingUp && isConnected)
        {
            StartCoroutine(startVentUpAnimation());
        }
        else if (isFacingDown && isConnected)
        {
            StartCoroutine(startVentDownAnimation());
        }
    }


    // Coroutines de lancement d'animation de tournoiement
    IEnumerator startVentRightAnimation()
    {
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
