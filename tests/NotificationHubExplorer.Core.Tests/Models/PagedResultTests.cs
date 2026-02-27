using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Core.Tests.Models;

[TestClass]
public class PagedResultTests
{
    [TestMethod]
    public void HasMore_Should_ReturnFalse_When_ContinuationTokenIsNull()
    {
        var result = new PagedResult<string>
        {
            Items = new[] { "item1" },
            ContinuationToken = null
        };

        result.HasMore.Should().BeFalse();
    }

    [TestMethod]
    public void HasMore_Should_ReturnTrue_When_ContinuationTokenIsSet()
    {
        var result = new PagedResult<string>
        {
            Items = new[] { "item1" },
            ContinuationToken = "token123"
        };

        result.HasMore.Should().BeTrue();
    }

    [TestMethod]
    public void Empty_Should_ReturnPagedResultWithNoItems_And_NoContinuationToken()
    {
        var result = PagedResult<string>.Empty();

        result.Items.Should().BeEmpty();
        result.ContinuationToken.Should().BeNull();
        result.HasMore.Should().BeFalse();
    }

    [TestMethod]
    public void Items_Should_ContainAllProvidedItems()
    {
        var items = new[] { "a", "b", "c" };
        var result = new PagedResult<string>
        {
            Items = items
        };

        result.Items.Should().BeEquivalentTo(items);
    }
}
