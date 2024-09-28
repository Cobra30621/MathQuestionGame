using System.Collections.Generic;
using System.Linq;
using Map_System.Scripts.MapData;
using UnityEngine;

namespace Map
{
    public static class MapGenerator
    {
        private static MapConfig config;

        private static readonly List<NodeType> RandomNodes = new List<NodeType>
        {NodeType.Event, NodeType.Store, NodeType.Treasure, NodeType.MinorEnemy, NodeType.CampFire};

        private static List<float> layerDistances;
        private static List<List<Point>> paths;
        // ALL nodes by layer:
        private static readonly List<List<Node>> nodes = new List<List<Node>>();

        public static Map GetMap(MapConfig conf)
        {
            if (conf == null)
            {
                Debug.LogWarning("Config was null in MapGenerator.Generate()");
                return null;
            }

            config = conf;
            nodes.Clear();

            GenerateLayerDistances();

            for (var i = 0; i < conf.layers.Count; i++)
                PlaceLayer(i);

            GeneratePaths();

            RandomizeNodePositions();

            SetUpConnections();

            // select all the nodes with connections:
            var nodesList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            return new Map(conf.mapName, nodesList, new List<Point>());
        }

        private static void GenerateLayerDistances()
        {
            layerDistances = new List<float>();
            foreach (var layer in config.layers)
                layerDistances.Add(layer.distanceFromPreviousLayer);
        }

        private static float GetDistanceToLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;

            return layerDistances.Take(layerIndex + 1).Sum();
        }

        private static void PlaceLayer(int layerIndex)
        {
            var layer = config.layers[layerIndex];
            var nodesOnThisLayer = new List<Node>();

            // offset of this layer to make all the nodes centered:
            var offset = layer.nodesApartDistance * config.GridWidth / 2f;

            for (var i = 0; i < layer.NodeCount; i++)
            {
                var nodeType = layer.GetNodeTypeByWeight();
           
                var node = new Node(nodeType,  new Point(i, layerIndex))
                {
                    position = new Vector2(-offset + i * layer.nodesApartDistance, GetDistanceToLayer(layerIndex))
                };
                nodesOnThisLayer.Add(node);
            }

            nodes.Add(nodesOnThisLayer);
        }

        private static void RandomizeNodePositions()
        {
            for (var index = 0; index < nodes.Count; index++)
            {
                var list = nodes[index];
                var layer = config.layers[index];
                var distToNextLayer = index + 1 >= layerDistances.Count
                    ? 0f
                    : layerDistances[index + 1];
                var distToPreviousLayer = layerDistances[index];

                foreach (var node in list)
                {
                    var xRnd = Random.Range(-1f, 1f);
                    var yRnd = Random.Range(-1f, 1f);

                    var x = xRnd * layer.nodesApartDistance / 2f;
                    var y = yRnd < 0 ? distToPreviousLayer * yRnd / 2f : distToNextLayer * yRnd / 2f;

                    node.position += new Vector2(x, y) * layer.randomizePosition;
                }
            }
        }

        private static void SetUpConnections()
        {
            foreach (var path in paths)
            {
                for (var i = 0; i < path.Count - 1; ++i)
                {
                    var node = GetNode(path[i]);
                    var nextNode = GetNode(path[i + 1]);
                    node.AddOutgoing(nextNode.point);
                    nextNode.AddIncoming(node.point);
                }
            }
        }


        private static Node GetNode(Point p)
        {
            if (p.y >= nodes.Count) return null;
            if (p.x >= nodes[p.y].Count) return null;

            return nodes[p.y][p.x];
        }

        private static void GeneratePaths()
        {
            paths = new List<List<Point>>();
            
            var path1 = new List<Point>();
            for (int i = 0; i < config.layers.Count; i++)
            {
                path1.Add(new Point(0, i));
            }
            paths.Add(path1);
            
            var path2 = new List<Point>();
            for (int i = 0; i < config.layers.Count; i++)
            {
                int x = config.layers[i].NodeCount - 1;
                
                path2.Add(new Point(x, i));
            }
            paths.Add(path2);
            
        }
    }
}