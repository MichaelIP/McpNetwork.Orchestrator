namespace McpNetwork.Orchestrator.Extensions
{
    internal static class ExceptionExtension
    {
        public static string GetFirstMessage(this Exception exception)
        {
            var result = exception.Message;
            while (exception.InnerException != null)
            {
                result = exception.InnerException.Message;
                exception = exception.InnerException;
            }
            return result;
        }
    }
}
