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

        Assert.Equal(count, result);
    }

    [Fact(DisplayName = "Enumerate should throw if enumerable is null")]
    public void EnumerateShouldThrowIfEnumerableIsNull() =>
        Assert.Throws<ArgumentNullException>(() => ((IEnumerable<object>)null).Enumerate());

    [Fact(DisplayName = "EnumerateAsync should force enumeration")]
    public async Task EnumerateAsyncShouldForceEnumeration()
    {
        const int count = 100;

        int result = 0;

        await AsyncEnumerable.Range(0, count)
            .Select(_ => result++)
            .EnumerateAsync();

        Assert.Equal(count, result);
    }

    [Fact(DisplayName = "EnumerateAsync should throw if enumerable is null")]
    public void EnumerateAsyncShouldThrowIfEnumerableIsNull() =>
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = ((IAsyncEnumerable<object>)null).EnumerateAsync();
        });
}
