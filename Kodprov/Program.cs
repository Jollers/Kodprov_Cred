using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Kodprov.Models;
using System.Xml.Serialization;

namespace Kodprov
{
    class Program
    {
        static void Main(string[] args)
        {
            string SourcePath = @"C:\Users\Joakim\Desktop\test\Indata.txt";
            string DestinationPath = @"C:\Users\Joakim\Desktop\test\XmlResult.xml";

            /*
             Har gjort 2 metoder. ConvertToXML och ConvertToXML2.
            ConvertToXML2 är mitt första försök. När jag blev klar så insåg jag att det va inget bra sätt att göra det på så jag började om.
            Tycker ConvertToXML är mycket bättre och snyggare metod.
            Jag låter den första metoden ligga kvar för att visa mitt tankesätt.
             */
            ConvertToXML(SourcePath, DestinationPath);
        }


        public static void ConvertToXML(string SourcePath, string DestinationPath)
        {
            var report = new Report();
            List<XmlModel> models = new List<XmlModel>();

            string INVID = "";
            string BILLERID = "";
            string BILLERNAME1 = "";
            string INVOICEEID = "";
            string INVOICEENAME1 = "";
            string EBID = "";
            string TIMESTAMP = "";
            string ES3USER = "";
            string ERROR = "";
            string ERRORCODE = "";

            foreach (string Ln in File.ReadAllLines(SourcePath))
            {
                if (Ln.StartsWith("-"))
                {
                    string trimmedString = String.Concat(Ln.Where(c => !Char.IsWhiteSpace(c)));
                    string name = trimmedString.Split(':')[0].Trim(new Char[] { '-' });
                    string formatedName = Regex.Replace(name, "[()]", string.Empty);
                    string value = trimmedString.Split(':')[1];

                    switch (formatedName)
                    {
                        default:
                            break;
                        case "INVID":
                            INVID = value;
                            break;
                        case "BILLERID":
                            BILLERID = value;
                            break;
                        case "BILLERNAME1":
                            BILLERNAME1 = value;
                            break;
                        case "INVOICEEID":
                            INVOICEEID = value;
                            break;
                        case "INVOICEENAME1":
                            INVOICEENAME1 = value;
                            break;
                        case "EBID":
                            EBID = value;
                            break;
                        case "TIMESTAMP":
                            TIMESTAMP = value;
                            break;
                        case "ES3USER":
                            ES3USER = value;
                            break;
                        case "ERROR":
                            ERROR = value;
                            break;
                        case "ERRORCODEs":
                            ERRORCODE = value;
                            break;
                    }
                    if (Ln.EndsWith(","))
                    {
                        models.Add(new XmlModel(INVID, BILLERID, BILLERNAME1, INVOICEEID, INVOICEENAME1, EBID, TIMESTAMP, ES3USER, ERROR, ERRORCODE ));
                    }
                }
            }

            List<ReportInvoice> invoices = new List<ReportInvoice>();
            foreach (var x in models)
            {
                var reportInvoice = new ReportInvoice();
                reportInvoice.INVID = x.INVID;
                reportInvoice.BILLERID = x.BILLERID;
                reportInvoice.BILLERNAME1 = x.BILLERNAME1;
                reportInvoice.INVOICEEID = x.INVOICEEID;
                reportInvoice.INVOICEENAME1 = x.INVOICEENAME1;
                reportInvoice.EBID = x.EBID;
                reportInvoice.TIMESTAMP = x.TIMESTAMP;
                reportInvoice.ES3USER = x.ES3USER;
                reportInvoice.ERROR = x.ERROR;
                reportInvoice.ERRORCODE = x.ERRORCODE;

                invoices.Add(reportInvoice);
            }

            report.Invoices = invoices.ToArray();
            var serializer = new XmlSerializer(typeof(Report));
            using (var stream = new StreamWriter(DestinationPath))
            {
                serializer.Serialize(stream, report);
            }
        }



        //Detta är mitt första försök.
        public static void ConvertToXML2(string SourcePath, string DestinationPath)
        {
            using (XmlWriter writer = XmlWriter.Create(DestinationPath))
            {
                writer.WriteStartElement("Report");
                writer.WriteStartElement("Invoices");
                foreach (string Ln in File.ReadAllLines(SourcePath))
                {
                    if (Ln.StartsWith("*"))
                    {
                        writer.WriteStartElement("Invoice");
                    }
                    else if (Ln.StartsWith("-"))
                    {
                        string trimmedString = String.Concat(Ln.Where(c => !Char.IsWhiteSpace(c)));
                        string name = trimmedString.Split(':')[0].Trim(new Char[] { '-', });
                        string formatedName = Regex.Replace(name, "[()]", string.Empty);
                        string value = trimmedString.Split(':')[1];
                        writer.WriteElementString(formatedName, value);
                    }

                    if (Ln.EndsWith(","))
                    {
                        writer.WriteEndElement();
                    }

                }
                writer.WriteEndElement();
                writer.Flush();
            };
        }
    }
}
