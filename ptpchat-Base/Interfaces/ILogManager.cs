namespace PtpChat.Base.Interfaces
{
	/// <summary>
	/// Handles logging for this program
	/// </summary>
	public interface ILogManager
    {

		/// <summary>
		/// Log at DEBUG level, more verbose than INFO, for debugging.
		/// <param name="message">What will be added to the log at this level.</param>
		/// </summary>
		void Debug(string message);

		/// <summary>
		/// Log at INFO level, for general information not specific to errors.
		/// <param name="message">What will be added to the log at this level.</param>
		/// </summary>
		void Info(string message);

		/// <summary>
		/// Log at WARN level, an issue or error that does not disrupt the program massivley.
		/// <param name="message">What will be added to the log at this level.</param>
		/// </summary>
		void Warning(string message);

		/// <summary>
		/// Log at ERROR level, an event which has prevented normal operation.
		/// <param name="message">What will be added to the log at this level.</param>
		/// </summary>
		void Error(string message);

		/// <summary>
		/// Log at FATAL level, an event which will now stop the program from running.
		/// <param name="message">What will be added to the log at this level.</param>
		/// </summary>
		void Fatal(string message);

	}
}