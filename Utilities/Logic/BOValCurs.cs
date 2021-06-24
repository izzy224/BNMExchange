using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WPF_BNM_CURS.Utilities.DAL;
using WPF_BNM_CURS.Utilities.Models;

namespace WPF_BNM_CURS.Utilities.Logic
{
    public class BOValCurs
    {
        public ParseXML ParseXML;
        public List<Valute> Valutes;

        public BOValCurs(DateTime date)
        {
            ParseXML = new ParseXML();
            ParseXML.GetData(date);
            Valutes = new List<Valute>();

            XmlNodeList xmlValutes = ParseXML.Data.DocumentElement.SelectNodes("//ValCurs/Valute");

            foreach (XmlNode x in xmlValutes)
            {

                Valutes.Add(new Valute()
                {
                    NumCode = x.ChildNodes[0].InnerText.Trim(),
                    CharCode = x.ChildNodes[1].InnerText.Trim(),
                    Nominal = Int32.Parse(x.ChildNodes[2].InnerText.Trim()),
                    Name = x.ChildNodes[3].InnerText.Trim(),
                    Value = decimal.Parse(x.ChildNodes[4].InnerText.Trim())
                }) ;
            }
            
        }

        public List<Valute> GetData()
        {
            if (Valutes.Any())
            {
                return Valutes;
            }
            return null;

        }
        
    }
}
