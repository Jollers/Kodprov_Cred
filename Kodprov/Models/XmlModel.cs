using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kodprov.Models
{
    public class XmlModel
    {
        public string INVID { get; set; }
        public string BILLERID { get; set; }
        public string BILLERNAME1 { get; set; }
        public string INVOICEEID { get; set; }
        public string INVOICEENAME1 { get; set; }
        public string EBID { get; set; }
        public string TIMESTAMP { get; set; }
        public string ES3USER { get; set; }
        public string ERROR { get; set; }
        public string ERRORCODE { get; set; }

        public XmlModel(string iNVID, string bILLERID, string bILLERNAME1, string iNVOICEEID, string iNVOICEENAME1, string eBID, string tIMESTAMP, string eS3USER, string eRROR, string eRRORCODE)
        {
            INVID = iNVID;
            BILLERID = bILLERID;
            BILLERNAME1 = bILLERNAME1;
            INVOICEEID = iNVOICEEID;
            INVOICEENAME1 = iNVOICEENAME1;
            EBID = eBID;
            TIMESTAMP = tIMESTAMP;
            ES3USER = eS3USER;
            ERROR = eRROR;
            ERRORCODE = eRRORCODE;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name, p => p.GetGetMethod().Invoke(this, null)).GetEnumerator();
        }
    }
}
