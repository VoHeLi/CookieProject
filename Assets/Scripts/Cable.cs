using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : Element
{

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite cableLeft;
    [SerializeField] private Sprite cableRight;
    [SerializeField] private Sprite cableLine;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {

        // Get des voisins
        Element.TypeElement voisinGauche = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        Element.TypeElement voisinDroite = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());
        

        // Change de sprite voisins pour changer de sprites (connecté à droite, à gauche, aux deux cotés)
        // TODO : vérifier que la case en dessous est différente d'une grille
        if (!canConnectLeft() && !canConnectRight())
        {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
        if (!canConnectLeft() && canConnectRight())
        {
            GetComponent<SpriteRenderer>().sprite = cableRight;
        }
        if (canConnectLeft() && !canConnectRight())
        {
            GetComponent<SpriteRenderer>().sprite = cableLeft;
        }
        if (canConnectLeft() && canConnectRight())
        {
            GetComponent<SpriteRenderer>().sprite = cableLine;
        }
        
    }

    private bool canConnectLeft()
    {
        GameObject voisinG = GrilleElementManager.instance.GetObjetAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        if (voisinG == null)
        {
            return false;
        }
        else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Eolienne_left ) || (voisinG.GetComponent<Element>().type == Element.TypeElement.Piston_right) || (voisinG.GetComponent<Element>().type == Element.TypeElement.Ventilateur_right))
        {
            return false;
        } else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Eolienne_up) && voisinG.GetComponent<Ventilateur>().getIsConnectedRight())
        {
            return false;
        }
        else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Eolienne_down) && voisinG.GetComponent<Ventilateur>().getIsConnectedRight())
        {
            return false;
        }
        else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Ventilateur_up) && voisinG.GetComponent<Ventilateur>().getIsConnectedRight())
        {
            return false;
        }
        else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Ventilateur_down) && voisinG.GetComponent<Ventilateur>().getIsConnectedRight())
        {
            return false;
        } else if (voisinG.GetComponent<Element>().type == Element.TypeElement.None)
        {
            return false;
        } else
        {
            return true;
        }
    }

    private bool canConnectRight()
    {
        GameObject voisinD = GrilleElementManager.instance.GetObjetAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());

        if (voisinD == null)
        {
            return false;
        } else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Eolienne_right) || (voisinD.GetComponent<Element>().type == Element.TypeElement.Piston_left) || (voisinD.GetComponent<Element>().type == Element.TypeElement.Ventilateur_left))
        {
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Eolienne_up) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Eolienne_down) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Ventilateur_up) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Ventilateur_down) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            return false;
        }
        else if (voisinD.GetComponent<Element>().type == Element.TypeElement.None)
        {
            return false;
        }
        else if (voisinD.GetComponent<Element>().type == Element.TypeElement.Poteau && voisinD.GetComponent<SpriteRenderer>().flipX )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
