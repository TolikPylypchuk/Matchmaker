using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions;

using FsCheck;

using Xunit;

namespace Matchmaker.Linq
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class EnumerableTests
    {
        [Fact]
        public Property EnumerateForcesEnumeration()
        {
            const int count = 100;

            int result = 0;

            Enumerable.Range(0, count)
                .Select(_ => result++)
                .Enumerate();

            return (result == count).ToProperty();
        }

        [Fact]
        public void EnumerateThrowsIfEnumerableIsNull()
        {
            Action action = () => ((IEnumerable<object>)null).Enumerate();
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
