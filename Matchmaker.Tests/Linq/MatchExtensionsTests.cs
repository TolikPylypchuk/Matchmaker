using System;
using System.Linq;

using FsCheck;
using FsCheck.Xunit;

namespace Matchmaker.Linq
{
    public class MatchExtensionsTests
    {
        [Property]
        public Property EnumerableEnumerateForcesEnumeration(int count)
        {
            count = Math.Abs(count) % 100;

            int result = 0;

            Enumerable.Range(0, count)
                .Select(_ => result++)
                .Enumerate();

            return (result == count).ToProperty();
        }
    }
}
