using System;

namespace Trinitigame.Http
{
	public class HttpWrapper
	{
		private const string TrinitiHttpServerUrl = "http://173.201.190.171:8781";

		private const string TrinitiHttpGameServerDo = "/gameapi/dshotGame.do";

		private const string TrinitiHttpPlatformServerDo = "/gameapi/dshotAccount.do";

		public static void SendJsonToGameServer(HttpResponseDelegate httpResponseDelegate, string actionName, string jsonText)
		{
			TrinitiWebClient trinitiWebClient = new TrinitiWebClient();
			trinitiWebClient.OnHttpResponse = httpResponseDelegate;
			Uri uri = new Uri("http://173.201.190.171:8781");
			trinitiWebClient.UploadValuesAsync(uri, "/gameapi/dshotGame.do", actionName, jsonText);
		}

		public static void SendJsonToPlatformServer(HttpResponseDelegate httpResponseDelegate, string actionName, string jsonText)
		{
			TrinitiWebClient trinitiWebClient = new TrinitiWebClient();
			trinitiWebClient.OnHttpResponse = httpResponseDelegate;
			Uri uri = new Uri("http://173.201.190.171:8781");
			trinitiWebClient.UploadValuesAsync(uri, "/gameapi/dshotAccount.do", actionName, jsonText);
		}
	}
}
