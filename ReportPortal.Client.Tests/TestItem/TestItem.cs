﻿using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using System.Threading.Tasks;
using Xunit;

namespace ReportPortal.Client.Tests.TestItem
{
    public class TestItemFixture : BaseFixture, IDisposable
    {
        private string _launchId;

        public TestItemFixture()
        {
            _launchId = Task.Run(async () => await Service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            })).Result.Id;
        }

        public void Dispose()
        {
            Task.Run(async () => await Service.DeleteLaunchAsync(_launchId)).Wait();
        }

        [Fact]
        public async Task StartFinishDeleteTest()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);
            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartForceFinishIncompleteLaunch()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            }, true);
        }

        [Fact]
        public async Task StartFinishDeleteTestWithTag()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Tags = new List<string> { "MyTag1" }
            });

            var uniqueTags = await Service.GetUniqueTagsAsync(_launchId, "myta");
            Assert.Contains("MyTag1", uniqueTags);

            Assert.NotNull(test.Id);
            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartFinishDeleteSeveralTests()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var test2 = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test2",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test2.Id);

            var tests = await Service.GetTestItemsAsync(new FilterOption
            {
                Filters = new List<Filter>
                        {
                            new Filter(FilterOperation.Equals, "launch", _launchId)
                        }
            });
            Assert.Equal(2, tests.TestItems.Count());

            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var message2 = await Service.FinishTestItemAsync(test2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message2.Info);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task StartFinishDeleteFullTest()
        {
            var now = DateTime.UtcNow;
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Description = "Desc for test"
            });
            Assert.NotNull(test.Id);

            var getTest = await Service.GetTestItemAsync(test.Id);
            Assert.Null(getTest.ParentId);
            Assert.Equal("Test1", getTest.Name);
            Assert.Equal(now.ToString(), getTest.StartTime.ToString());
            Assert.Equal(TestItemType.Test, getTest.Type);
            Assert.Equal("Desc for test", getTest.Description);

            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
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
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = type
            });
            Assert.NotNull(test.Id);
            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [Theory]
        [InlineData(Status.Failed)]
        [InlineData(Status.Passed)]
        [InlineData(Status.Skipped)]
        public async Task VerifyStatusesOfTests(Status status)
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);
            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = status
            });
            Assert.Contains("successfully", message.Info);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [Fact]
        public async Task FinishTestWithIssue()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Id);
            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
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
            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
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
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Id);
            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
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

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [Fact]
        public async Task CreateTestForSuites()
        {
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite,
                Tags = new List<string> { "abc", "qwe" }
            });

            var test = await Service.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Id);

            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var messageSuite = await Service.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);

            var delMessageSuite = await Service.DeleteTestItemAsync(suite.Id);
            Assert.Contains("successfully", delMessageSuite.Info);
        }

        [Fact]
        public async Task CreateStepForTestInSuite()
        {
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test = await Service.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Id);

            Assert.True((await Service.GetTestItemAsync(suite.Id)).HasChilds);

            var step = await Service.StartTestItemAsync(test.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step.Id);

            var messageStep = await Service.FinishTestItemAsync(step.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });

            Assert.Contains("successfully", messageStep.Info);

            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);

            var messageSuite = await Service.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageSuite.Info);

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);

            var delMessageSuite = await Service.DeleteTestItemAsync(suite.Id);
            Assert.Contains("successfully", delMessageSuite.Info);
        }

        [Fact]
        public async Task StartUpdateFinishDeleteTest()
        {
            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var updateMessage = await Service.UpdateTestItemAsync(test.Id, new UpdateTestItemRequest()
            {
                Description = "newDesc",
                Tags = new List<string> { "tag1", "tag2" }
            });
            Assert.Contains("successfully", updateMessage.Info);

            var updatedTest = await Service.GetTestItemAsync(test.Id);
            Assert.Equal("newDesc", updatedTest.Description);
            Assert.Equal(new List<string> { "tag1", "tag2" }, updatedTest.Tags);

            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task AssignTestItemIssuesTest()
        {
            var suite = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test1 = await Service.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test1.Id);

            var step1 = await Service.StartTestItemAsync(test1.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step1.Id);

            var messageStep1 = await Service.FinishTestItemAsync(step1.Id, new FinishTestItemRequest
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

            var messageTest1 = await Service.FinishTestItemAsync(test1.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageTest1.Info);

            var test2 = await Service.StartTestItemAsync(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test2.Id);

            var step2 = await Service.StartTestItemAsync(test2.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step2.Id);

            var messageStep2 = await Service.FinishTestItemAsync(step2.Id, new FinishTestItemRequest
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

            var messageTest2 = await Service.FinishTestItemAsync(test2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", messageTest2.Info);

            var messageSuite = await Service.FinishTestItemAsync(suite.Id, new FinishTestItemRequest
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
                    new ExternalSystemIssue { TicketId = "XXXXX-15", Url = "https://jira.epam.com/jira/browse/XXXXX-15" }
                }
            };

            var assignedIssues = await Service.AssignTestItemIssuesAsync(new AssignTestItemIssuesRequest
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

            step1 = await Service.GetTestItemAsync(step1.Id);
            Assert.NotNull(step1.Issue);

            step2 = await Service.GetTestItemAsync(step2.Id);
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

            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test1.Id);
            Assert.Contains("successfully", delMessage.Info);
        }

        [Fact]
        public async Task GetTestItemHistory()
        {
            var histories = await Service.GetTestItemHistoryAsync("5bc4bada2ab79c0001391c15", 5, true);
            Assert.Equal(5, histories.Count);
            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [Fact]
        public async Task TrimTestItemName()
        {
            var namePrefix = "TrimLaunch";
            var testItemName = namePrefix + new string('_', 256 - namePrefix.Length + 1);

            var test = await Service.StartTestItemAsync(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = testItemName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var gotTestItem = await Service.GetTestItemAsync(test.Id);
            Assert.Equal(testItemName.Substring(0, 256), gotTestItem.Name);

            var message = await Service.FinishTestItemAsync(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            Assert.Contains("successfully", message.Info);
            await Service.FinishLaunchAsync(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = await Service.DeleteTestItemAsync(test.Id);
            Assert.Contains("successfully", delMessage.Info);
        }
    }
}