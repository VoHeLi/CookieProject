using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyResolver : MonoBehaviour
{
    struct GraphNode
    {
        public float rendement;
        public float animationTime;
        public List<GraphNode> sourceNodes;
        public List<GraphNode> destNodes;
        public Vector2Int spatialPosition;
        public Element.TypeElement type;
    }

    private bool[,] visitedElements;

    [SerializeField] private List<Element.TypeElement> leftElectricityElements;
    [SerializeField] private List<Element.TypeElement> rightElectricityElements;
    [SerializeField] private List<Element.TypeElement> upElectricityElements;
    [SerializeField] private List<Element.TypeElement> downElectricityElements;
    
    //On va créer un graphe de noeuds, chaque noeud représente un élément avec son rendement, son temps d'animation, ses noeuds sources et ses noeuds finaux
    //On commence de la fin, puis on remonte jusqu'au début
    public void ResolveLevel(GrilleElementManager grilleElementManager)
    {
        visitedElements = new bool[GlobalGrid.nbCaseX, GlobalGrid.nbCaseY];

        Queue<GraphNode> nodesToProcess = new Queue<GraphNode>();

        GraphNode targetNode = new GraphNode();
        targetNode.rendement = 1f;
        targetNode.animationTime = 0f;
        targetNode.sourceNodes = new List<GraphNode>();
        targetNode.destNodes = new List<GraphNode>();
        targetNode.spatialPosition = grilleElementManager.sourcePosition;
        targetNode.type = Element.TypeElement.TargetBattery;

        visitedElements[targetNode.spatialPosition.x, targetNode.spatialPosition.y] = true;

        while (nodesToProcess.Count > 0)
        {
            GraphNode currentNode = nodesToProcess.Dequeue();

            ProcessElectricityNode(currentNode, grilleElementManager, nodesToProcess);
        }

    }

    private void ProcessElectricityNode(GraphNode node, GrilleElementManager grilleElementManager, Queue<GraphNode> nodesToProcess)
    {
        List<Vector2Int> neighbours = GetElectricityNeighbours(node.spatialPosition.x, node.spatialPosition.y, grilleElementManager);

        foreach (Vector2Int neighbour in neighbours)
        {
            if (visitedElements[neighbour.x, neighbour.y])
            {
                continue;
            }

            GraphNode neighbourNode = new GraphNode();
            neighbourNode.rendement = 1f;
            neighbourNode.animationTime = 0f;
            neighbourNode.sourceNodes = new List<GraphNode>();
            neighbourNode.destNodes = new List<GraphNode>();
            neighbourNode.spatialPosition = neighbour;
            neighbourNode.type = grilleElementManager.GetElementTypeAtPosition(neighbour.x, neighbour.y);

            visitedElements[neighbour.x, neighbour.y] = true;

            node.destNodes.Add(neighbourNode);
            neighbourNode.sourceNodes.Add(node);

            nodesToProcess.Enqueue(neighbourNode);
        }
    }

    private List<Vector2Int> GetElectricityNeighbours(int targetX, int targetY, GrilleElementManager grilleElementManager)
    {
        Element.TypeElement leftElement = grilleElementManager.GetElementTypeAtPosition(targetX - 1, targetY);
        Element.TypeElement rightElement = grilleElementManager.GetElementTypeAtPosition(targetX + 1, targetY);
        Element.TypeElement upElement = grilleElementManager.GetElementTypeAtPosition(targetX, targetY + 1);
        Element.TypeElement downElement = grilleElementManager.GetElementTypeAtPosition(targetX, targetY - 1);

        List<Vector2Int> neighbours = new List<Vector2Int>();


        if (leftElectricityElements.Contains(leftElement))
        {
            neighbours.Add(new Vector2Int(targetX - 1, targetY));
        }
        if (rightElectricityElements.Contains(rightElement))
        {
            neighbours.Add(new Vector2Int(targetX + 1, targetY));
        }
        if (upElectricityElements.Contains(upElement))
        {
            neighbours.Add(new Vector2Int(targetX, targetY + 1));
        }
        if (downElectricityElements.Contains(downElement))
        {
            neighbours.Add(new Vector2Int(targetX, targetY - 1));
        }


        return neighbours;
    }
}
