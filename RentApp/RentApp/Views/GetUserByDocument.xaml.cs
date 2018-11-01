using RentApp.Entities.BackEndEntities.Request;
using RentApp.Entities.BackEndEntities.Response;
using RentApp.Entities.Referentials;
using RentApp.Proxy.ServiceClient;
using Rg.Plugins.Popup.Services;
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
    public partial class GetUserByDocument : ContentPage
    {
        public GetUserByDocument()
        {
            InitializeComponent();
        }


        async void btnConsultarUsuario_Clicked(object sender, EventArgs e)
        {
            UserServiceClient client = new UserServiceClient();
            try
            {
                ResponseBack<UserResponse> response = new ResponseBack<UserResponse>();

                UserGetRequest request = new UserGetRequest();
                if (!string.IsNullOrEmpty(txtCedula.Text))
                {
                    request.userId = txtCedula.Text;
                    Task.Run(async () =>
                    {
                        response = await client.GetUserByDocument(request);
                    }
                    ).GetAwaiter().GetResult();

                    if (response.Data.Count > 0)
                    {
                        var result = response.Data.FirstOrDefault();
                        await PopupNavigation.PushAsync(new PopupUserDetail(result));
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "No se encontraron registros para este usuario", "OK");

                    }

                }

                else
                {
                    await DisplayAlert("Incorrecto", "Debe digitar un número de documento", "OK");
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
                throw;
            }


        }
    }
}