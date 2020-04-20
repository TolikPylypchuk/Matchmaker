# Changelog

# v2.0

What's new in version 2.0 (almost all changes are breaking):

- Library renamed to Matchmaker
- Stopped using the DLR for ignoring the intermediate types
- Dropped the dependency on [language-ext](https://github.com/louthy/language-ext)
- Completely updated the pattern hierarchy and built-in patterns
- The deprecated `StructNull` pattern is removed
- Matching with fall-through became lazy
- Strict matching with fall-through was removed
- Descriptions were added to patterns
- Creating custom patterns became possible through factory methods and extension methods
- The default mode of match statements changed from non-strict to strict

Read the [migration guide](https://tolikpylypchuk.github.io/Matchmaker/v2.0.0/articles/migration.html) for more info.

## v1.2

What's new in version 1.2:

- Added tests
- Deprecated the `StructNull` pattern in favour of `ValueNull`
- Fixed a bug which made strict matching with fall-through unusable
- Fixed null handling in some predefined patterns
- Minor code refactoring

## v1.1

What's new in version 1.1:

- `Matcher` classes renamed to `Match` (breaking change).
- `OptionUnsafe` is now used instead of `Option` in patterns to fully support `null` values (breaking change).
- `ExecuteOnStrict` in `Match<TInput>` renamed to `ExecuteStrict` (breaking change).
- Implemented matching with fall-through
- Added two new patterns - `Null` for classes and `StructNull` for nullable structs.

Although there are several breaking changes, the major version is not incremented, as these changes
will probably not actually break anything, because not many people have used version 1.0 so far.

## v1.0

The initial version of the PatternMatching library.
