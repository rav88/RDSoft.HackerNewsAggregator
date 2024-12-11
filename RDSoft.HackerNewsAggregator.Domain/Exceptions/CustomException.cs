namespace RDSoft.HackerNewsAggregator.Domain.Exceptions
{
	public class CustomException : Exception
	{
		public string? ErrorCode { get; }

		public object? ErrorDetails { get; }

		public CustomException(string message, string? errorCode = null, object? errorDetails = null)
			: base(message)
		{
			ErrorCode = errorCode;
			ErrorDetails = errorDetails;
		}

		public CustomException(string message, Exception innerException, string? errorCode = null, object? errorDetails = null)
			: base(message, innerException)
		{
			ErrorCode = errorCode;
			ErrorDetails = errorDetails;
		}

		public override string ToString()
		{
			var baseMessage = base.ToString();
			return $"{baseMessage}\nErrorCode: {ErrorCode}\nErrorDetails: {ErrorDetails}";
		}
	}
}
