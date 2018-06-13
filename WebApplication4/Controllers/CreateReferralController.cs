using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using CreateReferralTestHarness.Models;

namespace CreateReferralTestHarness.Controllers
{
    //public class CreateReferralController : ApiController
    //{
    //}
    public class CreateReferralController : Controller
    {
        CreateReferral _model;
        static string _accessToken;

        //string postURL = "https://nbty--dev5.cs69.my.salesforce.com/services/apexrest/referralservice";
        string postURL = "https://nbty--UAT.cs64.my.salesforce.com/services/apexrest/referralservice";

        public ActionResult Index()
        {
            return View();
        }

        private void getAccessToken()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using ( WebClient client = new WebClient() )
                {
                    string tokenURL = "https://nbty--UAT.cs64.my.salesforce.com//services/oauth2/token";

                    var data = "grant_type=refresh_token"
                         + "&client_id=" + HttpUtility.UrlEncode( "3MVG967gVD5fuTmL1omVvIWrWPtG_rpNRy9MO5Cq5LQc.LLg2fMDrW3QsNB5tGFHSsvLAj5dPkDeNt0yFyoI5" )
                         + "&client_secret=" + HttpUtility.UrlEncode( "5046757764784457611" )
                         + "&refresh_token=" + HttpUtility.UrlEncode( "5Aep8612Xuhpe0phpPd40Q87srqTgH2W1bpmH8fICFgKwkmywG01JDppdvK20OSeSdm5CJlIHbHx7o.3Hay3jay" );

                    client.Headers[ HttpRequestHeader.ContentType ] = "application/x-www-form-urlencoded";

                    // https://nbty--UAT.cs64.my.salesforce.com//services/oauth2/token
                    var jsonResponse = client.UploadString( tokenURL, data );

                    OAuth oAuth = JsonConvert.DeserializeObject<OAuth>( jsonResponse );

                    _accessToken = oAuth.access_token;
                }
            }
            catch ( Exception e )
            {
                //var db = Database.OpenNamedConnection( "OrderProcessingDb" );
                //var result = db.SafetechEvents.Insert( EventMessage: "SalesForce post call Error: " + e.Message );
                //return Content( e.Message, "application/json" );
            }
        }


        [HttpPost]
        public ActionResult Send( CreateReferral model )
        {
            try
            {
                _model = model;

                if ( _accessToken == null )
                {
                    getAccessToken();
                }

                if ( ModelState.IsValid )
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var request = JsonConvert.SerializeObject( model );

                    using ( WebClient client = new WebClient() )
                    {
                        client.Headers[ HttpRequestHeader.Accept ] = "application/json";
                        client.Headers[ HttpRequestHeader.ContentType ] = "application/json";
                        //                        client.Headers[ HttpRequestHeader.ContentType ] = "application/json; charset=utf-8";
                        client.Headers[ HttpRequestHeader.Authorization ] = "Bearer " + _accessToken;

                        //                        client.Encoding = System.Text.Encoding.UTF8;

                        var jsonResponse = client.UploadString( postURL, request );

                        return Content( jsonResponse, "application/json" );

                        //throw new NotImplementedException();
                    }
                }
            }
            catch ( System.Net.WebException e )
            {
                if ( ( (HttpWebResponse)e.Response ).StatusCode == HttpStatusCode.Unauthorized )
                {
                    // get token and retry
                    getAccessToken();

                    //sendJourney( _accessToken, _model );
                    Send( model );

                    return Content( e.Message, "application/json" );
                }
                return Content( e.Message, "application/json" );
            }

            return View( "Index", model );
        }

        private class ClientDetails
        {
            public string clientId { get; set; }
            public string clientSecret { get; set; }
        }

        private class OAuth
        {
            public string id { get; set; }
            public string issued_at { get; set; }
            public string scope { get; set; }
            public string instance_url { get; set; }
            public string signature { get; set; }
            public string access_token { get; set; }
        }
    }
}

