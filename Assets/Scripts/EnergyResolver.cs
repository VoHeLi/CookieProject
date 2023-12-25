using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnergyResolver : MonoBehaviour
{
    public static EnergyResolver instance;

    public string nextLevel;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of EnergyResolver!");
        }
        instance = this;
    }

    public class GraphNode
    {
        public float rendement;
        public float animationTime;
        public List<GraphNode> sourceNodes;
        public List<GraphNode> destNodes;
        public Vector2Int spatialPosition;
        public Element.TypeElement type;
        public int distanceToNextNode;
        public bool spawnBall = false;
        public Vector2Int ballDirection;
    }


    [SerializeField] private GameObject debugEnergyPrefab;
    private List<GameObject> debugEnergyObjects = new List<GameObject>();

    [SerializeField] private GameObject windPrefab;

    private List<GameObject> particleEffects;

    private bool[,] visitedElements;

    [SerializeField] private List<Element.TypeElement> leftElectricityElements;
    [SerializeField] private List<Element.TypeElement> rightElectricityElements;
    [SerializeField] private List<Element.TypeElement> upElectricityElements;
    [SerializeField] private List<Element.TypeElement> downElectricityElements;

    [SerializeField] private List<Element.TypeElement> ventilatorElements;
    [SerializeField] private List<Element.TypeElement> eoliennesElements;
    [SerializeField] private List<Element.TypeElement> pistonsElements;

    private AudioSource _victorySource;
    private AudioSource _defeatSource;


    private void Start()
    {
        _victorySource = GameObject.Find("VictorySound").GetComponent<AudioSource>();
        _defeatSource = GameObject.Find("DefeatSound").GetComponent<AudioSource>();
    }

    public void ResolveLevelPart(GrilleElementManager grilleElementManager, Vector2Int startPosition)
    {
        /*if(visitedElements != null)
        ClearEnergyObjects();*/

        visitedElements = new bool[GlobalGrid.nbCaseX, GlobalGrid.nbCaseY]; //RESET ONLY FOR THE FIRST MECHANICAL PART OF THE LEVEL
        ResolveLevelPart(grilleElementManager, startPosition, 1.0f); //TODO : CHANGE START ENERGY
    }

        //On va creer un graphe de noeuds, chaque noeud represente un element avec son rendement, son temps d'animation, ses noeuds sources et ses noeuds finaux
        //On commence de la fin, puis on remonte jusqu'au debut
    public void ResolveLevelPart(GrilleElementManager grilleElementManager, Vector2Int startPosition, float energy)
    {
        if(grilleElementManager.sourcePosition.x == -1000)
        {
            Debug.LogError("No source position set!");
            return;
        }
        
        
        

        Queue<GraphNode> nodesToProcess = new Queue<GraphNode>();

        GraphNode beginNode = new GraphNode();
        beginNode.rendement = 1f;
        beginNode.animationTime = 0.1f;
        beginNode.sourceNodes = new List<GraphNode>();
        beginNode.destNodes = new List<GraphNode>();
        beginNode.spatialPosition = startPosition;
        beginNode.type = Element.TypeElement.Batterie;
        beginNode.distanceToNextNode = 0;

        visitedElements[beginNode.spatialPosition.x, beginNode.spatialPosition.y] = true;


        nodesToProcess.Enqueue(beginNode);
        while (nodesToProcess.Count > 0)
        {
            GraphNode currentNode = nodesToProcess.Dequeue();

            if(currentNode.type == Element.TypeElement.TargetBattery)
            {
                Debug.Log("finished!");
                break;
            }


            if (ventilatorElements.Contains(currentNode.type))
            {
                ProcessVentilateurNode(currentNode, grilleElementManager, nodesToProcess);
                continue;
            }

            if (pistonsElements.Contains(currentNode.type))
            {
                ProcessPistonNode(currentNode, grilleElementManager, nodesToProcess);
                continue;
            }

           
            ProcessElectricityNode(currentNode, grilleElementManager, nodesToProcess);

        }

        StartCoroutine(DisplayGraphAnimation(beginNode, energy));

    }

    private void ProcessElectricityNode(GraphNode node, GrilleElementManager grilleElementManager, Queue<GraphNode> nodesToProcess)
    {
        List<Vector2Int> neighbours = GetElectricityNeighbours(node.spatialPosition.x, node.spatialPosition.y, node.type, grilleElementManager);

        //Remove already explored Node 

        List<Vector2Int> newNeighbours = new List<Vector2Int>();
        foreach (Vector2Int neighbour in neighbours)
        {
            if (!visitedElements[neighbour.x, neighbour.y])
            {
                newNeighbours.Add(neighbour);
            }
        }

        if(newNeighbours.Count == 0)
        {
            return;
        }

        Vector2Int neightbour = newNeighbours[0]; //TODO : TAKE THE HIGHEST PRIORITY NEIGHBOUR
        foreach(Vector2Int otherPossibility in newNeighbours)
        {
            if (grilleElementManager.GetElementTypeAtPosition(otherPossibility.x, otherPossibility.y) == Element.TypeElement.Poteau)
            {
                neightbour = otherPossibility;
                break;
            }
        }
            
        GraphNode neighbourNode = new GraphNode();
        neighbourNode.rendement = 0.99f;
        neighbourNode.animationTime = 0.2f;
        neighbourNode.sourceNodes = new List<GraphNode>();
        neighbourNode.destNodes = new List<GraphNode>();
        neighbourNode.spatialPosition = neightbour;
        neighbourNode.type = grilleElementManager.GetElementTypeAtPosition(neightbour.x, neightbour.y);

        visitedElements[neightbour.x, neightbour.y] = true;

        node.destNodes.Add(neighbourNode);
        neighbourNode.sourceNodes.Add(node);


        // Debug.Log("Adding node to process : " + neighbourNode.spatialPosition);
        nodesToProcess.Enqueue(neighbourNode);
        
    }

    private List<Vector2Int> GetElectricityNeighbours(int targetX, int targetY, Element.TypeElement currentElement, GrilleElementManager grilleElementManager)
    {
        Element.TypeElement leftElement = grilleElementManager.GetElementTypeAtPosition(targetX - 1, targetY);
        Element.TypeElement rightElement = grilleElementManager.GetElementTypeAtPosition(targetX + 1, targetY);
        Element.TypeElement upElement = grilleElementManager.GetElementTypeAtPosition(targetX, targetY + 1);
        Element.TypeElement downElement = grilleElementManager.GetElementTypeAtPosition(targetX, targetY - 1);

        List<Vector2Int> neighbours = new List<Vector2Int>();



        if (upElectricityElements.Contains(upElement) && downElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX, targetY + 1));
        }
        if (downElectricityElements.Contains(downElement) && upElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX, targetY - 1));
        }
        if (leftElectricityElements.Contains(leftElement) && rightElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX - 1, targetY));
        }
        if (rightElectricityElements.Contains(rightElement) && leftElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX + 1, targetY));
        }


        return neighbours;
    }

    private void ProcessVentilateurNode(GraphNode node, GrilleElementManager grilleElementManager, Queue<GraphNode> nodesToProcess)
    {
        Vector2Int windDir = Vector2Int.zero;
        switch (node.type)
        {
            case Element.TypeElement.Ventilateur_down:
                windDir = Vector2Int.down;
                break;
            case Element.TypeElement.Ventilateur_left:
                windDir = Vector2Int.left;
                break;
            case Element.TypeElement.Ventilateur_right:
                windDir = Vector2Int.right;
                break;
            case Element.TypeElement.Ventilateur_up:
                windDir = Vector2Int.up;
                break;
        }

        int distance = 1;
        float rendement = 0.95f; //TODO : GET REAL RENDEMENT
        Vector2Int nextBlock = node.spatialPosition + windDir;
        while(!eoliennesElements.Contains(grilleElementManager.GetElementTypeAtPosition(nextBlock.x, nextBlock.y)))
        {
            // Debug.Log("Next block : " + nextBlock); 

            nextBlock += windDir;
            rendement *= 0.95f;
            distance++;

            if (!GlobalGrid.IsInGrid(nextBlock.x, nextBlock.y) || !GetBlock.instance.CanWindGoThrought(nextBlock.x, nextBlock.y))
            {
                node.distanceToNextNode = distance;
                nextBlock -= windDir;
                return;
            }

            if(distance >= 20)
            {
                node.distanceToNextNode = distance;
                return;
            }
        }


        // Debug.Log("Distance to next Node : " + distance + " SpatialPos : " + node.spatialPosition.ToString());
        node.distanceToNextNode = distance;

        GraphNode neighbourNode = new GraphNode();
        neighbourNode.rendement = rendement;
        neighbourNode.animationTime = 0.2f*distance;
        neighbourNode.sourceNodes = new List<GraphNode>();
        neighbourNode.destNodes = new List<GraphNode>();
        neighbourNode.spatialPosition = nextBlock;
        neighbourNode.type = grilleElementManager.GetElementTypeAtPosition(nextBlock.x, nextBlock.y);
        visitedElements[nextBlock.x, nextBlock.y] = true; //TODO CHECK IN GRILLE



        node.destNodes.Add(neighbourNode);
        neighbourNode.sourceNodes.Add(node);

        // Debug.Log("Adding node to process : " + neighbourNode.spatialPosition);
        nodesToProcess.Enqueue(neighbourNode);
    }

    private void ProcessPistonNode(GraphNode node, GrilleElementManager grilleElementManager, Queue<GraphNode> nodesToProcess)
    {
        node.spawnBall = true;
        node.ballDirection = node.type == Element.TypeElement.Piston_right ? Vector2Int.right : Vector2Int.left;
    }
    public IEnumerator DisplayGraphAnimation(GraphNode beginNode, float energy)
    {
        /*if(debugEnergyObjects != null)
        {
            ClearEnergyObjects();
        }*/

        debugEnergyObjects = new List<GameObject>();

        Queue<GraphNode> nodesToProcess = new Queue<GraphNode>();
        nodesToProcess.Enqueue(beginNode);

        Batterie sourceBattery = GrilleElementManager.instance.elementObjects[GrilleElementManager.instance.sourcePosition.x, GrilleElementManager.instance.sourcePosition.y].GetComponent<Batterie>();
        StartCoroutine(sourceBattery.StartDrainingAnimation());

        GraphNode lastNode = beginNode;
        while (nodesToProcess.Count > 0)
        {
            GraphNode currentNode = nodesToProcess.Dequeue();

            yield return new WaitForSeconds(currentNode.animationTime);

            if(UI.instance.isRunning == false)
            {
                break;
            }

            if (currentNode.type == Element.TypeElement.TargetBattery)
            {
                // Debug.Log("Draining...");
                _victorySource.Play();
                StartCoroutine(DrainBattery(currentNode, energy));
                break;
            }

            energy *= currentNode.rendement;

            DisplayNode(currentNode, energy);

            foreach (GraphNode destNode in currentNode.destNodes)
            {
                nodesToProcess.Enqueue(destNode);
            }

            lastNode = currentNode;
        }

        //DEFEAT
        if(lastNode.type != Element.TypeElement.Piston_left && lastNode.type != Element.TypeElement.Piston_right)
        {
            _defeatSource.Play();
        }

    }

    private IEnumerator DrainBattery(GraphNode destNode, float energy)
    {
        Batterie targetBattery = GrilleElementManager.instance.elementObjects[destNode.spatialPosition.x, destNode.spatialPosition.y].GetComponent<Batterie>();
        

        StartCoroutine(targetBattery.StartFillingAnimation(energy));

        

        UI.instance.levelClearDisplay();

        yield return new WaitForSeconds(5.0f);

        if(nextLevel != null && nextLevel != " ")
        {
            SceneManager.LoadSceneAsync(nextLevel);
        }
    }

    public void DisplayNode(GraphNode node, float energy)
    {
        GameObject debugEnergyObject = Instantiate(debugEnergyPrefab, new Vector3(node.spatialPosition.x * GlobalGrid.caseSize, node.spatialPosition.y * GlobalGrid.caseSize, 0), Quaternion.identity);
        debugEnergyObject.transform.parent = transform;
        debugEnergyObject.transform.localScale = Vector3.one * energy * 0.1f;
        debugEnergyObjects.Add(debugEnergyObject);

        if (ventilatorElements.Contains(node.type))
        {
            DisplayWind(node);
            GrilleElementManager.instance.elementObjects[node.spatialPosition.x, node.spatialPosition.y].GetComponent<Ventilateur>().StartAnimation();
        }

        if (eoliennesElements.Contains(node.type))
        {
            GrilleElementManager.instance.elementObjects[node.spatialPosition.x, node.spatialPosition.y].GetComponent<Ventilateur>().StartAnimation();
        }

        if(node.spawnBall)
        {
            GrilleElementManager.instance.elementObjects[node.spatialPosition.x, node.spatialPosition.y].GetComponent<Piston>().LaunchBall(node.ballDirection, energy*30);
        }
    }

    private void DisplayWind(GraphNode node)
    {
        GameObject wind = Instantiate(windPrefab, new Vector3(node.spatialPosition.x * GlobalGrid.caseSize, node.spatialPosition.y * GlobalGrid.caseSize, 0), Quaternion.identity);
        wind.transform.parent = transform;
        debugEnergyObjects.Add(wind);

        WindParticleSettings windParticleSettings = wind.GetComponent<WindParticleSettings>();
        windParticleSettings.SetWindLength(node.distanceToNextNode);

        switch (node.type)
        {
            case Element.TypeElement.Ventilateur_up:
                wind.transform.rotation = Quaternion.Euler(0, 0, 90);
                wind.transform.position += Vector3.up * GlobalGrid.caseSize * 0.5f;
                break;
            case Element.TypeElement.Ventilateur_left:
                wind.transform.rotation = Quaternion.Euler(0, 0, 180);
                wind.transform.position -= Vector3.right * GlobalGrid.caseSize * 0.5f;
                break;
            case Element.TypeElement.Ventilateur_right:
                wind.transform.rotation = Quaternion.Euler(0, 0, 0);
                wind.transform.position -= Vector3.left * GlobalGrid.caseSize * 0.5f;
                break;
            case Element.TypeElement.Ventilateur_down:
                wind.transform.rotation = Quaternion.Euler(0, 0, 270);
                wind.transform.position += Vector3.down * GlobalGrid.caseSize * 0.5f;
                break;
        }
    }

    public void ClearEnergyObjects()
    {
        foreach (GameObject debugEnergyObject in debugEnergyObjects)
        {
            Destroy(debugEnergyObject);
        }
        //debugEnergyObjects.Clear();

        // Debug.Log(visitedElements.GetLength(0) + "," + visitedElements.GetLength(1));
        for(int i = 0; i < visitedElements.GetLength(0); i++)
        {
            for(int j = 0; j < visitedElements.GetLength(1); j++)
            {
                // Debug.Log("ici : (" + i + ";" + j + ")");
                if (visitedElements[i, j])
                {
                    GameObject gO = GrilleElementManager.instance.elementObjects[i, j];
                    gO.GetComponent<Element>().StopAnimation();

                    Element.TypeElement type = GrilleElementManager.instance.elementMaps[i, j];
                    // GrilleElementManager.instance.elementMaps[i, j] = Element.TypeElement.None;
                    // GrilleElementManager.instance.UpdateElementObject(i, j, false);
                    GrilleElementManager.instance.elementMaps[i, j] = type;
                    // GrilleElementManager.instance.UpdateElementObject(i, j, false);
                    if(type == Element.TypeElement.Piston_left || type == Element.TypeElement.Piston_right)
                    {
                        gO.TryGetComponent<Piston>(out Piston piston);
                        piston.ResetBall();
                    }
                    // Debug.Log(type);

                }




                //visitedElements[i, j] = false;
            }
        }

        // GrilleElementManager.instance.LoadInitialElements();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Resolving level...");
            ResolveLevelPart(GrilleElementManager.instance, GrilleElementManager.instance.sourcePosition);
        }
    }
}
