using Matchmaker.Linq;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represnts types of pattern composition.
    /// </summary>
    /// <seealso cref="PatternExtensions.Compose{T}(IPattern{T, T}, IPattern{T, T}, PatternComposition)" />
    /// <seealso cref="PatternExtensions.Compose{T}(IPattern{T, T}, IPattern{T, T}, PatternComposition, string)" />
    /// <seealso cref="PatternExtensions.And{T}(IPattern{T, T}, IPattern{T, T})" />
    /// <seealso cref="PatternExtensions.And{T}(IPattern{T, T}, IPattern{T, T}, string)" />
    /// <seealso cref="PatternExtensions.Or{T}(IPattern{T, T}, IPattern{T, T})" />
    /// <seealso cref="PatternExtensions.Or{T}(IPattern{T, T}, IPattern{T, T}, string)" />
    /// <seealso cref="PatternExtensions.Xor{T}(IPattern{T, T}, IPattern{T, T})" />
    /// <seealso cref="PatternExtensions.Xor{T}(IPattern{T, T}, IPattern{T, T}, string)" />
    public enum PatternComposition
    {
        /// <summary>
        /// Represents the 'and' composition - the result is successful only when both results are successful.
        /// </summary>
        And,

        /// <summary>
        /// Represents the 'or' composition - the result is successful when either of the results is successful.
        /// </summary>
        Or,

        /// <summary>
        /// Represents the 'xor' composition - the result is successful when only one of the results is successful.
        /// </summary>
        Xor
    }
}
