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
            XmlNodeList xnList = doc.SelectNodes("/DocuSignEnvelopeInformation/EnvelopeStatus");
            if (xnList.Count > 0)
            {
                foreach (XmlNode xn in xnList)
                {
                    string envelopeId = xn["EnvelopeId"].InnerText;
                    string status = xn["Status"].InnerText;
                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId + "_" + status + ".xml"), doc.OuterXml);
                }
            }
            else
            {
                System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + Guid.NewGuid() + ".xml"), doc.OuterXml);
            }
        }
    }
}