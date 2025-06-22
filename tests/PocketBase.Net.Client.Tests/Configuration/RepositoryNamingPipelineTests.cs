using NSubstitute;
using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities.Records;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration;

public class RepositoryNamingPipelineTests
{
    public record FamousAuthorRecord : RecordBase { }

    [Fact(DisplayName = "Should return the default collection name")]
    public void GetCollectionNameOf_ShouldReturnDefaultName()
    {
        // Arrange
        var namingPipeline = new RepositoryNamingPipeline();

        // Act
        var result = namingPipeline.GetCollectionNameOf<FamousAuthorRecord>();

        // Assert
        result.ShouldBe("famousAuthors");
    }

    [Fact(DisplayName = "Should remove all rules when replacing with empty set")]
    public void ReplaceRulesWith_ShouldRemoveAllRulesWhenEmptySet()
    {
        // Arrange
        var pipeline = new RepositoryNamingPipeline();

        // Act
        var result = pipeline
            .ReplaceRulesWith([])
            .GetCollectionNameOf<FamousAuthorRecord>();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact(DisplayName = "Should append a single rule")]
    public void AppendRule_ShouldAddSingleRuleToPipeline()
    {
        // Arrange
        var pipeline = new RepositoryNamingPipeline();

        var mockRule = Substitute.For<ICollectionNamingRule>();
        mockRule.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_mock");

        // Act
        var result = pipeline
            .AppendRule(mockRule)
            .GetCollectionNameOf<FamousAuthorRecord>();

        // Assert
        mockRule
            .Received(1)
            .BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
    }

    [Fact(DisplayName = "Should not recompute the name once generated")]
    public void AppendRule_ShouldNotRecomputeNameOnceGenerated()
    {
        // Arrange
        var pipeline = new RepositoryNamingPipeline();

        var mockRule = Substitute.For<ICollectionNamingRule>();
        mockRule.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_mock");

        // Act
        pipeline.AppendRule(mockRule);

        pipeline.GetCollectionNameOf<FamousAuthorRecord>();
        pipeline.GetCollectionNameOf<FamousAuthorRecord>();

        // Assert
        mockRule
            .Received(1)
            .BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
    }

    [Fact(DisplayName = "Should append multiple rules in order")]
    public void AppendRules_ShouldAddMultipleRulesToPipeline()
    {
        // Arrange
        var pipeline = new RepositoryNamingPipeline();

        var mockRule1 = Substitute.For<ICollectionNamingRule>();
        mockRule1.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_mock1");

        var mockRule2 = Substitute.For<ICollectionNamingRule>();
        mockRule2.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_mock2");

        // Act
        pipeline
            .AppendRules(mockRule1, mockRule2)
            .GetCollectionNameOf<FamousAuthorRecord>();

        // Assert
        Received.InOrder(() =>
        {
            mockRule1.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
            mockRule2.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
        });
    }

    [Fact(DisplayName = "Should call rules in order when generating name")]
    public void GetCollectionNameOf_ShouldCallRulesInOrder()
    {
        // Arrange
        var pipeline = new RepositoryNamingPipeline();

        var mockRule1 = Substitute.For<ICollectionNamingRule>();
        mockRule1.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_1");

        var mockRule2 = Substitute.For<ICollectionNamingRule>();
        mockRule2.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_2");

        var mockRule3 = Substitute.For<ICollectionNamingRule>();
        mockRule3.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>())
            .Returns(callInfo => callInfo.Arg<string>() + "_3");

        pipeline.ReplaceRulesWith([mockRule1, mockRule2, mockRule3]);

        // Act
        var result = pipeline.GetCollectionNameOf<FamousAuthorRecord>();

        // Assert
        Received.InOrder(() =>
        {
            mockRule1.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
            mockRule2.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
            mockRule3.BuildNameFor<FamousAuthorRecord>(Arg.Any<string>());
        });

        result.ShouldEndWith("_1_2_3");
    }
}
