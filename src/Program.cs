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
            // Generated encrypted seeding
            Db.Db.Seed();
            return;
        }
        else if (args.Length > 0 && args[0] == "--load-dump")
        {
            // Load encrypted seed from db/seeded.sql
            Db.Db.LoadDump();
            return;
        }
        else if (args.Length > 0 && args[0] == "--raw-migrate")
        {
            // Generate raw (unecrypted seeding)
            Db.Db.RawMigrate();
            return;
        }
        else if (args.Length > 0 && args[0] == "--raw-seed")
        {
            // Generate raw (unecrypted seeding)
            Db.Db.RawSeed();
            return;
        }
        else if (args.Length > 0 && args[0] == "--convert-dump")
        {
            // Convert from kating's dump to converted dump
            Db.Db.ConvertToEncrypted();
            return;
        }
        else if (args.Length > 0 && args[0] == "--stress")
        {
            // Run stress test statistics
            Cli.Cli.RunStress();
            return;
        }
        else if (args.Length > 0 && args[0] == "--cli")
        {
            // Only for test cli program
            // Cli.Cli.RunRegex();
            Cli.Cli.RunQuery();
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
