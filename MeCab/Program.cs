using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeCab
{
    class Program
    {
        private static readonly int indicateWordCount = 100;     // 表示するワード数　100
        private static string csvPath;                          // 読み込むファイルのパス
        private static string outputPath;                       // 書き出す先のパス

        static void Main(string[] args)
        {
            List<string> nounList = new List<string>();
            Console.WriteLine("読み込むCSVファイルのパスを入力してください。");
            csvPath = Console.ReadLine();
            Console.WriteLine("出力先のパスを入力してください。");
            outputPath = Console.ReadLine();
            string[] files = Directory.GetFiles(csvPath, "*.csv");

            foreach (string file in files)
            {
                Console.WriteLine("file:" + file);
                List<string> newsTextList = ReadCSV(file);
                foreach(string newsText in newsTextList)
                {
                    nounList.Add(ExtractionNoun(newsText));
                }
            }
            WriteData(nounList);
//            Aggregate(nounList);
            WordCloud wordCloud = new WordCloud(400, 200);
            wordCloud.MakeImg(ListAggregate(nounList), outputPath);
        }


        private static string ExtractionNoun(string text)           // 名詞で形態素解析を行う
        {
            string resultText = "";

            var tagger = MeCabTagger.Create();
            foreach (var node in tagger.ParseToNodes(text))
            {
                if(0 < node.CharType)
                {
                    if(node.Feature.Contains("名詞"))
                    {
                        resultText += node.Surface + ",";
                    }
                }
            }
            return resultText.TrimEnd(',');
        }

        private static List<string> ReadCSV(string filePath)                // 引数のファイルパスのcsvファイルを読み込む
        {
            List<string> _list = new List<string>();
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            while(sr.Peek() != -1)
            {
                string[] values = sr.ReadLine().Split(',');
                _list.Add(values[1]);
            }
            sr.Close();
            return _list;
        }

        private static void WriteData(List<string> _nounList)               // 形態素解析後のデータを一時的にファイル出力する
        {  
            StreamWriter sw = new StreamWriter(@"D:\データ収集\data.txt", false, Encoding.UTF8);
            foreach (string _noun in _nounList)
            {
                sw.WriteLine(_noun);
            }
            sw.Close();
        }

        private static List<string> MakeNounList(List<string> _list)
        {
            List<string> list = new List<string>();
            foreach(string str in _list)
            {
                string[] strArray = str.Split(",");
                foreach(string s in strArray)
                {
                    list.Add(s);
                }
            }
            return list;
        }

        private static List<AggregateData> ListAggregate(List<string> _nounList) // 集計サンプルメソッド
        {
            //            List<string> sampleList = _nounList;
            List<string> sampleList = MakeNounList(_nounList);
            List <AggregateData> _list = new List<AggregateData>();
            NGWord ng = new NGWord();

            while(sampleList.Count > 0)
            {
                int count = 0;
                string str = sampleList[0];
                foreach(string listStr in sampleList)       // 同じ語句の個数をカウントする
                {
                    if(str == listStr)
                    {
                        count++;
                    }
                }
                for(int i=0;i<count;i++)                    // カウントした語句はリストから削除する
                {
                    sampleList.Remove(str);
                }
                if(!ng.CheckNG(str))                         // NGリストにない場合
                {
                    _list.Add(new AggregateData { Word = str, Count = count });     // カウント済みリストに追加する
                }
            }
            Console.WriteLine("個数：" + _list.Count);

            List<AggregateData> returnList = new List<AggregateData>();
            for(int i=0;i<indicateWordCount;i++)             // 回数が多い順にlistを作成
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
                returnList.Add(new AggregateData { Word = _list[maxIndex].Word, Count = _list[maxIndex].Count});
                _list.RemoveAt(maxIndex);
            }

            foreach (AggregateData data in returnList)
            {
                Console.WriteLine("word:" + data.Word + " 回数：" + data.Count);
            }

            return returnList;
        }

        private static void Aggregate(List<string> _nounList)               // 形態素解析後の名詞データを集計する
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
                        if (aggregateData.Word == str)
                        {
                            judge = false;
                            break;
                        }
                        count++;
                    }
                    if (judge)
                    {
                        _list.Add(new AggregateData { Word = str, Count = 1 });
                    }
                    else
                    {
                        AggregateData _aggregateData = _list[count];
                        _list.RemoveAt(count);
                        _list.Add(new AggregateData { Word = _aggregateData.Word, Count = _aggregateData.Count + 1 });
                    }
                }
            }

            NGWord NG = new NGWord();
            List<AggregateData> dataList = new List<AggregateData>();
            for(int i=0;i<indicateWordCount;i++)
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
                    dataList.Add(new AggregateData() { Word = _list[maxIndex].Word, Count = _list[maxIndex].Count });
                }
                _list.RemoveAt(maxIndex);
            }

            WordCloud wordCloud = new WordCloud(400, 200);
            wordCloud.MakeImg(dataList, outputPath);
        }
    }

    public struct AggregateData
    {
        public string Word;
        public int Count;
    }
}
