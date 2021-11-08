using System;
using System.Collections.Generic;

namespace dijkstra_test_csharp
{
    class Program
    {
        List<Node> globe = new List<Node>();
        static void Main(string[] args)
        {
            Program p = new Program();

            Node node = p.CreateNode("Aki", 38.738439f, 140.741562f);
            Node node2 = p.CreateNodeWithOrigin("Yukiban", 38.658362f, 140.863543f, node);
            Node node3 = p.CreateNodeWithOrigin("Osaki City Hall", 38.576945f, 140.955698f, node2);
            Node node4 = p.CreateNodeWithOrigin("Nishi-Furukawa", 38.578384f, 140.895211f, node2);
            Node node5 = p.CreateNodeWithOrigin("Sakurazutsimi Park", 38.524911f, 140.955379f, node3);
            node4.AddConnection(node3);
            node4.AddConnection(node5);

            List<Connection> NodeConnections = new List<Connection>();

            foreach (Node point in p.globe)
            {
                foreach (Connection c in point.GetConnections())
                {
                    NodeConnections.Add(c);
                }
            }

            foreach (Connection con in NodeConnections)
            {
                Console.WriteLine("{0} - Distance: {1} km", con.GetName(), con.GetDistance());
            }
        }

        Node CreateNode(string name, float lat, float lon)
        {
            Node tempNode = new Node(name, lat, lon);
            globe.Add(tempNode);
            return tempNode;
        }

        Node CreateNodeWithOrigin(string name, float lat, float lon, Node peer)
        {
            Node tempNode = new Node(name, lat, lon, peer);
            globe.Add(tempNode);
            return tempNode;
        }
    }
}
