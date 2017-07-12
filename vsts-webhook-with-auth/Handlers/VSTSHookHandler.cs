using Microsoft.AspNet.WebHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.WebHooks.Payloads;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web.Http;

namespace vsts_webhook_with_auth.Handlers
{
	public class VSTSHookHandler : VstsWebHookHandlerBase
	{
		public override Task ExecuteAsync(WebHookHandlerContext context, WorkItemCreatedPayload payload)
		{
			Trace.WriteLine($"Event WorkItemCreated triggered for work item {payload.Resource.Id}");
			return base.ExecuteAsync(context, payload);
		}
	}
}