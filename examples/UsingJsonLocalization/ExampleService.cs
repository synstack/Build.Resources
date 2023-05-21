using Microsoft.Extensions.Localization;

namespace UsingJsonLocalization;

public class ExampleService
{
    private readonly IStringLocalizer<ExampleService> _localizer;

    public ExampleService(IStringLocalizer<ExampleService> localizer)
    {
        _localizer = localizer;
    }

    public void Say()
    {
        Console.WriteLine(_localizer["Message"]);
        Console.WriteLine(_localizer["Nested:Message", "qwq"]);
    }
}