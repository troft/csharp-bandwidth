# Bandwidth C# Catapult Client 


## Overview
***

This library provides access to all of the functions of the [Catpult Voice and Messaging API](https://catapult.inetwork.com ).  For more information on the underlying REST APIs, you can consult the [Documentation](https://catapult.inetwork.com/docs).

This library is compiled as a Portable library.  If functions are not supported in Portable environments (such as environment variables), an alternative method is provided. 

### Instantiating the Client
The client can be instantiated using the singleton methods on the Client class.

    var client = Client.GetInstance("userId", "token", "secret");
    
You can also instantiate via environment variables:  

	//Environment Variables
	BANDWIDTH_USER_ID
	BANDWIDTH_API_TOKEN
	BANDWIDTH_API_SECRET
	BANDWIDTH_API_ENDPOINT
	BANDWIDTH_API_VERSION
	
	//If these env variables are set, then instantiate this way:
	
	var client = Client.GetInstance();
	

### Conventions for the SDK

All REST objects will generally have the following methods: 

Get

List

Create

Delete (if appropriate)

These methods are static on each of the entities.  For example:

	// using a client you instantiate directly
	var call = Call.Create(client, "to", "from").Result;
	
	// Using the default client
	var call = Call.Create("to", "from").Result;
	
The above code will create the new call and return you the call object fully populated with the call Id and other call data.  Each returned object will also have an internal Client property set, making follow-on operations such as Delete much simpler.

All methods use the async Task framework to return results.
  
  


## API Functions
***

## Account
### Get account information
[Documentation](https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account)

	var account = Account.Get().Result;
	
### Get account transactions
[Documentation](https://catapult.inetwork.com/docs/api-docs/account/#GET-/v1/users/{userId}/account/transactions)

	var query = new Dictionary<string, object>{{"maxItems", 5}, {"fromDate", "2014-11-05"});
	var transactions = Account.GetTransactions(query).Result;
	
## Applications
### Get list of applications
[Documentation](https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications)

	var query = new Dictionary<string, object>{{"page", 1}, {"size", 20}});
	var applications = Application.List(query).Result

### Create an application
[Documentation](https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications)

	var appData = new Dictionary<string, object>
	{
		{"name", "New Application"},
		{"incomingCallUrl", "https://yourcallbackurl.com/call"},
		{"incomingSmsUrl", "https://yourcallbackurl.com/message"}, 
		{"callbackHttpMethod", "post"},
		{"autoAnswer", true}
	}
	var application = Application.Create(appData).Result;
	
### Get application
[Documentation](https://catapult.inetwork.com/docs/api-docs/applications/#GET-/v1/users/{userId}/applications/{applicationId})

	var application = Application.Get("applicationId").Result;
	
### Update application
[Documentation](https://catapult.inetwork.com/docs/api-docs/applications/#POST-/v1/users/{userId}/applications/{applicationId})

	var data = new Dictionary<string, object>{{"name", "Updated Name"}};
	var application = Application.Get("applicationId").Result;
	application.Update(data).Wait()
	
### Delete application
[Documentation](https://catapult.inetwork.com/docs/api-docs/applications/#DELETE-/v1/users/{userId}/applications/{applicationId})

	var application = Application.Get("applicationId").Result;
	application.Delete().Wait()
	
## Available Numbers
[Documentation](https://catapult.inetwork.com/docs/api-docs/available-numbers/)

### Search for local numbers
[Documentation](https://catapult.inetwork.com/docs/api-docs/available-numbers/#local-get)

	var query = new Dictionary<string, object>
	{
		{"city", "Raleigh"},
		{"quantity", 3}
	};
	
	var availableNumbers = AvailableNumber.searchLocal(query).Result;

### Search for toll-free numbers
[Documentation](https://catapult.inetwork.com/docs/api-docs/available-numbers/#GET-/v1/availableNumbers/tollFree)

	var query = new Dictionary<string, object>
	{
		{"quantity", 5}
	};
	
	var availableNumbers = AvailableNumber.searchTollFree(query).Result;


## Bridges
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/)

### List bridges	
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/#GET-/v1/users/{userId}/bridges)

	var bridges = Bridge.List().Result;
	
### Create a bridge
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges)

	var data = new Dictionary<string, object>
	{
		{"callIds", ["callId1", "callId2"]},
		{"bridgeAudio", "true"}
	};
	
	var bridge = Bridge.Create(data).Result;
	
	//OR
	
	var bridge = Bridge.Create("callId1", "callId2", true).Result;
	
### Get bridge
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/#GET-/v1/users/{userId}/bridges/{bridgeId})

	var bridge = Bridge.Get("bridgeId").Result;
	
### Update bridge
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges/{bridgeId})

	// put bridge on hold
	var data = new Dictionary<string, object>
	{
		{"bridgeAudio", "false"}
	}
	var bridge = Bridge.Get("bridgeId").Result;
	bridge.Update(data).Wait();
	
### Play audio in bridge
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/#POST-/v1/users/{userId}/bridges/{bridgeId}/audio)

	var data = new Dictionary<string, object>
	{
		{"fileUrl", "url"}
	}
	var bridge = Bridge.Get("bridgeId").Result;
	bridge.Update(data).Wait();

### Get calls on bridge
[Documentation](https://catapult.inetwork.com/docs/api-docs/bridges/#GET-/v1/users/{userId}/bridges/{bridgeId}/calls)

	var bridge = Bridge.Get("bridgeId").Result;
	var calls = Bridge.GetCalls().Result;
	


## Call
#### Make a Call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls)

	var call = Call.Create(client, "to", "from").Result
	var call = Call.Create(client, new Dictionary<string, object> {
		{"from", "+19195551212"}, {"to", "+19195551213"}, 
		{"callbackUrl", "https://yourcallback.com/call"}}).Result;
	
	var call = Call.Create("to", "from").Result; // Default client

#### Get a call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId})

	var call = Call.Get(client, "callId").Result;
	
#### List calls
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls)

	var calls = Call.List(client, params).Result
	var calls = Call.List(client, new Dictionary<string, object>{{"from", "a number"}})

#### Update a call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId})

	var data = new Dictionary<string, object> {{"state", "completed"}};
	var call = Call.Get("id").Result;
	call.Update(data).Wait()

#### Play audio or speak a sentence in a call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/audio)

	var data = new Dictionary<string, object>{{"fileUrl", "url"}};
	var call = Call.Get("id").Result
	call.PlayAudio(data).Wait()
	
#### Send DTMF
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/dtmf)

	var data = new Dictionary<string, object>{{"dtmfOut", "123"}};
	var call = Call.Get("id").Result
	call.SendDtmf(data).Wait()
	
#### Get list of events for call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/events)

	var call = Call.Get("id").Result;
	var events = call.GetEvents().Result
	
#### Get event details for call event
[Documentation] (https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/events/{callEventId})

	var call = Call.Get("id").Result;
	var event = Call.GetEvent("eventId").Result

#### Get recordings for call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/recordings)

	var call = Call.Get("id").Result;
	var recordings = call.GetRecordings().Results


#### Get transcriptions for call
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/transcriptions)

	var call = Call.Get("id").Result
	var transcriptions = Call.GetTranscriptions().Result;
	
#### Gather Dtmf Digits
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/gather)

	var data = new Dictionary<string, object>    {
    	{"tag", "tag"},        {"maxDigits", 1}    };

	var call = Call.Get("id").Result;
	call.CreateGather(data).Wait();
	
#### Get gather parameters and results
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#GET-/v1/users/{userId}/calls/{callId}/gather/{gatherId})

	var call = Call.Get("id").Result
	var gather = call.GetGather("gatherId").Result
	
#### Update gather
[Documentation](https://catapult.inetwork.com/docs/api-docs/calls/#POST-/v1/users/{userId}/calls/{callId}/gather/{gatherId})

    var data = new Dictionary<string, object>    {        {"state", "completed"}    };
	
	var call = Call.Get("id").Result;
	call.UpdateGather("gatherId", data).Wait();


## Messages
***

	
	
