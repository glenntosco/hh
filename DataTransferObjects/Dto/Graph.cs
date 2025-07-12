using System.Collections.Generic;
using System.Linq;

namespace Pro4Soft.DataTransferObjects.Dto
{
    public class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();

        public Node Start => Nodes.Single(c => c.Inbound.Count == 0);
        public Node End => Nodes.Single(c => c.Outbound.Count == 0);
    }

    public class Node
    {
        public string ProdStep { get; set; }
        public string Description { get; set; }

        public List<Edge> Inbound { get; set; } = new List<Edge>();
        public List<Edge> Outbound { get; set; } = new List<Edge>();
    }

    public class Edge
    {
        public string ProdStep { get; set; }
        public string Description { get; set; }

        public bool IsCaptureDetails { get; set; }
        public DetailsType? DetailsType { get; set; }
        public string DetailsLabel { get; set; }
    }

    public enum DetailsType
    {
        Scan,
        ManualEntry
    }

    public class WorkflowHelper
    {
        public string Layout { get; set; }
        public string Data { get; set; }
    }
}