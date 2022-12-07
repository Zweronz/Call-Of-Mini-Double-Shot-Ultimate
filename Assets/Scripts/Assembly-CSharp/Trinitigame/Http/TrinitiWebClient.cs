using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Trinitigame.Http
{
	public class TrinitiWebClient
	{
		private const int MAX_HTTP_BUFF_LEN = 10240;

		private HttpResponseDelegate onHttpResponse;

		private TcpClient tcpClient;

		private Thread thrSocketReader;

		private bool m_bEncrypt;

		public HttpResponseDelegate OnHttpResponse
		{
			get
			{
				return onHttpResponse;
			}
			set
			{
				onHttpResponse = value;
			}
		}

		public void UploadValuesAsync(Uri uri, string url, string actionName, string jsonData)
		{
			//Discarded unreachable code: IL_0057
			bool flag = false;
			try
			{
				IPAddress address = IPAddress.Parse(uri.Host);
				tcpClient = new TcpClient();
				tcpClient.Client.Connect(address, uri.Port);
			}
			catch (Exception ex)
			{
				OnHttpResponse(true, "Http error creating http connection: " + ex.ToString());
				return;
			}
			try
			{
				string empty = string.Empty;
				byte[] bytes = Encoding.UTF8.GetBytes(empty);
				StringBuilder stringBuilder = new StringBuilder();
				string text = url + "?action=" + actionName + "&json=" + jsonData;
				stringBuilder.Append("POST " + text + " HTTP/1.0\r\n");
				stringBuilder.Append("Content-Type: application/x-www-form-urlencoded; charset=utf-8\r\n");
				stringBuilder.AppendFormat("Content-Length: {0}\r\n", bytes.Length);
				stringBuilder.Append("\r\n");
				stringBuilder.Append(empty);
				StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream());
				streamWriter.Write(stringBuilder.ToString() + '\0');
				streamWriter.Flush();
			}
			catch (Exception ex2)
			{
				flag = true;
				tcpClient.Close();
				OnHttpResponse(true, "Error during http request: " + ex2.ToString() + " " + ex2.StackTrace);
			}
			if (!flag)
			{
				thrSocketReader = new Thread(ThreadProc);
				thrSocketReader.Start();
			}
		}

		private void ThreadProc()
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = new byte[10240];
			try
			{
				int num;
				if ((num = tcpClient.GetStream().Read(array, 0, 10240)) > 0)
				{
					byte[] array2 = new byte[num];
					Buffer.BlockCopy(array, 0, array2, 0, num);
					stringBuilder.Append(Encoding.UTF8.GetString(array2));
					array = new byte[10240];
				}
			}
			catch (Exception ex)
			{
				OnHttpResponse(true, "Error during ThreadProc: " + ex.ToString() + " " + ex.StackTrace);
			}
			finally
			{
				tcpClient.Close();
			}
			string[] array3 = Regex.Split(stringBuilder.ToString(), "\r\n\r\n");
			string message = Regex.Replace(array3[1], "\\s+$", string.Empty);
			OnHttpResponse(false, message);
		}
	}
}
