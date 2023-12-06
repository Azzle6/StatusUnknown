using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VectorField;

public class SniperIdle : EnemyState
{
    const int obstructLayer = 8; // obstaclelayer
    const float TpOffset = 1;
    EnemySniper currentContext => context as EnemySniper;
    SniperStats sniperStats => currentContext.sniperStats;
    float attackCooldown;
    public override void DebugGizmos()
    {
        //Gizmos.color = (CombatManager.PlayerInView(transform.position, transform.forward, sniperStats.AttackRange, 180, currentContext.obstacleMask)) ? Color.green : Color.red; ;
        Gizmos.DrawWireSphere(transform.position, sniperStats.AttackRange);
        //Gizmos.DrawCube(VectorFieldNavigator.WorldPositiondToNode(transform.position, 4).Position,Vector3.one);
        var postions = TpPositions();
        foreach ( var postion in postions )
        {
            Gizmos.DrawSphere(postion, 0.2f);
        }
        //DebugTP();
    }

    public override void Update()
    {
        attackCooldown -=Time.deltaTime;
        currentContext.tpCooldown -= Time.deltaTime;
        bool playerInView = CombatManager.PlayerInView(transform.position, transform.forward, sniperStats.AttackRange, 180, currentContext.obstacleMask);

        /*if (playerInView && attackCooldown < 0)
            currentContext.SwitchState(new SniperAttack());*/

        if(currentContext.tpCooldown < 0)
        {
            transform.position = TpPosition();
            currentContext.tpCooldown = sniperStats.TpRndCooldown;
          
        }
        if(CombatManager.playerTransform != null && playerInView) 
            transform.forward = CombatManager.playerTransform.position - transform.position;
    }
    Vector3 TpPosition()
    {
        List<Vector3> Positions = TpPositions();
        if(Positions.Count <=  0) return transform.position;
        return Positions[Random.Range(0,Positions.Count)] + Vector3.up * TpOffset;
    }
    void DebugTP()
    {
        //if(CombatManager.playerTransform == null) return positions;

        Node startNode = VectorFieldNavigator.WorldPositiondToNode(CombatManager.playerTransform.position, 4);
        //if(startNode == null) return positions;

        Queue<Node> nodeToCheck = new Queue<Node>();
        HashSet<Node> visitedNode = new HashSet<Node>();

        nodeToCheck.Enqueue(startNode);
        visitedNode.Add(startNode);

        while (nodeToCheck.Count > 0)
        {
            Debug.Log("Node count " + nodeToCheck.Count);
            Node node = nodeToCheck.Dequeue();
            //Gizmos.DrawSphere(node.Position, 0.2f);

            for (int i = 0; i < node.linkedBoundPositions.Count; i++)
            {
                Vector3 linkedBoundPosition = node.linkedBoundPositions[i];
                if (!VectorFieldNavigator.NodeField.ContainsKey(linkedBoundPosition))
                    continue;

                Node linkedNode = VectorFieldNavigator.NodeField[linkedBoundPosition];
                

                if (!visitedNode.Contains(linkedNode))
                {
                    Debug.Log("add vistedNode ");
                    visitedNode.Add(linkedNode);

                    float sqrtMagnitude = (linkedNode.Position - CombatManager.playerTransform.position).magnitude;
                    if (sqrtMagnitude < sniperStats.maxTpRange)
                    {
                        nodeToCheck.Enqueue(linkedNode);
                        Gizmos.color = Color.yellow;
                        // Node valid
                        if (sqrtMagnitude > sniperStats.minTpRange)
                        {
                            Gizmos.DrawSphere(linkedNode.Position, 0.2f);
                        }
                           
                    }
                }
                
            }
        }

        
    }
    List<Vector3> TpPositions()
    {
        
        List<Vector3> positions = new List<Vector3>();
        //if(CombatManager.playerTransform == null) return positions;

        Node startNode = VectorFieldNavigator.WorldPositiondToNode(CombatManager.playerTransform.position,4);
        //if(startNode == null) return positions;

        Queue<Node> nodeToCheck = new Queue<Node>();
        HashSet<Node> visitedNode = new HashSet<Node>();

        nodeToCheck.Enqueue(startNode);
        visitedNode.Add(startNode);

        while (nodeToCheck.Count > 0)
        {
            Node node = nodeToCheck.Dequeue();
            //Debug.Log($"Check Node {node}");
            for(int i = 0;i <node.linkedBoundPositions.Count;i++)
            {
                Vector3 linkedBoundPosition = node.linkedBoundPositions[i];
                if( !VectorFieldNavigator.NodeField.ContainsKey(linkedBoundPosition)) 
                    continue;
                
                Node linkedNode = VectorFieldNavigator.NodeField[linkedBoundPosition];

                if (linkedNode != null && !visitedNode.Contains(linkedNode))
                { 
                    visitedNode.Add(linkedNode);
                    float distance = (linkedNode.Position - CombatManager.playerTransform.position).magnitude;
                    if(distance < sniperStats.maxTpRange)
                    {
                        nodeToCheck.Enqueue(linkedNode);
                        // Node valid
                        if (distance > sniperStats.minTpRange )
                            positions.Add(linkedNode.Position);
                    }
                }
            }

        }

        return positions;
    }

    protected override void Initialize()
    {
        attackCooldown = sniperStats.AttackCooldown+currentContext.initialAttackDuration;
        currentContext.initialAttackDuration = 0;
    }

}
