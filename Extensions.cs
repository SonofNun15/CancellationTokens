namespace CancellationTokenTest;

public static class Extensions
{
	public static async Task Times(this int count, Func<int, Task> action, CancellationToken token)
	{
		for (var i = 0; i < count; i++)
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
				
			await action(i);
		}
	}
}
