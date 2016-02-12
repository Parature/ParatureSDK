using ParatureSDK.Fields;
using ParatureSDK.ParaObjects.EntityReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParatureSDK.ParaObjects
{
	/// <summary>
	/// Representation of the ArticleTranslation entity in the API
	/// </summary>
	public class ArticleTranslation : ParaEntity, IMutableEntity
	{
		/// <summary>
		/// Translated Article Answer
		/// </summary>
		public string Answer_Translation
		{
			get { return GetFieldValue<string>("Answer_Translation"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Answer_Translation");
				if (field == null)
				{
					field = new StaticField
					{
						Name = "Answer_Translation",
						FieldType = "text",
						DataType = "string"
					};
					StaticFields.Add(field);
				}

				field.Value = value;
			}
		}

		/// <summary>
		/// Reference to the Article being translated
		/// </summary>
		public ArticleReference Article
		{
			get { return GetFieldValue<ArticleReference>("Primary_Article"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Primary_Article");
				if (field == null)
				{
					field = new StaticField
					{
						Name = "Primary_Article",
						FieldType = "entity",
						DataType = "entity"
					};
					StaticFields.Add(field);
				}

				field.Value = value;
			}
		}


		/// <summary>
		/// Translated keyword values
		/// </summary>
		public string Keywords_Translation
		{
			get { return GetFieldValue<string>("Keywords_Translation"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Keywords_Translation");
				if (field == null)
				{
					field = new StaticField
					{
						Name = "Keywords_Translation",
						FieldType = "text",
						DataType = "string"
					};
					StaticFields.Add(field);
				}

				field.Value = value;
			}
		}

		/// <summary>
		/// Language string for the translation
		/// </summary>
		public string Language
		{
			get { return GetFieldValue<string>("Language"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Language");
				if (field == null)
				{
					field = new StaticField
					{
						Name = "Language",
						FieldType = "text",
						DataType = "string"
					};
					StaticFields.Add(field);
				}

				field.Value = value;
			}
		}

		/// <summary>
		/// Flag for whether or not this translation needs updating
		/// </summary>
		public bool Needs_Update
		{
			get { return GetFieldValue<bool>("Needs_Update"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Needs_Update");
				if (field == null)
				{
					field = new StaticField
					{
						Name = "Needs_Update",
						FieldType = "checkbox",
						DataType = "boolean"
					};
					StaticFields.Add(field);
				}

				field.Value = value;
			}
		}

		/// <summary>
		/// Indicates whether the translation is published
		/// </summary>
		public bool Published
		{
			get { return GetFieldValue<bool>("Published"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Published");
				if (field == null)
				{
					field = new StaticField
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
		/// Translated Question
		/// </summary>
		public string Question_Translation
		{
			get { return GetFieldValue<string>("Question_Translation"); }
			set
			{
				var field = StaticFields.FirstOrDefault(f => f.Name == "Question_Translation");
				if (field == null)
				{
					field = new StaticField
					{
						Name = "Question_Translation",
						FieldType = "text",
						DataType = "string"
					};
					StaticFields.Add(field);
				}

				field.Value = value;
			}
		}

		public override string GetReadableName()
		{
			return Question_Translation;
		}
	}
}
