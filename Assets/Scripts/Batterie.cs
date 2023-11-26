using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batterie : Element
{

    private float energy;

    [SerializeField] private Sprite batterie100;
    [SerializeField] private Sprite batterie75;
    [SerializeField] private Sprite batterie50;
    [SerializeField] private Sprite batterie25;
    [SerializeField] private Sprite batterie0;

    [SerializeField] private float dureeAnimation = 5f;
    [SerializeField] private float animationFPS = 10f;

    [SerializeField] private Sprite cableBatterieRight;
    [SerializeField] private Sprite cableBatterieLeft;

    [SerializeField] private Sprite smallSpark;
    [SerializeField] private Sprite bigSpark;

    [SerializeField] private bool isTargetBatterie;

    private bool isBatterieConnectedRight = false;
    private bool isBatterieConnectedLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isTargetBatterie)
        {
            energy = 0f;
            
        } else
        {
            energy = 100f;
        }
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        Debug.Log("Energy de depart : " + energy);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Energy = " + energy);

        if (energy <= 100f && energy > 75f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie100;
        }
        if (energy <= 75f && energy > 50f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie75;
        }
        if (energy <= 50f && energy > 25f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie50;
        }
        if (energy <= 25f && energy > 0f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie25;
        }
        if (energy == 0)
        {
            GetComponent<SpriteRenderer>().sprite = batterie0;
        }

        // Recherche des voisins
        Element.TypeElement voisinGauche = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        Element.TypeElement voisinDroite = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());

        // Change de sprite voisins pour changer de sprites (connecté à droite, à gauche, aux deux cotés)
        // TODO : vérifier que la case en dessous est différente d'une grille
        if (!canConnectLeft() && !canConnectRight())
        {
            if (transform.childCount >= 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            if (transform.childCount >= 2)
            {
                Destroy(transform.GetChild(1).gameObject);
            }
        }
        if (!canConnectLeft() && canConnectRight() && !isBatterieConnectedRight)
        {
            if (transform.childCount >= 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            GameObject rightBatterieCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = rightBatterieCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = cableBatterieRight;
            cableSpriteRenderer.sortingOrder = 0;

            isBatterieConnectedRight = true;
            isBatterieConnectedLeft = false;
        }
        if (canConnectLeft() && !canConnectRight() && !isBatterieConnectedLeft)
        {
            if (transform.childCount >= 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            GameObject leftBatterieCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer = leftBatterieCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer.sprite = cableBatterieLeft;
            cableSpriteRenderer.sortingOrder = 0;

            isBatterieConnectedRight = false;
            isBatterieConnectedLeft = true;
        }
        if (canConnectLeft() && canConnectRight())
        {
            GameObject rightBatterieCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer0 = rightBatterieCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer0.sprite = cableBatterieRight;
            cableSpriteRenderer0.sortingOrder = 0;
            GameObject leftBatterieCable = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer cableSpriteRenderer1 = leftBatterieCable.AddComponent<SpriteRenderer>();
            cableSpriteRenderer1.sprite = cableBatterieLeft;
            cableSpriteRenderer1.sortingOrder = 0;

            isBatterieConnectedRight = true;
            isBatterieConnectedLeft = true;
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


    // Coroutine qui baisse le niveau de l'énergie en $dureeAnimation secondes
    public IEnumerator StartDrainingAnimation()
    {
        int i = 1;

        for (float energyLevel = 100f; energyLevel >= 0; energyLevel -= 100/(animationFPS*dureeAnimation))
        {
            energy = energyLevel;

            if (i == 5)
            {
                if (this.transform.childCount >= 1)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                GameObject sparkSmall = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer smallSparkSpriteRenderer = sparkSmall.AddComponent<SpriteRenderer>();
                smallSparkSpriteRenderer.sprite = smallSpark;
                smallSparkSpriteRenderer.sortingOrder = 2;
            }
            Debug.Log(i);
            i++;

            yield return new WaitForSeconds(1/(animationFPS*2));

            if (i == 10)
            {
                if (this.transform.childCount >= 1)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                GameObject sparkBig = Instantiate(new GameObject("CableObject"), this.transform);
                SpriteRenderer bigSparkSpriteRenderer = sparkBig.AddComponent<SpriteRenderer>();
                bigSparkSpriteRenderer.sprite = bigSpark;
                bigSparkSpriteRenderer.sortingOrder = 2;

                i = 1;
            } else
            {
                i++;
            }

            yield return new WaitForSeconds(1 / (animationFPS * 2));
        }
    }
    public IEnumerator StartFillingAnimation(float energy)
    {
        for (float energyLevel = 0f; energyLevel >= 100; energyLevel += 100 / (animationFPS * dureeAnimation))
        {
            energy = energyLevel;

            if (this.transform.childCount >= 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            GameObject sparkBig = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer bigSparkSpriteRenderer = sparkBig.AddComponent<SpriteRenderer>();
            bigSparkSpriteRenderer.sprite = bigSpark;
            bigSparkSpriteRenderer.sortingOrder = 2;

            yield return new WaitForSeconds(1 / animationFPS);

            if (this.transform.childCount >= 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            GameObject sparkSmall = Instantiate(new GameObject("CableObject"), this.transform);
            SpriteRenderer smallSparkSpriteRenderer = sparkSmall.AddComponent<SpriteRenderer>();
            smallSparkSpriteRenderer.sprite = smallSpark;
            smallSparkSpriteRenderer.sortingOrder = 2;

            yield return new WaitForSeconds(1 / animationFPS);

        }

        if (this.transform.childCount >= 1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    private bool canConnectLeft()
    {
        GameObject voisinG = GrilleElementManager.instance.GetObjetAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        if (voisinG == null)
        {
            return false;
        }
        else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Eolienne_left) || (voisinG.GetComponent<Element>().type == Element.TypeElement.Piston_right) || (voisinG.GetComponent<Element>().type == Element.TypeElement.Ventilateur_right))
        {
            return false;
        }
        else if ((voisinG.GetComponent<Element>().type == Element.TypeElement.Eolienne_up) && voisinG.GetComponent<Ventilateur>().getIsConnectedRight())
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
        }
        else if (voisinG.GetComponent<Element>().type == Element.TypeElement.None)
        {
            return false;
        }
        else
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
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Eolienne_right) || (voisinD.GetComponent<Element>().type == Element.TypeElement.Piston_left) || (voisinD.GetComponent<Element>().type == Element.TypeElement.Ventilateur_left))
        {
            Debug.Log("Debug1");
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Eolienne_up) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            Debug.Log("Debug2");
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Eolienne_down) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            Debug.Log("Debug3");
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Ventilateur_up) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            Debug.Log("Debug4");
            return false;
        }
        else if ((voisinD.GetComponent<Element>().type == Element.TypeElement.Ventilateur_down) && voisinD.GetComponent<Ventilateur>().getIsConnectedLeft())
        {
            Debug.Log("Debug5");
            return false;
        }
        else if (voisinD.GetComponent<Element>().type == Element.TypeElement.None)
        {
            Debug.Log("Debug6");
            return false;
        }
        else
        {
            Debug.Log("Debug7");
            return true;
        }
    }
}
