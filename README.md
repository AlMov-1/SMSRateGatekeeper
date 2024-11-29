# SMSRateGatekeeperService

## Description
This is an example demo project.
See UI for thi service here: https://github.com/AlMov-1/sms-rate-gatekeeper-ui

This is a .NET Core (C#) microservice that acts as a gatekeeper, deciding
whether a message can be sent from a given business phone number before calling the
external provider. This microservice will be called from various applications and services across
your infrastructure.

As a hypothetical SMS provider has a limitations for the number of message to be sent from the account and from the different numbers.
This service will track thos limits and make a decision whenther to send the message to provider or not to, as exciding the providers limits might 
encure highe cost for the account owner.

The service responds to the message Post request by Status code and message indicating the success or failure (in case when limit for the time is being reached).

The service is also configured with SignalR to send real time nitfication to consuming UI.
The notification will contai the 
- Result of an attempt
- Phone number from which the message was sent
- The percentage of the limit reached for the phone number
- The prcentage of the limit rewached for the whole account

The limit (per second) for the account and the individual number is configurable.
Change configuration to desirable amount in the appsettings.json file:

```
"SMSProvider": {
  "Limits": {
    "MaxCountForBusinessPerSec": 4,
    "MaxCountForEntireAccountPerSec": 20
  },
  ...
}
```

