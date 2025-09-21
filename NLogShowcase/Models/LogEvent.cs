using System;

namespace NLogShowcase.Models
{
    public class LogEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public string EventType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Properties { get; set; } = new();
        public TimeSpan? Duration { get; set; }
        public bool Success { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PerformanceMetrics
    {
        public string OperationName { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public long MemoryUsed { get; set; }
        public int ItemsProcessed { get; set; }
        public double ThroughputPerSecond => ItemsProcessed / Duration.TotalSeconds;
    }
}