using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PatternMatching.Tests.Samples;

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
		public void TestTypes()
		{
			Func<ConsList, int> sum = null;

			sum = Match.Create<ConsList, int>()
					.Case(ConsCell.Pattern, cell => cell.Head + sum(cell.Tail))
					.Case(Empty.Pattern, _ => 0)
					.Compile();

			Assert.AreEqual(3, sum(ConsList.Cell(1, ConsList.Cell(2, ConsList.Empty))));
		}
	}
}
