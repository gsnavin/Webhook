using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace Webhook.Controllers
{
    public class WebhookController : ApiController
    {
        // Process the incoming webhook data. See the DocuSign Connect guide
        // for more information
        //
        // Strategy: examine the data to pull out the envelope_id and time_generated fields.
        // Then store the entire xml on our local file system using those fields.
        //
        // If the envelope status=="Completed" then store the files as doc1.pdf, doc2.pdf, etc
        //
        // This function could also enter the data into a dbms, add it to a queue, etc.
        // Note that the total processing time of this function must be less than
        // 100 seconds to ensure that DocuSign's request to your app doesn't time out.
        // Tip: aim for no more than a couple of seconds! Use a separate queuing service
        // if need be.
        public void Post(HttpRequestMessage request)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(request.Content.ReadAsStreamAsync().Result);

            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("a", "http://www.docusign.net/API/3.0");

            XmlNode envelopeStatus = doc.SelectSingleNode("//a:EnvelopeStatus", mgr);
            XmlNode envelopeId = envelopeStatus.SelectSingleNode("//a:EnvelopeID", mgr);
            XmlNode status = envelopeStatus.SelectSingleNode("//a:Status", mgr);
            if(envelopeId != null)
            {
                System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId.InnerText + "/" +
                    envelopeId.InnerText + "_" + status.InnerText + "_" + Guid.NewGuid() + ".xml"), doc.OuterXml);
            }

            if (status.InnerText == "Completed") {
                // Loop through the DocumentPDFs element, storing each document.

                XmlNodeList pdfs = doc.SelectNodes("//a:DocumentPDFs", mgr);
                foreach (XmlNode pdf in pdfs) {
                    string documentName =pdf.SelectSingleNode("//a:Name").InnerText;
                    string documentId =pdf.SelectSingleNode("//a:DocumentID").InnerText;
                    string byteStr =pdf.SelectSingleNode("//a:PDFBytes").InnerText;

                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId + "/PDFs/" + documentName), byteStr);
                }
            }
        }
    }
}