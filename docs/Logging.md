# Log

In any place of your test it's possible to send log messages to ReportPortal using

```csharp
using ReportPortal.Shared;

Context.Current.Log.Info("my message");
```

or send log messages directly to launch:

```csharp
using ReportPortal.Shared;

Context.Launch.Log.Info("my message comes to launch instead of test");
```

This approach is used by any existing log framework appenders for ReportPortal like [Serilog](https://github.com/reportportal/logger-net-serilog) or [log4net](https://github.com/reportportal/logger-net-log4net).


# Attachments

You can attach any binary content

```csharp
Context.Current.Log.Info("my binary", "image/png", bytes);
// where bytes is byte[] and image/png is mime type of content
```

Or use file instead

```csharp
Context.Current.Log.Info("my file", new FileInfo(filePath));
// where filePath is relative/absolute path to your file
// mime type is determined automatically
```


# Scoping (nested steps)

```csharp
[Test]
public void Test1()
{
  using (var scope = Context.Current.Log.BeginScope("logical operation"))
  {
    Context.Current.Log.Info("message");
    // or
    scope.Info("message");

    using (var scope2 = scope.BeginScope("inner operation"))
    {
      Context.Current.Log.Info("inner message");

      // and change status
      scope2.Status = LogScopeStatus.Failed;
    }
  }
}
```


# Styling

It supports markdown formatting:
```csharp
using (var s = Log.BeginScope("Adding `2` and `3` in calculator..."))
{
  Context.Current.Log.Trace("Clicking on **2** button...");
}
```