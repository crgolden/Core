namespace Core
{
    using System;
    using System.Collections.Generic;
    using Elasticsearch.Net;
    using JetBrains.Annotations;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Elasticsearch;
    using Serilog.Sinks.Elasticsearch;
    using static System.Net.Security.SslPolicyErrors;
    using static System.TimeSpan;
    using static Serilog.Sinks.Elasticsearch.EmitEventFailureHandling;
    using static Serilog.Sinks.Elasticsearch.RegisterTemplateRecovery;

    [PublicAPI]
    public class ElasticsearchOptions
    {
        public Func<string, long?, string, string> BufferCleanPayload { get; set; }

        public Func<string, DateTime, string> BufferIndexDecider { get; set; }

        public Action<LogEvent> FailureCallback { get; set; }

        public Func<object> GetTemplateContent { get; set; }

        public Func<LogEvent, DateTimeOffset, string> IndexDecider { get; set; }

        public Func<ConnectionConfiguration, ConnectionConfiguration> ModifyConnectionSettings { get; set; }

        public Func<LogEvent, string> PipelineNameDecider { get; set; }

        public IList<Uri> Nodes { get; set; } = new List<Uri>();

        public ElasticsearchSinkOptions SinkOptions
        {
            get
            {
                var sinkOptions = new ElasticsearchSinkOptions(Nodes)
                {
                    AutoRegisterTemplate = AutoRegisterTemplate,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion,
                    BatchPostingLimit = BatchPostingLimit,
                    BufferBaseFilename = BufferBaseFilename,
                    BufferCleanPayload = BufferCleanPayload,
                    BufferFileCountLimit = BufferFileCountLimit,
                    BufferFileSizeLimitBytes = BufferFileSizeLimitBytes,
                    BufferIndexDecider = BufferIndexDecider,
                    BufferLogShippingInterval = BufferLogShippingInterval,
                    BufferRetainedInvalidPayloadsLimitBytes = BufferRetainedInvalidPayloadsLimitBytes,
                    Connection = Connection,
                    ConnectionTimeout = ConnectionTimeout,
                    CustomDurableFormatter = CustomDurableFormatter,
                    CustomFormatter = CustomFormatter,
                    DeadLetterIndexName = DeadLetterIndexName,
                    DetectElasticsearchVersion = DetectElasticsearchVersion,
                    EmitEventFailure = EmitEventFailure,
                    FailureCallback = FailureCallback,
                    FailureSink = FailureSink,
                    FormatProvider = FormatProvider,
                    FormatStackTraceAsArray = FormatStackTraceAsArray,
                    GetTemplateContent = GetTemplateContent,
                    IndexDecider = IndexDecider,
                    IndexFormat = IndexFormat,
                    InlineFields = InlineFields,
                    LevelSwitch = LevelSwitch,
                    MinimumLogEventLevel = MinimumLogEventLevel,
                    ModifyConnectionSettings = ModifyConnectionSettings,
                    NumberOfReplicas = NumberOfReplicas,
                    NumberOfShards = NumberOfShards,
                    OverwriteTemplate = OverwriteTemplate,
                    Period = Period,
                    PipelineName = PipelineName,
                    PipelineNameDecider = PipelineNameDecider,
                    QueueSizeLimit = QueueSizeLimit,
                    RegisterTemplateFailure = RegisterTemplateFailure,
                    Serializer = Serializer,
                    SingleEventSizePostingLimit = SingleEventSizePostingLimit,
                    TemplateName = TemplateName,
                    TypeName = TypeName
                };
                if (!sinkOptions.ConnectionPool.UsingSsl)
                {
                    sinkOptions.ModifyConnectionSettings = x =>
                    {
                        return x.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => arg4 == None);
                    };
                }

                return sinkOptions;
            }
        }

        public bool AutoRegisterTemplate { get; set; }

        public AutoRegisterTemplateVersion AutoRegisterTemplateVersion { get; set; }

        public RegisterTemplateRecovery RegisterTemplateFailure { get; set; } = IndexAnyway;

        public string TemplateName { get; set; } = "serilog-events-template";

        public bool OverwriteTemplate { get; set; }

        public int? NumberOfShards { get; set; }

        public int? NumberOfReplicas { get; set; }

        public string IndexFormat { get; set; } = "logstash-{0:yyyy.MM.dd}";

        public string DeadLetterIndexName { get; set; } = "deadletter-{0:yyyy.MM.dd}";

        public string TypeName { get; set; } = ElasticsearchSinkOptions.DefaultTypeName;

        public string PipelineName { get; set; }

        public int BatchPostingLimit { get; set; } = 50;

        public long? SingleEventSizePostingLimit { get; set; }

        public TimeSpan Period { get; set; } = FromSeconds(2.0);

        public IFormatProvider FormatProvider { get; set; }

        public IConnection Connection { get; set; }

        public TimeSpan ConnectionTimeout { get; set; } = FromSeconds(5.0);

        public bool InlineFields { get; set; }

        public LogEventLevel? MinimumLogEventLevel { get; set; }

        public LoggingLevelSwitch LevelSwitch { get; set; }

        public IElasticsearchSerializer Serializer { get; set; }

        public string BufferBaseFilename { get; set; }

        public long? BufferFileSizeLimitBytes { get; set; } = 104857600L;

        public TimeSpan? BufferLogShippingInterval { get; set; }

        public long? BufferRetainedInvalidPayloadsLimitBytes { get; set; }

        public ITextFormatter CustomFormatter { get; set; } = new ExceptionAsObjectJsonFormatter(renderMessage: true);

        public ITextFormatter CustomDurableFormatter { get; set; }

        public EmitEventFailureHandling EmitEventFailure { get; set; } = WriteToSelfLog;

        public ILogEventSink FailureSink { get; set; }

        public int QueueSizeLimit { get; set; } = 100000;

        public int? BufferFileCountLimit { get; set; } = 31;

        public bool FormatStackTraceAsArray { get; set; }

        public static string DefaultTypeName { get; } = "logevent";

        public bool DetectElasticsearchVersion { get; set; }
    }
}
