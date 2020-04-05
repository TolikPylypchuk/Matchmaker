using System;
using System.Collections.Immutable;
using System.Linq;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a general non-transforming pattern.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="PatternBase{TInput, TMatchResult, TPattern}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern" />
    public sealed class SimplePattern<TInput> : PatternBase<TInput, TInput, SimplePattern<TInput>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePattern{TInput}" /> class
        /// with the specified condition.
        /// </summary>
        /// <param name="condition">The condition of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition" /> is <see langword="null" />.
        /// </exception>
        public SimplePattern(Func<TInput, bool> condition)
            : base(ImmutableList<Func<TInput, bool>>.Empty.Add(
                condition ?? throw new ArgumentNullException(nameof(condition))))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePattern{TInput}" /> class
        /// with the specified condition.
        /// </summary>
        /// <param name="condition">The condition of this pattern.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        public SimplePattern(Func<TInput, bool> condition, string description)
            : base(
                ImmutableList<Func<TInput, bool>>.Empty.Add(
                    condition ?? throw new ArgumentNullException(nameof(condition))),
                description)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePattern{TInput}" /> class
        /// with the specified conditions.
        /// </summary>
        /// <param name="conditions">The conditions of this pattern.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="conditions" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        private SimplePattern(IImmutableList<Func<TInput, bool>> conditions, string description)
            : base(conditions, description)
        { }

        /// <summary>
        /// Matches the input with this pattern, and returns the input value if successful.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the transformed result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override MatchResult<TInput> Match(TInput input)
            => this.Conditions.All(condition => condition(input))
                ? MatchResult.Success(input)
                : MatchResult.Failure<TInput>();

        /// <summary>
        /// Returns a new pattern which includes the specified condition.
        /// </summary>
        /// <param name="condition">The condition to add.</param>
        /// <returns>A new pattern which includes the specified condition.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition" /> is <see langword="null" />.
        /// </exception>
        public override SimplePattern<TInput> When(Func<TInput, bool> condition)
            => condition != null
                ? new SimplePattern<TInput>(this.Conditions.Add(condition), this.Description)
                : throw new ArgumentNullException(nameof(condition));

        /// <summary>
        /// Returns a pattern which is matched successfully only if both
        /// this and other pattern are matched successfully.
        /// </summary>
        /// <param name="other">The other pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully only if both
        /// this and other pattern are matched successfully.
        /// </returns>
        /// <remarks>This is a short-circuiting operation.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.
        /// </exception>
        public SimplePattern<TInput> And(SimplePattern<TInput> other)
            => other != null
                ? new SimplePattern<TInput>(this.Conditions.AddRange(other.Conditions), this.Description)
                : throw new ArgumentNullException(nameof(other));

        /// <summary>
        /// Returns a pattern which is matched successfully if
        /// either this or the other pattern is matched successfully.
        /// </summary>
        /// <param name="other">The other pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully if
        /// either this or the other pattern is matched successfully.
        /// </returns>
        /// <remarks>This is a short-circuiting operation.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.
        /// </exception>
        public SimplePattern<TInput> Or(SimplePattern<TInput> other)
            => other != null
                ? new SimplePattern<TInput>(input => this.Match(input).IsSuccessful || other.Match(input).IsSuccessful)
                : throw new ArgumentNullException(nameof(other));

        /// <summary>
        /// Returns a pattern which is matched successfully if
        /// matches with this pattern and the other pattern yield different results.
        /// </summary>
        /// <param name="other">The other pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully if
        /// matches with this pattern and the other pattern yield different results.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.
        /// </exception>
        public SimplePattern<TInput> Xor(SimplePattern<TInput> other)
            => other != null
                ? new SimplePattern<TInput>(input => this.Match(input).IsSuccessful ^ other.Match(input).IsSuccessful)
                : throw new ArgumentNullException(nameof(other));

        /// <summary>
        /// Returns a pattern which is matched successfully only if both
        /// the first pattern and the second pattern are matched successfully.
        /// </summary>
        /// <param name="pattern1">The first pattern.</param>
        /// <param name="pattern2">The second pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully only if both
        /// the first pattern and the second pattern are matched successfully.
        /// </returns>
        /// <remarks>This is a short-circuiting operator.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern1" /> or <paramref name="pattern2" /> is <see langword="null" />.
        /// </exception>
        public static SimplePattern<TInput> operator &(SimplePattern<TInput> pattern1, SimplePattern<TInput> pattern2)
            => pattern1 != null
                ? pattern2 != null
                    ? pattern1.And(pattern2)
                    : throw new ArgumentNullException(nameof(pattern2))
                : throw new ArgumentNullException(nameof(pattern1));

        /// <summary>
        /// Returns a pattern which is matched successfully if
        /// either the first or the second pattern is matched successfully.
        /// </summary>
        /// <param name="pattern1">The first pattern.</param>
        /// <param name="pattern2">The second pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully if
        /// either the first or the second pattern is matched successfully.
        /// </returns>
        /// <remarks>This is a short-circuiting operator.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern1" /> or <paramref name="pattern2" /> is <see langword="null" />.
        /// </exception>
        public static SimplePattern<TInput> operator |(SimplePattern<TInput> pattern1, SimplePattern<TInput> pattern2)
            => pattern1 != null
                ? pattern2 != null
                    ? pattern1.Or(pattern2)
                    : throw new ArgumentNullException(nameof(pattern2))
                : throw new ArgumentNullException(nameof(pattern1));

        /// <summary>
        /// Returns a pattern which is matched successfully if
        /// matches with the first pattern and the second pattern yield different results.
        /// </summary>
        /// <param name="pattern1">The first pattern.</param>
        /// <param name="pattern2">The second pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully if
        /// matches with the first pattern and the second pattern yield different results.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern1" /> or <paramref name="pattern2" /> is <see langword="null" />.
        /// </exception>
        public static SimplePattern<TInput> operator ^(SimplePattern<TInput> pattern1, SimplePattern<TInput> pattern2)
            => pattern1 != null
                ? pattern2 != null
                    ? pattern1.Xor(pattern2)
                    : throw new ArgumentNullException(nameof(pattern2))
                : throw new ArgumentNullException(nameof(pattern1));

        /// <summary>
        /// Returns a pattern which is matched successfully
        /// when the specified pattern is not matched successfully.
        /// </summary>
        /// <param name="pattern">The pattern to invert.</param>
        /// <returns>
        /// A pattern which is matched successfully
        /// when the specified pattern is not matched successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> is <see langword="null" />.
        /// </exception>
        public static SimplePattern<TInput> operator ~(SimplePattern<TInput> pattern)
            => Pattern.Not(pattern);
    }
}
