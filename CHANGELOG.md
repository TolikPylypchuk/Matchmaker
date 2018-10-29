# Changelog

## v1.1

What's new in version 1.1:

- `Matcher` classes renamed to `Match` (breaking change).
- OptionUnsafe is now used instead of `Option` in patterns to fully support `null` values (breaking change).
- Added two new patterns - `Null` for classes and `StructNull` for nullable structs.

Although there are several breaking changes, the major version is not incremented, as these changes
will probably not actually break anything, because they not that common and because not many people
have used version 1.0 so far.

## v1.0

The initial version of the PatternMatching library.
