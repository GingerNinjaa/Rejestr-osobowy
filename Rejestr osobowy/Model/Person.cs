using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rejestr_osobowy.Model
{
    [Serializable()]
    [XmlRoot("Osoby")]
    public class Person
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public Adress adress { get; set; }
    }
}
