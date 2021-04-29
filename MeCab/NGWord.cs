using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeCab
{
    public class NGWord
    {
        readonly List<string> ngWordList = new List<string>();

        public NGWord()
        {
            SetList();
        }

        private void SetList()
        {
            ngWordList.Add("人");
            ngWordList.Add("年");
            ngWordList.Add("日");
            ngWordList.Add("者");
            ngWordList.Add("の");
            ngWordList.Add("こと");
        }

        public bool CheckNG(string str)
        {
            bool b = false;
            foreach(string ngStr in ngWordList)
            {
                if (str == ngStr)
                {
                    b = true;
                    break;
                }
            }
            if (Regex.IsMatch(str, @"^[\d]+$"))
            {
                b = true;
            }
            return b;
        }
    }
}
