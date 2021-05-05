using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MeCab
{
    public class WordCloud
    {
        readonly Bitmap img = null;
        readonly Graphics g;
        Random rnd = new Random();
        string font = "MS UI Gothic";
        readonly int width;
        readonly int height;

        public WordCloud(int _width, int _height)
        {
            width = _width;
            height = _height;
            img = new Bitmap(_width, _height);
            g = Graphics.FromImage(img);
        }

        public void SetFont(string _font)
        {
            font = _font;
        }

        private void DrawWord(AggregateData aggregate, int _maxCount)
        {
            double ratio = (double)aggregate.Count / (double)_maxCount;
            Console.WriteLine("ratio:" + ratio);
            int size = (int)Math.Round(200 * ratio);
            g.DrawString(aggregate.Word, new Font(font, size), Brushes.Blue, rnd.Next(0, width-30), rnd.Next(0, height-30));
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
