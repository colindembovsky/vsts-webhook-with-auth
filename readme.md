# Sample VSTS WebHook WebAPI with Basic Authentication

This sample repo shows a WebAPI WebHook project that responds to VSTS web hooks. This sample includes a [BasicAuthHandler](vsts-webhook-with-auth/Handlers/BasicAuthHandler.cs) that checks for a basic auth header before allowing the webhook.

## Build
- Compile the solution in VS
- Set the username, password and code you want in the `web.config`
- Override a `VstsWebHookHandlerBase` method in the [VSTSHookHandler.cs](vsts-webhook-with-auth/Handlers/VSTSHookHandler.cs) class to respond to those events (only `workitem.created` is currently stubbed)
- Run by using F5

## Test
- Open [Postman](https://www.getpostman.com/)
- To import the IISExpress self-signed cert, open the running URL in IE and proceed to the site. Click on the Lock icon and import the certificate to `Trusted Root Certification Authorities` and restart Chrome.
- Set url to `https://localhost:44388/api/webhooks/incoming/vsts?code=<code>` where `<code>` is the 
- Set `METHOD` to `POST`
- Add a basic auth header using the username/password you specified in the `web.config`
- Go to the [Team Services service hooks events page](https://www.visualstudio.com/en-us/docs/integrate/get-started/service-hooks/events) and grab a sample payload for an event that you have implemented in [VSTSHookHandler.cs](vsts-webhook-with-auth/Handlers/VSTSHookHandler.cs)
- Copy this into the `Body` section of the `POST`, set the radio to `raw` and set the type to `JSON (application/json)`
- Click Send and you should get a 200!