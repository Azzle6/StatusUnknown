using System.Collections.Generic;

[System.Serializable]
public class LinkedNodes 
{
    public List<Node> nodes;

    public LinkedNodes(List<Node> nodes)
    {
        this.nodes = nodes;
    }
}
