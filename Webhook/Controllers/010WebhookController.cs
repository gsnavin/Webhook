using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Webhook.Controllers
{
    public class _010WebhookController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Home";

            return View();
        }

        public ActionResult SendSignRequest()
        {
            //string ds_signer1_name = get_fake_name();
            //string ds_signer1_email = make_temp_email();
            //string ds_cc1_name = get_fake_name();
            //string ds_cc1_email = make_temp_email();

            ////string webhook_url = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.Port != 80 && Request.Url.Port != 443 ? (":" + Request.Url.Port) : "") + "?op=webhook";
            //string webhook_url = Request.Url.GetLeftPart(UriPartial.Authority) + "/status";

            return View();
        }

        public ActionResult Webhook() {
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
	
            //String strmContents;
            
            //Int32 strLen; //, strRead;
            
            // Create a Stream object.
            System.IO.Stream str = Request.InputStream;
            
            // Find number of bytes in stream.
            //strLen = Convert.ToInt32(str.Length);
            
            // Create a byte array.
            byte[] strArr = new byte[str.Length];
            
            // Read stream into byte array.
            //strRead = str.Read(strArr, 0, strLen);

            XmlDocument doc = new XmlDocument();
            doc.Load(str);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/DocuSignEnvelopeInformation/EnvelopeStatus");
            string envelopeId = nodes[0].SelectSingleNode("EnvelopeID").InnerText;

            System.IO.File.WriteAllBytes(Server.MapPath("~/Documents/"+envelopeId+".xml"), strArr);

            //// Convert byte array to a text string.
            //strmContents = "";
            //for (counter = 0; counter < strLen; counter++)
            //{
            //    strmContents = strmContents + strArr[counter].ToString();            
            //}




            //data = file_get_contents('php://input');
            //xml = simplexml_load_string ($data, "SimpleXMLElement", LIBXML_PARSEHUGE);
            //envelope_id = (string)$xml->EnvelopeStatus->EnvelopeID;
            //time_generated = (string)$xml->EnvelopeStatus->TimeGenerated;
	
            //// Store the file. Create directories as needed
            //// Some systems might still not like files or directories to start with numbers.
            //// So we prefix the envelope ids with E and the timestamps with T
            //$files_dir = getcwd() . '/' . $this->xml_file_dir;
            //if(! is_dir($files_dir)) {mkdir ($files_dir, 0755);}
            //$envelope_dir = $files_dir . "E" . $envelope_id;
            //if(! is_dir($envelope_dir)) {mkdir ($envelope_dir, 0755);}

            //$filename = $envelope_dir . "/T" . 
            //    str_replace (':' , '_' , $time_generated) . ".xml"; // substitute _ for : for windows-land
            //$ok = file_put_contents ($filename, $data);
	
            //if ($ok === false) {
            //    // Couldn't write the file! Alert the humans!
            //    error_log ("!!!!!! PROBLEM DocuSign Webhook: Couldn't store $filename !");
            //    exit (1);
            //}
            //// log the event
            //error_log ("DocuSign Webhook: created $filename");
		
            //if ((string)$xml->EnvelopeStatus->Status === "Completed") {
            //    // Loop through the DocumentPDFs element, storing each document.
            //    foreach ($xml->DocumentPDFs->DocumentPDF as $pdf) {
            //        $filename = $this->doc_prefix . (string)$pdf->DocumentID . '.pdf';
            //        $full_filename = $envelope_dir . "/" . $filename;
            //        file_put_contents($full_filename, base64_decode ( (string)$pdf->PDFBytes ));
            //    }
            //}

            return Content("");
	    }

        public ActionResult SendSignatureRequest()
        {
            string ds_signer1_name = get_fake_name();
            string ds_signer1_email = make_temp_email();
            string ds_cc1_name = get_fake_name();
            string ds_cc1_email = make_temp_email();
            string webhook_url = Request.Url.GetLeftPart(UriPartial.Authority) + "/webhook";

            string accountId = Login();

            if (accountId == null) {
                return Content("[\"ok\" => false, \"html\" => \"<h3>Problem</h3><p>Couldn't login to DocuSign: \"]");
            }


		    // *** This snippet is from file 010.webhook_lib.php ***
		    // The envelope request includes a signer-recipient and their tabs object,
		    // and an eventNotification object which sets the parameters for
		    // webhook notifications to us from the DocuSign platform
		    List<EnvelopeEvent> envelope_events = new List<EnvelopeEvent>();

		    EnvelopeEvent envelope_event1 = new EnvelopeEvent();
		    envelope_event1.EnvelopeEventStatusCode = "sent";
		    envelope_events.Add(envelope_event1);
		    EnvelopeEvent envelope_event2 = new EnvelopeEvent();
		    envelope_event2.EnvelopeEventStatusCode = "delivered";
		    envelope_events.Add(envelope_event2);
		    EnvelopeEvent envelope_event3 = new EnvelopeEvent();
		    envelope_event3.EnvelopeEventStatusCode = "completed";
		    envelope_events.Add(envelope_event3);
		    EnvelopeEvent envelope_event4 = new EnvelopeEvent();
		    envelope_event4.EnvelopeEventStatusCode = "declined";
		    envelope_events.Add(envelope_event4);
		    EnvelopeEvent envelope_event5 = new EnvelopeEvent();
		    envelope_event5.EnvelopeEventStatusCode = "voided";
		    envelope_events.Add(envelope_event5);

		    List<RecipientEvent> recipient_events = new List<RecipientEvent>();
		    RecipientEvent recipient_event1 = new RecipientEvent();
		    recipient_event1.RecipientEventStatusCode = "Sent";
		    recipient_events.Add(recipient_event1);
		    RecipientEvent recipient_event2 = new RecipientEvent();
		    recipient_event2.RecipientEventStatusCode = "Delivered";
		    recipient_events.Add(recipient_event2);
		    RecipientEvent recipient_event3 = new RecipientEvent();
		    recipient_event3.RecipientEventStatusCode = "Completed";
		    recipient_events.Add(recipient_event3);
		    RecipientEvent recipient_event4 = new RecipientEvent();
		    recipient_event4.RecipientEventStatusCode = "Declined";
		    recipient_events.Add(recipient_event4);
		    RecipientEvent recipient_event5 = new RecipientEvent();
		    recipient_event5.RecipientEventStatusCode = "AuthenticationFailed";
		    recipient_events.Add(recipient_event5);
		    RecipientEvent recipient_event6 = new RecipientEvent();
		    recipient_event6.RecipientEventStatusCode = "AutoResponded";
		    recipient_events.Add(recipient_event6);

		    EventNotification event_notification = new EventNotification();
		    event_notification.Url = webhook_url;
		    event_notification.LoggingEnabled = "true";
		    event_notification.RequireAcknowledgment ="true";
		    event_notification.UseSoapInterface= "false";
		    event_notification.IncludeCertificateWithSoap= "false";
		    event_notification.SignMessageWithX509Cert= "false";
		    event_notification.IncludeDocuments= "true";
		    event_notification.IncludeEnvelopeVoidReason= "true";
		    event_notification.IncludeTimeZone= "true";
		    event_notification.IncludeSenderAccountAsCustomField= "true";
		    event_notification.IncludeDocumentFields= "true";
		    event_notification.IncludeCertificateOfCompletion= "true";
		    event_notification.EnvelopeEvents = envelope_events;
		    event_notification.RecipientEvents = recipient_events;

		    Document document = new Document();
		    document.DocumentId= "1";
		    document.Name = "NDA.pdf";

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Documents\NDA.pdf");
            Byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Documents/NDA.pdf").Replace("_", ""));
		    document.DocumentBase64 = Convert.ToBase64String(bytes);

		    SignHere sign_here_tab = new SignHere();
		    sign_here_tab.AnchorString= "signer1sig";
		    sign_here_tab.AnchorXOffset= "0";
		    sign_here_tab.AnchorYOffset= "0";
		    sign_here_tab.AnchorUnits= "mms";
		    sign_here_tab.RecipientId= "1";
		    sign_here_tab.Name= "Please sign here";
		    sign_here_tab.Optional= "false";
		    sign_here_tab.ScaleValue = 1;
		    sign_here_tab.TabLabel= "signer1sig";

		    FullName full_name_tab = new FullName();
		    full_name_tab.AnchorString= "signer1name";
		    full_name_tab.AnchorYOffset= "-6";
		    full_name_tab.FontSize= "Size12";
		    full_name_tab.RecipientId= "1";
		    full_name_tab.TabLabel= "Full Name";
		    full_name_tab.Name= "Full Name";

		    DocuSign.eSign.Model.Text text_tab = new DocuSign.eSign.Model.Text();
		    text_tab.AnchorString= "signer1company";
		    text_tab.AnchorYOffset= "-8";
		    text_tab.FontSize= "Size12";
		    text_tab.RecipientId= "1";
		    text_tab.TabLabel= "Company";
		    text_tab.Name= "Company";
		    text_tab.Required= "false";

		    DateSigned date_signed_tab = new DateSigned();
		    date_signed_tab.AnchorString= "signer1date";
		    date_signed_tab.AnchorYOffset= "-6";
		    date_signed_tab.FontSize= "Size12";
		    date_signed_tab.RecipientId= "1";
		    date_signed_tab.Name= "Date Signed";
		    date_signed_tab.TabLabel= "Company";

		    DocuSign.eSign.Model.Tabs tabs = new DocuSign.eSign.Model.Tabs();
		    tabs.SignHereTabs = new List<SignHere>();
            tabs.SignHereTabs.Add(sign_here_tab);
		    tabs.FullNameTabs = new List<FullName>();
            tabs.FullNameTabs.Add(full_name_tab);
		    tabs.TextTabs = new List<Text>();
            tabs.TextTabs.Add(text_tab);
		    tabs.DateSignedTabs = new List<DateSigned>();
            tabs.DateSignedTabs.Add(date_signed_tab);

		    Signer signer = new Signer();
            signer.Email = ds_signer1_email;
		    signer.Name = ds_signer1_name;
		    signer.RecipientId= "1";
		    signer.RoutingOrder= "1";
		    signer.Tabs = tabs;

		    CarbonCopy carbon_copy = new CarbonCopy();
            carbon_copy.Email = ds_cc1_email;
		    carbon_copy.Name = ds_cc1_name;
		    carbon_copy.RecipientId= "2";
		    carbon_copy.RoutingOrder= "2";

		    Recipients recipients = new Recipients();
		    recipients.Signers = new List<Signer>();
            recipients.Signers.Add(signer);
		    recipients.CarbonCopies = new List<CarbonCopy>();
            recipients.CarbonCopies.Add(carbon_copy);

		    EnvelopeDefinition envelope_definition = new EnvelopeDefinition();
		    envelope_definition.EmailSubject= "Please sign the " + "NDA.pdf" + " document";
		    envelope_definition.Documents = new List<Document>();
            envelope_definition.Documents.Add(document);
		    envelope_definition.Recipients = recipients;
		    envelope_definition.EventNotification = event_notification;
		    envelope_definition.Status= "sent";

            string username = Environment.GetEnvironmentVariable("docusignApiUsername") ?? Properties.Settings.Default.docusignApiUsername;
            string password = Environment.GetEnvironmentVariable("docusignApiPassword") ?? Properties.Settings.Default.docusignApiPassword;
            string integratorKey = Environment.GetEnvironmentVariable("docusignApiIntegratorKey") ?? Properties.Settings.Default.docusignApiIntegratorKey;

            string authHeader = "{\"Username\":\"" + username + "\", \"Password\":\"" + password + "\", \"IntegratorKey\":\"" + integratorKey + "\"}";

            Configuration configuration = new Configuration(new ApiClient("https://demo.docusign.net/restapi"));
            configuration.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

		    EnvelopesApi envelopesApi = new EnvelopesApi(configuration);

		    EnvelopeSummary envelope_summary = envelopesApi.CreateEnvelope(accountId, envelope_definition, null);
		    if ( envelope_summary == null || envelope_summary.EnvelopeId == null ) {
			    return Content("[\"ok\" => false, html => \"<h3>Problem</h3>\" \"<p>Error calling DocuSign</p>\"]");
		    }

		    string envelope_id = envelope_summary.EnvelopeId;

            // Create instructions for reading the email
		    string html = "<h2>Signature request sent!</h2>" +
			    "<p>Envelope ID: " + envelope_id + "</p>" +
			    "<h2>Next steps</h2>" +
			    "<h3>1. Open the Webhook Event Viewer</h3>" +
			    "<p><a href='" + Request.Url.GetLeftPart(UriPartial.Authority) + "?op=status&envelope_id=" + envelope_id + "'" +
				    "  class='btn btn-primary' role='button' target='_blank' style='margin-right:1.5em;'>" +
				    "View Events</a> (A new tab/window will be used.)</p>" +
			    "<h3>2. Respond to the Signature Request</h3>";

		    string email_access = get_temp_email_access(ds_signer1_email);
		    if (email_access != null) {
			    // A temp account was used for the email
			    html += "<p>Respond to the request via your mobile phone by using the QR code: </p>" +
				    "<p>" + get_temp_email_access_qrcode(email_access) + "</p>" +
				    "<p> or via <a target='_blank' href='" + email_access + "'>your web browser.</a></p>";
		    } else {
			    // A regular email account was used
			    html += "<p>Respond to the request via your mobile phone or other mail tool.</p>" +
				    "<p>The email was sent to " + ds_signer1_name + " &lt;" + ds_signer1_email + "&gt;</p>";
		    }

		    //return Content("['ok'  => true,'envelope_id' => "+envelope_id+",'html' => "+ html+",'js' => [['disable_button' => 'sendbtn']]]");  // js is an array of items
            return Content(html);
        }

        private string Login()
        {
            string username = Environment.GetEnvironmentVariable("docusignApiUsername") ?? Properties.Settings.Default.docusignApiUsername;
            string password = Environment.GetEnvironmentVariable("docusignApiPassword") ?? Properties.Settings.Default.docusignApiPassword;
            string integratorKey = Environment.GetEnvironmentVariable("docusignApiIntegratorKey") ?? Properties.Settings.Default.docusignApiIntegratorKey;

            string authHeader = "{\"Username\":\"" + username + "\", \"Password\":\"" + password + "\", \"IntegratorKey\":\"" + integratorKey + "\"}";

            Configuration configuration = new Configuration(new ApiClient("https://demo.docusign.net/restapi"));
            configuration.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

            // the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object
            AuthenticationApi authApi = new AuthenticationApi(configuration);
            LoginInformation loginInfo = authApi.Login();

            // find the default account for this user
            foreach (LoginAccount loginAccount in loginInfo.LoginAccounts)
            {
                if (loginAccount.IsDefault == "true")
                {
                    return loginAccount.AccountId;
                }
            }

            return null;
        }

        private string get_temp_email_access(string email) {
		    // just create something unique to use with maildrop.cc
		    // Read the email at http://maildrop.cc/inbox/<mailbox_name>
		    string url = "https://mailinator.com/inbox.jsp?to=";
		    string[] parts = email.Split('@');
		    if (parts[1] != "mailinator.com") {
			    return null;
		    }
		    return url + parts[0];
	    }

        private string get_temp_email_access_qrcode(string address) {
		    // $url = "http://open.visualead.com/?size=130&type=png&data=";
		    string url = "https://chart.googleapis.com/chart?cht=qr&chs=150x150&chl=";
		    url += Url.Encode(address);
		    int size = 150;
		    string html = "<img height='"+size+"' width='"+size+"' src='"+url+"' alt='QR Code' style='margin:10px 0 10px;' />";
		    return html;
	    }

        private string get_fake_name() {
		    string[] first_names = new string[] {"Verna", "Walter", "Blanche", "Gilbert", "Cody", "Kathy",
		    "Judith", "Victoria", "Jason", "Meghan", "Flora", "Joseph", "Rafael",
		    "Tamara", "Eddie", "Logan", "Otto", "Jamie", "Mark", "Brian", "Dolores",
		    "Fred", "Oscar", "Jeremy", "Margart", "Jennie", "Raymond", "Pamela",
		    "David", "Colleen", "Marjorie", "Darlene", "Ronald", "Glenda", "Morris",
		    "Myrtis", "Amanda", "Gregory", "Ariana", "Lucinda", "Stella", "James",
		    "Nathaniel", "Maria", "Cynthia", "Amy", "Sylvia", "Dorothy", "Kenneth",
		    "Jackie"};

		    string[] last_names = new string[] {"Francisco", "Deal", "Hyde", "Benson", "Williamson", 
		    "Bingham", "Alderman", "Wyman", "McElroy", "Vanmeter", "Wright", "Whitaker", 
		    "Kerr", "Shaver", "Carmona", "Gremillion", "O'Neill", "Markert", "Bell", 
		    "King", "Cooper", "Allard", "Vigil", "Thomas", "Luna", "Williams", 
		    "Fleming", "Byrd", "Chaisson", "McLeod", "Singleton", "Alexander", 
		    "Harrington", "McClain", "Keels", "Jackson", "Milne", "Diaz", "Mayfield", 
		    "Burnham", "Gardner", "Crawford", "Delgado", "Pape", "Bunyard", "Swain", 
		    "Conaway", "Hetrick", "Lynn", "Petersen"};

            Random random = new Random();
		    string first = first_names[random.Next(0, first_names.Length - 1)];
		    string last = last_names[random.Next(0, first_names.Length - 1)];
		    return first + " " + last;
	    }

        private string make_temp_email() {
	 	    // just create something unique to use with maildrop.cc
		    // Read the email at http://maildrop.cc/inbox/<mailbox_name>	
		    string ip = "100";
		    if (Request.ServerVariables["REMOTE_ADDR"] != null) {
			    ip = Request.ServerVariables["REMOTE_ADDR"];
		    }

            Random random = new Random();
		    string email = random.Next(0, 25) + DateTime.Now.ToString() + ip;
            email = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(email));

            return email.Substring(email.Length - 20, 20) + "@mailinator.com";
	    }
    }
}