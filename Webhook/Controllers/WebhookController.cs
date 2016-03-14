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
        // POST api/<controller>
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
                System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId.InnerText + "_" + status.InnerText + "_" + Guid.NewGuid() + ".xml"), doc.OuterXml);
            }
        }
    }
}