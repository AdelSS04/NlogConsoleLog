using Microsoft.Extensions.Logging;

namespace NLogShowcase.Services
{
    public class ExceptionDemoService
    {
        private readonly ILogger<ExceptionDemoService> _logger;
        private static readonly Random _random = new();

        public ExceptionDemoService(ILogger<ExceptionDemoService> logger)
        {
            _logger = logger;
        }

        public void DemonstrateExceptionLogging()
        {
            _logger.LogInformation("=== Demonstrating Exception Logging ===");

            DemonstrateBasicExceptionLogging();
            DemonstrateExceptionWithContext();
            DemonstrateNestedExceptions();
            DemonstrateExceptionRecovery();
            DemonstrateCustomExceptions();
        }

        private void DemonstrateBasicExceptionLogging()
        {
            _logger.LogInformation("--- Basic Exception Logging ---");

            try
            {
                SimulateArgumentException();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument validation failed for operation {Operation}", "ValidateInput");
            }

            try
            {
                SimulateFileNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Required file not found: {FileName}", ex.FileName);
            }

            try
            {
                SimulateInvalidOperationException();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation attempted during {Phase}", "DataProcessing");
            }
        }

        private void DemonstrateExceptionWithContext()
        {
            _logger.LogInformation("--- Exception Logging with Context ---");

            var userId = "USER123";
            var orderId = "ORD456";
            var sessionId = Guid.NewGuid().ToString();

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["UserId"] = userId,
                ["OrderId"] = orderId,
                ["SessionId"] = sessionId
            }))
            {
                try
                {
                    SimulateBusinessLogicException();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Business logic error occurred while processing order {OrderId} for user {UserId}", 
                        orderId, userId);
                }
            }
        }

        private void DemonstrateNestedExceptions()
        {
            _logger.LogInformation("--- Nested Exception Logging ---");

            try
            {
                SimulateNestedExceptions();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Operation failed with nested exceptions. Root cause analysis required.");
                
                var currentEx = ex;
                int level = 0;
                while (currentEx != null)
                {
                    _logger.LogDebug("Exception level {Level}: {ExceptionType} - {Message}", 
                        level, currentEx.GetType().Name, currentEx.Message);
                    currentEx = currentEx.InnerException;
                    level++;
                }
            }
        }

        private void DemonstrateExceptionRecovery()
        {
            _logger.LogInformation("--- Exception Recovery and Retry Logging ---");

            const int maxRetries = 3;
            var operationId = Guid.NewGuid().ToString();

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogDebug("Attempting operation {OperationId}, attempt {Attempt}/{MaxRetries}", 
                        operationId, attempt, maxRetries);

                    if (ShouldSimulateFailure() && attempt < maxRetries)
                    {
                        throw new TimeoutException($"Operation timed out on attempt {attempt}");
                    }

                    _logger.LogInformation("Operation {OperationId} completed successfully on attempt {Attempt}", 
                        operationId, attempt);
                    break;
                }
                catch (Exception ex) when (attempt < maxRetries)
                {
                    _logger.LogWarning(ex, "Operation {OperationId} failed on attempt {Attempt}/{MaxRetries}. Retrying in {DelayMs}ms", 
                        operationId, attempt, maxRetries, 1000 * attempt);
                    
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Operation {OperationId} failed permanently after {MaxRetries} attempts", 
                        operationId, maxRetries);
                    throw;
                }
            }
        }

        private void DemonstrateCustomExceptions()
        {
            _logger.LogInformation("--- Custom Exception Types ---");

            try
            {
                throw new BusinessRuleViolationException("INSUFFICIENT_FUNDS", 
                    "Account balance insufficient for transaction", 
                    new Dictionary<string, object>
                    {
                        ["AccountId"] = "ACC123",
                        ["RequestedAmount"] = 1000.00m,
                        ["AvailableBalance"] = 250.00m,
                        ["TransactionId"] = "TXN789"
                    });
            }
            catch (BusinessRuleViolationException ex)
            {
                _logger.LogError(ex, "Business rule violation: {RuleCode} - {Message}. Context: {@ErrorContext}", 
                    ex.RuleCode, ex.Message, ex.Context);
            }

            try
            {
                throw new ValidationException("Email address format is invalid")
                    .AddValidationError("Email", "john.doe@", "Invalid email format")
                    .AddValidationError("Age", "-5", "Age must be positive");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed with {ErrorCount} errors: {@ValidationErrors}", 
                    ex.ValidationErrors.Count, ex.ValidationErrors);
            }
        }

        private void SimulateArgumentException()
        {
            throw new ArgumentException("Invalid parameter value", "parameterName");
        }

        private void SimulateFileNotFoundException()
        {
            throw new FileNotFoundException("Configuration file not found", "config.json");
        }

        private void SimulateInvalidOperationException()
        {
            throw new InvalidOperationException("Cannot perform operation in current state");
        }

        private void SimulateBusinessLogicException()
        {
            throw new InvalidOperationException("Business rule validation failed: Customer credit limit exceeded");
        }

        private void SimulateNestedExceptions()
        {
            try
            {
                try
                {
                    throw new ArgumentException("Invalid input parameter");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Data processing failed", ex);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Application operation failed", ex);
            }
        }

        private bool ShouldSimulateFailure()
        {
            return _random.Next(1, 4) == 1;
        }
    }

    public class BusinessRuleViolationException : Exception
    {
        public string RuleCode { get; }
        public Dictionary<string, object> Context { get; }

        public BusinessRuleViolationException(string ruleCode, string message, Dictionary<string, object>? context = null) 
            : base(message)
        {
            RuleCode = ruleCode;
            Context = context ?? new Dictionary<string, object>();
        }
    }

    public class ValidationException : Exception
    {
        public List<ValidationError> ValidationErrors { get; } = new();

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException AddValidationError(string field, string value, string error)
        {
            ValidationErrors.Add(new ValidationError { Field = field, Value = value, Error = error });
            return this;
        }
    }

    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}