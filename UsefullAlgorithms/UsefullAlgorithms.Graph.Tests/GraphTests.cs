using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UsefullAlgorithms.Graph.Algorithms;

namespace UsefullAlgorithms.Graph.Tests
{
    [TestClass]
    public class GraphTests
    {
        private Graph<string, Edge<string>> g;

        [TestInitialize]
        public void Initialize()
        {
            g = GetGraph();
        }

        [TestMethod]
        public void Graphs_CheckCounts()
        {
            Assert.AreEqual(4, g.VerticesCount);
            Assert.AreEqual(5, g.EdgesCount);
        }

        [TestMethod]
        public void Graphs_CheckAdjacents()
        {
            var adjacents = g.GetAdjacents("A");
            Assert.AreEqual(2, adjacents.Count());

            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("B")));
            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("C")));

            adjacents = g.GetAdjacents("B");
            Assert.AreEqual(2, adjacents.Count());

            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("C")));
            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("D")));

            adjacents = g.GetAdjacents("B");
            Assert.AreEqual(2, adjacents.Count());

            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("C")));
            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("D")));

            adjacents = g.GetAdjacents("C");
            Assert.AreEqual(0, adjacents.Count());
            
            adjacents = g.GetAdjacents("D");
            Assert.AreEqual(1, adjacents.Count());

            Assert.IsTrue(adjacents.Any(f => f.Data.Equals("A")));
        }

        [TestMethod]
        public void Graphs_CheckEdges()
        {
            var edges = g.GetEdges("A");
            Assert.AreEqual(2, edges.Count());

            Assert.IsTrue(edges.Any(f => f.Source.Data.Equals("A") && f.Destination.Data.Equals("B")));
            Assert.IsTrue(edges.Any(f => f.Source.Data.Equals("A") && f.Destination.Data.Equals("C")));

            edges = g.GetEdges("B");
            Assert.AreEqual(2, edges.Count());

            Assert.IsTrue(edges.Any(f => f.Source.Data.Equals("B") && f.Destination.Data.Equals("C")));
            Assert.IsTrue(edges.Any(f => f.Source.Data.Equals("B") && f.Destination.Data.Equals("D")));

            edges = g.GetEdges("C");
            Assert.AreEqual(0, edges.Count());

            edges = g.GetEdges("D");
            Assert.AreEqual(1, edges.Count());

            Assert.IsTrue(edges.Any(f => f.Source.Data.Equals("D") && f.Destination.Data.Equals("A")));
        }

        [TestMethod]
        public void Graphs_CheckEdge()
        {
            var edge = g.GetEdge("A", "B");
            Assert.IsTrue(edge.Source.Data == "A" && edge.Destination.Data == "B");

            edge = g.GetEdge("A", "C");
            Assert.IsTrue(edge.Source.Data == "A" && edge.Destination.Data == "C");

            edge = g.GetEdge("B", "C");
            Assert.IsTrue(edge.Source.Data == "B" && edge.Destination.Data == "C");

            edge = g.GetEdge("B", "D");
            Assert.IsTrue(edge.Source.Data == "B" && edge.Destination.Data == "D");

            edge = g.GetEdge("D", "A");
            Assert.IsTrue(edge.Source.Data == "D" && edge.Destination.Data == "A");

            edge = g.GetEdge("D", "B");
            Assert.IsNull(edge);
        }

        [TestMethod]
        public void Graphs_CheckAddition()
        {
            Graph<string, Edge<string>> g = new Graph<string, Edge<string>>();

            g.Add("F");

            Assert.IsTrue(g.HasVertex("F"));
        }

        [TestMethod]
        public void Graphs_CheckRemove()
        {
            g.Remove("A");

            Assert.IsFalse(g.HasVertex("A"));
            Assert.IsNull(g.GetEdge("A", "B"));
            Assert.IsNull(g.GetEdge("A", "C"));
            Assert.IsNull(g.GetEdge("D", "A"));
        }

        [TestMethod]
        public void Graphs_CheckEdgeConnections()
        {
            Assert.IsFalse(g.HasEdge("A", "A"));
            g.Connect("A", "A", new DirectedEdge<string>(g.GetByValue("A"), g.GetByValue("A"), Edge<string>.Relation.SelfConnected));
            Assert.IsTrue(g.HasEdge("A", "A"));
        }

        [TestMethod]
        public void Graphs_HasCycles()
        {
            g = new Graph<string, Edge<string>>();

            g.Add("A", "B", "C");

            g.Connect("A", "B", new DirectedEdge<string>(g.GetByValue("A"), g.GetByValue("B"), Edge<string>.Relation.ParentToChild));
            g.Connect("B", "A", new DirectedEdge<string>(g.GetByValue("B"), g.GetByValue("A"), Edge<string>.Relation.ChildToParent));
            g.Connect("B", "B", new DirectedEdge<string>(g.GetByValue("B"), g.GetByValue("B"), Edge<string>.Relation.SelfConnected));

            Assert.AreEqual(true, g.HasCycle(g.GetByValue("A")));
        }

        [TestMethod]
        public void Graphs_TestDepthFirstSearchTraverse()
        {
            g.Remove(g.GetEdge("B", "C"));

            string tOrder = string.Empty;

            var enumerator = g.GetEnumerator(f => new DepthFirstSearch<string, Edge<string>>(f, "A", true));
            while (enumerator.MoveNext())
            {
                tOrder += enumerator.Current.Data;
            }

            Assert.AreEqual("ACBD", tOrder);
        }

        [TestMethod]
        public void Graphs_TestBreadthFirstSearchTraverse()
        {
            g.Remove(g.GetEdge("B", "C"));

            string tOrder = string.Empty;

            var enumerator = g.GetEnumerator(f => new BreadthFirstSearch<string, Edge<string>>(f, "A", true));
            while (enumerator.MoveNext())
            {
                tOrder += enumerator.Current.Data;
            }

            Assert.AreEqual("ABCD", tOrder);
        }

        [TestMethod]
        public void Graphs_TestFindPathsSearch()
        {
            var graph = GetGraph();
            var search = new UnknownTopologySearch<string, Edge<string>>();
            var routes = search.FindRoutes(graph, graph.GetByValue("A"), graph.GetByValue("D"));
        }

        [TestMethod]
        public void Graphs_TestFindPathsSearch3()
        {
            var graph = GetWeightedGraph();
            var search = new DijkstraSearch<string, WeightedEdge<string>>();
            var routes = search.FindRoutes(graph, graph.GetByValue("A"), graph.GetByValue("F"));
        }

        private static Graph<string, Edge<string>> GetGraph()
        {
            Graph<string, Edge<string>> g = new Graph<string, Edge<string>>();

            g.Add("A");
            g.Add("B");
            g.Add("C");
            g.Add("D");

            g.Connect("A", "B", new DirectedEdge<string>(g.GetByValue("A"), g.GetByValue("B"), Edge<string>.Relation.ParentToChild));
            g.Connect("A", "C", new DirectedEdge<string>(g.GetByValue("A"), g.GetByValue("C"), Edge<string>.Relation.ParentToChild));
            g.Connect("B", "C", new DirectedEdge<string>(g.GetByValue("B"), g.GetByValue("C"), Edge<string>.Relation.ParentToChild));
            g.Connect("B", "D", new DirectedEdge<string>(g.GetByValue("B"), g.GetByValue("D"), Edge<string>.Relation.ParentToChild));
            g.Connect("D", "A", new DirectedEdge<string>(g.GetByValue("D"), g.GetByValue("A"), Edge<string>.Relation.ParentToChild));

            return g;
        }

        private static Graph<string, WeightedEdge<string>> GetWeightedGraph()
        {
            Graph<string, WeightedEdge<string>> g = new Graph<string, WeightedEdge<string>>();

            g.Add("A");
            g.Add("B");
            g.Add("C");
            g.Add("D");
            g.Add("E");
            g.Add("F");

            g.Connect("A", "B", new WeightedEdge<string>(5, g.GetByValue("A"), g.GetByValue("B")));
            g.Connect("A", "D", new WeightedEdge<string>(1, g.GetByValue("A"), g.GetByValue("D")));
            g.Connect("B", "C", new WeightedEdge<string>(1, g.GetByValue("B"), g.GetByValue("C")));
            g.Connect("D", "E", new WeightedEdge<string>(1, g.GetByValue("D"), g.GetByValue("E")));
            g.Connect("E", "F", new WeightedEdge<string>(1, g.GetByValue("E"), g.GetByValue("F")));
            g.Connect("D", "F", new WeightedEdge<string>(3, g.GetByValue("D"), g.GetByValue("F")));
            g.Connect("C", "F", new WeightedEdge<string>(1, g.GetByValue("C"), g.GetByValue("F")));

            return g;
        }
    }
}
