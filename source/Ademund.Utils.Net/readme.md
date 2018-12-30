In response to [this SO question](https://stackoverflow.com/questions/3253701/get-public-external-ip-address), 
fires off parallel requests to each url defined in checkIPUrls, returns the fist valid response.

usage:
```c#
	using Ademund.Utils.Net;
	string myIp = await IPUtils.GetExternalIPAddressAsync();
```
	
params:
 - proxyAddress: Optional proxy address, e.g. 127.0.0.1:8888

 returns: 
  - A string representation of an IP Address, or null if no valid responses are received
