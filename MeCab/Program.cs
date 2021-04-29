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
            Aggregate(nounList);
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

        private static void Aggregate(List<string> _nounList)
        {
            List<AggregateData> _list = new List<AggregateData>();

            foreach(string _noun in _nounList)
            {
                string[] strArray = _noun.Split(",");
                foreach(string str in strArray)
                {
                    int count = 0;
                    bool judge = true;
                    foreach (AggregateData aggregateData in _list)
                    {
                        //                        if (aggregateData.Word == _noun)
                        if (aggregateData.Word == str)
                        {
                            judge = false;
                            break;
                        }
                        count++;
                    }
//                    Console.WriteLine("str:" + str);
                    if (judge)
                    {
                        //                        _list.Add(new AggregateData { Word = _noun, Count = 1 });
                        _list.Add(new AggregateData { Word = str, Count = 1 });
                    }
                    else
                    {
                        AggregateData _aggregateData = _list[count];
                        _list.RemoveAt(count);
                        _list.Add(new AggregateData { Word = _aggregateData.Word, Count = _aggregateData.Count + 1 });
                    }
                }
                //Console.ReadKey();
            }

            NGWord NG = new NGWord();
            for(int i=0;i<5;i++)
            {
                int max = 0;
                int maxIndex = 0;
                int count = 0;
                foreach (AggregateData _aggregateData in _list)
                {
                    if (max < _aggregateData.Count)
                    {
                        max = _aggregateData.Count;
                        maxIndex = count;
                    }
                    count++;
                }
                if (NG.CheckNG(_list[maxIndex].Word))
                {
                    i--;
                }
                else
                {
                    Console.WriteLine("名詞：" + _list[maxIndex].Word + " 回数：" + _list[maxIndex].Count);
                }
                _list.RemoveAt(maxIndex);
            }
        }
    }

    public struct AggregateData
    {
        public string Word;
        public int Count;
    }
}
