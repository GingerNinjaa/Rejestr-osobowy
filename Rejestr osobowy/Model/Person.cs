using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Rejestr_osobowy.Model
{
    [Serializable()]
    [XmlRoot("Lista Osób")]
    [XmlType(TypeName ="Osoba")]
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string streat { get; set; }
        public string number { get; set; }
    }
}
