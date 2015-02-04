using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualBasic;
using ParatureSDK.Fields;
using ParatureSDK.ParaHelper;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.XmlToObjectParser
{
    /// <summary>
    /// Includes all common parsing methods used by the other XML parser classes.
    /// </summary>
    internal class CommonParser
    {
        /// <summary>
        /// This methods will fill and return a custom field object. Whenever Parsing an XML and getting a "custom_field" node, just pass that node to this method, and it will return the filled custom field object.
        /// </summary>
        public static CustomField FillCustomField(bool MinimalisticLoad, XmlNode Node)
        {
            var cf = new CustomField
            {
                Name = Node.Attributes["display-name"].Value,
                Id = Int64.Parse(Node.Attributes["id"].Value)
            };

            if (ParserUtils.CheckNodeAttributeNotNull(Node, "required") == true)
            {
                try
                {
                    cf.Required = Convert.ToBoolean(Node.Attributes["required"].Value);
                }
                catch (Exception exx)
                {
                    cf.Required = false;
                }
            }
            else
            {
                cf.Required = false;
            }

            cf.MaxLength = 0;
            if (ParserUtils.CheckNodeAttributeNotNull(Node, "max-length") == true)
            {
                if (String.IsNullOrEmpty(Node.Attributes["max-length"].Value) == false &&
                    Information.IsNumeric(Node.Attributes["max-length"].Value))
                {
                    cf.MaxLength = Int32.Parse(Node.Attributes["max-length"].Value);
                }
            }


            if (ParserUtils.CheckNodeAttributeNotNull(Node, "editable") == true)
            {
                try
                {
                    cf.Editable = Convert.ToBoolean(Node.Attributes["editable"].Value);
                }
                catch (Exception exx)
                {
                    cf.Editable = false;
                }
            }
            else
            {
                cf.Editable = false;

            }

            //parse the data type
            try
            {
                cf.DataType = ParaEnumProvider.CustomFieldDataTypeProvider(Node.Attributes["data-type"].Value);
            }
            catch (Exception exx)
            {
                cf.DataType = ParaEnums.FieldDataType.Unknown;
            }


            if (ParserUtils.CheckNodeAttributeNotNull(Node, "Dependent") == true)
            {
                cf.Dependent = Convert.ToBoolean(Node.Attributes["Dependent"].Value);

            }
            if (ParserUtils.CheckNodeAttributeNotNull(Node, "multi-value") == true)
            {
                cf.MultiValue = Convert.ToBoolean(Node.Attributes["multi-value"].Value);

            }
            else
            {
                cf.MultiValue = false;

            }

            if (Node.ChildNodes.Count > 0)
            {
                // Only if there are children nodes, which implies a multivalue fields
                // With optionally field dependencies.

                //Since even when this is a regular customer, the inner xml will be detected
                //as a child node, the ismultivalue flag will let us know if it is really a
                //multivalue field.
                bool ismultivalue = false;

                foreach (XmlNode optionNode in Node.ChildNodes)
                {
                    if (optionNode.LocalName.ToLower() == "option")
                    {
                        ismultivalue = ExtractOptionFieldFromXmlNode(MinimalisticLoad, optionNode, cf);
                    }

                    if (ismultivalue == false)
                    {
                        var nodeText = HelperMethods.SafeHtmlDecode(ParserUtils.NodeGetInnerText(Node));
                        var dataType = Node.Attributes["data-type"].Value.ToLower();
                        //won't see static field data types or multi values 
                        switch (dataType)
                        {
                            case "date":
                                DateTime result;
                                nodeText = nodeText.Replace("z", "");

                                if (DateTime.TryParse(nodeText, out result))
                                {
                                    cf.Value = result;
                                }
                                break;
                            case "boolean":
                                cf.Value = Convert.ToBoolean(nodeText);
                                break;
                            case "string":
                                cf.Value = nodeText;
                                break;
                            case "int":
                                cf.Value = Convert.ToInt32(nodeText);
                                break;
                            default:
                                //no idea what the data type is, so assume its a string
                                cf.Value = nodeText;
                                break;
                        }

                    }
                }
            }

            return cf;
        }

        private static bool ExtractOptionFieldFromXmlNode(bool minimalisticLoad, XmlNode optionNode,
            CustomField cf)
        {
            var ismultivalue = true;
            var cfo = new CustomFieldOptions
            {
                CustomFieldOptionID = Int64.Parse(optionNode.Attributes["id"].Value)
            };
            if (ParserUtils.CheckNodeAttributeNotNull(optionNode, "Dependent") == true)
            {
                cfo.Dependent = Convert.ToBoolean(optionNode.Attributes["Dependent"].Value);
            }
            if (ParserUtils.CheckNodeAttributeNotNull(optionNode, "selected") == true)
            {
                cfo.IsSelected = Convert.ToBoolean(optionNode.Attributes["selected"].Value);
            }
            else
            {
                cfo.IsSelected = false;
            }


            foreach (XmlNode child in optionNode.ChildNodes)
            {
                if (child.LocalName.ToLower() == "value")
                {
                    cfo.CustomFieldOptionName = child.InnerText;
                }

                if (child.LocalName.ToLower() == "enables" && minimalisticLoad == false)
                {
                    //// TO DO here: Add logic for custom fields option dependencies
                    var cfod = new DependantCustomFields();
                    if (child.FirstChild != null)
                    {
                        string customField = child.FirstChild.InnerText.Substring(0,
                            child.FirstChild.InnerText.IndexOf("]") + 1);
                        string tmp = "";
                        foreach (char c in customField)
                        {
                            if (Char.IsNumber(c))
                            {
                                tmp += c.ToString();
                            }
                        }
                        if (child.FirstChild.InnerText.Contains("/Option"))
                        {
                            string[] options =
                                child.FirstChild.InnerText.Substring(
                                    child.FirstChild.InnerText.IndexOf("/Option"))
                                    .Split(new String[] {"or"}, StringSplitOptions.RemoveEmptyEntries);
                            long[] ops = new long[options.Length];
                            for (int i = 0; i < options.Length; i++)
                            {
                                string temp = "";
                                foreach (char c in options[i])
                                {
                                    if (Char.IsNumber(c))
                                    {
                                        temp += c.ToString();
                                    }
                                }
                                ops[i] = Int64.Parse(temp);
                            }
                            cfod.DependantFieldOptions = ops;
                        }
                        if (String.IsNullOrEmpty(tmp) == true)
                        {
                            cfod = null;
                        }
                        else
                        {
                            cfod.DependantFieldID = Int64.Parse(tmp);
                            cfod.DependantFieldPath = child.FirstChild.InnerText;
                        }
                    }

                    //// Do the parsing of the Dependent custom field, and the options, above
                    //// Then uncomment the next line.
                    if (cfod != null)
                    {
                        cfo.DependantCustomFields.Add(cfod);
                    }
                }
            }
            if (cfo.IsSelected == true || minimalisticLoad == false)
            {
                cf.CustomFieldOptionsCollection.Add(cfo);
            }

            return ismultivalue;
        }

        /// <summary>
        /// Parses an action history node and returns a list of actionhistory objects.
        /// </summary>
        static public List<ActionHistory> FillActionHistory(XmlNode Node)
        {

            List<ActionHistory> ahlist = new List<ActionHistory>();

            foreach (XmlNode Historychild in Node.ChildNodes)
            {
                ActionHistory ah = new ActionHistory();

                ah.ActionHistoryID = Int64.Parse(Historychild.Attributes["id"].InnerText.ToString());

                foreach (XmlNode child in Historychild.ChildNodes)
                {
                    if (child.LocalName.ToLower() == "history_attachments")
                    {
                        ah.History_Attachments = CommonParser.FillAttachments(child);
                    }

                    if (child.LocalName.ToLower() == "action")
                    {
                        ah.Action.Id = Int32.Parse(child.Attributes["id"].Value);
                        ah.Action.Name = child.Attributes["name"].Value;
                    }
                    if (child.LocalName.ToLower() == "old_status")
                    {
                        ah.Old_Status.StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        ah.Old_Status.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "new_status")
                    {
                        ah.New_Status.StatusID = Int32.Parse(child.ChildNodes[0].Attributes["id"].Value);
                        ah.New_Status.Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "comments")
                    {
                        ah.Comments = child.InnerText.ToString();
                    }
                    if (child.LocalName.ToLower() == "show_to_customer")
                    {
                        if (!Boolean.TryParse(child.InnerText, out ah.Show_To_Customer))
                        {
                            //uh oh
                        }
                    }
                    if (child.LocalName.ToLower() == "time_spent")
                    {
                        ah.Time_Spent = Int32.Parse(child.InnerText.ToString());
                    }
                    if (child.LocalName.ToLower() == "action_date")
                    {
                        ah.Action_Date = DateTime.Parse(child.InnerText.ToString());
                    }
                    if (child.LocalName.ToLower() == "action_performer")
                    {
                        string performer;

                        if (child.HasChildNodes)
                        {
                            performer = child.ChildNodes[0].LocalName.ToLower();
                        }
                        else
                        {
                            performer = child.Attributes["performer-type"].Value.ToLower();
                        }
                        if (performer == "csr")
                        {
                            ah.Action_Performer.ActionHistoryPerformerType = ParaEnums.ActionHistoryPerformerType.Csr;
                            ah.Action_Performer.CsrPerformer.Id = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value);
                            ah.Action_Performer.CsrPerformer.Full_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                        else if (performer == "customer")
                        {
                            ah.Action_Performer.ActionHistoryPerformerType = ParaEnums.ActionHistoryPerformerType.Customer;
                            ah.Action_Performer.CustomerPerformer.Id = Int64.Parse(child.ChildNodes[0].Attributes["id"].Value);
                            ah.Action_Performer.CustomerPerformer.First_Name = child.ChildNodes[0].ChildNodes[0].InnerText.ToString();
                        }
                        else
                        {
                            ah.Action_Performer.ActionHistoryPerformerType = ParaEnums.ActionHistoryPerformerType.System;
                        }
                    }
                }

                ahlist.Add(ah);

            }

            return ahlist;

        }


        /// <summary>
        /// Parses an Attachments node and returns a list of Attachments objects.
        /// </summary>
        static public List<Attachment> FillAttachments(XmlNode Node)
        {
            List<Attachment> atlist = new List<Attachment>();
            foreach (XmlNode attachNode in Node.ChildNodes)
            {
                Attachment at = new Attachment();

                at = FillAttachment(attachNode);
                atlist.Add(at);
            }
            return atlist;
        }


        static public Attachment FillAttachment(XmlNode Node)
        {

            Attachment at = new Attachment();

            at.AttachmentURL = Node.Attributes["href"].Value;
            at.GUID = Node.ChildNodes[0].InnerText.ToString();
            at.Name = Node.ChildNodes[1].InnerText.ToString();

            return at;

        }
    }
}