using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using Xunit;

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
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Uuid);
            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task StartFinishTestWithTag()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Tags = new List<string> { "MyTag1" }
            });

            Assert.NotNull(test.Uuid);
            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
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

            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Uuid);

            var test2 = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test2.Uuid);

            var tests = await Service.GetTestItemsAsync(new FilterOption
            {
                Filters = new List<Filter>
                        {
                            new Filter(FilterOperation.Equals, "launchId", _fixture.LaunchId),
                            new Filter(FilterOperation.Equals, "name", testItemName)
                        }
            });
            Assert.Equal(2, tests.TestItems.Count());

            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var message2 = await Service.FinishTestItemAsync(test2.Uuid, new FinishTestItemRequest
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
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Description = "Desc for test"
            };

            var test = await Service.StartTestItemAsync(startTestItemRequest);
            Assert.NotNull(test.Uuid);

            var getTest = await Service.GetTestItemAsync(test.Uuid);
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

            var message = await Service.FinishTestItemAsync(test.Uuid, finishTestItemRequest);
            Assert.Contains("successfully", message.Info);

            getTest = await Service.GetTestItemAsync(test.Uuid);
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
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = type
            });
            Assert.NotNull(test.Uuid);
            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
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
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Uuid);
            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = status
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task FinishTestWithIssue()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Uuid);
            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
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
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Uuid);
            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
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
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite,
                Tags = new List<string> { "abc", "qwe" }
            });

            var test = await Service.StartTestItemAsync(suite.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Uuid);

            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var messageSuite = await Service.FinishTestItemAsync(suite.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);
        }

        [Fact]
        public async Task CreateStepForTestInSuite()
        {
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test = await Service.StartTestItemAsync(suite.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Uuid);

            Assert.True((await Service.GetTestItemAsync(suite.Uuid)).HasChildren);

            var step = await Service.StartTestItemAsync(test.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step.Uuid);

            var messageStep = await Service.FinishTestItemAsync(step.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            Assert.Contains("successfully", messageStep.Info);

            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var messageSuite = await Service.FinishTestItemAsync(suite.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);
        }

        [Fact]
        public async Task StartUpdateFinishTest()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Uuid);

            var tempTest = await Service.GetTestItemAsync(test.Uuid);
            var updateMessage = await Service.UpdateTestItemAsync(tempTest.Id, new UpdateTestItemRequest()
            {
                Description = "newDesc",
                //Tags = new List<string> { "tag1", "tag2" }
            });
            Assert.Contains("successfully", updateMessage.Info);

            var updatedTest = await Service.GetTestItemAsync(test.Uuid);
            Assert.Equal("newDesc", updatedTest.Description);
            //Assert.Equal(new List<string> { "tag1", "tag2" }, updatedTest.Tags);

            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task AssignTestItemIssuesTest()
        {
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test1 = await Service.StartTestItemAsync(suite.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test1.Uuid);

            var step1 = await Service.StartTestItemAsync(test1.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step1.Uuid);

            var messageStep1 = await Service.FinishTestItemAsync(step1.Uuid, new FinishTestItemRequest
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

            var messageTest1 = await Service.FinishTestItemAsync(test1.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageTest1.Info);

            var test2 = await Service.StartTestItemAsync(suite.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test2.Uuid);

            var step2 = await Service.StartTestItemAsync(test2.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step2.Uuid);

            var messageStep2 = await Service.FinishTestItemAsync(step2.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                {
                    Comment = "Comment 2",
                    Type = "ab001"
                }
            });

            Assert.Contains("successfully", messageStep2.Info);

            var messageTest2 = await Service.FinishTestItemAsync(test2.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageTest2.Info);

            var messageSuite = await Service.FinishTestItemAsync(suite.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);

            var issue1 = new Issue
            {
                Comment = "New Comment 1",
                Type = "pb001"
            };

            var issue2 = new Issue
            {
                Comment = "New Comment 2",
                Type = "si001",
                //ExternalSystemIssues = new List<ExternalSystemIssue>
                //{
                //    new ExternalSystemIssue { TicketId = "XXXXX-15", Url = "https://jira.epam.com/jira/browse/XXXXX-15" }
                //}
            };

            var tempStep1 = await Service.GetTestItemAsync(step1.Uuid);
            var tempStep2 = await Service.GetTestItemAsync(step2.Uuid);
            var assignedIssues = await Service.AssignTestItemIssuesAsync(new AssignTestItemIssuesRequest
            {
                Issues = new List<TestItemIssueUpdate>
                {
                    new TestItemIssueUpdate
                    {
                        Issue = issue1,
                        TestItemId = tempStep1.Id
                    },
                    new TestItemIssueUpdate
                    {
                        Issue = issue2,
                        TestItemId = tempStep2.Id
                    }
                }
            });

            Assert.Equal(2, assignedIssues.Count());

            Assert.Equal(issue1.Comment, assignedIssues.First().Comment);
            Assert.Equal(issue1.Type, assignedIssues.First().Type);
            //Assert.Null(assignedIssues.First().ExternalSystemIssues);

            Assert.Equal(issue2.Comment, assignedIssues.ElementAt(1).Comment);
            Assert.Equal(issue2.Type, assignedIssues.ElementAt(1).Type);
            Assert.NotNull(assignedIssues.ElementAt(1).ExternalSystemIssues);

            //Assert.Single(assignedIssues.ElementAt(1).ExternalSystemIssues);
            //Assert.True(assignedIssues.ElementAt(1).ExternalSystemIssues.First().SubmitDate - DateTime.UtcNow < TimeSpan.FromMinutes(1));
            //Assert.Equal(Username, assignedIssues.ElementAt(1).ExternalSystemIssues.First().Submitter);
            //Assert.Equal(issue2.ExternalSystemIssues.First().TicketId, assignedIssues.ElementAt(1).ExternalSystemIssues.First().TicketId);
            //Assert.Equal(issue2.ExternalSystemIssues.First().Url, assignedIssues.ElementAt(1).ExternalSystemIssues.First().Url);

            var stepInfo1 = await Service.GetTestItemAsync(step1.Uuid);
            Assert.NotNull(stepInfo1.Issue);

            var stepInfo2 = await Service.GetTestItemAsync(step2.Uuid);
            Assert.NotNull(stepInfo2.Issue);

            Assert.Equal(issue1.Comment, stepInfo1.Issue.Comment);
            Assert.Equal(issue1.Type, stepInfo1.Issue.Type);
            //Assert.Null(stepInfo1.Issue.ExternalSystemIssues);

            Assert.Equal(issue2.Comment, stepInfo2.Issue.Comment);
            Assert.Equal(issue2.Type, stepInfo2.Issue.Type);
            //Assert.NotNull(stepInfo2.Issue.ExternalSystemIssues);

            //Assert.Single(stepInfo2.Issue.ExternalSystemIssues);
            //Assert.True(stepInfo2.Issue.ExternalSystemIssues.First().SubmitDate - DateTime.UtcNow < TimeSpan.FromMinutes(1));
            //Assert.Equal(Username, stepInfo2.Issue.ExternalSystemIssues.First().Submitter);
            //Assert.Equal(issue2.ExternalSystemIssues.First().TicketId, stepInfo2.Issue.ExternalSystemIssues.First().TicketId);
            //Assert.Equal(issue2.ExternalSystemIssues.First().Url, stepInfo2.Issue.ExternalSystemIssues.First().Url);
        }

        [Fact]
        public async Task GetTestItemHistory()
        {
            var launchName = Guid.NewGuid().ToString();
            var launch1 = await Service.StartLaunchAsync(new StartLaunchRequest { Name = launchName, StartTime = DateTime.UtcNow });
            var test1 = await Service.StartTestItemAsync(new StartTestItemRequest { LaunchUuid = launch1.Uuid, Name = "ABC", StartTime = DateTime.UtcNow });

            var launch2 = await Service.StartLaunchAsync(new StartLaunchRequest { Name = launchName, StartTime = DateTime.UtcNow });
            var test2 = await Service.StartTestItemAsync(new StartTestItemRequest { LaunchUuid = launch2.Uuid, Name = "ABC", StartTime = DateTime.UtcNow });

            var gotTest2 = await Service.GetTestItemAsync(test2.Uuid);

            var histories = await Service.GetTestItemHistoryAsync(new List<long> { gotTest2.Id }, 5, true);
            Assert.Equal(2, histories.Count);

            var gotLaunch1 = await Service.GetLaunchAsync(launch1.Uuid);
            var gotLaunch2 = await Service.GetLaunchAsync(launch2.Uuid);

            await Service.StopLaunchAsync(gotLaunch1.Id, new FinishLaunchRequest { EndTime = DateTime.UtcNow });
            await Service.StopLaunchAsync(gotLaunch2.Id, new FinishLaunchRequest { EndTime = DateTime.UtcNow });

            await Service.DeleteLaunchAsync(gotLaunch1.Id);
            await Service.DeleteLaunchAsync(gotLaunch2.Id);
        }

        [Fact]
        public async Task TrimTestItemName()
        {
            var namePrefix = "TrimLaunch";
            var testItemName = namePrefix + new string('_', 256 - namePrefix.Length + 1);

            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Uuid);

            var gotTestItem = await Service.GetTestItemAsync(test.Uuid);
            Assert.Equal(testItemName.Substring(0, 256), gotTestItem.Name);

            var message = await Service.FinishTestItemAsync(test.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
        }

        [Fact]
        public async Task RetryTest()
        {
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "RetrySuite",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var firstAttempt = await Service.StartTestItemAsync(suite.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "RetryTest",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step,
                IsRetry = true // this is required to show test as retried
            });

            await Service.FinishTestItemAsync(firstAttempt.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                //IsRetry = true
            });

            var secondAttempt = await Service.StartTestItemAsync(suite.Uuid, new StartTestItemRequest
            {
                LaunchUuid = _fixture.LaunchUuid,
                Name = "RetryTest",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step,
                IsRetry = true
            });

            await Service.FinishTestItemAsync(secondAttempt.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                //IsRetry = true
            });

            await Service.FinishTestItemAsync(suite.Uuid, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow
            });

            var launch = await Service.GetLaunchAsync(_fixture.LaunchId);

            Assert.True(launch.HasRetries);
        }
    }
}