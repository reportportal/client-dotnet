# Configuration Source
There are several configuration sources to configure reporting.

- Json file
- Directory source
- Environment variables

Configuration sources are not mandatory, you can use any of them, or in combination. But specifying of same properties is mandatory, e.g. URI to server Web API. If some property is defined in several sources, the latest one is used by reporters. If you want concatenate a value which is already defined in configuration chain, just begin your value from `++` entry. Specifying `reportportal_launch_description` env variable with `++this is additional launch info` will be used in cooperation with defined property value in json file. Or `reportportal_launch_tags` = `++more_tag_1;and_one_more_2` tags will be added to launch instead of overwriting it. 

## Json file
Agents read the `ReportPortal.config.json` file for configuration properties. This file should be located in the same directory where agent dll file is located (usually it's your tests output `bin\Debug` directory).

```
bin
|- Debug
    |- YourTests.dll
    |- ReportPortal.Shared.dll
    |- ReportPortal.config.json
```

Values in json file are considered as flatten list. The following json file will be converted to `Section1:PropertyAbc`.
```json
{
    "Section1": {
        "PropertyAbc": "Value"
    }
}
```

## Directory source
It's easier to define some property in any textual file. Agent finds all these files and considers them as configuration source. For example to define `Section1:PropertyAbc` property just create a file with `ReportPortal_Section1_PropertyAbc` name, put any value into this file, and agent will take it during test results reporting.

## Environment variables
Sometimes it's useful to specify configuration properties via environment variables. To specify `Section1:PropertyAbc` property just set environment variable with `ReportPortal_Section1_PropertyAbc` name. Variable names should start from `ReportPortal_` or `RP_` prefix, and `_` symbol is used as delimeter of nested variables. Variable names are case-insensitive.

# HTTP
## Proxy
`Server:Proxy:Url` - url to proxy server to be used for http requests like `http://myproxy.corp:8080`.
`Server:Proxy:Username`, `Server:Proxy:Domain` and `Server:Proxy:Password` to specify credentials for proxy server which require authorization.

## Timeout
`Server:Timeout` - how many seconds to wait when awaiting response from server.

# HTTP requests retry
During tests execution agent sends test results as http requests to server. In case of fast test execution, or parallel tests execution, agent produces many requests to server. These requests are being sent in background and in parallel. Some requests might be failed due any reason e.g. short-term service unavailability, or network bandwith. To negotiate this issue several methodics can be applied.

## Throttle requests
Configure `Server:MaximumConnectionsNumber` property to set the maximum number of concurrent requests (default is `10`).

## Retry requests
Set `Server:Retry:Strategy` property to `Exponential` (default) or `Linear` or `None`.

### Exponential
Failed requests will be retried with continuosly increased delays between attemps.

- `Server:Retry:MaxAttempts` - how many times retry request (3 attempts by default)
- `Server:Retry:BaseIndex` - exponential index for delaying (2 seconds by default)

Given default values reporter will wait 8 seconds (2^3), then 16 seconds (2^4).

### Linear
- `Server:Retry:MaxAttempts` - how many times retry request (3 attempts by default)
- `Server:Retry:Delay` - how long to wait between attempts (5000 milliseconds by default)

### None
Requests are not repeated, only 1 attempt is allocated for all requests.


# Reporting Experience

If you want to redirect agent to send test results into some another way, there are several options:
- `Launch:Id` (UUID of existing launch) - agent will append test results into provided Launch ID. Launch should be *IN_PROGRESS* state, agent will not finish it. It's your responsibility to start and finish launch. Usefull for distributed test execution, where tests are running on different machines and you want to see consolidated report.
- `Launch:Rerun` (true/false/yes/no) - agent will try to add new tests into existing launch (compared by name) or adds new attempt/retry for existing tests.
- `Launch:RerunOf` (UUID of existing launch) - agent will try to add new tests into existing launch (by ID) or adds new attempt/retry for existing tests.


# Analytics

Each time when new launch is posted to RP server, reporting engine sends this fact to google analytics service. It doesn't collect sensetive information, just name and version of used engine/agent.

This behavior can be turned off through `Analytics:Enabled` configuration property.