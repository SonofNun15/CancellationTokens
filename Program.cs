using CancellationTokenTest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

const int repetitions = 20;

app.MapGet("/long-request", async (CancellationToken token) =>
	{
		// This is not really needed in a web request as it appears to
		// be the default behavior, but could be used in other situations:
		// token.ThrowIfCancellationRequested();
		
		try
		{
			await repetitions.Times(async (i) =>
			{
				Console.WriteLine($"{i}: Long request running...");
				await Task.Delay(1000, token);
			}, token);
			
			Console.WriteLine("Long request complete.");
			
			return new Random().Next(0, 100);
		}
		catch (OperationCanceledException e)
		{
			Console.WriteLine("Long request cancelled.");
			throw;
		}
	})
	.WithName("LongRequest")
	.WithOpenApi();

app.Run();
