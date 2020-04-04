using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using static LanguageExt.Prelude;

namespace Matchmaker
{
    /// <summary>
    /// Contains some frequently used patterns.
    /// </summary>
    /// <seealso cref="IPattern{TInput, TMatchResult}" />
    /// <seealso cref="Pattern{TInput, TMatchResult}" />
    /// <seealso cref="SimplePattern{TInput}" />
    public static class Pattern
    {
        /// <summary>
        /// Returns a pattern which is always matched successfully.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is always matched successfully.</returns>
        /// <remarks>
        /// This pattern should be used as the default case of a match expression, if one is needed.
        /// </remarks>
        public static SimplePattern<TInput> Any<TInput>()
            => new SimplePattern<TInput>(_ => true);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <seealso cref="ValueNull{TInput}" />
        public static SimplePattern<TInput> Null<TInput>()
            where TInput : class
            => new SimplePattern<TInput>(input => input == null);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <remarks>
        /// This pattern is obsolete and will be removed in version 2.0.
        /// Use the <see cref="ValueNull{TInput}" /> pattern instead.
        /// This change was done to match the naming of .NET types (e.g. <see cref="ValueTuple" />).
        /// </remarks>
        [ExcludeFromCodeCoverage]
        [Obsolete("Use the ValueNull pattern instead")]
        public static SimplePattern<TInput?> StructNull<TInput>()
            where TInput : struct
            => new SimplePattern<TInput?>(input => input == null);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is <see langword="null" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <returns>A pattern which is matched successfully when the input value is <see langword="null" />.</returns>
        /// <seealso cref="Null{TInput}" />
        public static SimplePattern<TInput?> ValueNull<TInput>()
            where TInput : struct
            => new SimplePattern<TInput?>(input => input == null);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is equal to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to check for equality.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is equal to the specified value.
        /// </returns>
        /// <seealso cref="EqualTo{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> EqualTo<TInput>(TInput value)
            where TInput : IEquatable<TInput>
            => new SimplePattern<TInput>(input => Equals(input, value));

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
        public static SimplePattern<TInput> EqualTo<TInput>(Func<TInput> valueProvider)
            where TInput : IEquatable<TInput>
            => valueProvider != null
                ? new SimplePattern<TInput>(input => Equals(input, valueProvider()))
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
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> LessThan<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, value) < 0);

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
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> LessThan<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => valueProvider != null
                ? new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, valueProvider()) < 0)
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value
        /// is less than or equal to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value
        /// is less than or equal to the specified value.
        /// </returns>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> LessOrEqual<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, value) <= 0);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value
        /// is less than or equal to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value
        /// is less than or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> LessOrEqual<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => valueProvider != null
                ? new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, valueProvider()) <= 0)
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value is greater than the specified value.
        /// </returns>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> GreaterThan<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, value) > 0);

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
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> GreaterThan<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => valueProvider != null
                ? new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, valueProvider()) > 0)
                : throw new ArgumentNullException(nameof(valueProvider));

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value
        /// is greater than or equal to the specified value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="value">The value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value
        /// is greater than or equal to the specified value.
        /// </returns>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(Func{TInput})" />
        public static SimplePattern<TInput> GreaterOrEqual<TInput>(TInput value)
            where TInput : IComparable<TInput>
            => new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, value) >= 0);

        /// <summary>
        /// Returns a pattern which is matched successfully when the input value
        /// is greater than or equal to the provided value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value of the expression.</typeparam>
        /// <param name="valueProvider">The provider of the value to compare with.</param>
        /// <returns>
        /// A pattern which is matched successfully when the input value
        /// is greater than or equal to the provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueProvider" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="LessThan{TInput}(TInput)" />
        /// <seealso cref="LessThan{TInput}(Func{TInput})" />
        /// <seealso cref="LessOrEqual{TInput}(TInput)" />
        /// <seealso cref="LessOrEqual{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterThan{TInput}(TInput)" />
        /// <seealso cref="GreaterThan{TInput}(Func{TInput})" />
        /// <seealso cref="GreaterOrEqual{TInput}(TInput)" />
        public static SimplePattern<TInput> GreaterOrEqual<TInput>(Func<TInput> valueProvider)
            where TInput : IComparable<TInput>
            => valueProvider != null
                ? new SimplePattern<TInput>(input => Comparer<TInput>.Default.Compare(input, valueProvider()) >= 0)
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
        /// This pattern can be used to match discriminated unions which are implemented as class hierarchies.
        /// </remarks>
        public static Pattern<TInput, TType> Type<TInput, TType>()
            where TType : TInput
            => new Pattern<TInput, TType>(input => input is TType result ? SomeUnsafe(result) : None);

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
        public static SimplePattern<TInput> Not<TInput, TMatchResult>(IPattern<TInput, TMatchResult> pattern)
            => pattern != null
                ? new SimplePattern<TInput>(input => !pattern.Match(input).IsSome)
                : throw new ArgumentNullException(nameof(pattern));
    }
}
