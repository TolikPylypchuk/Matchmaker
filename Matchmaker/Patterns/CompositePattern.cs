using System;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represents a pattern which is composed of two other patterns.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the input value of the expression and also the type of the result of this pattern's match.
    /// </typeparam>
    internal sealed class CompositePattern<T> : Pattern<T, T>
    {
        /// <summary>
        /// The left pattern.
        /// </summary>
        private readonly IPattern<T, T> leftPattern;

        /// <summary>
        /// The right pattern.
        /// </summary>
        private readonly IPattern<T, T> rightPattern;

        /// <summary>
        /// The composition which should be applied to the patterns.
        /// </summary>
        private readonly PatternComposition composition;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePattern{T}" /> class.
        /// </summary>
        /// <param name="leftPattern">The left pattern.</param>
        /// <param name="rightPattern">The right pattern.</param>
        /// <param name="composition">The composition which should be applied to the patterns.</param>
        internal CompositePattern(IPattern<T, T> leftPattern, IPattern<T, T> rightPattern, PatternComposition composition)
            : this(
                leftPattern,
                rightPattern,
                composition,
                leftPattern.Description.Length > 0 && rightPattern.Description.Length > 0
                    ? CreateDescription(leftPattern.Description, rightPattern.Description, composition)
                    : String.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePattern{T}" /> class.
        /// </summary>
        /// <param name="leftPattern">The left pattern.</param>
        /// <param name="rightPattern">The right pattern.</param>
        /// <param name="composition">The composition which should be applied to the patterns.</param>
        /// <param name="description">The description of this pattern.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        internal CompositePattern(
            IPattern<T, T> leftPattern,
            IPattern<T, T> rightPattern,
            PatternComposition composition,
            string description)
            : base(description)
        {
            this.leftPattern = leftPattern;
            this.rightPattern = rightPattern;
            this.composition = composition;
        }

        /// <summary>
        /// Matches the input with this pattern, and returns a result.
        /// </summary>
        /// <param name="input">The input value to match.</param>
        /// <returns>
        /// A successful match result which contains the result of the match,
        /// if this match is successful. Otherwise, a failed match result.
        /// </returns>
        public override MatchResult<T> Match(T input)
            => this.IsCompositionSuccessful(input)
                ? MatchResult.Success(input)
                : MatchResult.Failure<T>();

        /// <summary>
        /// Determines whether the composition of the patterns yields a successful result.
        /// </summary>
        /// <param name="input">The input for the patterns to match.</param>
        /// <returns>
        /// <see langword="true" /> if the composition is successful. Otherwise, <see langword="false" />.
        /// </returns>
        private bool IsCompositionSuccessful(T input)
            => composition switch
            {
                PatternComposition.And =>
                    this.leftPattern.Match(input).IsSuccessful && this.rightPattern.Match(input).IsSuccessful,
                PatternComposition.Or =>
                    this.leftPattern.Match(input).IsSuccessful || this.rightPattern.Match(input).IsSuccessful,
                PatternComposition.Xor =>
                    this.leftPattern.Match(input).IsSuccessful ^ this.rightPattern.Match(input).IsSuccessful,
                _ => false
            };

        private static string CreateDescription(
            string leftDescription,
            string rightDescription,
            PatternComposition composition)
        {
            string format = composition switch
            {
                PatternComposition.And => Pattern.DefaultAndDescriptionFormat,
                PatternComposition.Or => Pattern.DefaultOrDescriptionFormat,
                PatternComposition.Xor => Pattern.DefaultXorDescriptionFormat,
                _ => String.Empty
            };

            return String.Format(format, leftDescription, rightDescription);
        }
    }
}
