#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Instrumentation;
using DotNetNuke.Services.Authentication;
using DotNetNuke.Services.Authentication.OAuth;

#endregion

namespace DNNAuth.LinkedIn.Components
{
    public class LinkedInClient : OAuthClientBase
    {
        public LinkedInClient(int portalId, AuthMode mode) : base(portalId, mode, "LinkedIn")
        {
			TokenEndpoint = new Uri("https://www.linkedin.com/uas/oauth2/accessToken");
            TokenMethod = HttpMethod.POST;
            AuthorizationEndpoint = new Uri("https://www.linkedin.com/uas/oauth2/authorization");
			MeGraphEndpoint = new Uri("https://api.linkedin.com/v1/people/~?format=json");

			Scope = HttpUtility.UrlEncode("r_basicprofile r_emailaddress");
			AuthTokenName = "LinkedInUserToken";
            OAuthVersion = "2.0";
            LoadTokenCookie(String.Empty);
        }

	    public override TUserData GetCurrentUser<TUserData>()
	    {
            LoadTokenCookie(String.Empty);
            if (!IsCurrentUserAuthorized())
            {
                return null;
            }
			string responseText = ExecuteWebRequest(MeGraphEndpoint, "Bearer " + AuthToken);
            var user = Json.Deserialize<TUserData>(responseText);
            return user;
	    }

		private string ExecuteWebRequest(Uri uri, string authHeader)
		{
			var request = WebRequest.CreateDefault(uri);
			if (!String.IsNullOrEmpty(authHeader))
			{
				request.Headers.Add(HttpRequestHeader.Authorization, authHeader);
			}

			try
			{
				using (WebResponse response = request.GetResponse())
				{
					return ReadResponse(response);
				}
			}
			catch (WebException ex)
			{
				string responseText = ReadResponse(ex.Response);
				if (request != null)
				{
					LoggerSource.Instance.GetLogger(typeof(LinkedInClient)).ErrorFormat("WebResponse exception: {0}", responseText);
				}
			}
			return null;
		}

	    private static string ReadResponse(WebResponse response)
	    {
		    using (Stream responseStream = response.GetResponseStream())
		    {
			    if (responseStream != null)
			    {
				    using (var responseReader = new StreamReader(responseStream))
				    {
					    return responseReader.ReadToEnd();
				    }
			    }
		    }
		    return null;
	    }

	    protected override TimeSpan GetExpiry(string responseText)
        {
            var tokenDictionary = Json.Deserialize<Dictionary<string, object>>(responseText);
            return new TimeSpan(0, 0, Convert.ToInt32(tokenDictionary["expires_in"]));
        }

        protected override string GetToken(string responseText)
        {
			var tokenDictionary = Json.Deserialize<Dictionary<string, object>>(responseText);
            return Convert.ToString(tokenDictionary["access_token"]);
        }
    }
}