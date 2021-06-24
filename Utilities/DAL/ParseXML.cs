using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WPF_BNM_CURS.Utilities.DAL
{
    public class ParseXML
    {
        public XmlDocument Data;
        public XmlDocument GetData(DateTime date)
        {
            Data = new XmlDocument();
            string dtString = date.ToString("d", new CultureInfo("de-DE"));
            Data.Load($"https://www.bnm.md/ro/official_exchange_rates?get_xml=1&date={dtString}");
            return Data;
        }
    }
}
