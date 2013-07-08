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
using Controle.Helpers;
using MahApps.Metro.Controls;

namespace Controle
{
	/// <summary>
	/// Interaction logic for ManageCategory.xaml
	/// </summary>
    public partial class ManageCategory : MetroWindow
	{
		Category _editing = null;

		public ManageCategory()
		{
			InitializeComponent();
			ListaCategorias();
		}

		private void ListaCategorias()
		{
			using (var cat = Database.GetInstance.redisClient.As<Category>())
			{
				var mpvm = new List<Category>();
				foreach (Category item in cat.GetAll())
					mpvm.Add(item);

				dataGrid1.ItemsSource = mpvm;
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Category category;

			category = new Category()
			{
				Id = Database.GetInstance.redisClient.As<Category>().GetNextSequence(),
				Name = txtCategoria.Text,
				Description = txtDescricao.Text
			};

			if (_editing == null)
				category.Id = Database.GetInstance.redisClient.As<Category>().GetNextSequence();
			else
				category.Id = _editing.Id;

			if (category.Validate())
			{
				using (var trans = Database.GetInstance.redisClient.CreateTransaction())
				{
					trans.QueueCommand(r => r.Store<Category>(category));
					trans.Commit();
				}
				ListaCategorias();
				LimpaCampos();
			}			
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var categoriaSelecionada = (Category)dataGrid1.SelectedItem;

			if (categoriaSelecionada != null)
			{
				MessageBoxResult result = Helpers.ErrorInfo.ConfirmatioMessage("Deseja excluir o registro?");
				if (result == MessageBoxResult.Yes)
				{
					Database.GetInstance.redisClient.As<Category>().DeleteById(categoriaSelecionada.Id);
				}
			}
			ListaCategorias();
			LimpaCampos();
		}

		private void LimpaCampos()
		{
			txtCategoria.Text = String.Empty;
			txtDescricao.Text = String.Empty;
			_editing = null;
		}

		private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var categoriaSelecionada = (Category)dataGrid1.SelectedItem;

			if (categoriaSelecionada != null)
			{
				txtCategoria.Text = categoriaSelecionada.Name;
				txtDescricao.Text = categoriaSelecionada.Description;

				_editing = new Category
				{
					Id = categoriaSelecionada.Id,
					Name = txtCategoria.Text,
					Description = txtDescricao.Text
				};
			}
		}
	}
}
