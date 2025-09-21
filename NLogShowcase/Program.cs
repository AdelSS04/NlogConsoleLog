using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLogShowcase.Services;

namespace NLogShowcase
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Contains("--verify"))
            {
                RunVerificationMode(args);
                return;
            }

            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("=".PadRight(80, '='));
                logger.LogInformation("NLog Showcase Application Started");
                logger.LogInformation("Demonstrating comprehensive logging features and best practices");
                logger.LogInformation("=".PadRight(80, '='));

                var loggingDemo = host.Services.GetRequiredService<LoggingDemoService>();
                var exceptionDemo = host.Services.GetRequiredService<ExceptionDemoService>();
                var performanceDemo = host.Services.GetRequiredService<PerformanceDemoService>();

                await ShowInteractiveMenu(logger, loggingDemo, exceptionDemo, performanceDemo);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Application failed to start or encountered a critical error");
                throw;
            }
            finally
            {
                logger.LogInformation("NLog Showcase Application Ended");
                logger.LogInformation("Check the logs folder for generated log files");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<LoggingDemoService>();
                    services.AddSingleton<ExceptionDemoService>();
                    services.AddSingleton<PerformanceDemoService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    
                    logging.AddNLog("nlog.config");
                });

        private static void RunVerificationMode(string[] args)
        {
            Console.WriteLine("Running NLog Showcase in verification mode...");
            
            try
            {
                var host = CreateHostBuilder(args).Build();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                
                logger.LogInformation("Verification mode: Application started successfully");
                logger.LogInformation("Verification mode: Testing service dependencies");
                
                var loggingDemo = host.Services.GetRequiredService<LoggingDemoService>();
                var exceptionDemo = host.Services.GetRequiredService<ExceptionDemoService>();
                var performanceDemo = host.Services.GetRequiredService<PerformanceDemoService>();
                
                logger.LogInformation("Verification mode: All services resolved successfully");
                logger.LogInformation("Verification mode: Testing basic logging functionality");
                
                loggingDemo.DemonstrateLoggingLevels();
                
                logger.LogInformation("Verification mode: Application verification completed successfully");
                Console.WriteLine("✅ NLog Showcase verification passed!");
                
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Verification failed: {ex.Message}");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        private static async Task ShowInteractiveMenu(ILogger<Program> logger, 
            LoggingDemoService loggingDemo, 
            ExceptionDemoService exceptionDemo, 
            PerformanceDemoService performanceDemo)
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                Console.WriteLine();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                            NLog Showcase Menu                               ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("Choose a demonstration to run:");
                Console.WriteLine();
                Console.WriteLine("  1. Logging Levels Demo          - Show all log levels (Trace to Critical)");
                Console.WriteLine("  2. Structured Logging Demo      - Demonstrate parameterized logging");
                Console.WriteLine("  3. Conditional Logging Demo     - Show filtering and conditional logging");
                Console.WriteLine("  4. Custom Properties Demo       - Logging with scopes and context");
                Console.WriteLine("  5. Message Templates Demo       - Different formatting and templates");
                Console.WriteLine("  6. Exception Handling Demo      - Exception logging best practices");
                Console.WriteLine("  7. Performance Monitoring Demo  - Timing and performance metrics");
                Console.WriteLine("  8. Run All Basic Demos          - Execute all logging demonstrations");
                Console.WriteLine("  9. Run All Exception Demos      - Execute all exception demonstrations");
                Console.WriteLine(" 10. Run All Performance Demos    - Execute all performance demonstrations");
                Console.WriteLine(" 11. Run Complete Showcase        - Execute everything!");
                Console.WriteLine();
                Console.WriteLine("  0. Exit");
                Console.WriteLine();
                Console.Write("Enter your choice (0-11): ");

                var input = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (input?.Trim())
                    {
                        case "1":
                            logger.LogInformation("User selected: Logging Levels Demo");
                            loggingDemo.DemonstrateLoggingLevels();
                            break;

                        case "2":
                            logger.LogInformation("User selected: Structured Logging Demo");
                            loggingDemo.DemonstrateStructuredLogging();
                            break;

                        case "3":
                            logger.LogInformation("User selected: Conditional Logging Demo");
                            loggingDemo.DemonstrateConditionalLogging();
                            break;

                        case "4":
                            logger.LogInformation("User selected: Custom Properties Demo");
                            loggingDemo.DemonstrateCustomPropertiesAndContext();
                            break;

                        case "5":
                            logger.LogInformation("User selected: Message Templates Demo");
                            loggingDemo.DemonstrateMessageTemplates();
                            break;

                        case "6":
                            logger.LogInformation("User selected: Exception Handling Demo");
                            exceptionDemo.DemonstrateExceptionLogging();
                            break;

                        case "7":
                            logger.LogInformation("User selected: Performance Monitoring Demo");
                            await performanceDemo.DemonstratePerformanceLogging();
                            break;

                        case "8":
                            logger.LogInformation("User selected: Run All Basic Demos");
                            RunAllBasicDemos(loggingDemo);
                            break;

                        case "9":
                            logger.LogInformation("User selected: Run All Exception Demos");
                            exceptionDemo.DemonstrateExceptionLogging();
                            break;

                        case "10":
                            logger.LogInformation("User selected: Run All Performance Demos");
                            await performanceDemo.DemonstratePerformanceLogging();
                            break;

                        case "11":
                            logger.LogInformation("User selected: Run Complete Showcase");
                            await RunCompleteShowcase(logger, loggingDemo, exceptionDemo, performanceDemo);
                            break;

                        case "0":
                            logger.LogInformation("User requested exit");
                            exitRequested = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 0 and 11.");
                            logger.LogWarning("User entered invalid menu choice: {UserInput}", input);
                            break;
                    }

                    if (!exitRequested && !string.IsNullOrEmpty(input) && input.Trim() != "0")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Demo completed! Check the console output above and the log files in the 'logs' folder.");
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while running demo for user choice: {UserChoice}", input);
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }

        private static void RunAllBasicDemos(LoggingDemoService loggingDemo)
        {
            loggingDemo.DemonstrateLoggingLevels();
            Thread.Sleep(1000);
            loggingDemo.DemonstrateStructuredLogging();
            Thread.Sleep(1000);
            loggingDemo.DemonstrateConditionalLogging();
            Thread.Sleep(1000);
            loggingDemo.DemonstrateCustomPropertiesAndContext();
            Thread.Sleep(1000);
            loggingDemo.DemonstrateMessageTemplates();
        }

        private static async Task RunCompleteShowcase(ILogger<Program> logger,
            LoggingDemoService loggingDemo,
            ExceptionDemoService exceptionDemo,
            PerformanceDemoService performanceDemo)
        {
            logger.LogInformation("Starting complete NLog showcase demonstration");

            Console.WriteLine("Running complete showcase - this will take a few minutes...");
            Console.WriteLine();

            RunAllBasicDemos(loggingDemo);
            
            Console.WriteLine("Basic logging demos completed. Starting exception demos...");
            Thread.Sleep(2000);
            
            exceptionDemo.DemonstrateExceptionLogging();
            
            Console.WriteLine("Exception demos completed. Starting performance demos...");
            Thread.Sleep(2000);
            
            await performanceDemo.DemonstratePerformanceLogging();

            logger.LogInformation("Complete NLog showcase demonstration finished");
            Console.WriteLine();
            Console.WriteLine("Complete showcase finished! Check all the generated log files in the 'logs' folder.");
        }
    }
}
