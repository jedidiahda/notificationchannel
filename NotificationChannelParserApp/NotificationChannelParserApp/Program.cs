// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationChannelParserApp;
using System.Threading.Channels;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<NotificationWorkerService>();
builder.Services.AddLogging(logBuiler =>
{
    logBuiler.ClearProviders();
    logBuiler.AddConsole();
});
var channelBE = Channel.CreateUnbounded<string>();

//[BE][FE][Urgent] there is error
Console.Write("Input: ");
var input = Console.ReadLine();

if (input.IndexOf("[BE]") >= 0) await channelBE.Writer.WriteAsync("BE");
if (input.IndexOf("[FE]") >= 0) await channelBE.Writer.WriteAsync("FE");
if (input.IndexOf("[QA]") >= 0) await channelBE.Writer.WriteAsync("QA");
if (input.IndexOf("[Urgent]") >= 0) await channelBE.Writer.WriteAsync("Urgent");

builder.Services.AddSingleton(channelBE);

IHost app = builder.Build();

await app.RunAsync();

