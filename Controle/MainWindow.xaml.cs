using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controle.database;
using ServiceStack.Redis;
using Controle.Domain;
using Xceed.Wpf.Toolkit;

namespace Controle
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OnApplicationExit(object sender, EventArgs e)
		{
			try
			{
				Database.GetInstance.redisClient.SaveAsync();
				Database.GetInstance.closeClient();
			}
			catch (RedisException ex) { throw new RedisException(ex.Message); }
		}

		private void openManager_Click(object sender, RoutedEventArgs e)
		{
			ModalManageProduct.Content = new ManageProduct();
			ModalManageProduct.Show();
		}

		private void openManagerCategory_Click(object sender, RoutedEventArgs e)
		{
			ModalManageCategory.Content = new ManageCategory();
			ModalManageCategory.Show();
		}

		private void openManagerProvider_Click(object sender, RoutedEventArgs e)
		{
			ModalManageProvider.Content = new ManageProvider();
			ModalManageProvider.Show();
		}

		private void openManagerMake_Click(object sender, RoutedEventArgs e)
		{
			ModalManageMake.Content = new ManageMake();
			ModalManageMake.Show();
		}
	}
}
