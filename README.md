# NekoSpace.Build.Resources

[![NuGet Version](https://img.shields.io/nuget/v/NekoSpace.Build.Resources.Json?label=NuGet)](https://www.nuget.org/packages/BingChat)
![.NET Version](https://img.shields.io/badge/.NET-Standard_2.0-blue)
[![License](https://img.shields.io/badge/License-MIT-lightblue)](./LICENSE)
![Resource](https://img.shields.io/badge/Resource-JSON-wheat)

Embed lightweight JSON resources into your .NET application using MSBuild Task, instead of traditional .resx files.

## Features

- Compile JSON resources to binary .resources files, and embed them into assembly, results the same functionality as ResX.
- Cleaner, more human-readable content compared to ResX, even outside IDEs. (Without IDEs the .resx file is totally a mess)
- Easy to use. Just a few lines to include all JSON resources, so you won't mess up your project file.
- Support nested JSON structures.

## Getting Started

First install this package using package manager or using dotnet CLI:

```
dotnet add package NekoSpace.Build.Resources.Json
```

Then, add a single line to your csproj file:

```xml
<ItemGroup>
    <!-- Embed our JSON resources. -->
    <JsonResource Include="**\*.json" Exclude="bin\**\*; obj\**\*"/>
</ItemGroup>
```

Create a simple JSON file, for example called `ExampleResource.json`:

```json
{
  "ExampleKey": "English Content",
  "Group": {
    "ExampleSubKey": "Another English content"
  }
}
```

Here is a example for using the resources:

```csharp
// Get the assembly which embedded our resources.
var assembly = typeof(Program).Assembly;

// Instantiate the ResourceManager to manage our resources.
var manager = new ResourceManager("UsingJsonResources.ExampleResource", assembly);

// Resource defined in ExampleResource.json, and get parsed, embedded automatically.
Console.WriteLine(manager.GetString("ExampleKey"));

// Access nested resource.
Console.WriteLine(manager.GetString("Group:ExampleSubKey"));
```

Same as ResX, our resources can be easily localized!

Create another JSON file called `ExampleResource.zh-Hans.json`:

```json
{
  "ExampleKey": "中文内容"
}
```

Then get the resource string by passing a specified culture:

```csharp
// Override the culture.
Console.WriteLine(manager.GetString("ExampleKey", new CultureInfo("zh-Hans")));
```

You can find more examples [in the examples folder](./examples), such as combining usage with
[Microsoft.Extensions.Localization](https://learn.microsoft.com/dotnet/core/extensions/localization).

## License

This project is under [MIT license](./LICENSE).
