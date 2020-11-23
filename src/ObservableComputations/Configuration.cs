namespace ObservableComputations
{
	public static class Configuration
	{
		public static bool SaveInstantiatingStackTrace = false;
		public static bool SaveOcDispatcherInvocationStackTrace = false;

		public static bool TrackComputingsExecutingUserCode = false;
		public static bool TrackOcDispatcherInvocations = false;
		public static int EventUnsubscriberThreadsCount = 1;
	}
}
