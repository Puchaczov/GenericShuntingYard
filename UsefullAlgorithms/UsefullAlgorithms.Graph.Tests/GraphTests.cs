using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UsefullAlgorithms.Graph.Tests
{
    [TestClass]
    public class GraphTests
    {
        private Graph<string, Edge<string>> g = GetGraph();

        [TestMethod]
        public void Graphs_CheckCounts()
        {
            Assert.AreEqual(4, g.VerticlesCount);
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
            g.Connect("A", "A", new Edge<string>(g.GetByValue("A"), g.GetByValue("A")));
            Assert.IsTrue(g.HasEdge("A", "A"));
        }

        private static Graph<string, Edge<string>> GetGraph()
        {
            Graph<string, Edge<string>> g = new Graph<string, Edge<string>>();

            g.Add("A");
            g.Add("B");
            g.Add("C");
            g.Add("D");

            g.Connect("A", "B", new Edge<string>(g.GetByValue("A"), g.GetByValue("B")));
            g.Connect("A", "C", new Edge<string>(g.GetByValue("A"), g.GetByValue("C")));
            g.Connect("B", "C", new Edge<string>(g.GetByValue("B"), g.GetByValue("C")));
            g.Connect("B", "D", new Edge<string>(g.GetByValue("B"), g.GetByValue("D")));
            g.Connect("D", "A", new Edge<string>(g.GetByValue("D"), g.GetByValue("A")));

            return g;
        }
    }
}
