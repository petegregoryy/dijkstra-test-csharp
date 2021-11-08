using System;

class Connection
{
    Node _node;
    float _distance;

    string _name;

    public Connection(Node origin, Node node)
    {
        _node = node;
        _distance = CoordToKm(origin.GetLat(), origin.GetLong(), node.GetLat(), node.GetLong());
        _name = origin.GetName() + " to " + node.GetName();
    }

    public Node GetNode()
    {
        return _node;
    }

    public float GetDistance()
    {
        return _distance;
    }
    public string GetName()
    {
        return _name;
    }

    float CoordToKm(float lat1, float long1, float lat2, float long2)
    {
        int R = 6371;
        float dLat = deg2rad(lat2 - lat1);
        float dLon = deg2rad(long2 - long1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double d = R * c;
        return (float)d;
    }

    float deg2rad(float deg)
    {
        return deg * ((float)Math.PI / 180);
    }
}