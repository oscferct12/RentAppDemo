using RentApp.Entities.BackEndEntities.Request;
using RentApp.Entities.BackEndEntities.Response;
using RentApp.Proxy.ServiceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MovementRegister : ContentPage
	{
        private UserInsertRequest Usuario;
		public MovementRegister (UserInsertRequest usuario)
		{
			InitializeComponent ();
            this.Usuario = usuario;
		}

        async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            MovementServiceClient client = new MovementServiceClient();
            try
            {
                MovementInsertRequest request = new MovementInsertRequest
                {
                    userId = Usuario.id,
                    year = "2018",
                    monthRevenue = txtIngresos.Text,
                    otherRevenue = txtOtrosIngresos.Text,
                    expenses = txtEgresos.Text,
                    otherExpenses = txtOtrosEgresos.Text,
                    partitionKey = Usuario.id,
                    rowKey = "2018",
                    timestamp = DateTime.Now,
                    eTag = "Movimientos"
                };

                await client.InsertMovement(request);
                await DisplayAlert("Correcto", "cuenta registrada correctamente", "OK");
                await Navigation.PushAsync(new Menu());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Incorrecto", ex.Message, "OK");
            }
        }
    }
}