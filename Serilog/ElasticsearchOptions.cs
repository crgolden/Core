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

    /// <summary>Configuration settings for the Elasticsearch logging.</summary>
    [PublicAPI]
    public class ElasticsearchOptions
    {
        /// <summary>Gets or sets the buffer clean payload.</summary>
        /// <value>The buffer clean payload.</value>
        public Func<string, long?, string, string> BufferCleanPayload { get; set; }

        /// <summary>Gets or sets the buffer index decider.</summary>
        /// <value>The buffer index decider.</value>
        public Func<string, DateTime, string> BufferIndexDecider { get; set; }

        /// <summary>Gets or sets the failure callback.</summary>
        /// <value>The failure callback.</value>
        public Action<LogEvent> FailureCallback { get; set; }

        /// <summary>Gets or sets the content of the get template.</summary>
        /// <value>The content of the get template.</value>
        public Func<object> GetTemplateContent { get; set; }

        /// <summary>Gets or sets the index decider.</summary>
        /// <value>The index decider.</value>
        public Func<LogEvent, DateTimeOffset, string> IndexDecider { get; set; }

        /// <summary>Gets or sets the modify connection settings.</summary>
        /// <value>The modify connection settings.</value>
        public Func<ConnectionConfiguration, ConnectionConfiguration> ModifyConnectionSettings { get; set; }

        /// <summary>Gets or sets the pipeline name decider.</summary>
        /// <value>The pipeline name decider.</value>
        public Func<LogEvent, string> PipelineNameDecider { get; set; }

        /// <summary>Gets the nodes.</summary>
        /// <value>The nodes.</value>
        public IList<Uri> Nodes { get; } = new List<Uri>();

        /// <summary>Gets the sink options.</summary>
        /// <value>The sink options.</value>
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

        /// <summary>Gets or sets a value indicating whether to automatically register template.</summary>
        /// <value>
        ///   <c>true</c> if set to automatically register template; otherwise, <c>false</c>.</value>
        public bool AutoRegisterTemplate { get; set; }

        /// <summary>Gets or sets the automatic register template version.</summary>
        /// <value>The automatic register template version.</value>
        public AutoRegisterTemplateVersion AutoRegisterTemplateVersion { get; set; }

        /// <summary>Gets or sets the register template failure.</summary>
        /// <value>The register template failure.</value>
        public RegisterTemplateRecovery RegisterTemplateFailure { get; set; } = IndexAnyway;

        /// <summary>Gets or sets the name of the template.</summary>
        /// <value>The name of the template.</value>
        public string TemplateName { get; set; } = "serilog-events-template";

        /// <summary>Gets or sets a value indicating whether to overwrite template.</summary>
        /// <value>
        ///   <c>true</c> if set to overwrite template; otherwise, <c>false</c>.</value>
        public bool OverwriteTemplate { get; set; }

        /// <summary>Gets or sets the number of shards.</summary>
        /// <value>The number of shards.</value>
        public int? NumberOfShards { get; set; }

        /// <summary>Gets or sets the number of replicas.</summary>
        /// <value>The number of replicas.</value>
        public int? NumberOfReplicas { get; set; }

        /// <summary>Gets or sets the index format.</summary>
        /// <value>The index format.</value>
        public string IndexFormat { get; set; } = "logstash-{0:yyyy.MM.dd}";

        /// <summary>Gets or sets the name of the dead letter index.</summary>
        /// <value>The name of the dead letter index.</value>
        public string DeadLetterIndexName { get; set; } = "deadletter-{0:yyyy.MM.dd}";

        /// <summary>Gets or sets the name of the type.</summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; set; } = ElasticsearchSinkOptions.DefaultTypeName;

        /// <summary>Gets or sets the name of the pipeline.</summary>
        /// <value>The name of the pipeline.</value>
        public string PipelineName { get; set; }

        /// <summary>Gets or sets the batch posting limit.</summary>
        /// <value>The batch posting limit.</value>
        public int BatchPostingLimit { get; set; } = 50;

        /// <summary>Gets or sets the single event size posting limit.</summary>
        /// <value>The single event size posting limit.</value>
        public long? SingleEventSizePostingLimit { get; set; }

        /// <summary>Gets or sets the period.</summary>
        /// <value>The period.</value>
        public TimeSpan Period { get; set; } = FromSeconds(2.0);

        /// <summary>Gets or sets the format provider.</summary>
        /// <value>The format provider.</value>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>Gets or sets the connection.</summary>
        /// <value>The connection.</value>
        public IConnection Connection { get; set; }

        /// <summary>Gets or sets the connection timeout.</summary>
        /// <value>The connection timeout.</value>
        public TimeSpan ConnectionTimeout { get; set; } = FromSeconds(5.0);

        /// <summary>Gets or sets a value indicating whether to inline fields.</summary>
        /// <value>
        ///   <c>true</c> if set to inline fields; otherwise, <c>false</c>.</value>
        public bool InlineFields { get; set; }

        /// <summary>Gets or sets the minimum log event level.</summary>
        /// <value>The minimum log event level.</value>
        public LogEventLevel? MinimumLogEventLevel { get; set; }

        /// <summary>Gets or sets the level switch.</summary>
        /// <value>The level switch.</value>
        public LoggingLevelSwitch LevelSwitch { get; set; }

        /// <summary>Gets or sets the serializer.</summary>
        /// <value>The serializer.</value>
        public IElasticsearchSerializer Serializer { get; set; }

        /// <summary>Gets or sets the buffer base filename.</summary>
        /// <value>The buffer base filename.</value>
        public string BufferBaseFilename { get; set; }

        /// <summary>Gets or sets the buffer file size limit bytes.</summary>
        /// <value>The buffer file size limit bytes.</value>
        public long? BufferFileSizeLimitBytes { get; set; } = 104857600L;

        /// <summary>Gets or sets the buffer log shipping interval.</summary>
        /// <value>The buffer log shipping interval.</value>
        public TimeSpan? BufferLogShippingInterval { get; set; }

        /// <summary>Gets or sets the buffer retained invalid payloads limit bytes.</summary>
        /// <value>The buffer retained invalid payloads limit bytes.</value>
        public long? BufferRetainedInvalidPayloadsLimitBytes { get; set; }

        /// <summary>Gets or sets the custom formatter.</summary>
        /// <value>The custom formatter.</value>
        public ITextFormatter CustomFormatter { get; set; } = new ExceptionAsObjectJsonFormatter(renderMessage: true);

        /// <summary>Gets or sets the custom durable formatter.</summary>
        /// <value>The custom durable formatter.</value>
        public ITextFormatter CustomDurableFormatter { get; set; }

        /// <summary>Gets or sets the emit event failure.</summary>
        /// <value>The emit event failure.</value>
        public EmitEventFailureHandling EmitEventFailure { get; set; } = WriteToSelfLog;

        /// <summary>Gets or sets the failure sink.</summary>
        /// <value>The failure sink.</value>
        public ILogEventSink FailureSink { get; set; }

        /// <summary>Gets or sets the queue size limit.</summary>
        /// <value>The queue size limit.</value>
        public int QueueSizeLimit { get; set; } = 100000;

        /// <summary>Gets or sets the buffer file count limit.</summary>
        /// <value>The buffer file count limit.</value>
        public int? BufferFileCountLimit { get; set; } = 31;

        /// <summary>Gets or sets a value indicating whether to format the stack trace as an array.</summary>
        /// <value>
        ///   <c>true</c> if set to format the stack trace as an array; otherwise, <c>false</c>.</value>
        public bool FormatStackTraceAsArray { get; set; }

        /// <summary>Gets the default name of the type.</summary>
        /// <value>The default name of the type.</value>
        public string DefaultTypeName { get; } = "logevent";

        /// <summary>Gets or sets a value indicating whether to detect the Elasticsearch version.</summary>
        /// <value>
        ///   <c>true</c> if set to detect the Elasticsearch version; otherwise, <c>false</c>.</value>
        public bool DetectElasticsearchVersion { get; set; }
    }
}
