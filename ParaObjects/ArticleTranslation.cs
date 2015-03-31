using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
    public class ArticleTranslation : ParaEntityBaseProperties
    {
        /// <summary>
        /// Translated Article Answer
        /// </summary>
        public string Answer_Translation;

        /// <summary>
        /// Id of the Article the ArticleTranslation is referenced to
        /// </summary>
        public Int64 Articleid;

        public string Keywords_Translation;

        /// <summary>
        /// Language string for the translation
        /// </summary>
        public string Language;

        public bool Needs_Update;

        /// <summary>
        /// Indicates whether the translation is published
        /// </summary>
        public bool Published;

        /// <summary>
        /// Translated Question
        /// </summary>
        public string Question_Translation;
    }
}
