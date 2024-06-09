using Avalonia;
using Avalonia.ReactiveUI;
using DotNetEnv;
using System;


namespace src;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // 
        Env.Load();

        if (args.Length > 0 && args[0] == "--migrate")
        {
            // Migrate the database
            Db.Db.Migrate();
            return;
        }
        else if (args.Length > 0 && args[0] == "--seed")
        {
            // Run the seed
            Db.Db.Seed();
            return;
        }
        else if (args.Length > 0 && args[0] == "--load-dump")
        {
            // Run the test
            Db.Db.LoadDump();
            return;
        }
        else if (args.Length > 0 && args[0] == "--stress")
        {
            Cli.Cli.RunStress();
            return;
        }
        else if (args.Length > 0 && args[0] == "--cli")
        {
            // Only for test cli program
            // Cli.Cli.RunRegex();
            Cli.Cli.RunStress();
            return;
        }
        else
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
