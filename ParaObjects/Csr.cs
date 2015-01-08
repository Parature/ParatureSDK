using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class Csr : ObjectBaseProperties
    {
        // Specific properties for this module

        private Int64 _CsrID = 0;

        public Int64 CsrID
        {
            get { return _CsrID; }
            set { _CsrID = value; }
        }
        private string _Full_Name = "";

        public string Full_Name
        {
            get { return _Full_Name; }
            set { _Full_Name = value; }
        }
        public string Email = "";
        public string Fax = "";
        public string Phone_1 = "";
        public string Phone_2 = "";
        public string Screen_Name = "";
        /// <summary>
        /// The following strings are the valid options for Date_Format:
        /// mm/dd/yyyy | mm/dd/yy | dd/mm/yyyy | dd/mm/yy | month dd, yyyy | month dd, yy
        /// </summary>
        public string Date_Format = "";
        public string Password = "";
        public Timezone Timezone = new Timezone();
        public Status Status = new Status();
        public DateTime Date_Created;
        public List<Role> Role = new List<Role>();

        public Csr()
        {
        }

        public Csr(Csr csr)
        {
            this.CsrID = csr.CsrID;
            this.Full_Name = csr.Full_Name;
            this.Email = csr.Email;
            this.Fax = csr.Fax;
            this.Phone_1 = csr.Phone_1;
            this.Phone_2 = csr.Phone_2;
            this.Screen_Name = csr.Screen_Name;
            this.Date_Format = csr.Date_Format;
            this.Status = new Status(csr.Status);
            this.Timezone = new Timezone(csr.Timezone);
            this.Date_Created = csr.Date_Created;
            this.Role = new List<Role>(csr.Role);
            this.Password = csr.Password;
        }
    }
}