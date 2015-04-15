using System;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Knowledge Base module.
    /// </summary>
    public class Article : ParaEntity
    {
        public DateTime Date_Created
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Created");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Created");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Created",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public DateTime Date_Updated
        {
            get
            {
                return GetFieldValue<DateTime>("Date_Updated");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Date_Updated");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Date_Updated",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool Hide_On_Portal
        {
            get
            {
                return GetFieldValue<bool>("Hide_On_Portal");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Hide_On_Portal");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Hide_On_Portal",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool Is_Portal_Search_Pinned 
        {
            get
            {
                return GetFieldValue<bool>("Is_Portal_Search_Pinned ");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Is_Portal_Search_Pinned ");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Is_Portal_Search_Pinned ",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public bool Is_Desk_Search_Pinned
        {
            get
            {
                return GetFieldValue<bool>("Is_Desk_Search_Pinned");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Is_Desk_Search_Pinned");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Is_Desk_Search_Pinned",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The answer to the knowledge base question.
        /// </summary>
        public string Answer
        {
            get
            {
                return GetFieldValue<string>("Answer");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Answer");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Answer",
                        FieldType = "text",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        /// <summary>
        /// The date this Article will expire on.
        /// </summary>
        public DateTime? Expiration_Date
        {
            get
            {
                return GetFieldValue<DateTime?>("Expiration_Date");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Expiration_Date");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Expiration_Date",
                        FieldType = "usdate",
                        DataType = "date"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// Whether this article is published or not.
        /// </summary>
        public Boolean Published
        {
            get
            {
                return GetFieldValue<bool>("Published");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Published");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Published",
                        FieldType = "checkbox",
                        DataType = "boolean"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The question asked for this Article.
        /// </summary>
        public string Question
        {
            get
            {
                return GetFieldValue<string>("Question");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Question");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Question",
                        FieldType = "textarea",
                        DataType = "string"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The average rating this article received.
        /// </summary>
        public Int32 Rating
        {
            get
            {
                return GetFieldValue<Int32>("Rating");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Rating");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Rating",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int32 Portal_Search_Weight
        {
            get
            {
                return GetFieldValue<Int32>("Portal_Search_Weight");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Portal_Search_Weight");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Portal_Search_Weight",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Int32 Desk_Search_Weight
        {
            get
            {
                return GetFieldValue<Int32>("Desk_Search_Weight");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Desk_Search_Weight");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Desk_Search_Weight",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// The number of times this article has been viewed.
        /// </summary>
        public Int32 Times_Viewed
        {
            get
            {
                return GetFieldValue<Int32>("Times_Viewed");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Times_Viewed");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Times_Viewed",
                        FieldType = "int",
                        DataType = "int"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CsrReference Modified_By 
        {
            get
            {
                return GetFieldValue<CsrReference>("Modified_By");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Modified_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Modified_By",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }
        public CsrReference Created_By
        {
            get
            {
                return GetFieldValue<CsrReference>("Created_By");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Created_By");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Created_By",
                        FieldType = "entity",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// List of Folders under which this article is listed.
        /// </summary>
        public List<ArticleFolder> Folders
        {
            get
            {
                return GetFieldValue<List<ArticleFolder>>("Folders");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Folders");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Folders",
                        FieldType = "entitymultiple",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// List of Sla type objects. These SLAs are the ones allowed to see this article.
        /// </summary>
        public List<Sla> Permissions
        {
            get
            {
                return GetFieldValue<List<Sla>>("Permissions");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Permissions");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Permissions",
                        FieldType = "dropdown",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// List of products that are linked to this article. In case your config uses this feature.
        /// </summary>
        public List<Product> Products
        {
            get
            {
                return GetFieldValue<List<Product>>("Products");
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Products");
                if (field == null)
                {
                    field = new StaticField()
                    {
                        Name = "Products",
                        FieldType = "entitymultiple",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        /// <summary>
        /// List of articles that are related to the this article. Introduced in 15.1
        /// </summary>
        public List<Article> Related_Articles
        {
            get
            {
                return GetFieldValue<List<Article>>("Related_Articles"); 
            }
            set
            {
                var field = StaticFields.FirstOrDefault(f => f.Name == "Related_Articles");
                if (field == null)
                {
                    field = new StaticField
                    {
                        Name = "Related_Articles",
                        FieldType = "entitymultiple",
                        DataType = "entity"
                    };
                    StaticFields.Add(field);
                }

                field.Value = value;
            }
        }

        public Article()
        {
        }

        public Article(Article article)
            : base(article)
        {
            Id = article.Id;
            Date_Created = article.Date_Created;
            Date_Updated = article.Date_Updated;
            Answer = article.Answer;
            Expiration_Date = article.Expiration_Date;
            Rating = article.Rating;
            Times_Viewed = article.Times_Viewed;
            Modified_By = article.Modified_By;
            Created_By = article.Created_By;
            Folders = new List<ArticleFolder>(article.Folders);
            Permissions = new List<Sla>(article.Permissions);
            Products = new List<Product>(article.Products);
        }

        public override string GetReadableName()
        {
            return Question;
        }
    }
}