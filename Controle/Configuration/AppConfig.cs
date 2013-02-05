using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace Controle.config
{
	class AppConfig
	{
		public AppConfig() { }

		public string RedisPath
		{
			get { return Controle.Properties.Settings.Default.redisPath; }
		}

		public string RedisLocal
		{
			get { return Controle.Properties.Settings.Default.redisLocal; }
		}

		public int RedisPort
		{
			get { return Controle.Properties.Settings.Default.redisPort; }
		}
	}
}
