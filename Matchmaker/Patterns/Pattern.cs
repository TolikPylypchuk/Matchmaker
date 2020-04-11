using System;
using System.Collections.Generic;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Contains factory methods for creating patterns.
    /// </summary>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    public static class Pattern
    {
        /// <summary>
        /// The default description of the 'any' pattern.
        /// </summary>
        /// <seealso cref="Any{TInput}()" />
        public static readonly string DefaultAnyDescription = "any x";

        /// <summary>
        /// The default description of the 'return' pattern.
        /// </summary>
        /// <seealso cref="Return{TInput, TValue}(TValue)" />
        public static readonly string DefaultReturnDescriptionFormat = "return {0}";

        /// <summary>
        /// The default description of the lazy 'return' pattern.
        /// </summary>
        /// <seealso cref="Return{TInput, TValue}(Func{TValue})" />
        public static readonly string DefaultLazyReturnDescription = "return <provided value>";

        /// <summary>
        /// The default description of 'null' patterns.
        /// </summary>
        /// <seealso cref="Null{TInput}()" />
        /// <seealso cref="ValueNull{TInput}()" />
        public static readonly string DefaultNullDescription = "x is null";

        /// <summary>
        /// The default description of equality patterns.
        /// </summary>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        public static readonly string DefaultEqualToDescriptionFormat = "x = {0}";

        /// <summary>
        /// The default description of lazy equality patterns.
        /// </summary>
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        public static readonly string DefaultLazyEqualToDescription = "x = <provided value>";

        /// <summary>
        /// The default description of less-than patterns.
        /// </summary>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        public static readonly string DefaultLessThanDescriptionFormat = "x < {0}";

        /// <summary>
        /// The default description of lazy less-than patterns.
        /// </summary>
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        public static readonly string DefaultLazyLessThanDescription = "x < <provided value>";

        /// <summary>
        /// The default description of less-or-equal patterns.
        /// </summary>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        public static readonly string DefaultLessOrEqualDescriptionFormat = "x <= {0}";

        /// <summary>
        /// The default description of lazy less-or-equal patterns.
        /// </summary>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        public static readonly string DefaultLazyLessOrEqualDescription = "x <= <provided value>";

        /// <summary>
        /// The default description of greater-than patterns.
        /// </summary>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        public static readonly string DefaultGreaterThanDescriptionFormat = "x > {0}";

        /// <summary>
        /// The default description of lazy greater-than patterns.
        /// </summary>
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        public static readonly string DefaultLazyGreaterThanDescription = "x > <provided value>";

        /// <summary>
        /// The default description of greater-or-equal patterns.
        /// </summary>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        public static readonly string DefaultGreaterOrEqualDescriptionFormat = "x >= {0}";

        /// <summary>
        /// The default description of lazy greater-or-equal patterns.
        /// </summary>
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        public static readonly string DefaultLazyGreaterOrEqualDescription = "x >= <provided value>";

        /// <summary>
        /// The default description of type patterns.
        /// </summary>
        /// <seealso cref="Type{TInput, TType}()" />
        public static readonly string DefaultTypeDescriptionFormat = "x is {0}";

        /// <summary>
        /// The default description of piping patterns.
        /// </summary>
        public static readonly string DefaultPipeDescriptionFormat = "({0}) = ({1})";

        /// <summary>
        /// The default description of the 'and' pattern combinator.
        /// </summary>
        public static readonly string DefaultAndDescriptionFormat = "({0}) and ({1})";

        /// <summary>
        /// The default description of the 'or' pattern combinator.
        /// </summary>
        public static readonly string DefaultOrDescriptionFormat = "({0}) or ({1})";

        /// <summary>
        /// The default description of the 'xor' pattern combinator.
        /// </summary>
        public static readonly string DefaultXorDescriptionFormat = "({0}) xor ({1})";

        /// <summary>
        /// The default description of the 'not' pattern combinators.
        /// </summary>
        /// <seealso cref="Not{TInput, TMatchResult}(IPattern{TInput, TMatchResult})" />
        public static readonly string DefaultNotDescriptionFormat = "not ({0})";

        /// <summary>
        /// Creates a pattern which uses a specified function to match its inputs.
        /// </summary>
        /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
        /// <typeparam name="TMatchResult">The type of the pattern's results.</typeparam>
        /// <param name="matcher">The function which matches the inputs.</param>
        /// <returns>
        /// A pattern which matches its inputs according to the specified matcher function.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matcher" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="CreatePattern{TInput, TMatchResult}(Func{TInput, MatchResult{TMatchResult}}, string)" />
        /// <seealso cref="CreatePattern{TInput}(Func{TInput, bool})" />
        /// <seealso cref="CreatePattern{TInput}(Func{TInput, bool}, string)" />
        public static IPattern<TInput, TMatchResult> CreatePattern<TInput, TMatchResult>(
            Func<TInput, MatchResult<TMatchResult>> matcher)
            => new SimplePattern<TInput, TMatchResult>(matcher ?? throw new ArgumentNullException(nameof(matcher)));

        /// <summary>
        /// Creates a pattern which uses a specified function to match its inputs and has a specified description.
        /// </summary>
        /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
        /// <typeparam name="TMatchResult">The type of the pattern's results.</typeparam>
        /// <param name="matcher">The function which matches the inputs.</param>
        /// <param name="description">The pattern's description.</param>
        /// <returns>
        /// A pattern which matches its inputs according to the specified matcher function.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matcher" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="CreatePattern{TInput, TMatchResult}(Func{TInput, MatchResult{TMatchResult}})" />
        /// <seealso cref="CreatePattern{TInput}(Func{TInput, bool})" />
        /// <seealso cref="CreatePattern{TInput}(Func{TInput, bool}, string)" />
        public static IPattern<TInput, TMatchResult> CreatePattern<TInput, TMatchResult>(
            Func<TInput, MatchResult<TMatchResult>> matcher,
            string description)
            => new SimplePattern<TInput, TMatchResult>(
                matcher ?? throw new ArgumentNullException(nameof(matcher)),
                description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Creates a pattern which uses a specified predicate to match its inputs.
        /// </summary>
        /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
        /// <param name="predicate">The predicate which matches the inputs.</param>
        /// <returns>
        /// A pattern which matches its inputs according to the specified predicate.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="CreatePattern{TInput, TMatchResult}(Func{TInput, MatchResult{TMatchResult}})" />
        /// <seealso cref="CreatePattern{TInput, TMatchResult}(Func{TInput, MatchResult{TMatchResult}}, string)" />
        /// <seealso cref="CreatePattern{TInput}(Func{TInput, bool}, string)" />
        public static IPattern<TInput, TInput> CreatePattern<TInput>(Func<TInput, bool> predicate)
            => predicate != null
                ? new SimplePattern<TInput, TInput>(
                    input => predicate(input)
                        ? MatchResult.Success(input)
                        : MatchResult.Failure<TInput>())
                : throw new ArgumentNullException(nameof(predicate));

        /// <summary>
        /// Creates a pattern which uses a specified predicate to match its inputs and has a specified description.
        /// </summary>
        /// <typeparam name="TInput">The type of the pattern's inputs.</typeparam>
        /// <param name="predicate">The predicate which matches the inputs.</param>
        /// <param name="description">The pattern's description.</param>
        /// <returns>
        /// A pattern which matches its inputs according to the specified predicate.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="CreatePattern{TInput, TMatchResult}(Func{TInput, MatchResult{TMatchResult}})" />
        /// <seealso cref="CreatePattern{TInput, TMatchResult}(Func{TInput, MatchResult{TMatchResult}}, string)" />
        /// <seealso cref="CreatePattern{TInput}(Func{TInput, bool})" />
        public static IPattern<TInput, TInput> CreatePattern<TInput>(
            Func<TInput, bool> predicate,
            string description)
            => predicate != null
                ? new SimplePattern<TInput, TInput>(
                    input => predicate(input)
                        ? MatchResult.Success(input)
                        : MatchResult.Failure<TInput>(),
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(predicate));

        /// <summary>
        /// Returns a pattern which is always matched successfully.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is always matched successfully.</returns>
        /// <remarks>
        /// This pattern should be used as the default case of a match expression, if one is needed.
        /// </remarks>
        public static IPattern<TInput, TInput> Any<TInput>()
            => Any<TInput>(DefaultAnyDescription);

        /// <summary>
        /// Returns a pattern which is always matched successfully.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>A pattern which is always matched successfully.</returns>
        /// <remarks>
        /// This pattern should be used as the default case of a match expression, if one is needed.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        public static IPattern<TInput, TInput> Any<TInput>(string description)
            => CreatePattern<TInput>(_ => true, description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Returns a pattern which always successfully returns the specified value, discarding its input value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TValue">The type of the value to return.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A pattern which always successfully returns the specified value.</returns>
        /// <remarks>
        /// This pattern is much like the <see cref="Any{TInput}()" /> pattern,
        /// except it returns the specified value instead of the pattern's input.
        /// </remarks>
        public static IPattern<TInput, TValue> Return<TInput, TValue>(TValue value)
            => Return<TInput, TValue>(value, String.Format(DefaultReturnDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which always successfully returns the specified value, discarding its input value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TValue">The type of the value to return.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>A pattern which always successfully returns the specified value.</returns>
        /// <remarks>
        /// This pattern is much like the <see cref="Any{TInput}(string)" /> pattern,
        /// except it returns the specified value instead of the pattern's input.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        public static IPattern<TInput, TValue> Return<TInput, TValue>(TValue value, string description)
            => CreatePattern<TInput, TValue>(
                _ => MatchResult.Success(value),
                description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Returns a pattern which always successfully returns the provided value, discarding its input value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TValue">The type of the value to return.</typeparam>
        /// <param name="valueProvider">The provider of the value to return.</param>
        /// <returns>A pattern which always successfully returns the provided value.</returns>
        /// <remarks>
        /// This pattern is much like the <see cref="Any{TInput}()" /> pattern,
        /// except it returns the provided value instead of the pattern's input.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider"/> is <see langword="null" />.
        /// </exception>
        public static IPattern<TInput, TValue> Return<TInput, TValue>(Func<TValue> valueProvider)
            => Return<TInput, TValue>(valueProvider, DefaultLazyReturnDescription);

        /// <summary>
        /// Returns a pattern which always successfully returns the provided value, discarding its input value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TValue">The type of the value to return.</typeparam>
        /// <param name="valueProvider">The provider of the value to return.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>A pattern which always successfully returns the provided value.</returns>
        /// <remarks>
        /// This pattern is much like the <see cref="Any{TInput}(string)" /> pattern,
        /// except it returns the provided value instead of the pattern's input.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        public static IPattern<TInput, TValue> Return<TInput, TValue>(Func<TValue> valueProvider, string description)
            => CreatePattern<TInput, TValue>(
                _ => MatchResult.Success(valueProvider()),
                description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <seealso cref="Null{TInput}(string)" />
        /// <seealso cref="ValueNull{TInput}()" />
        /// <seealso cref="ValueNull{TInput}(string)" />
        public static IPattern<TInput, TInput> Null<TInput>()
            where TInput : class
            => Null<TInput>(DefaultNullDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="Null{TInput}()" />
        /// <seealso cref="ValueNull{TInput}()" />
        /// <seealso cref="ValueNull{TInput}(string)" />
        public static IPattern<TInput, TInput> Null<TInput>(string description)
            where TInput : class
            => CreatePattern<TInput>(
                input => input == null,
                description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <seealso cref="Null{TInput}()" />
        /// <seealso cref="Null{TInput}(string)" />
        /// <seealso cref="ValueNull{TInput}(string)" />
        public static IPattern<TInput?, TInput?> ValueNull<TInput>()
            where TInput : struct
            => ValueNull<TInput>(DefaultNullDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="Null{TInput}()" />
        /// <seealso cref="Null{TInput}(string)" />
        /// <seealso cref="ValueNull{TInput}()" />
        public static IPattern<TInput?, TInput?> ValueNull<TInput>(string description)
            where TInput : struct
            => CreatePattern<TInput?>(
                input => input == null,
                description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to check for equality.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the specified value.
        /// </returns>
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(TInput value)
            => EqualTo(value, String.Format(DefaultEqualToDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to check for equality.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(Func<TInput> valueProvider)
            => EqualTo(valueProvider, DefaultLazyEqualToDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the specified value
        /// according to the specified equality comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to check for equality.</param>
        /// <param name="comparer">The equality comparer to use for checking equality.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(TInput value, IEqualityComparer<TInput> comparer)
            => EqualTo(value, comparer, String.Format(DefaultEqualToDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the provided value
        /// according to the specified equality comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to check for equality.</param>
        /// <param name="comparer">The equality comparer to use for checking equality.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(
            Func<TInput> valueProvider,
            IEqualityComparer<TInput> comparer)
            => EqualTo(valueProvider, comparer, DefaultLazyEqualToDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to check for equality.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(TInput value, string description)
            => EqualTo(value, EqualityComparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to check for equality.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(Func<TInput> valueProvider, string description)
            => EqualTo(valueProvider, EqualityComparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the specified value
        /// according to the specified equality comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to check for equality.</param>
        /// <param name="comparer">The equality comparer to use for checking equality.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(
            TInput value,
            IEqualityComparer<TInput> comparer,
            string description)
            => comparer != null
                ? CreatePattern<TInput>(
                    input => comparer.Equals(input, value),
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to check for equality.</param>
        /// <param name="comparer">The equality comparer to use for checking equality.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
        /// is <see langword="null" />.
        /// </exception>
        /// <seealso cref="EqualTo{TInput}(TInput)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, IEqualityComparer{TInput})" />
        /// <seealso cref="EqualTo{TInput}(TInput, string)" />
        /// <seealso cref="EqualTo{TInput}(Func{TInput}, string)" />
        /// <seealso cref="EqualTo{TInput}(TInput, IEqualityComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> EqualTo<TInput>(
            Func<TInput> valueProvider,
            IEqualityComparer<TInput> comparer,
            string description)
            => valueProvider != null
                ? comparer != null
                    ? CreatePattern<TInput>(
                        input => comparer.Equals(input, valueProvider()),
                        description ?? throw new ArgumentNullException(nameof(description)))
                    : throw new ArgumentNullException(nameof(comparer))
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the specified value.
        /// </returns>
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => LessThan(value, Comparer<TInput>.Default, String.Format(DefaultLessThanDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => LessThan(valueProvider, Comparer<TInput>.Default, DefaultLazyLessThanDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the specified value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(TInput value, IComparer<TInput> comparer)
            => LessThan(value, comparer, String.Format(DefaultLessThanDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the provided value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(Func<TInput> valueProvider, IComparer<TInput> comparer)
            => LessThan(valueProvider, comparer, DefaultLazyLessThanDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(TInput value, string description)
            where TInput : IComparable<TInput>
            => LessThan(value, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(Func<TInput> valueProvider, string description)
            where TInput : IComparable<TInput>
            => LessThan(valueProvider, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the specified value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(
            TInput value,
            IComparer<TInput> comparer,
            string description)
            => comparer != null
                ? CreatePattern<TInput>(
                    input => comparer.Compare(input, value) < 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than the provided value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
        /// is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessThan{TInput}(TInput, string)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessThan{TInput}(TInput, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessThan<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer,
            string description)
            => valueProvider != null
                ? comparer != null
                    ? CreatePattern<TInput>(
                        input => comparer.Compare(input, valueProvider()) < 0,
                        description ?? throw new ArgumentNullException(nameof(description)))
                    : throw new ArgumentNullException(nameof(comparer))
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal
        /// to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
        /// </returns>
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => LessOrEqual(value, Comparer<TInput>.Default, String.Format(DefaultLessOrEqualDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal to
        /// the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => LessOrEqual(valueProvider, Comparer<TInput>.Default, DefaultLazyLessOrEqualDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal
        /// to the specified value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(TInput value, IComparer<TInput> comparer)
            => LessOrEqual(value, comparer, String.Format(DefaultLessOrEqualDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal to
        /// the provided value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer)
            => LessOrEqual(valueProvider, comparer, DefaultLazyLessOrEqualDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal
        /// to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(TInput value, string description)
            where TInput : IComparable<TInput>
            => LessOrEqual(value, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal
        /// to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(Func<TInput> valueProvider, string description)
            where TInput : IComparable<TInput>
            => LessOrEqual(valueProvider, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal
        /// to the specified value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(
            TInput value,
            IComparer<TInput> comparer,
            string description)
            => comparer != null
                ? CreatePattern<TInput>(
                    input => comparer.Compare(input, value) <= 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is less than or equal
        /// to the provided value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is less than or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
        /// is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="LessOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> LessOrEqual<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer,
            string description)
            => valueProvider != null
                ? comparer != null
                    ? CreatePattern<TInput>(
                        input => comparer.Compare(input, valueProvider()) <= 0,
                        description ?? throw new ArgumentNullException(nameof(description)))
                    : throw new ArgumentNullException(nameof(comparer))
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the specified value.
        /// </returns>
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => GreaterThan(value, Comparer<TInput>.Default, String.Format(DefaultGreaterThanDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => GreaterThan(valueProvider, Comparer<TInput>.Default, DefaultLazyGreaterThanDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the specified value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(TInput value, IComparer<TInput> comparer)
            => GreaterThan(value, comparer, String.Format(DefaultGreaterThanDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the provided value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer)
            => GreaterThan(valueProvider, comparer, DefaultLazyGreaterThanDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(TInput value, string description)
            where TInput : IComparable<TInput>
            => GreaterThan(value, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(Func<TInput> valueProvider, string description)
            where TInput : IComparable<TInput>
            => GreaterThan(valueProvider, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the specified value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(
            TInput value,
            IComparer<TInput> comparer,
            string description)
            => comparer != null
                ? CreatePattern<TInput>(
                    input => comparer.Compare(input, value) > 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the provided value
        /// according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
        /// is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput, string)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterThan{TInput}(TInput, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterThan<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer,
            string description)
            => valueProvider != null
                ? comparer != null
                    ? CreatePattern<TInput>(
                        input => comparer.Compare(input, valueProvider()) > 0,
                        description ?? throw new ArgumentNullException(nameof(description)))
                    : throw new ArgumentNullException(nameof(comparer))
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal
        /// to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the specified value.
        /// </returns>
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => GreaterOrEqual(
                value, Comparer<TInput>.Default, String.Format(DefaultGreaterOrEqualDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal to
        /// the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => GreaterOrEqual(valueProvider, Comparer<TInput>.Default, DefaultLazyGreaterOrEqualDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal
        /// to the specified value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(TInput value, IComparer<TInput> comparer)
            => GreaterOrEqual(value, comparer, String.Format(DefaultGreaterOrEqualDescriptionFormat, value));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal to
        /// the provided value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer)
            => GreaterOrEqual(valueProvider, comparer, DefaultLazyGreaterOrEqualDescription);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal
        /// to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(TInput value, string description)
            where TInput : IComparable<TInput>
            => GreaterOrEqual(value, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal
        /// to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(Func<TInput> valueProvider, string description)
            where TInput : IComparable<TInput>
            => GreaterOrEqual(valueProvider, Comparer<TInput>.Default, description);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal
        /// to the specified value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the specified value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(
            TInput value,
            IComparer<TInput> comparer,
            string description)
            => comparer != null
                ? CreatePattern<TInput>(
                    input => comparer.Compare(input, value) >= 0,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than or equal
        /// to the provided value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <param name="comparer">The comparer to use for comparison.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than
        /// or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" />, <paramref name="comparer" /> or <paramref name="description" />
        /// is <see langword="null" />.
        /// </exception>
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, IComparer{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput}, string)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput, IComparer{TInput}, string)" />
        public static IPattern<TInput, TInput> GreaterOrEqual<TInput>(
            Func<TInput> valueProvider,
            IComparer<TInput> comparer,
            string description)
            => valueProvider != null
                ? comparer != null
                    ? CreatePattern<TInput>(
                        input => comparer.Compare(input, valueProvider()) >= 0,
                        description ?? throw new ArgumentNullException(nameof(description)))
                    : throw new ArgumentNullException(nameof(comparer))
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is of the specified type.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TType">The type to check for.</typeparam>
        /// <returns>
        /// A pattern which is matched successfully when the input value is of the specified type.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This pattern fails if the input is <see langword="null" />.
        /// </para>
        /// <para>
        /// This pattern can be used to match discriminated unions which are implemented as class hierarchies.
        /// </para>
        /// </remarks>
        /// <seealso cref="Type{TInput, TType}(string)" />
        public static IPattern<TInput, TType> Type<TInput, TType>()
            where TType : TInput
            => Type<TInput, TType>(String.Format(DefaultTypeDescriptionFormat, typeof(TType)));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is of the specified type.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TType">The type to check for.</typeparam>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is of the specified type.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This pattern fails if the input is <see langword="null" />.
        /// </para>
        /// <para>
        /// This pattern can be used to match discriminated unions which are implemented as class hierarchies.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="Type{TInput, TType}()" />
        public static IPattern<TInput, TType> Type<TInput, TType>(string description)
            where TType : TInput
            => CreatePattern<TInput, TType>(
                input => input is TType result ? MatchResult.Success(result) : MatchResult.Failure<TType>(),
                description ?? throw new ArgumentNullException(nameof(description)));

        /// <summary>
        /// Returns a pattern which is matched successfully when the specified pattern is not matched successfully.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
        /// <param name="pattern">The pattern to invert.</param>
        /// <returns>
        /// A pattern which is matched successfully when the specified pattern is not matched successfully.
        /// </returns>
        /// <remarks>
        /// This pattern ignores the specified pattern's transformation
        /// and returns the input value if matched successfully.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="Not{TInput, TMatchResult}(IPattern{TInput, TMatchResult}, string)" />
        public static IPattern<TInput, TInput> Not<TInput, TMatchResult>(IPattern<TInput, TMatchResult> pattern)
            => pattern != null
                ? Not(
                    pattern,
                    pattern.Description.Length != 0
                        ? String.Format(DefaultNotDescriptionFormat, pattern.Description)
                        : String.Empty)
                : throw new ArgumentNullException(nameof(pattern));

        /// <summary>
        /// Returns a pattern which is matched successfully when the specified pattern is not matched successfully.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <typeparam name="TMatchResult">The type of the result of this pattern's match.</typeparam>
        /// <param name="pattern">The pattern to invert.</param>
        /// <param name="description">The description of the pattern.</param>
        /// <returns>
        /// A pattern which is matched successfully when the specified pattern is not matched successfully.
        /// </returns>
        /// <remarks>
        /// This pattern ignores the specified pattern's transformation
        /// and returns the input value if matched successfully.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern" /> or <paramref name="description" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="Not{TInput, TMatchResult}(IPattern{TInput, TMatchResult})" />
        public static IPattern<TInput, TInput> Not<TInput, TMatchResult>(
            IPattern<TInput, TMatchResult> pattern,
            string description)
            => pattern != null
                ? CreatePattern<TInput>(
                    input => !pattern.Match(input).IsSuccessful,
                    description ?? throw new ArgumentNullException(nameof(description)))
                : throw new ArgumentNullException(nameof(pattern));
    }
}