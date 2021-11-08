using System;
using System.Collections.Generic;

namespace dijkstra_test_csharp
{
    class Program
    {
        List<Node> globe = new List<Node>();
        List<Node> queue = new List<Node>();
        static void Main(string[] args)
        {
            Program p = new Program();

            Node node = p.CreateNode("Aki", 38.738439f, 140.741562f);
            Node node2 = p.CreateNodeWithOrigin("Yukiban", 38.658362f, 140.863543f, node, true);
            Node node3 = p.CreateNodeWithOrigin("Osaki City Hall", 38.576945f, 140.955698f, node2, true);
            Node node4 = p.CreateNodeWithOrigin("Nishi-Furukawa", 38.578384f, 140.895211f, node2, true);
            Node node5 = p.CreateNodeWithOrigin("Sakurazutsimi Park", 38.524911f, 140.955379f, node3, true);
            Node node6 = p.CreateNodeWithOrigin("Magariyashik", 38.537837f, 141.002235f, node3, true);
            Node node7 = p.CreateNodeWithOrigin("Pacific Ocean East", -2.351121f, 175.280974f, node6, true);
            Node node8 = p.CreateNodeWithOrigin("Pacific Ocean West", -4.099388f, -170.180773f, node7, true);
            node4.AddConnectionBothWay(node3);
            node4.AddConnectionBothWay(node5);
            node5.AddConnectionBothWay(node6);

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

            string name = "Aki";
            p.globe.Find(point => point.GetName() == name);
            p.Dijkstra(node6, node2);

        }

        Node CreateNode(string name, float lat, float lon)
        {
            Node tempNode = new Node(name, lat, lon);
            globe.Add(tempNode);
            return tempNode;
        }

        Node CreateNodeWithOrigin(string name, float lat, float lon, Node peer, bool both)
        {
            if (both)
            {

                Node tempNode = new Node(name, lat, lon, peer, true);
                globe.Add(tempNode);
                return tempNode;
            }
            else
            {

                Node tempNode = new Node(name, lat, lon, peer);
                globe.Add(tempNode);
                return tempNode;
            }
        }


        void Dijkstra(Node origin, Node target)
        {
            int firstIndex = IndexByName(origin.GetName());
            globe[firstIndex].UpdateDistance(0, null);
            queue.Add(globe[firstIndex]);

            for (int k = 0; k < queue.Count; k++)
            {
                for (int i = 0; i < queue[k].GetConnections().Count; i++)
                {
                    if (!queue[k].GetConnections()[i].GetNode().IsVisited())
                    {
                        queue.Add(queue[k].GetConnections()[i].GetNode());
                        queue[k].GetConnections()[i].GetNode().UpdateDistance(queue[k].GetConnections()[i].GetDistance() + queue[k].GetDistance(), queue[k]);
                        queue[k].GetConnections()[i].GetNode().SetVisited();
                        Console.WriteLine("Parent: {0} Visiting: {1} Distance: {2}", queue[k].GetName(), queue[k].GetConnections()[i].GetNode().GetName(), queue[k].GetConnections()[i].GetNode().GetDistance());
                    }
                }
            }

            int targetIndex = IndexByName(target.GetName());
            List<Node> path = new List<Node>();

            Node shortestParent = globe[targetIndex];

            while (shortestParent != null)
            {
                Console.WriteLine("Shortest Parent: {0}", shortestParent.GetName());
                path.Add(shortestParent);
                shortestParent = shortestParent.GetShortestParent();

            }
            int count = 0;
            for (int i = path.Count - 1; i >= 0; i--)
            {
                count++;
                Console.WriteLine("Step {2}: {0} - {1}", path[i].GetName(), path[i].GetDistance(), count);

            }


        }

        int IndexByName(string name)
        {
            return globe.FindIndex(point => point.GetName() == name);
        }
    }
}
