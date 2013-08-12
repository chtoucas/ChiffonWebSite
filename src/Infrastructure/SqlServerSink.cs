namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Serilog.Events;
    using Serilog.Formatting.Json;
    using Serilog.Sinks.PeriodicBatching;

    public class SqlServerSink : PeriodicBatchingSink
    {
        public SqlServerSink(int batchSizeLimit, TimeSpan period) : base(batchSizeLimit, period) { }

        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            base.EmitBatch(events);
        }
    }
}