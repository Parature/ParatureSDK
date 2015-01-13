using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class Csr : ParaEntityBaseProperties
    {
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
            Id = csr.Id;
            Full_Name = csr.Full_Name;
            Email = csr.Email;
            Fax = csr.Fax;
            Phone_1 = csr.Phone_1;
            Phone_2 = csr.Phone_2;
            Screen_Name = csr.Screen_Name;
            Date_Format = csr.Date_Format;
            Status = new Status(csr.Status);
            Timezone = new Timezone(csr.Timezone);
            Date_Created = csr.Date_Created;
            Role = new List<Role>(csr.Role);
            Password = csr.Password;
        }
    }
}