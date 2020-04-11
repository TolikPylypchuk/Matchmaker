using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Execution;

using FsCheck;
using FsCheck.Xunit;

using Xunit;

namespace Matchmaker.Patterns
{
    public class EqualToTests
    {
        private static readonly IEqualityComparer<string> StringEqualityComparer = EqualityComparer<string>.Default;

        [Property]
        public Property EqualToShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.EqualTo(y).Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property LazyEqualToShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (Equals(x, y) == Pattern.EqualTo(() => y).Match(x).IsSuccessful).ToProperty();

        [Property]
        public Property EqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (StringEqualityComparer.Equals(x, y) ==
                    Pattern.EqualTo(y, StringEqualityComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property LazyEqualToWithComparerShouldSucceedOnlyOnEqualObjects(string x, string y)
            => (StringEqualityComparer.Equals(x, y) ==
                    Pattern.EqualTo(() => y, StringEqualityComparer).Match(x).IsSuccessful)
                .ToProperty();

        [Property]
        public Property EqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(string x, string y, string description)
        {
            Func<bool> equalToSucceedsOnlyOnEqualObjects = () =>
                Equals(x, y) == Pattern.EqualTo(y, description).Match(x).IsSuccessful;
            return equalToSucceedsOnlyOnEqualObjects.When(description != null);
        }

        [Property]
        public Property LazyEqualToWithDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            string y,
            string description)
        {
            Func<bool> equalToSucceedsOnlyOnEqualObjects = () =>
                Equals(x, y) == Pattern.EqualTo(() => y, description).Match(x).IsSuccessful;
            return equalToSucceedsOnlyOnEqualObjects.When(description != null);
        }

        [Property]
        public Property EqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            string y,
            string description)
        {
            Func<bool> equalToSucceedsOnlyOnEqualObjects = () =>
                StringEqualityComparer.Equals(x, y) ==
                Pattern.EqualTo(y, StringEqualityComparer, description).Match(x).IsSuccessful;
            return equalToSucceedsOnlyOnEqualObjects.When(description != null);
        }

        [Property]
        public Property LazyEqualToWithComparerAndDescriptionShouldSucceedOnlyOnEqualObjects(
            string x,
            string y,
            string description)
        {
            Func<bool> equalToSucceedsOnlyOnEqualObjects = () =>
                StringEqualityComparer.Equals(x, y) ==
                Pattern.EqualTo(() => y, StringEqualityComparer, description).Match(x).IsSuccessful;
            return equalToSucceedsOnlyOnEqualObjects.When(description != null);
        }

        [Property]
        public Property EqualToShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.EqualTo(x).Description == String.Format(Pattern.DefaultEqualToDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyEqualToShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.EqualTo(() => x).Description == Pattern.DefaultLazyEqualToDescription).ToProperty();

        [Property]
        public Property EqualToWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.EqualTo(x, StringEqualityComparer).Description ==
                String.Format(Pattern.DefaultEqualToDescriptionFormat, x))
                .ToProperty();

        [Property]
        public Property LazyEqualToWithComparerShouldHaveCorrectDefaultDescription(string x)
            => (Pattern.EqualTo(() => x, StringEqualityComparer).Description == Pattern.DefaultLazyEqualToDescription)
                .ToProperty();

        [Property]
        public Property EqualToShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> equalToHasCorrectDefaultDescription = () =>
                Pattern.EqualTo(x, description).Description == description;
            return equalToHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyEqualToShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> equalToHasCorrectDefaultDescription = () =>
                Pattern.EqualTo(() => x, description).Description == description;
            return equalToHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property EqualToWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> equalToHasCorrectDefaultDescription = () =>
                Pattern.EqualTo(x, StringEqualityComparer, description).Description == description;
            return equalToHasCorrectDefaultDescription.When(description != null);
        }

        [Property]
        public Property LazyEqualToWithComparerShouldHaveSpecifiedDescription(string x, string description)
        {
            Func<bool> equalToHasCorrectDefaultDescription = () =>
                Pattern.EqualTo(() => x, StringEqualityComparer, description).Description == description;
            return equalToHasCorrectDefaultDescription.When(description != null);
        }

        [Fact]
        public void LazyEqualToShouldBeLazy()
        {
            Action action = () => Pattern.EqualTo<string>(
                () => throw new AssertionFailedException("Lazy EqualTo is not lazy"));
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Fact]
        public void LazyEqualToWithComparerShouldBeLazy()
        {
            Action action = () => Pattern.EqualTo(
                () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
                StringEqualityComparer);
            action.Should().NotThrow<AssertionFailedException>();
        }

        [Property]
        public void LazyEqualToWithDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.EqualTo<string>(
                    () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void LazyEqualToWithComparerAndDescriptionShouldBeLazy(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.EqualTo(
                    () => throw new AssertionFailedException("Lazy EqualTo is not lazy"),
                    StringEqualityComparer,
                    description);
                action.Should().NotThrow<AssertionFailedException>();
            }
        }

        [Property]
        public void EqualToShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.EqualTo(x, (IEqualityComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyEqualToShouldThrowIfComparerIsNull(string x)
        {
            Action action = () => Pattern.EqualTo(() => x, (IEqualityComparer<string>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void EqualToShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.EqualTo(x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyEqualToShouldThrowIfDescriptionIsNull(string x)
        {
            Action action = () => Pattern.EqualTo(() => x, (string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void EqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.EqualTo(x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyEqualToShouldThrowIfComparerIsNullAndDescriptionIsNotNull(string x, string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.EqualTo(() => x, null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void EqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.EqualTo(x, StringEqualityComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyEqualToShouldThrowIfComparerIsNotNullAndDescriptionIsNull(string x)
        {
            Action action = () => Pattern.EqualTo(() => x, StringEqualityComparer, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyEqualToShouldThrowIfValueProviderIsNull()
        {
            Action action = () => Pattern.EqualTo((Func<int>)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerIsNotNull()
        {
            Action action = () => Pattern.EqualTo((Func<string>)null, StringEqualityComparer);
            action.Should().Throw<ArgumentNullException>();
        }

        [Property]
        public void LazyEqualToShouldThrowIfValueProviderIsNullAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.EqualTo((Func<string>)null, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [Property]
        public void LazyEqualToShouldThrowIfValueProviderIsNullAndComparerAndDescriptionIsNotNull(string description)
        {
            if (description != null)
            {
                Action action = () => Pattern.EqualTo((Func<string>)null, StringEqualityComparer, description);
                action.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
