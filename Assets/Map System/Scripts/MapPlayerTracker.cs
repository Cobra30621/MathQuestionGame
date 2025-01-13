using System;
using System.Linq;
using DG.Tweening;
using Encounter;
using Log;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = true;
        public float enterNodeDelay = 1f;
        public MapManager mapManager=> MapManager.Instance;
        public MapView view;

        public static MapPlayerTracker Instance;


        private void Awake()
        {
            Instance = this;
        }


        public void SelectNode(MapNode mapNode)
        {
            if (mapManager.Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            mapManager.Locked = lockAfterSelecting;
            mapManager.SelectedNode(mapNode);
            
            mapNode.SetState(NodeStates.Visited);
            // view.SetAttainableNodes();
            // view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private void EnterNode(MapNode mapNode)
        {
            EventLogger.Instance.LogEvent(LogEventType.MapEncounter, 
                $"進入節點: {mapNode.Node.nodeType}");
      
            EncounterManager.Instance.EnterNode(mapNode.Node.nodeType);
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}