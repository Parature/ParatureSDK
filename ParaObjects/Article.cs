using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Knowledge Base module.
    /// </summary>
    public partial class Article : ObjectBaseProperties
    {
        // Specific properties for this module

        /// <summary>
        /// The unique identified of the Knowledge Base id.
        /// </summary>
        public Int64 Articleid = 0;
        public string Date_Created = "";
        public string Date_Updated = "";
        /// <summary>
        /// The answer to the knowledge base question.
        /// </summary>
        public string Answer = "";
        /// <summary>
        /// The date this Article will expire on.
        /// </summary>
        public string Expiration_Date = "";
        /// <summary>
        /// Whether this article is published or not.
        /// </summary>
        public Boolean Published = new Boolean();
        /// <summary>
        /// The question asked for this Article.
        /// </summary>
        public string Question = "";
        /// <summary>
        /// The average rating this article received.
        /// </summary>
        public Int32 Rating = 0;
        /// <summary>
        /// The number of times this article has been viewed.
        /// </summary>
        public Int32 Times_Viewed = 0;
        public Csr Modified_By = new Csr();
        public Csr Created_By = new Csr();

        /// <summary>
        /// List of Folders under which this article is listed.
        /// </summary>
        public List<Folder> Folders = new List<Folder>();

        /// <summary>
        /// List of Sla type objects. These SLAs are the ones allowed to see this article.
        /// </summary>
        public List<Sla> Permissions = new List<Sla>();

        /// <summary>
        /// List of products that are linked to this article. In case your config uses this feature.
        /// </summary>
        public List<Product> Products = new List<Product>();

        public Article()
        {
        }

        public Article(Article article)
            : base(article)
        {
            this.Articleid = article.Articleid;
            this.Date_Created = article.Date_Created;
            this.Date_Updated = article.Date_Updated;
            this.Answer = article.Answer;
            this.Expiration_Date = article.Expiration_Date;
            this.Rating = article.Rating;
            this.Times_Viewed = article.Times_Viewed;
            this.Modified_By = new Csr(article.Modified_By);
            this.Created_By = new Csr(article.Created_By);
            this.Folders = new List<Folder>(article.Folders);
            this.Permissions = new List<Sla>(article.Permissions);
            this.Products = new List<Product>(article.Products);
        }
    }
}