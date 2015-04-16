using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using ParatureSDK.ParaObjects.EntityReferences;

namespace ParatureSDK.ParaObjects
{
    public class Action : ActionBase
    {
        public bool FullyLoaded;
        [XmlAttribute("name")]
        public string Name = "";
        public string Comment = "";
        /// <summary>
        /// Indicates whether this action will be visible to the customer or not
        /// Only used for tickets.
        /// </summary>
        public bool ShowToCust = false;
        public bool ShowToAdditionalContact = false;
        public bool NotifyParent = false;
        public string EmailText;
        public ArrayList EmailCsrList;
        public ArrayList EmailCustList;
        public int TimeSpentHours = 0;
        public int TimeSpentMinutes = 0;

        public List<Attachment> Action_Attachments = new List<Attachment>();

        /// <summary>
        /// This property will only be considered when 
        /// the action if of type Assign to Queue.
        /// </summary>
        public Int64? AssignToQueue = 0;

        /// <summary>
        /// This property will only be considered when 
        /// the action if of type Assign to CSR.
        /// </summary>
        public Int64? AssignToCsr = 0;

        public Action()
        {
        }

        public Action(Action action)
        {
            FullyLoaded = action.FullyLoaded;
            Id = action.Id;
            Name = action.Name;
            Comment = action.Comment;
            ShowToCust = action.ShowToCust;
            EmailText = action.EmailText;
            EmailCsrList = action.EmailCsrList;
            EmailCustList = action.EmailCustList;
            TimeSpentHours = action.TimeSpentHours;
            TimeSpentMinutes = action.TimeSpentMinutes;
            AssignToQueue = action.AssignToQueue;
            AssignToCsr = action.AssignToCsr;
            Action_Attachments = action.Action_Attachments;
        }
    }
}