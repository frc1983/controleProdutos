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
using WPF.MDI;

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
			WindowContainer.Children.Add(GenerateChild("Produtos"));
		}

		private void openManagerCategory_Click(object sender, RoutedEventArgs e)
		{
			WindowContainer.Children.Add(GenerateChild("Categorias"));
		}

		private void openManagerProvider_Click(object sender, RoutedEventArgs e)
		{
			WindowContainer.Children.Add(GenerateChild("Fornecedores"));
		}

		private void openManagerMake_Click(object sender, RoutedEventArgs e)
		{
			WindowContainer.Children.Add(GenerateChild("Fabricantes"));
		}

		private MdiChild GenerateChild(string windowName)
		{
			UIElement windowContent = null;
			switch (windowName)
			{
				case "Produtos" :
					windowContent = new ManageProduct() { Visibility = Visibility.Visible };
					break;
				case "Categorias":
					windowContent = new ManageCategory() { Visibility = Visibility.Visible };
					break;
				case "Fornecedores":
					windowContent = new ManageProvider() { Visibility = Visibility.Visible };
					break;
				case "Fabricantes":
					windowContent = new ManageMake() { Visibility = Visibility.Visible };
					break;
			}

			var converter = new System.Windows.Media.BrushConverter();
			var brush = (Brush)converter.ConvertFromString("#FFFFFFFF");
			
			return new MdiChild()
			{
				Title = "Gerenciamento de " + windowName,
				Resizable = true,
				MinimizeBox = false,
				VerticalContentAlignment = System.Windows.VerticalAlignment.Top,
				HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left,
				Background = brush,
				BorderBrush = brush,
				Width = ((UserControl)windowContent).Width,
				Height = ((UserControl)windowContent).Height,
				MaxWidth = WindowContainer.ActualWidth,
				MaxHeight = WindowContainer.ActualHeight,
				BorderThickness = new Thickness(0),
				Content = windowContent
			};
		}
	}
}
