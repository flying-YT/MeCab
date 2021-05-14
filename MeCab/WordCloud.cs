using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MeCab
{
    public class WordCloud
    {
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

        private void DrawWord(AggregateData aggregate, int _maxCount)
        {
            double ratio = (double)aggregate.Count / (double)_maxCount;
            Console.WriteLine("ratio:" + ratio);
            int size = (int)Math.Round(250 * ratio);
            bool b = true;
            while(b)
            {
                int x = rnd.Next(0, width - 80);
                int y = rnd.Next(0, height - 50);
                demoG.DrawString(aggregate.Word, new Font(font, size), Brushes.Blue, x, y);
                Console.WriteLine("demo:" + aggregate.Word);
                if(CheckMeasureSize(aggregate.Word, new Font(font, size), x, y))
                {
                    g.DrawString(aggregate.Word, new Font(font, size), Brushes.Blue, x, y);
                    SetMeasureSize(aggregate.Word, new Font(font, size), x, y);
                    b = false;
                }
            }

        }

        private bool CheckMeasureSize(string str, Font fontData, int x, int y)
        {
            bool b = true;
            var size2 = demoG.MeasureString(str, fontData);
            Console.WriteLine("x=" + x + "~" + (x+size2.Width));
            Console.WriteLine("y=" + y + "~" + (y+size2.Height));

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

        private void SetMeasureSize(string str, Font fontData, int x, int y)
        {
            var size2 = g.MeasureString(str, fontData);
            for (int i = y; i < size2.Height+y; i++)
            {
                for (int w = x; w < size2.Width+x; w++)
                {
                    png[i, w] = true;
                }
            }
        }

        public void MakeImg(List<AggregateData> _list)
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

            img.Save(@"D:\データ収集\data.png");
        }

        private void ReleaseResources()
        {
            g.Dispose();
        }
    }
}
