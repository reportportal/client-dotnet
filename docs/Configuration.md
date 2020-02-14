# Configuration Source
There are several configuration sources to configure reporting.

- Json file
- Directory source
- Environment variables

## Json file
Agents read the `ReportPortal.config.json` file for configuration properties. This file should be located in the same directory where agent dll file is located.

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
Sometimes it's useful to specify configuration properties via environment variables. To specify `Section1:PropertyAbc` property just set environment variable with `ReportPortal_Section1_PropertyAbc` name. Variable names should start from `ReportPortal_` prefix, and `_` symbol is used as delimeter of nested variables.


# HTTP requests retry
During tests execution agent sends test results as http requests to server. In case of fast test execution, or parallel tests execution, agent produces many requests to server. These requests are being sent in background and in parallel. Some requests might be failed due any reason e.g. short-term service unavailability, or network bandwith. To negotiate this issue several methodics can be applied.

## Throttle requests
Configure `Server:MaximumConnectionsNumber` property to set the maximum number of concurrent requests.

## Retry requests
Set `Server:Retry:Strategy` property to `Exponential` (default) or `Linear`.

### Exponential
Failed requests will be retried with continuosly increased delays between attemps.

- `Server:Retry:MaxAttempts` - how many times retry request (3 attempts by default)
- `Server:Retry:BaseIndex` - exponential index for delaying (2 seconds by default)

Given default values reporter will wait 8 seconds (2^3), then 16 seconds (2^4).

### Linear
- `Server:Retry:MaxAttempts` - how many times retry request (3 attempts by default)
- `Server:Retry:Delay` - how long to wait between attempts (5000 milliseconds by default)