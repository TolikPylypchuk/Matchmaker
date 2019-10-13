using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatternMatching.Tests.Samples;

using static LanguageExt.Prelude;
using static PatternMatching.Pattern;

namespace PatternMatching.Tests
{
    [TestClass]
    public class MatchTest
    {
        [TestMethod]
        public void TestEquality()
        {
            string result =
                Match.Create<int, string>()
                    .Case(EqualTo(1), _ => "one")
                    .Case(EqualTo(2), _ => "two")
                    .Case(EqualTo(3), _ => "three")
                    .Case(EqualTo(4), _ => "four")
                    .Case(Any<int>(), i => i.ToString())
                    .ExecuteOn(5);

            Assert.AreEqual("5", result);
        }

        [TestMethod]
        public void TestComparison()
        {
            string result =
                Match.Create<int, string>()
                    .Case(
                        LessThan(1),
                        _ => "x < 1")
                    .Case(
                        GreaterOrEqual(1) & LessThan(2),
                        _ => "1 <= x < 2")
                    .Case(
                        GreaterOrEqual(2) & LessThan(3),
                        _ => "2 <= x < 3")
                    .Case(
                        GreaterOrEqual(3) & LessThan(4),
                        _ => "3 <= x < 4")
                    .Case(
                        GreaterOrEqual(4) & Not(GreaterThan(5)),
                        _ => "4 <= x <= 5")
                    .Case(
                        Any<int>(),
                        _ => "5 < x")
                    .ExecuteOn(5);

            Assert.AreEqual("4 <= x <= 5", result);
        }

        [TestMethod]
        public void TestLaziness()
        {
            Func<int> LongComputation(int delay, int value)
                => () =>
                {
                    Thread.Sleep(delay);
                    return value;
                };

            string result =
                Match.Create<int, string>()
                    .Case(EqualTo(LongComputation(0, 1)), _ => "one")
                    .Case(EqualTo(LongComputation(100, 1)), _ => "two")
                    .Case(EqualTo(LongComputation(200, 1)), _ => "three")
                    .Case(EqualTo(LongComputation(300, 1)), _ => "four")
                    .Case(Any<int>(), i => i.ToString())
                    .ExecuteOn(1);

            Assert.AreEqual("one", result);
        }

        [TestMethod]
        public void TestTypes()
        {
            Func<ConsList, int> sum = null;

            sum = Match.Create<ConsList, int>()
                .Case<ConsCell>(cell => cell.Head + sum(cell.Tail))
                .Case<Empty>(_ => 0)
                .ToFunction();

            Assert.AreEqual(3, sum(ConsList.Cell(1, ConsList.Cell(2, ConsList.Empty))));
        }

        [TestMethod]
        public void TestFallthrough()
        {
            SimplePattern<int> divisibleBy(int n) => new SimplePattern<int>(input => input % n == 0);

            var result = Enumerable.Range(0, 15)
                .Select(Match.Create<int, string>(fallthroughByDefault: true)
                    .Case(divisibleBy(3), _ => "Fizz")
                    .Case(divisibleBy(5), _ => "Buzz")
                    .Case(Not(divisibleBy(3) | divisibleBy(5)), n => n.ToString())
                    .ToFunctionWithFallthrough())
                .Select(items => items.Aggregate(String.Concat));

            var expectedList = List("FizzBuzz", "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz");

            foreach (var (expected, actual) in expectedList.Zip(result))
            {
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
