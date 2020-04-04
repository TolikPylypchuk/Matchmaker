# Changelog

# v2.0

What's new in version 2.0:

- Library renamed to Matchmaker (breaking change)
- Stopped using the DLR for ignoring the intermediate types
- Dropped the dependency on [language-ext](https://github.com/louthy/language-ext) (breaking change)
- The deprecated `StructNull` pattern is removed (breaking change)
- Matching with fallthrough became lazy in the match expression
- Strict matching with fallthrough was removed (breaking change)

## v1.2

What's new in version 1.2:

- Added tests
- Deprecated the `StructNull` pattern in favour of `ValueNull`
- Fixed a bug which made strict matching with fallthrough unusable
- Fixed null handling in some predefined patterns
- Minor code refactoring

## v1.1

What's new in version 1.1:

- `Matcher` classes renamed to `Match` (breaking change).
- `OptionUnsafe` is now used instead of `Option` in patterns to fully support `null` values (breaking change).
- `ExecuteOnStrict` in `Match<TInput>` renamed to `ExecuteStrict` (breaking change).
- Implemented matching with fallthrough
- Added two new patterns - `Null` for classes and `StructNull` for nullable structs.

Although there are several breaking changes, the major version is not incremented, as these changes
will probably not actually break anything, because not many people have used version 1.0 so far.

## v1.0

The initial version of the PatternMatching library.
