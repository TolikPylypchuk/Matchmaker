using Matchmaker.Linq;

namespace Matchmaker.Patterns
{
    /// <summary>
    /// Represnts types of pattern composition.
    /// </summary>
    /// <seealso cref="PatternExtensions" />
    /// <seealso cref="AsyncPatternExtensions" />
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
