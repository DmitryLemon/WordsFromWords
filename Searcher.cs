using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsFromWords
{
    class Searcher
    {

        private List<string> SearchResult = new List<string>();
        private List<string> WordList = new List<string>();
        List<int> WordListScore = new List<int>();

        public List<string> returnResult(string kekes, int choice)
        {
            switch(choice)
            {
                case 1: return search(kekes);
                    break;
                case 2: return readWordList(kekes);
                    break;
                default: return null;
                    break;
            }
        }
        public List<string> search(string searchword)
        {
            scoreListReset();
            for (int i = 0; i < searchword.Length; i++)
            {
                for (int j = 0; j < WordList.Count; j++)
                {
                    if (WordList[j].IndexOf(searchword[i]) != -1)
                    {
                        if (countLetters(WordList[j], searchword[i].ToString()) >= countLetters(searchword.Substring(i), searchword[i].ToString()))
                        WordListScore[j]++;
                    }
                }
            }

            SearchResult.Clear();
            for (int i = 0; i < WordList.Count; i++) 
            {
                if (WordList[i].Length <= WordListScore[i]) SearchResult.Add(WordList[i]);
            }

            return SearchResult;
        }

        public List<string> addWord(string newword, string wordspath)
        {
            readWordList(wordspath);

            if (WordList.IndexOf(newword) != -1)
            {
                System.Windows.Forms.MessageBox.Show("Это слово уже есть в списке");
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(wordspath, true))
                    {
                        sw.WriteLine(newword);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                WordList.Add(newword);
            }

            WordList.Sort();
            return WordList;
        }

        public List<string> readWordList(string wordspath)
        {
            WordList.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(wordspath, false))
                {
                    while (sr.Peek() != -1)
                    {
                        WordList.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            WordList.Sort();
            return WordList;
        }
        
        private void scoreListReset()
        {
            WordListScore.Clear();
            for (int i = 0; i < WordList.Count; i++)
            {
               WordListScore.Add(0);
            }
        }

        private int countLetters(string s, string ch)
        {
            int count = (s.Length - s.Replace(ch, "").Length) / ch.Length;
            return count;
        }
    }
}
