using Microsoft.Extensions.Logging;
using NLogShowcase.Models;
using System.Diagnostics;

namespace NLogShowcase.Services
{
    public class PerformanceDemoService
    {
        private readonly ILogger<PerformanceDemoService> _logger;
        private static readonly Random _random = new();

        public PerformanceDemoService(ILogger<PerformanceDemoService> logger)
        {
            _logger = logger;
        }

        public async Task DemonstratePerformanceLogging()
        {
            _logger.LogInformation("=== Demonstrating Performance Logging ===");

            DemonstrateBasicTiming();
            DemonstrateComplexOperationTiming();
            DemonstrateBatchProcessingMetrics();
            DemonstrateMemoryUsageTracking();
            await DemonstrateAsyncOperationTiming();
        }

        private void DemonstrateBasicTiming()
        {
            _logger.LogInformation("--- Basic Operation Timing ---");

            var stopwatch = Stopwatch.StartNew();
            var operationId = Guid.NewGuid().ToString();

            _logger.LogInformation("Starting operation {OperationId}", operationId);

            try
            {
                SimulateWork(100, 300);

                stopwatch.Stop();
                _logger.LogInformation("Operation {OperationId} completed successfully in {Duration}ms", 
                    operationId, stopwatch.ElapsedMilliseconds);

                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["Duration"] = stopwatch.ElapsedMilliseconds,
                    ["OperationId"] = operationId,
                    ["Success"] = true
                }))
                {
                    _logger.LogInformation("Performance metrics recorded for operation {OperationId}", operationId);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Operation {OperationId} failed after {Duration}ms", 
                    operationId, stopwatch.ElapsedMilliseconds);
            }
        }

        private void DemonstrateComplexOperationTiming()
        {
            _logger.LogInformation("--- Complex Operation with Nested Timing ---");

            var mainStopwatch = Stopwatch.StartNew();
            var operationId = Guid.NewGuid().ToString();

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["OperationId"] = operationId,
                ["OperationType"] = "ComplexDataProcessing"
            }))
            {
                _logger.LogInformation("Starting complex operation {OperationId}", operationId);

                var phase1Timer = Stopwatch.StartNew();
                _logger.LogDebug("Phase 1: Loading data");
                SimulateWork(50, 150);
                phase1Timer.Stop();
                _logger.LogInformation("Phase 1 completed in {Duration}ms", phase1Timer.ElapsedMilliseconds);

                var phase2Timer = Stopwatch.StartNew();
                _logger.LogDebug("Phase 2: Processing data");
                SimulateWork(100, 200);
                phase2Timer.Stop();
                _logger.LogInformation("Phase 2 completed in {Duration}ms", phase2Timer.ElapsedMilliseconds);

                var phase3Timer = Stopwatch.StartNew();
                _logger.LogDebug("Phase 3: Validating results");
                SimulateWork(25, 75);
                phase3Timer.Stop();
                _logger.LogInformation("Phase 3 completed in {Duration}ms", phase3Timer.ElapsedMilliseconds);

                mainStopwatch.Stop();

                var metrics = new PerformanceMetrics
                {
                    OperationName = "ComplexDataProcessing",
                    Duration = mainStopwatch.Elapsed,
                    ItemsProcessed = 1000,
                    MemoryUsed = GC.GetTotalMemory(false)
                };

                _logger.LogInformation("Complex operation {OperationId} completed. " +
                    "Total: {TotalDuration}ms, Phase1: {Phase1Duration}ms, Phase2: {Phase2Duration}ms, Phase3: {Phase3Duration}ms, " +
                    "Throughput: {Throughput:F2} items/sec",
                    operationId, mainStopwatch.ElapsedMilliseconds, 
                    phase1Timer.ElapsedMilliseconds, phase2Timer.ElapsedMilliseconds, phase3Timer.ElapsedMilliseconds,
                    metrics.ThroughputPerSecond);
            }
        }

        private void DemonstrateBatchProcessingMetrics()
        {
            _logger.LogInformation("--- Batch Processing Metrics ---");

            const int batchSize = 10;
            const int totalItems = 50;
            var batchId = Guid.NewGuid().ToString();
            var overallStopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Starting batch processing {BatchId}: {TotalItems} items in batches of {BatchSize}", 
                batchId, totalItems, batchSize);

            var processedItems = 0;
            var errors = 0;

            for (int batch = 1; batch <= (totalItems / batchSize); batch++)
            {
                var batchStopwatch = Stopwatch.StartNew();
                var batchItems = Math.Min(batchSize, totalItems - processedItems);

                try
                {
                    _logger.LogDebug("Processing batch {BatchNumber}/{TotalBatches} with {ItemCount} items", 
                        batch, totalItems / batchSize, batchItems);

                    for (int item = 1; item <= batchItems; item++)
                    {
                        SimulateItemProcessing();
                        processedItems++;

                        if (_random.Next(1, 20) == 1)
                        {
                            errors++;
                            _logger.LogWarning("Error processing item {ItemNumber} in batch {BatchNumber}", 
                                item, batch);
                        }
                    }

                    batchStopwatch.Stop();

                    var itemsPerSecond = batchItems / batchStopwatch.Elapsed.TotalSeconds;
                    _logger.LogInformation("Batch {BatchNumber} completed: {ItemCount} items in {Duration}ms " +
                        "({ItemsPerSecond:F2} items/sec)",
                        batch, batchItems, batchStopwatch.ElapsedMilliseconds, itemsPerSecond);
                }
                catch (Exception ex)
                {
                    batchStopwatch.Stop();
                    _logger.LogError(ex, "Batch {BatchNumber} failed after {Duration}ms", 
                        batch, batchStopwatch.ElapsedMilliseconds);
                    errors++;
                }
            }

            overallStopwatch.Stop();

            var successRate = (double)(processedItems - errors) / processedItems * 100;
            var overallThroughput = processedItems / overallStopwatch.Elapsed.TotalSeconds;

            _logger.LogInformation("Batch processing {BatchId} completed: {ProcessedItems} items processed, " +
                "{Errors} errors, {SuccessRate:F1}% success rate, {OverallThroughput:F2} items/sec, " +
                "Total duration: {TotalDuration}ms",
                batchId, processedItems, errors, successRate, overallThroughput, 
                overallStopwatch.ElapsedMilliseconds);
        }

        private void DemonstrateMemoryUsageTracking()
        {
            _logger.LogInformation("--- Memory Usage Tracking ---");

            var operationId = Guid.NewGuid().ToString();
            var initialMemory = GC.GetTotalMemory(true);

            _logger.LogInformation("Starting memory-intensive operation {OperationId}. Initial memory: {InitialMemory:N0} bytes", 
                operationId, initialMemory);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var data = new List<byte[]>();
                for (int i = 0; i < 100; i++)
                {
                    data.Add(new byte[1024 * 1024]);
                    
                    if (i % 25 == 0)
                    {
                        var currentMemory = GC.GetTotalMemory(false);
                        var memoryIncrease = currentMemory - initialMemory;
                        _logger.LogDebug("Memory checkpoint {Checkpoint}: {CurrentMemory:N0} bytes " +
                            "(+{MemoryIncrease:N0} bytes from start)",
                            i / 25 + 1, currentMemory, memoryIncrease);
                    }
                }

                var peakMemory = GC.GetTotalMemory(false);
                
                data.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                
                var finalMemory = GC.GetTotalMemory(false);
                stopwatch.Stop();

                _logger.LogInformation("Memory operation {OperationId} completed in {Duration}ms. " +
                    "Peak memory: {PeakMemory:N0} bytes (+{PeakIncrease:N0}), " +
                    "Final memory: {FinalMemory:N0} bytes (+{FinalIncrease:N0})",
                    operationId, stopwatch.ElapsedMilliseconds,
                    peakMemory, peakMemory - initialMemory,
                    finalMemory, finalMemory - initialMemory);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var errorMemory = GC.GetTotalMemory(false);
                _logger.LogError(ex, "Memory operation {OperationId} failed after {Duration}ms. " +
                    "Memory at error: {ErrorMemory:N0} bytes",
                    operationId, stopwatch.ElapsedMilliseconds, errorMemory);
            }
        }

        private async Task DemonstrateAsyncOperationTiming()
        {
            _logger.LogInformation("--- Async Operation Timing ---");

            var operationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Starting async operation {OperationId}", operationId);

            try
            {
                var tasks = new List<Task>();
                for (int i = 1; i <= 5; i++)
                {
                    var taskId = i;
                    tasks.Add(Task.Run(async () =>
                    {
                        var taskStopwatch = Stopwatch.StartNew();
                        _logger.LogDebug("Starting async task {TaskId} for operation {OperationId}", taskId, operationId);
                        
                        await Task.Delay(_random.Next(50, 200));
                        
                        taskStopwatch.Stop();
                        _logger.LogDebug("Async task {TaskId} completed in {Duration}ms", taskId, taskStopwatch.ElapsedMilliseconds);
                    }));
                }

                await Task.WhenAll(tasks);
                stopwatch.Stop();

                _logger.LogInformation("Async operation {OperationId} completed: {TaskCount} tasks in {Duration}ms", 
                    operationId, tasks.Count, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Async operation {OperationId} failed after {Duration}ms", 
                    operationId, stopwatch.ElapsedMilliseconds);
            }
        }

        private void SimulateWork(int minMs, int maxMs)
        {
            var delay = _random.Next(minMs, maxMs);
            System.Threading.Thread.Sleep(delay);
        }

        private void SimulateItemProcessing()
        {
            var delay = _random.Next(1, 10);
            System.Threading.Thread.Sleep(delay);
        }
    }
}