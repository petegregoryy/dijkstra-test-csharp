using System;

namespace dijkstra_test_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Node node = new Node(1.0f,1.0f);
            Node node2 = new Node(2.0f,1.0f);
            Connection nodeToNode2 = new Connection(node2,5.0f);

            node.AddPeer(nodeToNode2);
            Console.WriteLine("Lat {0} - Long {1}",node.GetCoordinates().X(),node.GetCoordinates().Y());
            Console.WriteLine("Connection X: {0} - Connection Y: {1}",node.GetPeers()[0].GetNode().GetCoordinates().X(),node.GetPeers()[0].GetNode().GetCoordinates().Y());
        }
    }
}
