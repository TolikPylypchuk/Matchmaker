# Changelog

## v1.2 (Work in Progress)

What's new in version 1.2:

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
