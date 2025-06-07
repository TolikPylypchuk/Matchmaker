namespace Matchmaker.Linq;

public class EnumerableTests
{
    [Fact(DisplayName = "Enumerate should force enumeration")]
    public void EnumerateShouldForceEnumeration()
    {
        const int count = 100;

        int result = 0;

        Enumerable.Range(0, count)
            .Select(_ => result++)
            .Enumerate();

        result.Should().Be(count);
    }

    [Fact(DisplayName = "Enumerate should throw if enumerable is null")]
    public void EnumerateShouldThrowIfEnumerableIsNull()
    {
        var action = () => ((IEnumerable<object>)null).Enumerate();
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "EnumerateAsync should force enumeration")]
    public async Task EnumerateAsyncShouldForceEnumeration()
    {
        const int count = 100;

        int result = 0;

        await AsyncEnumerable.Range(0, count)
            .Select(_ => result++)
            .EnumerateAsync();

        result.Should().Be(count);
    }

    [Fact(DisplayName = "EnumerateAsync should throw if enumerable is null")]
    public void EnumerateAsyncShouldThrowIfEnumerableIsNull()
    {
        var action = () =>
        {
            _ = ((IAsyncEnumerable<object>)null).EnumerateAsync();
        };

        action.Should().Throw<ArgumentNullException>();
    }
}
