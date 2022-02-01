# mapkitjs-aspnet-core-sample
Sample ASP.NET Core application that implements MapKit JS, including a JWT token generation. The project currently uses .NET 6, but the code should work fine on older versions, as well.

**Note that the sample project will not work correctly as-is because there are several stubs in the configuration -- you'll want to provide your own PrivateKey, KeyIdentifier, TeamID, and Origin in appsettings.json/appsettings.Development.json under the MapKitSettings section.**  The first three of those values will come from your own Apple Developer account.

There's also a post on this sample that goes into more detail at [https://www.adamrussell.com/mapkit-js-with-asp-net-core](https://www.adamrussell.com/mapkit-js-with-asp-net-core).