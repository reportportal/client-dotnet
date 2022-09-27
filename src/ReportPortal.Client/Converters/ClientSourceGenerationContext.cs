using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Converters
{
    /// <inheritdoc />

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]

    [JsonSerializable(typeof(Abstractions.Requests.StartLaunchRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.FinishLaunchRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.UpdateLaunchRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.AnalyzeLaunchRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.MergeLaunchesRequest))]

    [JsonSerializable(typeof(Abstractions.Responses.MessageResponse))]

    [JsonSerializable(typeof(Abstractions.Responses.LaunchResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.Content<Abstractions.Responses.LaunchResponse>))]
    [JsonSerializable(typeof(Abstractions.Responses.LaunchCreatedResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.LaunchFinishedResponse))]

    [JsonSerializable(typeof(Abstractions.Requests.StartTestItemRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.FinishTestItemRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.UpdateTestItemRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.AssignTestItemIssuesRequest))]

    [JsonSerializable(typeof(Abstractions.Responses.TestItemResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.Content<Abstractions.Responses.TestItemResponse>))]
    [JsonSerializable(typeof(Abstractions.Responses.TestItemCreatedResponse))]
    [JsonSerializable(typeof(IEnumerable<Abstractions.Responses.Issue>))]
    [JsonSerializable(typeof(Abstractions.Responses.Content<Abstractions.Responses.TestItemHistoryContainer>))]

    [JsonSerializable(typeof(Abstractions.Requests.CreateLogItemRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.CreateLogItemRequest[]))]

    [JsonSerializable(typeof(Abstractions.Responses.LogItemResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.Content<Abstractions.Responses.LogItemResponse>))]
    [JsonSerializable(typeof(Abstractions.Responses.LogItemCreatedResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.LogItemsCreatedResponse))]

    [JsonSerializable(typeof(Abstractions.Requests.CreateUserFilterRequest))]
    [JsonSerializable(typeof(Abstractions.Requests.UpdateUserFilterRequest))]

    [JsonSerializable(typeof(Abstractions.Responses.UserFilterResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.Content<Abstractions.Responses.UserFilterResponse>))]
    [JsonSerializable(typeof(Abstractions.Responses.UserFilterCreatedResponse))]

    [JsonSerializable(typeof(Abstractions.Responses.UserResponse))]

    [JsonSerializable(typeof(Abstractions.Responses.Project.ProjectResponse))]
    [JsonSerializable(typeof(Abstractions.Responses.Project.PreferenceResponse))]

    public partial class ClientSourceGenerationContext : JsonSerializerContext
    {
    }
}
