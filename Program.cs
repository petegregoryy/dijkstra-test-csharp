using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace dijkstra_test_csharp
{
    class Program
    {
        List<Node> globe = new List<Node>();
        List<Node> queue = new List<Node>();

        FeatureCollection linkCollection;
        FeatureCollection collection;

        static void Main(string[] args)
        {
            Program p = new Program();

            #region NodeCreation
            /*
            Node node = p.CreateNode("Aki", 38.738439f, 140.741562f);
            Node node2 = p.CreateNodeWithOrigin("Yubikan", 38.658362f, 140.863543f, node, true);
            Node node3 = p.CreateNodeWithOrigin("Osaki City Hall", 38.576945f, 140.955698f, node2, true);
            Node node4 = p.CreateNodeWithOrigin("Nishi-Furukawa", 38.578384f, 140.895211f, node2, true);
            Node node5 = p.CreateNodeWithOrigin("Sakurazutsimi Park", 38.524911f, 140.955379f, node3, true);
            Node node6 = p.CreateNodeWithOrigin("Magariyashik", 38.537837f, 141.002235f, node3, true);
            Node node7 = p.CreateNodeWithOrigin("Pacific Ocean East", -2.351121f, 175.280974f, node6, true);
            Node node8 = p.CreateNodeWithOrigin("Pacific Ocean West", -4.099388f, -170.180773f, node7, true);
            Node node9 = p.CreateNodeWithOrigin("Sendai", 38.268008f, 140.869364f, node5, true);
            node4.AddConnectionBothWay(node3);
            node4.AddConnectionBothWay(node5);
            node5.AddConnectionBothWay(node6);
            node6.AddConnectionBothWay(node9);
            */
            #endregion

            #region Json Serialisation and Conversion

            string filePathForLinks = "./data/features-inc-dupes.geojson";
            string filePath = "./data/features.geojson";



            string file = File.ReadAllText(filePath);
            string links = File.ReadAllText(filePathForLinks);

            p.linkCollection = JsonConvert.DeserializeObject<FeatureCollection>(links);
            p.collection = JsonConvert.DeserializeObject<FeatureCollection>(file);

            List<GeoFeature> geoFeats = new List<GeoFeature>();
            for (int i = 0; i < p.collection.features.Count; i++)
            {
                p.globe.Add(new Node(p.collection.features[i].properties.name, p.collection.features[i].geometry.coordinates[0], p.collection.features[i].geometry.coordinates[1], p.collection.features[i].properties.location));
            }
            Console.WriteLine("Removing Any Duplicates!");
            for (int x = 0; x < p.globe.Count; x++)
            {
                for (int y = 0; y < p.globe.Count; y++)
                {
                    if(x!=y){
                        if(p.globe[x].GetName() == p.globe[y].GetName()){
                            Console.WriteLine("Duplicate Detected! {0} - {1}", p.globe[y].GetName(), p.globe[y].GetLocation());
                            p.globe.RemoveAt(y);                            
                        }
                    }
                }
            }
            Console.WriteLine("Node Number: {0}", p.globe.Count);

            for (int i = 0; i < p.linkCollection.features.Count; i++)
            {
                if (p.linkCollection.features[i].properties.connectsFrom != null)
                {
                    Node origin = p.globe.Find(e => e.GetName() == p.linkCollection.features[i].properties.name);
                    Node target = p.globe.Find(e => e.GetName() == p.linkCollection.features[i].properties.connectsFrom);

                    origin.AddConnection(target);
                }
                else if (p.linkCollection.features[i].properties.connectsTo != null)
                {
                    Node origin = p.globe.Find(e => e.GetName() == p.linkCollection.features[i].properties.name);
                    Node target = p.globe.Find(e => e.GetName() == p.linkCollection.features[i].properties.connectsTo);

                    origin.AddConnection(target);
                }
            }

            Console.WriteLine("Pruning Connections");
            foreach (Node point in p.globe)
            {
                point.PruneConnections();
            }
            
            #endregion

            // Manual Link adding
            Node manualorigin = p.globe.Find(e => e.GetName() == "201799");
            Node manualtarget = p.globe.Find(e => e.GetName() == "202016");

            manualorigin.AddConnection(manualtarget);
            manualtarget.AddConnection(manualorigin);

            Node manualorigin2 = p.globe.Find(e => e.GetName() == "201055");
            Node manualtarget2 = p.globe.Find(e => e.GetName() == "201799");

            manualorigin2.AddConnection(manualtarget2);
            manualtarget2.AddConnection(manualorigin2);

            #region Adding Connections
            List<Connection> NodeConnections = new List<Connection>();

            foreach (Node point in p.globe)
            {
                foreach (Connection c in point.GetConnections())
                {
                    NodeConnections.Add(c);
                }
            }
            /*
            foreach (Connection con in NodeConnections)
            {
                Console.WriteLine("{0} - Distance: {1} km", con.GetName(), con.GetDistance());
            }
            */
            #endregion
            Node node = p.globe.Find(e => e.GetName() == "136033");
            Node node1 = p.globe.Find(e => e.GetName() == "202016");

            p.Dijkstra(node, node1);
        }

        #region Helpful Functions
        Node CreateNode(string name, float lat, float lon, string location)
        {
            Node tempNode = new Node(name, lat, lon, location);
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
        int IndexByName(string name)
        {
            return globe.FindIndex(point => point.GetName() == name);
        }
        #endregion

        void Dijkstra(Node origin, Node target)
        {
            Console.WriteLine("----Navigating----");
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
                        //Console.WriteLine("Parent: {0} - Visiting: {1} Distance: {2}", queue[k].GetName(), queue[k].GetConnections()[i].GetNode().GetName(), queue[k].GetConnections()[i].GetNode().GetDistance());
                    }
                }
            }

            int targetIndex = IndexByName(target.GetName());
            List<Node> path = new List<Node>();

            Node shortestParent = globe[targetIndex];

            while (shortestParent != null)
            {
                //Console.WriteLine("Shortest Parent: {0}", shortestParent.GetName());
                path.Add(shortestParent);
                shortestParent = shortestParent.GetShortestParent();
            }
            int count = 0;
            for (int i = path.Count - 1; i >= 0; i--)
            {
                count++;
                Console.WriteLine("Step {2}: {3} - {0} - {1}", path[i].GetLocation(), path[i].GetDistance(), count,path[i].GetName());

            }
            Node redSea1 = globe.Find(e => e.GetName() == "201055");
            Node redSea2 = globe.Find(e => e.GetName() == "198758");
            string sea1string = JsonConvert.SerializeObject(redSea1);
            string sea2string = JsonConvert.SerializeObject(redSea2);
            Console.WriteLine("Name: {0} Location: {1} Shortest Parent: {2} Distance: {3}",redSea1.GetName(),redSea1.GetLocation(),redSea1.GetShortestParent().GetName(),redSea1.GetDistance());
            Console.WriteLine("Name: {0} Location: {1} Shortest Parent: {2} Distance: {3}",redSea2.GetName(),redSea2.GetLocation(),redSea2.GetShortestParent().GetName(),redSea2.GetDistance());
            redSea1.ListConnections();
            redSea2.ListConnections();
        }


    }
}
