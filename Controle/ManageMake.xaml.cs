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
using Controle.Domain;
using Controle.database;
using MahApps.Metro.Controls;

namespace Controle
{
	/// <summary>
	/// Interaction logic for ManageCategory.xaml
	/// </summary>
    public partial class ManageMake : MetroWindow
	{
		Category _editing = null;

		public ManageMake()
		{
			InitializeComponent();
			ListaFabricantes();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Make maker;

			maker = new Make()
			{
				Name = txtFabricante.Text
			};

			if (_editing == null)
				maker.Id = Database.GetInstance.redisClient.As<Make>().GetNextSequence();
			else
				maker.Id = _editing.Id;

			if (maker.Validate())
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Make>(maker));

					trans.Commit();
				}
				ListaFabricantes();
				LimpaCampos();
			}			
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var fabricanteSelecionado = (Make)dataGrid1.SelectedItem;

			if (fabricanteSelecionado != null)
			{
				MessageBoxResult result = Helpers.ErrorInfo.ConfirmatioMessage("Deseja excluir o registro?");
				if (result == MessageBoxResult.Yes)
				{
					Database.GetInstance.redisClient.As<Make>().DeleteById(fabricanteSelecionado.Id);
				}
			}
			ListaFabricantes();
			LimpaCampos();
		}

		private void ListaFabricantes()
		{
			using (var mak = Database.GetInstance.redisClient.As<Make>())
			{
				var mpvm = new List<Make>();
				foreach (Make item in mak.GetAll())
					mpvm.Add(item);

				dataGrid1.ItemsSource = mpvm;
			}
		}

		private void LimpaCampos()
		{
			txtFabricante.Text = String.Empty;
			_editing = null;
		}

		private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var fabricanteSelecionado = (Make)dataGrid1.SelectedItem;

			if (fabricanteSelecionado != null)
			{
				txtFabricante.Text = fabricanteSelecionado.Name;

				_editing = new Category
				{
					Id = fabricanteSelecionado.Id,
					Name = txtFabricante.Text,
				};
			}
		}
	}
}
