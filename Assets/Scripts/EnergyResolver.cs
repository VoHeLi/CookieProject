using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnergyResolver : MonoBehaviour
{
    public static EnergyResolver instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of EnergyResolver!");
        }
        instance = this;
    }

    public struct GraphNode
    {
        public float rendement;
        public float animationTime;
        public List<GraphNode> sourceNodes;
        public List<GraphNode> destNodes;
        public Vector2Int spatialPosition;
        public Element.TypeElement type;
    }

    [SerializeField] private GameObject debugEnergyPrefab;
    private List<GameObject> debugEnergyObjects = new List<GameObject>();

    private bool[,] visitedElements;

    [SerializeField] private List<Element.TypeElement> leftElectricityElements;
    [SerializeField] private List<Element.TypeElement> rightElectricityElements;
    [SerializeField] private List<Element.TypeElement> upElectricityElements;
    [SerializeField] private List<Element.TypeElement> downElectricityElements;

    [SerializeField] private List<Element.TypeElement> ventilatorElements;
    [SerializeField] private List<Element.TypeElement> eoliennesElements;
    
    //On va créer un graphe de noeuds, chaque noeud représente un élément avec son rendement, son temps d'animation, ses noeuds sources et ses noeuds finaux
    //On commence de la fin, puis on remonte jusqu'au début
    public void ResolveLevel(GrilleElementManager grilleElementManager)
    {
        if(grilleElementManager.sourcePosition.x == -1000)
        {
            Debug.LogError("No source position set!");
            return;
        }

        visitedElements = new bool[GlobalGrid.nbCaseX, GlobalGrid.nbCaseY];

        Queue<GraphNode> nodesToProcess = new Queue<GraphNode>();

        GraphNode beginNode = new GraphNode();
        beginNode.rendement = 1f;
        beginNode.animationTime = 0f;
        beginNode.sourceNodes = new List<GraphNode>();
        beginNode.destNodes = new List<GraphNode>();
        beginNode.spatialPosition = grilleElementManager.sourcePosition;
        beginNode.type = Element.TypeElement.Batterie;

        visitedElements[beginNode.spatialPosition.x, beginNode.spatialPosition.y] = true;


        nodesToProcess.Enqueue(beginNode);
        while (nodesToProcess.Count > 0)
        {
            GraphNode currentNode = nodesToProcess.Dequeue();

            if(currentNode.type == Element.TypeElement.TargetBattery)
            {
                continue;
            }


            if (ventilatorElements.Contains(currentNode.type))
            {
                ProcessVentilateurNode(currentNode, grilleElementManager, nodesToProcess);
                continue;
            }

           
            ProcessElectricityNode(currentNode, grilleElementManager, nodesToProcess);

        }

        DisplayGraphAnimation(beginNode);

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
            
        GraphNode neighbourNode = new GraphNode();
        neighbourNode.rendement = 0.9f;
        neighbourNode.animationTime = 0f;
        neighbourNode.sourceNodes = new List<GraphNode>();
        neighbourNode.destNodes = new List<GraphNode>();
        neighbourNode.spatialPosition = neightbour;
        neighbourNode.type = grilleElementManager.GetElementTypeAtPosition(neightbour.x, neightbour.y);

        visitedElements[neightbour.x, neightbour.y] = true;

        node.destNodes.Add(neighbourNode);
        neighbourNode.sourceNodes.Add(node);


        Debug.Log("Adding node to process : " + neighbourNode.spatialPosition);
        nodesToProcess.Enqueue(neighbourNode);
        
    }

    private List<Vector2Int> GetElectricityNeighbours(int targetX, int targetY, Element.TypeElement currentElement, GrilleElementManager grilleElementManager)
    {
        Element.TypeElement leftElement = grilleElementManager.GetElementTypeAtPosition(targetX - 1, targetY);
        Element.TypeElement rightElement = grilleElementManager.GetElementTypeAtPosition(targetX + 1, targetY);
        Element.TypeElement upElement = grilleElementManager.GetElementTypeAtPosition(targetX, targetY + 1);
        Element.TypeElement downElement = grilleElementManager.GetElementTypeAtPosition(targetX, targetY - 1);

        List<Vector2Int> neighbours = new List<Vector2Int>();


        if (leftElectricityElements.Contains(leftElement) && rightElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX - 1, targetY));
        }
        if (rightElectricityElements.Contains(rightElement) && leftElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX + 1, targetY));
        }
        if (upElectricityElements.Contains(upElement) && downElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX, targetY + 1));
        }
        if (downElectricityElements.Contains(downElement) && upElectricityElements.Contains(currentElement))
        {
            neighbours.Add(new Vector2Int(targetX, targetY - 1));
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

        float rendement = 0.9f; //TODO : GET REAL RENDEMENT
        Vector2Int nextBlock = node.spatialPosition + windDir;
        while(!eoliennesElements.Contains(grilleElementManager.GetElementTypeAtPosition(nextBlock.x, nextBlock.y)))
        {
            Debug.Log("Next block : " + nextBlock); 

            nextBlock += windDir;
            rendement *= 0.8f;


            if(rendement < 0.1f)
            {
                  break;
            }
        }

        GraphNode neighbourNode = new GraphNode(); //TODO : REFACTORISER
        neighbourNode.rendement = rendement;
        neighbourNode.animationTime = 0f;
        neighbourNode.sourceNodes = new List<GraphNode>();
        neighbourNode.destNodes = new List<GraphNode>();
        neighbourNode.spatialPosition = nextBlock;
        neighbourNode.type = grilleElementManager.GetElementTypeAtPosition(nextBlock.x, nextBlock.y);
        visitedElements[nextBlock.x, nextBlock.y] = true;

        node.destNodes.Add(neighbourNode);
        neighbourNode.sourceNodes.Add(node);


        Debug.Log("Adding node to process : " + neighbourNode.spatialPosition);
        nodesToProcess.Enqueue(neighbourNode);
    }

    public void DisplayGraphAnimation(GraphNode beginNode)
    {
        if(debugEnergyObjects != null)
        {
            ClearEnergyObjects();
        }

        debugEnergyObjects = new List<GameObject>();

        Queue<GraphNode> nodesToProcess = new Queue<GraphNode>();
        nodesToProcess.Enqueue(beginNode);

        float energy = 1.0f; //DEBUG ENERGY

        while (nodesToProcess.Count > 0)
        {
            GraphNode currentNode = nodesToProcess.Dequeue();

            if (currentNode.type == Element.TypeElement.TargetBattery)
            {
                continue;
            }

            energy *= currentNode.rendement;

            DisplayNode(currentNode, energy);

            foreach (GraphNode destNode in currentNode.destNodes)
            {
                nodesToProcess.Enqueue(destNode);
            }
        }
    }

    public void DisplayNode(GraphNode node, float energy)
    {
        GameObject debugEnergyObject = Instantiate(debugEnergyPrefab, new Vector3(node.spatialPosition.x * GlobalGrid.caseSize, node.spatialPosition.y * GlobalGrid.caseSize, 0), Quaternion.identity);
        debugEnergyObject.transform.parent = transform;
        debugEnergyObject.transform.localScale = Vector3.one * energy;
        debugEnergyObjects.Add(debugEnergyObject);
    }

    public void ClearEnergyObjects()
    {
        foreach (GameObject debugEnergyObject in debugEnergyObjects)
        {
            Destroy(debugEnergyObject);
        }
        debugEnergyObjects.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Resolving level...");
            ResolveLevel(GrilleElementManager.instance);
        }
    }
}
