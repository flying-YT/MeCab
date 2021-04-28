using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeCab
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> nounList = new List<string>();
            Console.WriteLine("Hello World!");
            string[] files = Directory.GetFiles(@"D:\データ収集\csv", "*.csv");
            foreach(string file in files)
            {
                List<string> newsTextList = ReadCSV(file);
                foreach(string newsText in newsTextList)
                {
//                    Console.WriteLine("newsText:" + newsText);
                    nounList.Add(ExtractionNoun(newsText));
                }
            }
            WriteData(nounList);
        }


        private static string ExtractionNoun(string text)
        {
            string resultText = "";

            var tagger = MeCabTagger.Create();
            foreach (var node in tagger.ParseToNodes(text))
            {
                if(0 < node.CharType)
                {
//                    Console.WriteLine("node.Feature:" + node.Feature);
                    if(node.Feature.Contains("名詞"))
                    {
                        resultText += node.Surface + ",";
                    }
                }
            }
            return resultText.TrimEnd(',');
        }

        private static List<string> ReadCSV(string filePath)
        {
            List<string> _list = new List<string>();
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            while(sr.Peek() != -1)
            {
                string[] values = sr.ReadLine().Split(',');
                _list.Add(values[1]);
//                Console.WriteLine("values[1]:" + values[1]);
            }
            sr.Close();
            return _list;
        }

        private static void WriteData(List<string> _nounList)
        {
            StreamWriter sw = new StreamWriter(@"D:\データ収集\data.txt", false, Encoding.UTF8);
            foreach(string _noun in _nounList)
            {
                sw.WriteLine(_noun);
            }
            sw.Close();
        }
    }
}
