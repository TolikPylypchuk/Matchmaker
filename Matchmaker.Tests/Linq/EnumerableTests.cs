namespace Matchmaker.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using FsCheck;

using Xunit;

public class EnumerableTests
{
    [Fact]
    public Property EnumerateShouldForceEnumeration()
    {
        const int count = 100;

        int result = 0;

        Enumerable.Range(0, count)
            .Select(_ => result++)
            .Enumerate();

        return (result == count).ToProperty();
    }

    [Fact]
    public void EnumerateShouldThrowIfEnumerableIsNull()
    {
        var action = () => ((IEnumerable<object>)null).Enumerate();
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public Property EnumerateAsyncShouldForceEnumeration()
    {
        const int count = 100;

        int result = 0;

        AsyncEnumerable.Range(0, count)
            .Select(_ => result++)
            .EnumerateAsync()
            .Wait();

        return (result == count).ToProperty();
    }

    [Fact]
    public void EnumerateAsyncShouldThrowIfEnumerableIsNull()
    {
        var action = () =>
        {
            _ = ((IAsyncEnumerable<object>)null).EnumerateAsync();
        };

        action.Should().Throw<ArgumentNullException>();
    }
}
