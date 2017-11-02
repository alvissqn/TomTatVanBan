using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VnToolkit;

namespace LSATextSummarizationAccord
{
    public class TextRank
    {
        private const double DAMPING_FACTOR = 0.85;
        private string _pathToInputTxt = string.Empty;
        private List<Node> _listNode = new List<Node>();

        public string PathToInputTxt { get { return _pathToInputTxt; } }
        public List<Node> ListNode { get { return _listNode; } }

        public TextRank(string p_pathToInputTxt)
        {
            _pathToInputTxt = p_pathToInputTxt;
        }

        private void CreateNodeMap(List<List<ItemParagraph>> p_listParaGraph)
        {
            for (int i = 0; i < p_listParaGraph.Count; i++)
            {
                List<ItemParagraph> sentence1 = p_listParaGraph[i];

                for (int j = 0; j < p_listParaGraph.Count; j++)
                {
                    List<ItemParagraph> sentence2 = p_listParaGraph[j];

                    if (i != j)
                    {
                        Node srcNode;
                        Node destNode;

                        #region add vào node tổng

                        if (_listNode.Where(k => k.Tag.Equals(i)).Count() == 0)
                        {
                            srcNode = new Node(i);
                            _listNode.Add(srcNode);
                        }
                        else
                        {
                            srcNode = _listNode.First(k => k.Tag.Equals(i));
                        }

                        if (_listNode.Where(k => k.Tag.Equals(j)).Count() == 0)
                        {
                            destNode = new Node(j);
                            _listNode.Add(destNode);
                        }
                        else
                        {
                            destNode = _listNode.First(k => k.Tag.Equals(j));
                        }

                        #endregion

                        #region add vào ListInbound, ListOutbound của node con khi term trong câu này xuất hiện trong câu kia

                        bool contain = false;
                        foreach (ItemParagraph item1 in sentence1)
                        {
                            foreach (ItemParagraph item2 in sentence2)
                            {
                                if (item1.GiaTri.Equals(item2.GiaTri) && item1.TenKieu.Equals(item2.TenKieu))
                                {
                                    contain = true;
                                    break;
                                }
                            }
                        }

                        if (contain == true)
                        {
                            destNode.ListInbound.Add(srcNode);
                            srcNode.ListOutbound.Add(destNode);
                        }

                        #endregion
                    }
                }
            }
        }

        public void Process(Dictionary<int, double[]> p_tfidf, List<List<ItemParagraph>> p_listParaGraph)
        {
            CreateNodeMap(p_listParaGraph);

            //mình tạo dictCosine là để không mất công tính lại những cosine đã tính
            var dictionaryCosine = new Dictionary<Tuple<int, int>, double>();

            bool stop = false;

            // loop runs until old page rank and new page rank are within .001 of each other
            while (!stop)
            {
                double change = 0;
                foreach (Node x in _listNode)
                {
                    x.LastPageRank = x.PageRank;

                    double sum = 0;
                    foreach (Node y in x.ListInbound)
                    {
                        // calculate the summation of destination pageRank times 1/outbound link count (tính kiểu cũ là PageRank)
                        //sum += y.PageRank * (1.0 / ((double)y.ListOutbound.Count));

                        double tuSo = 0;
                        var keyTuSo = x.Tag < y.Tag ? new Tuple<int, int>(x.Tag, y.Tag) : new Tuple<int, int>(y.Tag, x.Tag);

                        if (!dictionaryCosine.ContainsKey(keyTuSo))
                        {
                            tuSo = SimilarityMatrics.FindCosineSimilarity(p_tfidf[y.Tag], p_tfidf[x.Tag]);
                            dictionaryCosine.Add(keyTuSo, tuSo);
                        }
                        else
                        {
                            tuSo = dictionaryCosine[keyTuSo];
                        }

                        double mauSo = 0;
                        foreach (Node z in y.ListOutbound)
                        {
                            double item = 0;
                            var keyMauSo = y.Tag < z.Tag ? new Tuple<int, int>(y.Tag, z.Tag) : new Tuple<int, int>(z.Tag, y.Tag);

                            if (!dictionaryCosine.ContainsKey(keyMauSo))
                            {
                                item = SimilarityMatrics.FindCosineSimilarity(p_tfidf[z.Tag], p_tfidf[y.Tag]);
                                dictionaryCosine.Add(keyMauSo, item);
                            }
                            else
                            {
                                item = dictionaryCosine[keyMauSo];
                            }
                            mauSo += item;
                        }

                        sum += y.PageRank * tuSo / mauSo;
                    }

                    // multiply summation by damping factor and add to leading term to complete algorithm iteration
                    x.PageRank = (1.0 - DAMPING_FACTOR) + DAMPING_FACTOR * sum;

                    change += Math.Abs(x.PageRank - x.LastPageRank);
                }

                //see if we need to go another iteration
                if (change < .001)
                {
                    stop = true;
                }
            }

            dictionaryCosine.Clear();
        }

        public string CreateSummary(int p_numOfRetainSentence)
        {
            List<int> sentencePosition = _listNode.OrderByDescending(k => k.PageRank).Take(p_numOfRetainSentence).Select(k => k.Tag).OrderBy(k => k).ToList();

            string[] readText = File.ReadAllLines(_pathToInputTxt);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < sentencePosition.Count; i++)
            {
                result.AppendLine(readText[sentencePosition[i]]);
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
