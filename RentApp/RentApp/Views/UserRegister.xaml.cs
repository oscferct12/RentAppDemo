using RentApp.Entities.BackEndEntities.Request;
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
    public partial class UserRegister : ContentPage
    {
        public UserRegister()
        {
            InitializeComponent();
        }



        async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            UserServiceClient client = new UserServiceClient();
            try
            {
                Loading(true);
                UserInsertRequest request = new UserInsertRequest
                {
                    id = txtCedula.Text,
                    direccion = txtDireccion.Text,
                    eTag = txtNombre.Text,
                    nombre = txtNombre.Text,
                    partitionKey = txtCedula.Text,
                    rowKey = txtNombre.Text,
                    telefono = txtTelefono.Text,
                    timestamp = DateTime.Now

                };

                await client.InsertUser(request);
                await DisplayAlert("Correcto", "Usuario registrado Correctamente", "OK");
                Loading(false);
                await Navigation.PushAsync(new MovementRegister(request));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Incorrecto", ex.Message, "OK");
            }
        }

        void Loading(bool mostrar)
        {
            Indicador.IsEnabled = mostrar;
            Indicador.IsRunning = mostrar;
            lblLoading.IsVisible = mostrar;
        }

    }
}