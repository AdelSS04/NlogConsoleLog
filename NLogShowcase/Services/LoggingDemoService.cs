using Microsoft.Extensions.Logging;
using NLogShowcase.Models;

namespace NLogShowcase.Services
{
    public class LoggingDemoService
    {
        private readonly ILogger<LoggingDemoService> _logger;
        private static readonly Random _random = new();

        public LoggingDemoService(ILogger<LoggingDemoService> logger)
        {
            _logger = logger;
        }

        public void DemonstrateLoggingLevels()
        {
            _logger.LogInformation("=== Demonstrating Logging Levels ===");

            _logger.LogTrace("Application trace: Entering method DemonstrateLoggingLevels()");

            _logger.LogDebug("Debug information: Current user count is {UserCount}", GetUserCount());

            _logger.LogInformation("User {UserId} successfully logged in from {IpAddress}", "12345", "192.168.1.100");

            _logger.LogWarning("Warning: API response time {ResponseTime}ms is above threshold {Threshold}ms", 
                1500, 1000);

            _logger.LogError("Failed to process order {OrderId}: Invalid payment method", "ORD-789");

            _logger.LogCritical("Critical system error: Database connection pool exhausted. Service shutting down.");

            _logger.LogTrace("Application trace: Exiting method DemonstrateLoggingLevels()");
        }

        public void DemonstrateStructuredLogging()
        {
            _logger.LogInformation("=== Demonstrating Structured Logging ===");

            var user = new User
            {
                Id = 123,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Role = "Administrator"
            };

            _logger.LogInformation("User created: {UserId} {UserName} {UserEmail} {UserRole}", 
                user.Id, user.Name, user.Email, user.Role);

            var logEvent = new LogEvent
            {
                EventType = "UserCreated",
                UserId = user.Id.ToString(),
                Action = "CreateUser",
                Resource = "UserManagement",
                Properties = new Dictionary<string, object>
                {
                    ["UserName"] = user.Name,
                    ["UserEmail"] = user.Email,
                    ["UserRole"] = user.Role,
                    ["CreatedBy"] = "System"
                }
            };

            _logger.LogInformation("Structured event: {@LogEvent}", logEvent);

            using (_logger.BeginScope("UserId:{UserId}", user.Id))
            {
                _logger.LogInformation("Processing user data validation");
                _logger.LogInformation("User data validation completed successfully");
            }
        }

        public void DemonstrateConditionalLogging()
        {
            _logger.LogInformation("=== Demonstrating Conditional Logging ===");

            for (int i = 1; i <= 5; i++)
            {
                var level = GetRandomLogLevel();
                var message = $"Processing item {i} with level {level}";

                switch (level)
                {
                    case LogLevel.Debug:
                        _logger.LogDebug("Debug: {Message}", message);
                        break;
                    case LogLevel.Information:
                        _logger.LogInformation("Info: {Message}", message);
                        break;
                    case LogLevel.Warning:
                        _logger.LogWarning("Warning: {Message}", message);
                        break;
                    case LogLevel.Error:
                        _logger.LogError("Error: {Message}", message);
                        break;
                }
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var expensiveData = GenerateExpensiveDebugData();
                _logger.LogDebug("Expensive debug data: {Data}", expensiveData);
            }
        }

        public void DemonstrateCustomPropertiesAndContext()
        {
            _logger.LogInformation("=== Demonstrating Custom Properties and Context ===");

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["OperationId"] = Guid.NewGuid().ToString(),
                ["RequestId"] = "REQ-" + DateTime.Now.Ticks,
                ["CorrelationId"] = "CORR-12345"
            }))
            {
                _logger.LogInformation("Starting complex operation");

                using (_logger.BeginScope("SubOperation:{SubOp}", "DataProcessing"))
                {
                    _logger.LogInformation("Processing data batch {BatchId}", "BATCH-001");
                    _logger.LogInformation("Data processing completed");
                }

                using (_logger.BeginScope("SubOperation:{SubOp}", "Validation"))
                {
                    _logger.LogInformation("Validating results");
                    _logger.LogInformation("Validation completed");
                }

                _logger.LogInformation("Complex operation completed successfully");
            }
        }

        public void DemonstrateMessageTemplates()
        {
            _logger.LogInformation("=== Demonstrating Message Templates ===");

            var orderId = "ORD-12345";
            var customerId = 67890;
            var amount = 99.99m;
            var currency = "USD";
            var timestamp = DateTime.UtcNow;

            _logger.LogInformation("Order {OrderId} created for customer {CustomerId}", orderId, customerId);
            
            _logger.LogInformation("Payment processed: {Amount:C} {Currency} for order {OrderId} at {Timestamp:yyyy-MM-dd HH:mm:ss}", 
                amount, currency, orderId, timestamp);

            var orderDetails = new
            {
                OrderId = orderId,
                CustomerId = customerId,
                Amount = amount,
                Currency = currency,
                Items = new[] { "Item1", "Item2", "Item3" },
                Timestamp = timestamp
            };

            _logger.LogInformation("Order details: {@OrderDetails}", orderDetails);

            var tags = new[] { "urgent", "vip-customer", "express-shipping" };
            _logger.LogInformation("Order {OrderId} tagged with: {Tags}", orderId, tags);
        }

        private int GetUserCount()
        {
            return _random.Next(100, 1000);
        }

        private LogLevel GetRandomLogLevel()
        {
            var levels = new[] { LogLevel.Debug, LogLevel.Information, LogLevel.Warning, LogLevel.Error };
            return levels[_random.Next(levels.Length)];
        }

        private string GenerateExpensiveDebugData()
        {
            System.Threading.Thread.Sleep(10);
            return $"Complex debug data generated at {DateTime.Now:HH:mm:ss.fff}";
        }
    }
}