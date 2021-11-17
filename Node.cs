using System;
using System.Collections.Generic;
class Node
{
    private float _lat;
    private float _long;
    private string _name;
    private double _nDistance;
    private Node _shortestParent;
    private bool _visited;

    private string _location;
    List<Connection> connections = new List<Connection>();

    public Node(string name, float lat, float lon, string location)
    {
        _lat = lat;
        _long = lon;
        _name = name;
        _nDistance = 999999;
        _visited = false;
        _location = location;
    }

    public Node(string name, float lat, float lon, Node peer)
    {
        _lat = lat;
        _long = lon;
        _name = name;
        _nDistance = 999999;
        _visited = false;
        AddConnectionBack(peer);
    }

    public Node(string name, float lat, float lon, Node peer, bool both)
    {
        _lat = lat;
        _long = lon;
        _name = name;
        _nDistance = 999999;
        _visited = false;
        if (both)
        {
            AddConnectionBothWay(peer);
        }
        else
        {
            AddConnectionBack(peer);
        }
    }

    public Node(string name, Vector v)
    {
        _lat = v.X();
        _long = v.Y();
        _name = name;
        _nDistance = 999999;
        _visited = false;
    }

    public void UpdateDistance(double dist, Node par)
    {
        if (dist < _nDistance)
        {
            _nDistance = dist;
            _shortestParent = par;
        }
    }

    public double GetDistance()
    {
        return _nDistance;
    }

    public List<Connection> GetConnections()
    {
        return connections;
    }

    public void AddConnection(Node peer)
    {
        Connection con = new Connection(this, peer);
        connections.Add(con);
    }

    public void AddConnectionBack(Node peer)
    {
        peer.AddConnection(this);
    }

    public void AddConnectionBothWay(Node peer)
    {
        Connection con = new Connection(this, peer);
        connections.Add(con);
        peer.AddConnection(this);
    }

    public Vector GetCoordinates()
    {
        return new Vector(_lat, _long);
    }

    public float GetLat()
    {
        return _lat;
    }

    public float GetLong()
    {
        return _long;
    }

    public string GetName()
    {
        return _name;
    }

    public bool IsVisited()
    {
        return _visited;
    }

    public void SetVisited()
    {
        _visited = true;
    }

    public Node GetShortestParent()
    {
        return _shortestParent;
    }

    public string GetLocation()
    {
        return _location;
    }
    List<int> toRemove = new List<int>();
    public void PruneConnections()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            for (int k = 0; k < connections.Count; k++)
            {
                if (i != k)
                {
                    if (connections[i].GetName() == connections[k].GetName() && connections[i].GetDistance() == connections[k].GetDistance())
                    {
                        //Console.WriteLine("Pruning Duplicate Connection - {0}", connections[i]);
                        toRemove.Add(k);
                    }
                    else
                    {
                        //Console.WriteLine("No Duplicate Found");
                    }
                }
            }
        }
        if(toRemove.Count != 0){
            
            toRemove.Sort();
            toRemove.Reverse();
            var toRemoveDistinct = new HashSet<int>(toRemove);
            List<int> toRemove2In = new List<int>();
            foreach (int item in toRemoveDistinct)
            {
                toRemove2In.Add(item);
            }
            int count = 0;
            int length = connections.Count;
            Console.WriteLine("Node: {0}", _name);
            while(count < length-1){
                Console.WriteLine("Index: {0} - Length: {1}", toRemove2In[count], length);
                connections.RemoveAt(toRemove2In[count]);
                count++;
                length = connections.Count;
                
            }
        }
        
        foreach (int index in toRemove)
        {
            //Console.WriteLine("Index: {0} - Length: {1}", index, connections.Count);
            //connections.RemoveAt(index);
        }
    }

    public void ListConnections(){
        for (int i = 0; i < connections.Count; i++)
        {
            Console.WriteLine("Connection: {0} - Distance in km: {1}", connections[i].GetName(),connections[i].GetDistance());
        }
    }
}