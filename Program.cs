using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example Setting Keywords
            var creds = CredentialProvider.Creds;

            var keywords = new List<string>();
            keywords.Add("one");
            keywords.Add("two");

            ArticleKeywordsExamples.AddKeywords(10, keywords, creds);
        }
    }
}
