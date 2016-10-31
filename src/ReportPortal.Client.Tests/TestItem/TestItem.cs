using System;
using System.Linq;
using System.Collections.Generic;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using NUnit.Framework;

namespace ReportPortal.Client.Tests.TestItem
{
    [TestFixture]
    public class TestItemFixture : BaseFixture
    {
        private string _launchId;

        [SetUp]
        public void SetUp()
        {
            _launchId = Service.StartLaunch(new StartLaunchRequest
            {
                Name = "StartFinishDeleteLaunch",
                StartTime = DateTime.UtcNow
            }).Id;
        }

        [TearDown]
        public void TearDown()
        {
            Service.DeleteLaunch(_launchId);
        }

        [Test]
        public void StartFinishDeleteTest()
        {
            var test = Service.StartTestItem(new StartTestItemRequest
                {
                    LaunchId = _launchId,
                    Name = "Test1",
                    StartTime = DateTime.UtcNow,
                    Type = TestItemType.Test
                });
            Assert.NotNull(test.Id);
            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = Status.Passed
                });
            StringAssert.Contains("successfully", message.Info);
            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartForceFinishIncompleteLaunch()
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            }, true);
        }

        [Test]
        public void StartFinishDeleteTestWithTag()
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Tags = new List<string> { "MyTag1" }
            });

            var uniqueTags = Service.GetUniqueTags(_launchId, "myta");
            CollectionAssert.Contains(uniqueTags, "MyTag1");

            Assert.NotNull(test.Id);
            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);
            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartFinishDeleteSeveralTests()
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var test2 = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test2",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test2.Id);

            var tests = Service.GetTestItems(new FilterOption
                {
                    Filters = new List<Filter>
                        {
                            new Filter(FilterOperation.Equals, "launch", _launchId)
                        }
                });
            Assert.AreEqual(tests.TestItems.Count(), 2);

            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);

            var message2 = Service.FinishTestItem(test2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message2.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void StartFinishDeleteFullTest()
        {
            var now = DateTime.UtcNow;
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Description = "Desc for test"
            });
            Assert.NotNull(test.Id);

            var getTest = Service.GetTestItem(test.Id);
            Assert.AreEqual(null, getTest.ParentId);
            Assert.AreEqual("Test1", getTest.Name);
            Assert.AreEqual(now.ToString(), getTest.StartTime.ToString());
            Assert.AreEqual(TestItemType.Test, getTest.Type);
            Assert.AreEqual("Desc for test", getTest.Description);

            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [TestCase(TestItemType.BeforeClass)]
        [TestCase(TestItemType.BeforeMethod)]
        [TestCase(TestItemType.Suite)]
        [TestCase(TestItemType.Test)]
        [TestCase(TestItemType.Step)]
        [TestCase(TestItemType.AfterMethod)]
        [TestCase(TestItemType.AfterClass)]
        public void VerifyTypeOfTests(TestItemType type)
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = type
            });
            Assert.NotNull(test.Id);
            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [TestCase(Status.Failed)]
        [TestCase(Status.Passed)]
        [TestCase(Status.Skipped)]
        public void VerifyStatusesOfTests(Status status)
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);
            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = status
            });
            StringAssert.Contains("successfully", message.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [Test]
        public void FinishTestWithIssue()
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Id);
            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                    {
                        Comment = "Comment",
                        Type = IssueType.ProductionBug
                    }
            });
            StringAssert.Contains("successfully", message.Info);
            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [TestCase(IssueType.AutomationBug)]
        [TestCase(IssueType.ProductionBug)]
        [TestCase(IssueType.SystemIssue)]
        [TestCase(IssueType.ToInvestigate)]
        [TestCase(IssueType.NoDefect)]
        public void VerifyTestIssueTypes(IssueType type)
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });
            Assert.NotNull(test.Id);
            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Skipped,
                Issue = new Issue
                {
                    Comment = "Comment",
                    Type = type
                }
            });
            StringAssert.Contains("successfully", message.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }

        [Test]
        public void CreateTestForSuites()
        {
            var suite = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite,
                Tags = new List<string> { "abc", "qwe" }
            });

            var test = Service.StartTestItem(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Id);

            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);

            var messageSuite = Service.FinishTestItem(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", messageSuite.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);

            var delMessageSuite = Service.DeleteTestItem(suite.Id);
            StringAssert.Contains("successfully", delMessageSuite.Info);
        }

        [Test]
        public void CreateStepForTestInSuite()
        {
            var suite = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test = Service.StartTestItem(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test.Id);

            Assert.IsTrue(Service.GetTestItem(suite.Id).HasChilds);

            var step = Service.StartTestItem(test.Id, new StartTestItemRequest
                {
                    LaunchId = _launchId,
                    Name = "Step1",
                    StartTime = DateTime.UtcNow,
                    Type = TestItemType.Step
                });

            Assert.NotNull(step.Id);

            var messageStep = Service.FinishTestItem(step.Id, new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = Status.Passed
                });

            StringAssert.Contains("successfully", messageStep.Info);

            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);

            var messageSuite = Service.FinishTestItem(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", messageSuite.Info);

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);

            var delMessageSuite = Service.DeleteTestItem(suite.Id);
            StringAssert.Contains("successfully", delMessageSuite.Info);
        }

        [Test]
        public void StartUpdateFinishDeleteTest()
        {
            var test = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });
            Assert.NotNull(test.Id);

            var updateMessage = Service.UpdateTestItem(test.Id, new UpdateTestItemRequest()
            {
                Description = "newDesc",
                Tags = new List<string> { "tag1", "tag2" }
            });
            StringAssert.Contains("successfully", updateMessage.Info);

            var updatedTest = Service.GetTestItem(test.Id);
            Assert.AreEqual("newDesc", updatedTest.Description);
            CollectionAssert.AreEquivalent(new List<string> { "tag1", "tag2" }, updatedTest.Tags);

            var message = Service.FinishTestItem(test.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", message.Info);
            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = Service.DeleteTestItem(test.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void AssignTestItemIssuesTest()
        {
            var suite = Service.StartTestItem(new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Suite1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Suite
            });

            var test1 = Service.StartTestItem(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test1.Id);

            var step1 = Service.StartTestItem(test1.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step1.Id);

            var messageStep1 = Service.FinishTestItem(step1.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                {
                    Comment = "Comment 1",
                    Type = IssueType.ToInvestigate
                }
            });

            StringAssert.Contains("successfully", messageStep1.Info);

            var messageTest1 = Service.FinishTestItem(test1.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", messageTest1.Info);

            var test2 = Service.StartTestItem(suite.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Test1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test
            });

            Assert.NotNull(test2.Id);

            var step2 = Service.StartTestItem(test2.Id, new StartTestItemRequest
            {
                LaunchId = _launchId,
                Name = "Step1",
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step
            });

            Assert.NotNull(step2.Id);

            var messageStep2 = Service.FinishTestItem(step2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Failed,
                Issue = new Issue
                {
                    Comment = "Comment 2",
                    Type = IssueType.AutomationBug
                }
            });

            StringAssert.Contains("successfully", messageStep2.Info);

            var messageTest2 = Service.FinishTestItem(test2.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", messageTest2.Info);

            var messageSuite = Service.FinishTestItem(suite.Id, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = Status.Passed
            });
            StringAssert.Contains("successfully", messageSuite.Info);

            var issue1 = new Issue
            {
                Comment = "New Comment 1",
                Type = IssueType.ProductionBug
            };

            var issue2 = new Issue
            {
                Comment = "New Comment 2",
                Type = IssueType.SystemIssue,
                ExternalSystemIssues = new List<ExternalSystemIssue>
                {
                    new ExternalSystemIssue { TicketId = "XXXXX-15", Url = "https://jira.epam.com/jira/browse/XXXXX-15" }
                }
            };

            var assignedIssues = Service.AssignTestItemIssues(new AssignTestItemIssuesRequest
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

            Assert.That(assignedIssues.Count(), Is.EqualTo(2));

            Assert.That(assignedIssues.First().Comment, Is.EqualTo(issue1.Comment));
            Assert.That(assignedIssues.First().Type, Is.EqualTo(issue1.Type));
            Assert.That(assignedIssues.First().ExternalSystemIssues, Is.Null);

            Assert.That(assignedIssues.ElementAt(1).Comment, Is.EqualTo(issue2.Comment));
            Assert.That(assignedIssues.ElementAt(1).Type, Is.EqualTo(issue2.Type));
            Assert.That(assignedIssues.ElementAt(1).ExternalSystemIssues, Is.Not.Null);

            Assert.That(assignedIssues.ElementAt(1).ExternalSystemIssues.Count(), Is.EqualTo(1));
            Assert.That(assignedIssues.ElementAt(1).ExternalSystemIssues.First().SubmitDate, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(5)));
            Assert.That(assignedIssues.ElementAt(1).ExternalSystemIssues.First().Submitter, Is.EqualTo(Username));
            Assert.That(assignedIssues.ElementAt(1).ExternalSystemIssues.First().TicketId, Is.EqualTo(issue2.ExternalSystemIssues.First().TicketId));
            Assert.That(assignedIssues.ElementAt(1).ExternalSystemIssues.First().Url, Is.EqualTo(issue2.ExternalSystemIssues.First().Url));

            step1 = Service.GetTestItem(step1.Id);
            Assert.NotNull(step1.Issue);

            step2 = Service.GetTestItem(step2.Id);
            Assert.NotNull(step2.Issue);

            Assert.That(step1.Issue.Comment, Is.EqualTo(issue1.Comment));
            Assert.That(step1.Issue.Type, Is.EqualTo(issue1.Type));
            Assert.That(step1.Issue.ExternalSystemIssues, Is.Null);

            Assert.That(step2.Issue.Comment, Is.EqualTo(issue2.Comment));
            Assert.That(step2.Issue.Type, Is.EqualTo(issue2.Type));
            Assert.That(step2.Issue.ExternalSystemIssues, Is.Not.Null);

            Assert.That(step2.Issue.ExternalSystemIssues.Count(), Is.EqualTo(1));
            Assert.That(step2.Issue.ExternalSystemIssues.First().SubmitDate, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(5)));
            Assert.That(step2.Issue.ExternalSystemIssues.First().Submitter, Is.EqualTo(Username));
            Assert.That(step2.Issue.ExternalSystemIssues.First().TicketId, Is.EqualTo(issue2.ExternalSystemIssues.First().TicketId));
            Assert.That(step2.Issue.ExternalSystemIssues.First().Url, Is.EqualTo(issue2.ExternalSystemIssues.First().Url));

            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
            var delMessage = Service.DeleteTestItem(test1.Id);
            StringAssert.Contains("successfully", delMessage.Info);
        }

        [Test]
        public void GetTestItemHistory()
        {
            var histories = Service.GetTestItemHistory("5472e38ee4b098dbedf8e860", 5, true);
            Assert.That(histories.Count, Is.EqualTo(5));
            Service.FinishLaunch(_launchId, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });
        }
    }
}