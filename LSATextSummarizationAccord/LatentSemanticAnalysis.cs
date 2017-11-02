using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Accord.MachineLearning;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics;
using Accord.Statistics.Analysis;

namespace LSATextSummarizationAccord
{
    public class LatentSemanticAnalysis
    {
        private string _pathToInputTxt = string.Empty;
        private double[] _length;

        public string PathToInputTxt { get { return _pathToInputTxt; } }
        public double[] Length { get { return _length; } }

        public LatentSemanticAnalysis(string p_pathToInputTxt)
        {
            _pathToInputTxt = p_pathToInputTxt;
        }

        public void Process(Dictionary<int, double[]> p_tfidf)
        {
            #region chuyển thành dạng jagged array

            double[][] pixels = new double[p_tfidf.Count][];
            foreach (KeyValuePair<int, double[]> mem in p_tfidf)
            {
                pixels[mem.Key] = mem.Value;
            }

            #endregion

            #region đi xác định xem câu thứ mấy sẽ là câu tóm tắt, xem Figure 7, 8 trang 49, 50 file Thesis đính kèm (sentence selection sử dụng Cross Method)

            //lý do tranpose - đổi dòng thành cột là do dạng term-document của SVD là column đại diện câu, row đại diện cho các term
            var svd = new SingularValueDecomposition(pixels.ToMatrix().Transpose());
            double[,] V = svd.RightSingularVectors;
            double[][] jaggedArrayV = V.ToArray();

            //pre-processing, bé hơn average thì set value  = 0
            int numRow = jaggedArrayV.GetLength(0);
            int numCol = jaggedArrayV[0].GetLength(0);
            for (int i = 0; i < numRow; i++)
            {
                double sum = 0;
                for (int j = 0; j < jaggedArrayV[i].Length; j++)
                {
                    sum += jaggedArrayV[i][j];
                }
                double average = sum / jaggedArrayV[i].Length;

                for (int j = 0; j < numCol; j++)
                {
                    if (jaggedArrayV[i][j] < average)
                    {
                        jaggedArrayV[i][j] = 0;
                    }
                }
            }

            //tính length cho từng sentence
            _length = new double[numCol];
            for (int i = 0; i < numCol; i++)
            {
                for (int j = 0; j < numRow; j++)
                {
                    _length[i] += jaggedArrayV[j][i];
                }
            }

            #endregion
        }

        public string CreateSummary(int p_numOfRetainSentence)
        {
            #region xác định vị trí câu và sắp xếp theo thứ tự xuất hiện

            //xác định vị trí
            List<int> sentencePosition = new List<int>();
            double[] maxs = _length.OrderByDescending(k => k).Take(p_numOfRetainSentence).ToArray();
            for (int i = 0; i < p_numOfRetainSentence; i++)
            {
                int maxIndex = Array.IndexOf(_length, maxs[i]);
                sentencePosition.Add(maxIndex);
            }

            //sắp xếp từ theo thứ tự xuất hiện;
            sentencePosition = sentencePosition.OrderBy(k => k).ToList();

            #endregion

            #region xuất câu ra

            string[] readText = File.ReadAllLines(_pathToInputTxt);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < sentencePosition.Count; i++)
            {
                result.AppendLine(readText[sentencePosition[i]]);
                result.AppendLine();
            }

            return result.ToString();

            #endregion
        }
    }
}
