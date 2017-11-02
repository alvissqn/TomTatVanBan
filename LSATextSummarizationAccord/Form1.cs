using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using VnToolkit;

namespace LSATextSummarizationAccord
{
    public partial class Form1 : Form
    {
        string _pathToVnSentDetectorBat = @"C:\vnSentDetector\vnSentDetector.bat";
        string _pathToVnTokenizerBat = @"C:\vnTagger\vnTagger.bat";
        string _pathToInputTxt = string.Empty;
        string[] _stopWords;
        Dictionary<int, double[]> _tfidf = new Dictionary<int, double[]>();
        List<List<ItemParagraph>> _listSentence = new List<List<ItemParagraph>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_Retain.Text = "5";

            button_LSA.Enabled = false;
            
        }

        private void button_LoadData_Click(object sender, EventArgs e)
        {
            #region duyệt đến file vnSentDetector.bat, vnTokenizer.bat và file txt cần tóm tắt; xác định số câu muốn giữ

            if (_pathToVnSentDetectorBat.Equals(string.Empty))
            {
                using (var pathToBat = new OpenFileDialog() { Title = "Duyệt đến file vnSentDetector.bat." })
                {
                    if (pathToBat.ShowDialog() == DialogResult.OK)
                    {
                        _pathToVnSentDetectorBat = pathToBat.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (_pathToVnTokenizerBat.Equals(string.Empty))
            {
                using (var pathToBat = new OpenFileDialog() { Title = "Duyệt đến file vnTagger.bat." })
                {
                    if (pathToBat.ShowDialog() == DialogResult.OK)
                    {
                        _pathToVnTokenizerBat = pathToBat.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            using (var pathToInputTxt = new OpenFileDialog() { Title = "Duyệt đến file txt cần tóm tắt." })
            {
                if (pathToInputTxt.ShowDialog() == DialogResult.OK)
                {
                    _pathToInputTxt = pathToInputTxt.FileName;
                }
                else
                {
                    return;
                }
            }

            #endregion

            #region load danh sách từ dừng

            string pathStopWords = Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "vietnamese-stopwords.txt");
            _stopWords = System.IO.File.ReadAllText(pathStopWords).Split('\n');

            #endregion

            #region tách câu

            int timeOut = 0;
            if (checkBox_Limit60s.Checked == true)
            {
                timeOut = 60;
            }

            var vnSentDetectorFile = new VnSentDetectorFile(_pathToInputTxt, _pathToVnSentDetectorBat, "", true, timeOut);
            vnSentDetectorFile.ProcessDocument();

            #endregion

            #region tính TFIDF

            _tfidf.Clear();
            _listSentence.Clear();

            var vnTaggerFile = new VnTaggerFile(_pathToInputTxt, _pathToVnTokenizerBat, "", "", true, 60);
            _tfidf = vnTaggerFile.FindTFIDF(new string[] { "Np", "Nc", "Nu", "N", "V", "A", "P", "R" }, _stopWords, true, -1, 0, 0);
            _listSentence = vnTaggerFile.ListParagraph;

            #endregion

            button_LSA.Enabled = true;
           
        }

        private void button_LSA_Click(object sender, EventArgs e)
        {
            int choose = 0;
            if (int.TryParse(textBox_Retain.Text, out choose) == false)
            {
                throw new Exception("Lỗi nhập số câu muốn giữ.");
            }

            var tool = new LatentSemanticAnalysis(_pathToInputTxt);
            tool.Process(_tfidf);
            textBox_LSA.Text = tool.CreateSummary(choose);
        }

  
    }
}
