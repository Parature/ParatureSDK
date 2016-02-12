using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects.EntityReferences
{
	public class ArticleReference : EntityReference<Article>
	{
		[XmlElement("Article")]
		public Article Article
		{
			get { return base.Entity; }
			set { base.Entity = value; }
		}
	}
}
