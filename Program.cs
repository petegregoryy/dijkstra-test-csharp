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
            Node node = p.CreateNode("London", 51.502913f, -0.111447f);
            Node node2 = p.CreateNode("Japan", 38.738439f, 140.741562f);


            node.AddConnectionBothWay(node2);
            Console.WriteLine("{2} - Lat: {0} - Long: {1}", node.GetCoordinates().X(), node.GetCoordinates().Y(), node.GetName());
            Console.WriteLine("{2} - Connection X: {0} - Connection Y: {1}", node.GetConnections()[0].GetNode().GetCoordinates().X(), node.GetConnections()[0].GetNode().GetCoordinates().Y(), node.GetConnections()[0].GetNode().GetName());
            Console.WriteLine("Distance: {0} km", node.GetConnections()[0].GetDistance());

            List<Connection> NodeConnections = new List<Connection>();



            NodeConnections.Add(node.GetConnections()[0]);
            NodeConnections.Add(node2.GetConnections()[0]);

            Console.WriteLine("{0} - Distance: {1} km", NodeConnections[0].GetName(), NodeConnections[0].GetDistance());
            Console.WriteLine("{0} - Distance: {1} km", NodeConnections[1].GetName(), NodeConnections[1].GetDistance());
        }

        Node CreateNode(string name, float lat, float lon)
        {
            Node tempNode = new Node(name, lat, lon);
            globe.Add(tempNode);
            return tempNode;
        }
    }
}
