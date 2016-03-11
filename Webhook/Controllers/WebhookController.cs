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
        public void Post([FromBody]string xmlinfo)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlinfo);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/DocuSignEnvelopeInformation/EnvelopeStatus");
            string envelopeId = nodes[0].SelectSingleNode("EnvelopeID").InnerText;

            //System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId + "_" + Guid.NewGuid() + ".xml"), xmlinfo);
            var filepath = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(filepath + "_" + envelopeId + ".xml", xmlinfo);
        }
    }
}