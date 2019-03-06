using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ReportPortal.Client.Api.TestItem.Model;
using ReportPortal.Client.Common.Model;
using ReportPortal.Client.Api.TestItem.Request;
using ReportPortal.Client.Common.Model.Filtering;

namespace ReportPortal.Client.Tests.TestItem
{
    public class TestItemFixture : BaseFixture, IClassFixture<LaunchFixtureBase>
    {
        private LaunchFixtureBase _fixture;

        public TestItemFixture(LaunchFixtureBase fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task StartFinishTest()
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);
            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task StartFinishTestWithTag()
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Tags = new List<string> { "MyTag1" }
            });

            var uniqueTags = await Service.TestItem.GetUniqueTagsAsync(_fixture.LaunchId, "myta");
            Assert.Contains("MyTag1", uniqueTags);

            Assert.NotNull(test.Id);
            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task StartFinishSeveralTests()
        {
            var testItemName = Guid.NewGuid().ToString();

            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var test2 = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test2.Id);

            var tests = await Service.TestItem.GetTestItemsAsync(new FilterOption
            {
                FilterConditions = new List<FilterCondition>
                        {
                            new FilterCondition(FilterOperation.Equals, "launch", _fixture.LaunchId),
                            new FilterCondition(FilterOperation.Equals, "name", testItemName)
                        }
            });
            Assert.Equal(2, tests.Collection.Count());

            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var message2 = await Service.TestItem.FinishTestItemAsync(test2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message2.Info);
        }

        [Fact]
        public async Task StartFinishFullTest()
        {
            var startTestItemRequest = new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Description = "Desc for test"
            };

            var test = await Service.TestItem.StartTestItemAsync(startTestItemRequest);
            Assert.NotNull(test.Id);

            var getTest = await Service.TestItem.GetTestItemAsync(test.Id);
            Assert.Null(getTest.ParentId);
            Assert.Equal(startTestItemRequest.Name, getTest.Name);
            Assert.Equal(startTestItemRequest.StartTime, getTest.StartTime);
            Assert.Equal(startTestItemRequest.Type, getTest.Type);
            Assert.Equal(startTestItemRequest.Description, getTest.Description);

            var finishTestItemRequest = new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            };

            var message = await Service.TestItem.FinishTestItemAsync(test.Id, finishTestItemRequest);
            Assert.Contains("successfully", message.Info);

            getTest = await Service.TestItem.GetTestItemAsync(test.Id);
            Assert.Equal(finishTestItemRequest.Status, getTest.Status);
            Assert.Equal(finishTestItemRequest.EndTime, getTest.EndTime);
        }

        [Theory]
        [InlineData(TestItemType.BeforeClass)]
        [InlineData(TestItemType.BeforeMethod)]
        [InlineData(TestItemType.Suite)]
        [InlineData(TestItemType.Test)]
        [InlineData(TestItemType.Step)]
        [InlineData(TestItemType.AfterMethod)]
        [InlineData(TestItemType.AfterClass)]
        public async Task VerifyTypeOfTests(TestItemType type)
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = type
            });
            Assert.NotNull(test.Id);
            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Theory]
        [InlineData(Status.Failed)]
        [InlineData(Status.Passed)]
        [InlineData(Status.Skipped)]
        public async Task VerifyStatusesOfTests(Status status)
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);
            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = status
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task FinishTestWithIssue()
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Id);
            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                {
                    Comment = "Comment",
                    Type = "PB001"
                }
            });
            Assert.Contains("successfully", message.Info);
        }

        public static List<object[]> WellKnownIssueTypesTestData => new List<object[]>
        { new object[] { WellKnownIssueType.ProductBug },
          new object[] { WellKnownIssueType.AutomationBug },
          new object[] { WellKnownIssueType.SystemIssue },
          new object[] { WellKnownIssueType.ToInvestigate },
          new object[] { WellKnownIssueType.NotDefect }
        };

        [Theory]
        [MemberData(nameof(WellKnownIssueTypesTestData), MemberType = typeof(TestItemFixture))]
        public async Task VerifyTestIssueTypes(string type)
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Id);
            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Skipped,
                Issue = new Issue
                {
                    Comment = "Comment",
                    Type = type
                }
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task CreateTestForSuites()
        {
            var suite = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite,
                Tags = new List<string> { "abc", "qwe" }
            });

            var test = await Service.TestItem.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Id);

            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var messageSuite = await Service.TestItem.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);
        }

        [Fact]
        public async Task CreateStepForTestInSuite()
        {
            var suite = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test = await Service.TestItem.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Id);

            Assert.True((await Service.TestItem.GetTestItemAsync(suite.Id)).HasChilds);

            var step = await Service.TestItem.StartTestItemAsync(test.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step.Id);

            var messageStep = await Service.TestItem.FinishTestItemAsync(step.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            Assert.Contains("successfully", messageStep.Info);

            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var messageSuite = await Service.TestItem.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);
        }

        [Fact]
        public async Task StartUpdateFinishTest()
        {
            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var updateMessage = await Service.TestItem.UpdateTestItemAsync(test.Id, new UpdateTestItemRequest()
            {
                Description = "newDesc",
                Tags = new List<string> { "tag1", "tag2" }
            });
            Assert.Contains("successfully", updateMessage.Info);

            var updatedTest = await Service.TestItem.GetTestItemAsync(test.Id);
            Assert.Equal("newDesc", updatedTest.Description);
            Assert.Equal(new List<string> { "tag1", "tag2" }, updatedTest.Tags);

            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task AssignTestItemIssuesTest()
        {
            var suite = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test1 = await Service.TestItem.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test1.Id);

            var step1 = await Service.TestItem.StartTestItemAsync(test1.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step1.Id);

            var messageStep1 = await Service.TestItem.FinishTestItemAsync(step1.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                {
                    Comment = "Comment 1",
                    Type = "TI001"
                }
            });

            Assert.Contains("successfully", messageStep1.Info);

            var messageTest1 = await Service.TestItem.FinishTestItemAsync(test1.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageTest1.Info);

            var test2 = await Service.TestItem.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test2.Id);

            var step2 = await Service.TestItem.StartTestItemAsync(test2.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step2.Id);

            var messageStep2 = await Service.TestItem.FinishTestItemAsync(step2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                {
                    Comment = "Comment 2",
                    Type = "AB001"
                }
            });

            Assert.Contains("successfully", messageStep2.Info);

            var messageTest2 = await Service.TestItem.FinishTestItemAsync(test2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageTest2.Info);

            var messageSuite = await Service.TestItem.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);

            var issue1 = new Issue
            {
                Comment = "New Comment 1",
                Type = "PB001"
            };

            var issue2 = new Issue
            {
                Comment = "New Comment 2",
                Type = "SI001",
                ExternalSystemIssues = new List<ExternalSystemIssue>
                {
                    new ExternalSystemIssue { TicketId = "XXXXX-15", Url = new Uri("https://jira.epam.com/jira/browse/XXXXX-15") }
                }
            };

            var assignedIssues = await Service.TestItem.AssignTestItemIssuesAsync(new AssignTestItemIssuesRequest
            {
                Issues = new List<TestItemIssueUpdate>
                {
                    new TestItemIssueUpdate
                    {
                        Issue = issue1,
                        TestItemId = step1.Id
                    },
                    new TestItemIssueUpdate
                    {
                        Issue = issue2,
                        TestItemId = step2.Id
                    }
                }
            });

            Assert.Equal(2, assignedIssues.Count());

            Assert.Equal(issue1.Comment, assignedIssues.First().Comment);
            Assert.Equal(issue1.Type, assignedIssues.First().Type);
            Assert.Null(assignedIssues.First().ExternalSystemIssues);

            Assert.Equal(issue2.Comment, assignedIssues.ElementAt(1).Comment);
            Assert.Equal(issue2.Type, assignedIssues.ElementAt(1).Type);
            Assert.NotNull(assignedIssues.ElementAt(1).ExternalSystemIssues);

            Assert.Single(assignedIssues.ElementAt(1).ExternalSystemIssues);
            Assert.True(assignedIssues.ElementAt(1).ExternalSystemIssues.First().SubmitDate - DateTime.UtcNow < TimeSpan.FromMinutes(1));
            Assert.Equal(Username, assignedIssues.ElementAt(1).ExternalSystemIssues.First().Submitter);
            Assert.Equal(issue2.ExternalSystemIssues.First().TicketId, assignedIssues.ElementAt(1).ExternalSystemIssues.First().TicketId);
            Assert.Equal(issue2.ExternalSystemIssues.First().Url, assignedIssues.ElementAt(1).ExternalSystemIssues.First().Url);

            step1 = await Service.TestItem.GetTestItemAsync(step1.Id);
            Assert.NotNull(step1.Issue);

            step2 = await Service.TestItem.GetTestItemAsync(step2.Id);
            Assert.NotNull(step2.Issue);

            Assert.Equal(issue1.Comment, step1.Issue.Comment);
            Assert.Equal(issue1.Type, step1.Issue.Type);
            Assert.Null(step1.Issue.ExternalSystemIssues);

            Assert.Equal(issue2.Comment, step2.Issue.Comment);
            Assert.Equal(issue2.Type, step2.Issue.Type);
            Assert.NotNull(step2.Issue.ExternalSystemIssues);

            Assert.Single(step2.Issue.ExternalSystemIssues);
            Assert.True(step2.Issue.ExternalSystemIssues.First().SubmitDate - DateTime.UtcNow < TimeSpan.FromMinutes(1));
            Assert.Equal(Username, step2.Issue.ExternalSystemIssues.First().Submitter);
            Assert.Equal(issue2.ExternalSystemIssues.First().TicketId, step2.Issue.ExternalSystemIssues.First().TicketId);
            Assert.Equal(issue2.ExternalSystemIssues.First().Url, step2.Issue.ExternalSystemIssues.First().Url);
        }

        [Fact]
        public async Task GetTestItemHistory()
        {
            var histories = await Service.TestItem.GetTestItemHistoryAsync("5bfe5b213cdea200012a9fb7", 5, true);
            Assert.Equal(5, histories.Count);
        }

        [Fact]
        public async Task TrimTestItemName()
        {
            var namePrefix = "TrimLaunch";
            var testItemName = namePrefix + new string('_', 256 - namePrefix.Length + 1);

            var test = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var gotTestItem = await Service.TestItem.GetTestItemAsync(test.Id);
            Assert.Equal(testItemName.Substring(0, 256), gotTestItem.Name);

            var message = await Service.TestItem.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task RetryTest()
        {
            var suite = await Service.TestItem.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "RetrySuite",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var firstAttempt = await Service.TestItem.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "RetryTest",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step,
                IsRetry = true // this is required to show test as retried
            });

            await Service.TestItem.FinishTestItemAsync(firstAttempt.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                //IsRetry = true
            });

            var secondAttempt = await Service.TestItem.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _fixture.LaunchId,
                Name = "RetryTest",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step,
                IsRetry = true
            });

            await Service.TestItem.FinishTestItemAsync(secondAttempt.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                //IsRetry = true
            });

            await Service.TestItem.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow
            });

            var launch = await Service.Launch.GetLaunchAsync(_fixture.LaunchId);

            Assert.True(launch.HasRetries);
        }
    }
}