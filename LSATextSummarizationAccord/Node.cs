using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSATextSummarizationAccord
{
    public class Node
    {
        int _tag;
        List<Node> _listInbound;
        List<Node> _listOutbound;
        double _pageRank;
        double _lastPageRank;

        public int Tag { get { return _tag; } set { _tag = value; } }
        public List<Node> ListInbound { get { return _listInbound; } set { _listInbound = value; } }
        public List<Node> ListOutbound { get { return _listOutbound; } set { _listOutbound = value; } }
        public double PageRank { get { return _pageRank; } set { _pageRank = value; } }
        public double LastPageRank { get { return _lastPageRank; } set { _lastPageRank = value; } }

        public Node(int p_tag)
        {
            _tag = p_tag;
            _listInbound = new List<Node>();
            _listOutbound = new List<Node>();
            _pageRank = _lastPageRank = 1;
        }
    }
}
