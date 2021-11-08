using System.Collections.Generic;

class Connection
{
    Node _node;
    float _distance;

    public Connection(Node node, float dist){
        _node = node;
        _distance = dist;
    }

    public Node GetNode(){
        return _node;
    }

    public float GetDistance(){
        return _distance;
    }
}