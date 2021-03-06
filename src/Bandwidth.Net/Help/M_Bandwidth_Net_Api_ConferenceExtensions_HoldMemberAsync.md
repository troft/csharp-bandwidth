﻿# ConferenceExtensions.HoldMemberAsync Method 
 

Hold/Unhold the conference member

**Namespace:**&nbsp;<a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api</a><br />**Assembly:**&nbsp;Bandwidth.Net (in Bandwidth.Net.dll) Version: 3.0.0-beta4

## Syntax

**C#**<br />
``` C#
public static Task HoldMemberAsync(
	this IConference instance,
	string conferenceId,
	string memberId,
	bool hold
)
```


#### Parameters
&nbsp;<dl><dt>instance</dt><dd>Type: <a href ="T_Bandwidth_Net_Api_IConference.md">Bandwidth.Net.Api.IConference</a><br />>Instance of <a href ="T_Bandwidth_Net_IPlayAudio.md">IPlayAudio</a></dd><dt>conferenceId</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Id of the conference</dd><dt>memberId</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Id of the member</dd><dt>hold</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">System.Boolean</a><br />'true' to hold member or 'false'</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd235678" target="_blank">Task</a><br />Task instance for async operation

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type <a href ="T_Bandwidth_Net_Api_IConference.md">IConference</a>. When you use instance method syntax to call this method, omit the first parameter. For more information, see <a href="http://msdn.microsoft.com/en-us/library/bb384936.aspx">Extension Methods (Visual Basic)</a> or <a href="http://msdn.microsoft.com/en-us/library/bb383977.aspx">Extension Methods (C# Programming Guide)</a>.

## See Also


#### Reference
<a href ="T_Bandwidth_Net_Api_ConferenceExtensions.md">ConferenceExtensions Class</a><br /><a href ="N_Bandwidth_Net_Api.md">Bandwidth.Net.Api Namespace</a><br />