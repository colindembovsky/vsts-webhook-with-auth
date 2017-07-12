using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace vsts_webhook_with_auth.Handlers
{
	public class BasicAuthenticationHandler : DelegatingHandler
	{
		private const string WWWAuthenticateHeader = "WWW-Authenticate";

		public string ValidUsername { get; protected set; }

		public string ValidPassword { get; protected set; }

		public BasicAuthenticationHandler()
		{
			ValidUsername = WebConfigurationManager.AppSettings["WebHookUsername"];
			ValidPassword = WebConfigurationManager.AppSettings["WebHookPassword"];
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var credentials = ParseAuthorizationHeader(request);

			if (credentials != null && CredentialsAreValid(credentials))
			{
				var identity = new BasicAuthenticationIdentity(credentials.Name, credentials.Password);
				Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
				return base.SendAsync(request, cancellationToken);
			}
			else
			{
				var response = request.CreateResponse(HttpStatusCode.Unauthorized, "Access denied");
				AddChallengeHeader(request, response);
				return Task.FromResult(response);
			}
		}

		protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage request)
		{
			string authHeader = null;
			var auth = request.Headers.Authorization;
			if (auth != null && auth.Scheme == "Basic")
			{
				authHeader = auth.Parameter;
			}

			if (string.IsNullOrEmpty(authHeader))
			{
				return null;
			}

			authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

			var tokens = authHeader.Split(':');
			if (tokens.Length < 2)
			{
				return null;
			}

			return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
		}

		protected void AddChallengeHeader(HttpRequestMessage request, HttpResponseMessage response)
		{
			var host = request.RequestUri.DnsSafeHost;
			response.Headers.Add(WWWAuthenticateHeader, $"Basic realm=\"{host}\"");
		}

		protected bool CredentialsAreValid(BasicAuthenticationIdentity creds)
		{
			return creds.Name == ValidUsername && creds.Password == ValidPassword;
		}

		protected class BasicAuthenticationIdentity : GenericIdentity
		{
			public string Password { get; set; }

			public BasicAuthenticationIdentity(string name, string password) 
				: base(name, "Basic")
			{
				this.Password = password;
			}
		}
	}
}