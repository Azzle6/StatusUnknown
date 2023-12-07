using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorField;

public class SniperIdle : EnemyState
{
    const int obstructLayer = 8; // obstaclelayer
    const float TpOffset = 1;
    EnemySniper currentContext => context as EnemySniper;
    SniperStats sniperStats => currentContext.sniperStats;
    float tpCooldown;
    float attackCooldown;
    public override void DebugGizmos()
    {
        Gizmos.color = (CombatManager.PlayerInView(transform.position, transform.forward, sniperStats.AttackRange, 180, currentContext.obstacleMask)) ? Color.green : Color.red; ;
        Gizmos.DrawWireSphere(transform.position, sniperStats.AttackRange);
        /*List<Vector3> Positions = TpPositions();
        foreach (Vector3 position in Positions)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(position, 0.2f);
        }*/
    }

    public override void Update()
    {
        attackCooldown -=Time.deltaTime;
        tpCooldown -= Time.deltaTime;
        bool playerInView = CombatManager.PlayerInView(transform.position, transform.forward, sniperStats.AttackRange, 180, currentContext.obstacleMask);

        if (playerInView && attackCooldown < 0)
            currentContext.SwitchState(new SniperAttack());

        if(!playerInView && tpCooldown < 0)
        {
            transform.position = TpPosition();
            tpCooldown = sniperStats.tpCooldown;
          
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
    List<Vector3> TpPositions()
    {
        
        List<Vector3> positions = new List<Vector3>();
        if(CombatManager.playerTransform == null) return positions;

        Node startNode = VectorFieldNavigator.WorldPositiondToNode(CombatManager.playerTransform.position);
        if(startNode == null) return positions;

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
                if(linkedNode != null && !visitedNode.Contains(linkedNode))
                { 
                    visitedNode.Add(linkedNode);
                    float sqrtMagnitude = Vector3.SqrMagnitude(linkedNode.Position - CombatManager.playerTransform.position);
                    if(sqrtMagnitude < Mathf.Pow(sniperStats.maxTpRange,2))
                    {
                        nodeToCheck.Enqueue(linkedNode);
                        // Node valid
                        if (sqrtMagnitude > Mathf.Pow(sniperStats.minTpRange, 2))
                            positions.Add(linkedNode.Position);
                    }
                }
            }

        }

        return positions;
    }

    protected override void Initialize()
    {
        tpCooldown = sniperStats.tpCooldown;
        attackCooldown = sniperStats.AttackCooldown;
    }
}
