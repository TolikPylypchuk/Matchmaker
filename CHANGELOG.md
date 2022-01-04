# Changelog

## v3.0.1

Whats's new in version 3.0.1:

- No changes to the library's code or API
- Matchmaker is now built with .NET 6 and uses C# 10 (but still targets .NET Standard 2.1)
- Improved the NuGet package (added a symbol package, a readme, Source Link, and updated the docs)
- The library is now trimmable
- GitHub Actions are now used for CI instead of AppVeyor

## v3.0

Whats's new in version 3.0:

- Changed the .NET Standard version to 2.1
- Added support for nullable reference types
- Added asynchronous pattern matcing

## v2.1

What's new in version 2.1:

- The less generic `IPattern<TInput>` interface was removed (breaking change)
- The `Type` pattern and the `Cast`extension methods now work correctly with `null` values

Even though this release contains a potentially breaking change, it is very unlikely that the removal of an
interface which was designed to be internal will actually break anything.

## v2.0

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

Read the [migration guide](https://matchmaker.tolik.io/v2.1.0/articles/migration.html) for more info.

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
