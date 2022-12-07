using System.Xml;
using UnityEngine;

public class GameParametersXML
{
	public void Load()
	{
		string path = "config.xml";
		XmlReader xmlReader = null;
		TextAsset textAsset = (TextAsset)Resources.Load(path);
		if (textAsset == null)
		{
			Debug.Log("XMLFile null");
			return;
		}
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.ProhibitDtd = false;
		xmlReader = XmlReader.Create(textAsset.text, xmlReaderSettings);
		ReadXml(xmlReader);
		if (xmlReader != null)
		{
			xmlReader.Close();
		}
	}

	private static void ReadXml(XmlReader reader)
	{
		while (reader.Read())
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Element:
				if (reader.IsEmptyElement)
				{
					Debug.Log(reader.Name);
					break;
				}
				Debug.Log(reader.Name);
				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						Debug.Log(reader.Name + "," + reader.Value);
					}
				}
				Debug.Log(reader.Name);
				break;
			case XmlNodeType.Text:
				Debug.Log(reader.Value);
				break;
			case XmlNodeType.EndElement:
				Debug.Log(reader.Name);
				break;
			}
		}
	}
}
