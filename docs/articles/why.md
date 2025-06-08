# Why This Library Was Written

## Should I Use This Library?

Pattern matching as a technique was created to make code which operates on data more succinct, clear, and readable. I
have to say, I'm not sure this library achieves this. There's no denying that this library is cumbersome, and some
features are kind of convoluted. You can certainly use it if you want to — it's extensively tested. But this is more of
an experimental effort — I wanted to see how powerful pattern matching can be in C#. In this article I'll try to explain
some reasons behind the features of this library. This is not documentation per se, it's more of a thought piece about
this library and a recap of its history.

Also, I should note that I haven't checked out any other pattern matching libraries for C#. I'm sure there are a lot of
them — I can't be the only one who tried to bring this technique into C#. But I wanted to create something by myself and
not get inspired by (read: steal) features from the other libraries as I'm sure there's not actually much space for
stretching here. Some pattern matching libraries are probably better than this one and I'm okay with it.

## Background

In the end of 2017, after studying F# and functional programming, I've decided to try to bring some functional features
into C#. I've created a small library for C# and named it [CSX — C# Extensions](https://github.com/TolikPylypchuk/CSX).
I worked on this library just for the fun of it and never intended for it to be anything serious. I've long since
stopped doing anything with it, because I've realized that it required a lot of work. My implementations of functional
data structures were quite naive, and I've decided that I don't want to invest time into optimizing them. Moreover, I
was discouraged by the fact that there already are great functional libraries out there, like
[language-ext](https://github.com/louthy/language-ext).

One feature from that library grew into something more: pattern matching. I've included it into CSX and tried to create
a simple way to implement pattern matching on arbitrary types through interfaces, but failed and realized that it can't
be done that way. I came up with a different way to do pattern matching on arbitrary types, but decided to implement it
separately from CSX. This is how the [PatternMatching](https://github.com/TolikPylypchuk/PatternMatching) library was
born.

## PatternMatching

I released the first version of PatternMatching in October 2018. It contained the bare minimum. Patterns are just
objects which implement an interface. A match expression is just an object as well. Internally the data about cases was
stored as `dynamic` because the match expression itself cannot know about the types of patterns' transformations. I
thought that was the only way to do that. Well, that or reflection. Boy was I wrong, but more on that later. The only
reason I've chosen the DLR instead of reflection was because it's easier to write code this way. I've done little
research about the performance comparison between the two approaches.

Since the name `PatternMatching` was already taken on NuGet, I've decided to name the package
`CSharpStuff.PatternMatching` (not my best decision).

Almost immediately after releasing version 1.0, I made some changes. Some of them were breaking. I know that breaking
changes require a bump of the major version, but I didn't think those changes were big enough to warrant a bump of the
major version. Also, a breaking change is only breaking if someone actually uses the code.

The one big change in version 1.1 was introduction of matching with fall-through. This is probably the most
'experimental' feature in the library. I understand that matching with fall-through probably makes code more difficult
to comprehend, but it was fun to play around with designing this feature. It includes major design deviations from the
established way of fall-through that's present in C, C++, and Java. C# doesn't have fall-through at all and I think it's
a good thing because the way it's implemented in those languages can lead to subtle errors. I understand why it works
this way, but the probability of making an error and letting code fall through to the next case is quite big (I should
know, I've done that mistake myself). That is why I've implemented it in a way that lets code fall through to the next
successful case, and it should be explicitly enabled in several places. I can't really say whether it's better or worse
than how `switch` works, but for me it makes at least the tiniest bit of sense.

After releasing version 1.1 (also in October 2018), I thought that that was it and didn't return to it for a year. But
version 1.1 was untested, and I knew that I needed to add tests to make this library usable. After I started adding
tests, I realized that it was _really_ unusable. Matching with fall-through didn't work _at all_. Adding tests proved
more difficult that I initially thought, simply because adding hundreds of them is not really fun. I decided to use
[FsCheck](https://github.com/fscheck/FsCheck) (which is for F# but can be used with C# as well) to write property-based
tests. I wanted my tests not just to test the functionality, but also to serve as additional documentation which
precisely describes the properties of classes and methods. Version 1.2 is the tested version of 1.1. Nobody should use
versions 1.0 and 1.1.

During the development of version 1.2 I came up with more ideas for improvement of PatternMatching. I also came up with
a better name for the library (PatternMatching is a lame name).

## Matchmaker

I've decided to rename the library to 'Matchmaker'. This makes it a triple pun on the word 'match' (Matchmaker does
pattern matching, and its logo is a match).

Even though there were a lot of breaking changes between these versions, and the library name is different, I believe
this is still the same library, because it stayed the same at its core.

After releasing version 1.2, it came to me how to implement type-ignorant matching without the use of neither the DLR
nor reflection. And to be honest, at that moment I felt dumb, because I haven't thought about it before, and the
solution was obvious.

In this version, I've also completely uprooted the pattern hierarchy and made working with them much easier. I've also
added some primitive caching. I'm not an expert on caching — this can be an extensive topic of research — so I can't
say that caching in this library is great — it's okay at best. If you need more extensive or better caching of match
expressions, you can create an issue (or better yet, a pull request) on GitHub. I will most definitely respond and will
do my best to implement it.

I've improved the performance of match expression initializations by making them static. This makes the code even more
cumbersome, but as for me, it's worth it.

One other thing that I've changed drastically is matching with fall-through. The reasoning behind it was shaky before,
and now it must seem even more weird. I've made it lazy, which makes it more powerful in that the user can, for example,
limit the number of executed cases, but it became even more cumbersome to use.

I've also realized that having different default modes of execution in match expressions and match statements was
unintuitive, so now match statements also match strictly by default.

A couple days after releasing version 2.0.0 I realized once again that I was being dumb and that the less generic
`IPattern<TInput>` interface was not needed. And when I removed it, the only possible point of type failure was gone.
Once again, even though this change is potentially breaking, I didn't bump the major version, because I don't think
it would break any actual code.

After releasing version 2.1.0, I started working on version 3.0.0 which contains 2 major additions: support for nullable
reference types and asynchronous pattern matching. Because of those additions, the .NET Standard version had to be
bumped to 2.1 (sorry, .NET Framework). Asynchronous pattern matching will probably not be used often. I wrote it simply
because I can, and why not — we have asynchronous enumeration, asynchronous disposal, asynchronous almost everything,
so why not asynchronous pattern matching?

Five years after releasing version 3.0.0, I've decided to release version 3.1.0 which adds support for .NET Standard 2.0
back and uses [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces) as the
polyfill for asynchronous interfaces. So now the library supports .NET Framework once again.

## So Why Was This Library Written?

Having recapped some history and explained some design decisions, I haven't actually said why I wrote this library.
Well, the answer is simple: I wrote this library to write a library. I wanted to know what this process is like.

Write code. Build it. Deploy it to NuGet. See people actually download it, even though it's a small number. Write tests.
A lot of tests. Test everything. There are more tests than lines of code that are actually tested. Write documentation.
Lots of it. There's more documentation than code in this library. Write articles on how to use this library.

All of this is a lot of work, and this is an exceedingly small library. But I really wanted it to be extensively tested
and documented. I know what it's like when you want to use a library and the only way of doing that is to browse its
source code. It's not fun. In the process of writing this library I've gained more appreciation of people that maintain
libraries. I realize now just how much work this actually is.

## What's Next

I'm not planning on writing new versions beyond 3.1. To be fair, I thought the same thing after releasing version 1.1
and yet here we are. This time I do believe that this library has enough features (probably more than enough). Maybe one
day I'll revisit this decision, but for now (June 2025) this is it; this is as good as it gets.
