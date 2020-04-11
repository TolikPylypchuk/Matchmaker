using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Linq
{
    public class EnumerableTests
    {
        [Property]
        public Property EnumerateForcesEnumeration(int count)
        {
            count = Math.Abs(count) % 100;

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
