

namespace RentApp.Proxy.ServiceClient
{
    using Newtonsoft.Json;
    using RentApp.Entities.BackEndEntities.Request;
    using RentApp.Entities.BackEndEntities.Response;
    using RentApp.Entities.Referentials;
    using RentApp.Utils.Resources;
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class UserServiceClient
    {
        private readonly Uri ServiceUrl;

        public UserServiceClient()
        {
            ServiceUrl = new Uri($"{ConnectionsResources.AppServiceUrl.ToString(CultureInfo.CurrentCulture)}/");
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task InsertUser(UserInsertRequest request)
        {
            var urlService = new Uri($"{ServiceUrl}/Usuario");
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(urlService, content);

            var responseString = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
            }
            //return JsonConvert.DeserializeObject<ResponseBack<string>>(responseString);
        }

        public async Task<ResponseBack<UserResponse>> GetUserByDocument(UserGetRequest request)
        {

            var urlService = new Uri($"{ServiceUrl}/Usuario/" + request.userId);
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(request.userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.GetAsync(urlService).ConfigureAwait(false);

            var responseString = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                 responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return JsonConvert.DeserializeObject<ResponseBack<UserResponse>>(responseString);
        }



    }
}
