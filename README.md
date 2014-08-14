## csharp-bandwidth
[![Build status](https://ci.appveyor.com/api/projects/status/bhv8hs3fx9k6c33i)](https://ci.appveyor.com/project/avbel/csharp-bandwidth)

.Net library for Catapult API

## Usage

Using REST client
```
  using (var client = new Client(Config.UserId, Config.ApiToken, Config.Secret))
  {
      var applications = await client.Applications.GetAll(); //Get all applications of user

      //making call
      var callId = await client.Calls.Create(new Call
      {
          From = "+1-202-555-0149",
          To = "+1-202-555-0148"
      });

      //sending sms
      var smsId = await client.Messages.Send(new Message
      {
          From = "+1-202-555-0149",
          To = "+1-202-555-0148",
          Text = "Hello"
      });
  }
```
See [Bandwidth Catapult Api Docs](https://catapult.inetwork.com/docs/api-docs/) for more details

Parsing callback events

```
//in request handler
var event = Bandwidth.Net.Events.Event.ParseRequestBody(requestBody);

```