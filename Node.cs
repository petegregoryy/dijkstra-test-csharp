using System.Collections.Generic;

class Node
{
    private float _lat;
    private float _long;

    private float _nDistance;
    List<Connection> peers;

    public Node(float lat, float lon){
        _lat = lat;
        _long = lon;
        _nDistance = 999999;
    }

    public Node(Vector v){
        _lat = v.X();
        _long = v.Y();
        _nDistance = 999999;
    }

    public void UpdateDistance(float dist){
        _nDistance = dist;
    }

    public List<Connection> GetPeers(){
        return peers;
    }

    public void AddPeer(Connection peer){
        peers.Add(peer);
    }

    public Vector GetCoordinates(){
        return new Vector(_lat,_long); 
    }
}