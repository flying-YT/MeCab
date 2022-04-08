using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeCab
{
    public class NGWord     // 形態素解析データから取り除く文字列を扱うクラス
    {
        readonly List<string> ngWordList = new List<string>();

        public NGWord()
        {
            SetList();
        }

        private void SetList()
        {
            ngWordList.Add("人");
            ngWordList.Add("万");
            ngWordList.Add("年");
            ngWordList.Add("日");
            ngWordList.Add("月");
            ngWordList.Add("分");
            ngWordList.Add("歳");
            ngWordList.Add("時");
            ngWordList.Add("者");
            ngWordList.Add("県");
            ngWordList.Add("市");
            ngWordList.Add("前");
            ngWordList.Add("中");
            ngWordList.Add("後");
            ngWordList.Add("的");
            ngWordList.Add("回");
            ngWordList.Add("円");
            ngWordList.Add("氏");
            ngWordList.Add("会");
            ngWordList.Add("性");
            ngWordList.Add("同");
            ngWordList.Add("大");
            ngWordList.Add("上");
            ngWordList.Add("化");
            ngWordList.Add("一");
            ngWordList.Add("今");
            ngWordList.Add("位");
            ngWordList.Add("初");
            ngWordList.Add("方");
            ngWordList.Add("戦");
            ngWordList.Add("私");
            ngWordList.Add("何");
            ngWordList.Add("度");
            ngWordList.Add("手");
            ngWordList.Add("選");
            ngWordList.Add("新");
            ngWordList.Add("区");
            ngWordList.Add("側");
            ngWordList.Add("代");
            ngWordList.Add("町");
            ngWordList.Add("派");
            ngWordList.Add("件");
            ngWordList.Add("億");
            ngWordList.Add("所");
            ngWordList.Add("点");
            ngWordList.Add("数");
            ngWordList.Add("元");
            ngWordList.Add("官");
            ngWordList.Add("率");
            ngWordList.Add("間");
            ngWordList.Add("系");
            ngWordList.Add("先");
            ngWordList.Add("以上");
            ngWordList.Add("午前");
            ngWordList.Add("午後");
            ngWordList.Add("お");
            ngWordList.Add("さ");
            ngWordList.Add("ら");
            ngWordList.Add("の");
            ngWordList.Add("ん");
            ngWordList.Add("ご");
            ngWordList.Add("新た");
            ngWordList.Add("それ");
            ngWordList.Add("こと");
            ngWordList.Add("これ");
            ngWordList.Add("さん");
            ngWordList.Add("よう");
            ngWordList.Add("ため");
            ngWordList.Add("たち");
            ngWordList.Add("もの");
            ngWordList.Add("ほか");
            ngWordList.Add("キロ");
            ngWordList.Add("ところ");
            ngWordList.Add("％");
            ngWordList.Add("・");
            ngWordList.Add(".");
            ngWordList.Add("(");
            ngWordList.Add(")");
            ngWordList.Add("-");
        }

        public bool CheckNG(string str)
        {
            bool b = false;
            if (Regex.IsMatch(str, @"^[\d]+$"))
            {
                return true;
            }
            foreach (string ngStr in ngWordList)
            {
                if (str == ngStr)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
    }
}
