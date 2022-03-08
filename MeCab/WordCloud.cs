using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MeCab
{
    public class WordCloud
    {
        private readonly int maxWordSize = 800;     // 最大文字サイズ 500
        private readonly int minWordSize = 8;
        readonly Bitmap img = null;
        readonly Bitmap demo = null;
        readonly Graphics g;
        readonly Graphics demoG;
        Random rnd = new Random();
        string font = "MS UI Gothic";
        readonly int width;
        readonly int height;

        bool[,] png = null;

        public WordCloud(int _width, int _height)
        {
            width = _width;
            height = _height;
            img = new Bitmap(_width, _height);
            demo = new Bitmap(_width, _height);
            g = Graphics.FromImage(img);
            demoG = Graphics.FromImage(demo);
            png = new bool[_height, _width];
        }

        public void SetFont(string _font)
        {
            font = _font;
        }

        private void DrawWord(AggregateData aggregate, int _maxCount)                           // 文字を描画する
        {
            double ratio = (double)aggregate.Count / (double)_maxCount;
            //            int size = (int)Math.Round(maxWordSize * ratio);
            int size = MeasurementWordSize(ratio);
            bool b = true;
            while(b)
            {
                int x = rnd.Next(0, width - 30);
                int y = rnd.Next(0, height - 30);
                //                demoG.DrawString(aggregate.Word, new Font(font, size), Brushes.Blue, x, y);
                demoG.DrawString(aggregate.Word, new Font(font, size), FontColor(), x, y);
                if (CheckMeasureSize(aggregate.Word, new Font(font, size), x, y))
                {
                    // g.DrawString(aggregate.Word, new Font(font, size), Brushes.Blue, x, y);
                    g.DrawString(aggregate.Word, new Font(font, size), FontColor(), x, y);
                    SetMeasureSize(aggregate.Word, new Font(font, size), x, y);
                    b = false;
                }
            }

        }

        private Brush FontColor()
        {
            int n = rnd.Next(3);
            switch(n)
            {
                case 0: return Brushes.Aqua;
                case 1: return Brushes.SpringGreen;
                case 2: return Brushes.Magenta;
                default: return Brushes.Blue;
            }
        }

        private int MeasurementWordSize(double _ratio)              // フォントサイズの最小値を決める
        {
            int size = (int)Math.Round(maxWordSize * _ratio);
            if(size < minWordSize)
            {
                size = minWordSize;
            }
            return size;
        }

        private bool CheckMeasureSize(string str, Font fontData, int x, int y)  // 描画文字と位置を計測し、範囲内かほかの文字と重なっていないか確認する。
        {
            bool b = true;
            var size2 = demoG.MeasureString(str, fontData);
            if(height <= size2.Height+y )
            {
                return false;
            }
            else if(width <= size2.Width+x)
            {
                return false;
            }
            for (int i = y; i < size2.Height+y; i++)
            {
                for (int w = x; w < size2.Width+x; w++)
                {
                    if (png[i, w])
                    {
                        b = false;
                        break;
                    }
                }
            }
            return b;
        }

        private void SetMeasureSize(string str, Font fontData, int x, int y)    // 描画位置を配列に格納する
        {
            var size2 = g.MeasureString(str, fontData);
            for (int i = y+0; i < size2.Height+y-0; i++)    // +2 -2
            {
                for (int w = x+4 ; w < size2.Width+x-4; w++) // +5 -5
                {
                    png[i, w] = true;
                }
            }
            Console.WriteLine("配置str:" + str );
        }

        public void MakeImg(List<AggregateData> _list, string outputPath)                          // 画像を出力する
        {
            g.FillRectangle(Brushes.White, g.VisibleClipBounds);

            int maxCount = 0;
            foreach(AggregateData data in _list)
            {
                maxCount += data.Count;
            }

            foreach (AggregateData data in _list)
            {
                DrawWord(data, maxCount);
            }

            string path = outputPath + "\\data.png";
            img.Save(path);
        }

        private void ReleaseResources()
        {
            g.Dispose();
        }
    }
}
