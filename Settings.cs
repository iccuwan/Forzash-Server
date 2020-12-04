using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Forzash_Server
{
	class Settings
	{
		private Dictionary<string, string> settings;
		private bool loaded = false;
		private string settingsPath = "settings.json";

		public Settings()
		{
			if (File.Exists(settingsPath))
			{
				try
				{
					string settingsJson = File.ReadAllText(settingsPath);
					settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(settingsJson);
					loaded = true;
					Console.WriteLine("Settings loaded!");
				}
				catch (Exception e)
				{
					Console.WriteLine("SETTINGS LOAD ERROR!");
					Console.WriteLine(e.Message);
				}
			}
			else
			{
				Console.WriteLine("settings.json not founded!");
			}
		}

		private int ReturnIntValue(string key)
		{
			if (loaded && settings.ContainsKey(key))
			{
				try
				{
					return int.Parse(settings[key]);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			return -1;
		}

		public int Port
		{
			get
			{
				return ReturnIntValue("port");
			}
		}

		public int MaxSlots
		{
			get
			{
				return ReturnIntValue("slots");
			}
		}

		public bool Loaded
		{
			get
			{
				return loaded;
			}
		}
	}
}
