# NLog Showcase

[![Build and Test](https://github.com/AdelSS04/NlogConsoleLog/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/AdelSS04/NlogConsoleLog/actions/workflows/build-and-test.yml)
[![Simple Build Check](https://github.com/AdelSS04/NlogConsoleLog/actions/workflows/quick-build.yml/badge.svg)](https://github.com/AdelSS04/NlogConsoleLog/actions/workflows/quick-build.yml)

A comprehensive interactive demonstration of NLog logging framework for .NET applications. This project provides hands-on exploration of NLog's powerful features through an intuitive menu-driven interface.

## 🎯 Interactive Menu System

The NLog Showcase features an interactive console menu with 11 different demonstrations, each designed to showcase specific NLog capabilities and logging best practices.

### � Complete Menu Options

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                            NLog Showcase Menu                               ║
╚══════════════════════════════════════════════════════════════════════════════╝

Choose a demonstration to run:

  1. Logging Levels Demo          - Show all log levels (Trace to Critical)
  2. Structured Logging Demo      - Demonstrate parameterized logging
  3. Conditional Logging Demo     - Show filtering and conditional logging
  4. Custom Properties Demo       - Logging with scopes and context
  5. Message Templates Demo       - Different formatting and templates
  6. Exception Handling Demo      - Exception logging best practices
  7. Performance Monitoring Demo  - Timing and performance metrics
  8. Run All Basic Demos          - Execute all logging demonstrations
  9. Run All Exception Demos      - Execute all exception demonstrations
 10. Run All Performance Demos    - Execute all performance demonstrations
 11. Run Complete Showcase        - Execute everything!

  0. Exit
```

## 🔍 Detailed Demo Explanations

### 1. Logging Levels Demo
**Purpose**: Demonstrates all NLog logging levels from Trace to Critical
**What it shows**:
- **Trace**: Very detailed diagnostic information (method entry/exit)
- **Debug**: Diagnostic information useful for debugging
- **Information**: General informational messages about application flow
- **Warning**: Potentially harmful situations that don't stop execution
- **Error**: Error events that don't stop the application
- **Critical**: Very serious error events that might terminate the application

**When to use**: Understanding the hierarchy and appropriate usage of log levels in your applications.

**Sample Output**:
```
Application trace: Entering method DemonstrateLoggingLevels()
Debug information: Current user count is 847
User 12345 successfully logged in from 192.168.1.100
Warning: API response time 1500ms is above threshold 1000ms
Failed to process order ORD-789: Invalid payment method
Critical system error: Database connection pool exhausted. Service shutting down.
```

### 2. Structured Logging Demo
**Purpose**: Shows how to use parameterized logging with named placeholders
**What it shows**:
- Named parameter logging with `{ParameterName}` syntax
- Complex object serialization using `{@ObjectName}` syntax
- Log scopes for contextual information
- Structured data that can be parsed by log analysis tools

**When to use**: Building modern applications that need searchable, filterable logs for monitoring and debugging.

**Sample Output**:
```
User created: 123 John Doe john.doe@example.com Administrator
Structured event: {"EventId":"guid","EventType":"UserCreated","UserId":"123",...}
Processing user data validation (within scope UserId:123)
```

### 3. Conditional Logging Demo
**Purpose**: Demonstrates smart logging that adapts based on configuration and conditions
**What it shows**:
- Performance-conscious logging with `IsEnabled()` checks
- Random log level generation for testing filters
- Expensive operation logging that only runs when debug is enabled
- Log filtering in action

**When to use**: Optimizing application performance by avoiding expensive logging operations when not needed.

**Sample Output**:
```
Processing item 1 with level Information
Debug: Processing item 2 with level Debug
Warning: Processing item 3 with level Warning
Expensive debug data: Complex debug data generated at 15:30:25.123
```

### 4. Custom Properties Demo
**Purpose**: Shows advanced logging context and correlation capabilities
**What it shows**:
- Nested logging scopes for operation tracking
- Custom properties attached to log entries
- Request correlation IDs for distributed tracing
- Hierarchical scope management

**When to use**: Building enterprise applications that need request tracing, correlation, and contextual debugging.

**Sample Output**:
```
Starting complex operation [OperationId: guid-123, RequestId: REQ-456, CorrelationId: CORR-12345]
  Processing data batch BATCH-001 [SubOperation: DataProcessing]
  Validating results [SubOperation: Validation]
Complex operation completed successfully
```

### 5. Message Templates Demo
**Purpose**: Demonstrates various formatting options and template styles
**What it shows**:
- Different parameter formatting (currency, dates, etc.)
- Object serialization and array logging
- Custom format strings
- Template reusability patterns

**When to use**: Creating consistent, well-formatted log messages that are both human-readable and machine-parseable.

**Sample Output**:
```
Order ORD-12345 created for customer 67890
Payment processed: $99.99 USD for order ORD-12345 at 2024-01-15 10:30:15
Order details: {"OrderId":"ORD-12345","CustomerId":67890,"Amount":99.99,...}
Order ORD-12345 tagged with: ["urgent", "vip-customer", "express-shipping"]
```

### 6. Exception Handling Demo
**Purpose**: Comprehensive exception logging patterns and best practices
**What it shows**:
- Basic exception logging with context
- Exception logging with additional contextual information
- Nested exception unwrapping and logging
- Retry patterns with exception tracking
- Custom exception types with structured data

**When to use**: Building robust applications that need comprehensive error tracking and debugging capabilities.

**Sample Output**:
```
Argument validation failed for operation ValidateInput
Business logic error occurred while processing order ORD456 for user USER123
Operation failed with nested exceptions. Root cause analysis required.
Operation attempt 1/3 failed. Retrying in 1000ms
Business rule violation: INSUFFICIENT_FUNDS - Account balance insufficient
```

### 7. Performance Monitoring Demo
**Purpose**: Advanced performance tracking and metrics collection
**What it shows**:
- Basic operation timing with Stopwatch
- Complex multi-phase operation timing
- Batch processing metrics with progress tracking
- Memory usage monitoring and garbage collection tracking
- Async operation timing patterns

**When to use**: Monitoring application performance, identifying bottlenecks, and tracking resource usage.

**Sample Output**:
```
Operation completed successfully in 247ms
Complex operation completed: Total: 425ms, Phase1: 127ms, Phase2: 198ms, Phase3: 75ms
Batch processing completed: 50 items processed, 2 errors, 96.0% success rate, 12.3 items/sec
Memory operation completed: Peak memory: 104,857,600 bytes (+100MB), Final: 4,194,304 bytes
Async operation completed: 5 tasks in 189ms
```

### 8. Run All Basic Demos
**Purpose**: Executes demonstrations 1-5 in sequence
**What it shows**: Complete overview of fundamental NLog features
**When to use**: Getting a comprehensive understanding of basic logging capabilities

### 9. Run All Exception Demos
**Purpose**: Executes all exception-related demonstrations
**What it shows**: Complete exception handling and error logging patterns
**When to use**: Learning comprehensive error handling strategies

### 10. Run All Performance Demos
**Purpose**: Executes all performance monitoring demonstrations
**What it shows**: Complete performance tracking and metrics collection
**When to use**: Understanding performance monitoring capabilities

### 11. Run Complete Showcase
**Purpose**: Executes every demonstration in the application
**What it shows**: Full spectrum of NLog capabilities
**When to use**: Comprehensive evaluation of NLog features or demonstration purposes

## 🛠️ NLog Configuration Features

The showcase includes three different NLog configurations:

### Main Configuration (`nlog.config`)
- **Multiple Targets**: ColoredConsole, File, JSON, Memory, Debug
- **Smart Filtering**: Level-based and logger-based filtering
- **Log Rotation**: Daily rotation with configurable retention
- **Performance Optimization**: Async targets and efficient layouts

### Development Configuration (`nlog.development.config`)
- **Verbose Logging**: Shows all log levels including Trace and Debug
- **Enhanced Console Output**: Colored console with detailed formatting
- **Quick Feedback**: Optimized for development workflow

### Production Configuration (`nlog.production.config`)
- **Performance Optimized**: Async wrappers and efficient targets
- **Minimal Noise**: Filtered Microsoft framework logs
- **Long Retention**: Extended log retention for error logs
- **JSON Format**: Machine-readable logs for analysis tools

## 🚀 Quick Start

1. **Clone and Run**:
   ```bash
   git clone https://github.com/AdelSS04/NlogConsoleLog.git
   cd NlogConsoleLog
   dotnet build
   dotnet run --project NLogShowcase
   ```

2. **Explore Features**: Choose any menu option (1-11) to see specific NLog capabilities

3. **Check Logs**: All demos generate logs in multiple formats in the `logs/` folder

## � Log Output Locations

- **Console**: Real-time colored output during demo execution
- **Text Files**: `logs/nlog-YYYY-MM-DD.log` - Detailed text logs
- **JSON Files**: `logs/nlog-YYYY-MM-DD.json` - Structured JSON logs
- **Error Files**: `logs/errors-YYYY-MM-DD.log` - Error-only logs
- **Performance Files**: `logs/performance-YYYY-MM-DD.log` - Performance metrics

## 🎓 Learning Outcomes

After exploring this showcase, you'll understand:

- **Log Level Strategy**: When and how to use different log levels
- **Structured Logging**: Building searchable and analyzable logs
- **Performance Logging**: Monitoring application performance through logs
- **Exception Patterns**: Best practices for error logging and debugging
- **Configuration Management**: Setting up NLog for different environments
- **Modern Logging**: Contemporary logging practices for .NET applications

## 🔧 Technical Requirements

- .NET 8.0 or later
- Windows, macOS, or Linux
- Console/Terminal application

## � CI/CD Pipeline

This project includes comprehensive GitHub Actions workflows:

### Build and Test Workflow
- **Triggers**: Push and Pull Requests to main/master branches
- **Multi-platform**: Tests on Ubuntu, Windows, and macOS
- **Features**:
  - .NET 8.0 setup and package caching
  - Build verification in Release configuration
  - Application startup verification using `--verify` mode
  - Build artifact upload for review

### Quick Build Check
- **Purpose**: Fast feedback for pull requests
- **Features**: Rapid build verification and basic functionality test

### Release Workflow
- **Triggers**: Version tags (v*)
- **Features**:
  - Cross-platform binary compilation (Windows, Linux, macOS)
  - Self-contained executable generation
  - Automatic GitHub release creation with binaries
  - Release notes generation

### Verification Mode
The application includes a special `--verify` mode for CI/CD testing:
```bash
dotnet run -- --verify
```
This mode runs a quick verification of all services and basic logging functionality without requiring user interaction.

## �📝 License

MIT License - see [LICENSE](LICENSE) for details

---

**Start exploring NLog's powerful logging capabilities today!** 

Each menu option provides hands-on experience with real working code and immediate visual feedback through both console output and generated log files.