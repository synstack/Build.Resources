using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsingJsonLocalization;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddTransient<ExampleService>();

var app = builder.Build();

var example = app.Services.GetRequiredService<ExampleService>();

CultureInfo.CurrentUICulture = new CultureInfo("en-US");
example.Say();

CultureInfo.CurrentUICulture = new CultureInfo("zh-Hans");
example.Say();