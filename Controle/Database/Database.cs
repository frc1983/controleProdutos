using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.config;
using System.Diagnostics;
using ServiceStack.Redis;

namespace Controle.database
{
	public class Database
	{
		private static Database SingletonDataBase;
		private static RedisClient _redisClient;
		private AppConfig _config;
		private bool redisIsRunning = false;


		public static Database GetInstance
		{
			get
			{
				if (SingletonDataBase == null)
					SingletonDataBase = new Database();

				return SingletonDataBase;
			}
		}

		public RedisClient redisClient 
		{
			get { return _redisClient; }
		}


		private Database()
		{
			this._config = new AppConfig();

			var process = Process.GetProcessesByName("redis-server");
			if (process.Count() > 0)
			{
			    this.redisIsRunning = true;
			    initClient();
			}
			else 
				initRedisServer();
		}

		private void initRedisServer()
		{
			Process myProcess = new Process();
			
			try
			{
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.FileName = _config.RedisPath;
				myProcess.StartInfo.CreateNoWindow = true;
				myProcess.Start();
				this.redisIsRunning = true;
				initClient();
			}
			catch (Exception e)
			{
				this.redisIsRunning = false;
				throw new Exception(e.Message);				
			}			
		}

		private void initClient()
		{
			if(redisIsRunning)
				_redisClient = new RedisClient(_config.RedisLocal, _config.RedisPort);
		}

		public void closeClient()
		{
			if (redisIsRunning)
				_redisClient.Quit();

			var process = Process.GetProcessesByName("redis-server");
			if (process.Count() > 0)
				process.FirstOrDefault().Kill();
		}
		
	}
}
