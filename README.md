# Demo Fhi.HelseId

This is a demo app using the Fhi.HelseId package:

https://github.com/folkehelseinstituttet/fhi.helseid

The app is a plain `dotnet new angular` app with the HelseId component added.

It demonstrates login, logout and protection of all other routes.

## Setup

### appsettings.json
Copy the `appsettings.Template.json` and name it `appsettings.json`.

### HelseID
You need to have a client that is registered in HelseId's test environment in order to setup this demo.
The client needs to be configured with a callback URL for localhost that matches the port you are running to app on.

By default this demo app is set up to run on port 51384 in [Program.cs](https://github.com/folkehelseinstituttet/fhi.helseid.demo/blob/master/Program.cs).
This is the configuration HelseId needs to setup as a Redirect URI for your app:

```
https://localhost:51384/signin-callback
```

This is because `/signin-callback` is the default CallbackPath [set by Fhi.HelseId](https://github.com/folkehelseinstituttet/fhi.helseid/blob/7e67dc39b942d294d407907b323544789a4eec50/Fhi.HelseId/Web/ExtensionMethods/HelseIdExtensions.cs).

HelseId will also provide you with a clientId and a client secret.

### Configure appsettings.json
Fill in the missing values for `ClientId` and `ClientSecret` under the section `DemoHelseIdConfig`.
Also fill in the necessary scopes.

```json
  "DemoHelseIdConfig": {
    "AuthUse": "true",
    "UseHprNumber": "false",
    "UseHttps": "true",
    "Authority": "https://helseid-sts.test.nhn.no/",
    "ClientId": "your_client_id",
    "ClientSecret": "your_client_secret",
    "Scopes": [
      "yourorg:your_api/gateway"
    ],
    "SecurityLevels": [ "3", "4" ]
  }
```

#### HPR
If you want to check HPR authorizations you need to have a user that can access the HPR test environment.
Fill in the login details under the section `HprKonfigurasjon`:

```json
  "HprKonfigurasjon": {
    "Url": "https://register.test.nhn.no/v2/HPR",
    "Brukernavn": "your_hpr_login",
    "Passord": "your_hpr_password"
  }
```

Also make sure to set the `UseHprNumber` to `true`:
```json
  "DemoHelseIdConfig": {
    "AuthUse": "true",
    "UseHprNumber": "true",
    "UseHttps": "true",
    "Authority": "https://helseid-sts.test.nhn.no/",
    "ClientId": "your_client_id",
    "ClientSecret": "your_client_secret",
    "Scopes": [
      "yourorg:your_api/gateway"
    ],
    "SecurityLevels": [ "3", "4" ]
  }
```