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
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/DocuSignEnvelopeInformation/EnvelopeStatus");
            string envelopeId = nodes[0].SelectSingleNode("EnvelopeID").InnerText;

            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId + "_" + Guid.NewGuid() + ".xml"), doc.OuterXml);
        }
    }
}