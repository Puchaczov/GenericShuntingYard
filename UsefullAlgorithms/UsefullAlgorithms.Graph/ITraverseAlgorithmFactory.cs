using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    public interface ITraverseAlgorithmFactory<TGraphDataType, TEdgeDataType, out TReturnType> where TGraphDataType: IEquatable<TGraphDataType> where TEdgeDataType : Edge<TGraphDataType>
    {
        IEnumerator<TReturnType> Create(Graph<TGraphDataType, TEdgeDataType> graph, TGraphDataType root);
    }
}
