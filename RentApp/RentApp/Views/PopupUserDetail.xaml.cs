using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using RentApp.Entities.BackEndEntities.Response;
using RentApp.Proxy.ServiceClient;
using RentApp.Entities.BackEndEntities.Request;
using System;
using RentApp.Entities.Referentials;
using System.Linq;

namespace RentApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupUserDetail : PopupPage
    {
        private UserResponse Usuario;

        public PopupUserDetail(UserResponse user)
        {
            InitializeComponent();
            this.Usuario = user;

            lblName.Text = Usuario.nombre;
            lblTelefono.Text = Usuario.telefono;
            lblDireccion.Text = Usuario.direccion;
            lblCedula.Text = Usuario.id;
            stckMovement.IsVisible = false;
        }


        async void btnConsultarMovimientos_Clicked(object sender, EventArgs e)
        {

            MovementServiceClient client = new MovementServiceClient();
            ResponseBack<MovementResponse> responseMovement = new ResponseBack<MovementResponse>();
            MovementGetRequest request = new MovementGetRequest();

            request.userId = Usuario.id;

            responseMovement = await client.GetMovementByDocument(request);

            if (responseMovement.Data.Count >0)
            {
                stckMovement.IsVisible = true;
                var result = responseMovement.Data.FirstOrDefault();

                lblIngresosMensuales.Text = result.monthRevenue;
                lblOtrosIngresos.Text = result.otherRevenue;
                lblEgresos.Text = result.expenses;
                lblOtrosEgresos.Text = result.otherExpenses;

            }

            else
            {
                await DisplayAlert("Alerta","No existen movimientos para este usuario","Cerrar");
            }

        }

        async void btnConsultarDeclaracion_Clicked(object sender, EventArgs e)
        {
            Decimal IngresoMensual = Convert.ToDecimal(lblIngresosMensuales.Text);

            if (IngresoMensual*12 > 48000000)
            {
                await DisplayAlert("Renta", "Debe cancelar $100.000 por declaración de renta", "OK");
            }
            else
            {
                await DisplayAlert("Renta", "No debe declararar renta", "OK");
            }
        }
            
    }
}