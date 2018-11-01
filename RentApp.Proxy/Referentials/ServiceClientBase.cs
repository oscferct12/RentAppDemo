// -----------------------------------------------------------------------

// <copyright file="ServiceClientBase.cs" company="Intergrupo S.A">

// WebClient Base

// </copyright>

// <summary>This file will be communicate with services api rest</summary>

// -----------------------------------------------------------------------

namespace RentApp.Proxy.Referentials

{

    //using RentApp.Entities.BackEndEntities;

    using RentApp.Entities.Referentials;

    using RentApp.Utils;

    using Newtonsoft.Json;

    using System;

    using System.Collections.Generic;

    using System.Globalization;

    using System.Linq;

    using System.Net.Http;

    using System.Net.Http.Headers;

    using System.Text;

    using System.Threading.Tasks;

    using Utils.Resources;



    /// <summary>

    /// This file will be communicate with services api rest

    /// </summary>

    /// <typeparam name="T">Especific object to return service</typeparam>

    public class ServiceClientBase<T> where T : class, new()

    {

        /// <summary>

        /// The media type JSON

        /// </summary>

        protected const string MediaTypeJson = "Application/JSON; charset=utf-8";



        /// <summary>

        /// Response backend services

        /// </summary>

        protected ResponseBack<T> Response;



        /// <summary>

        /// The service URL base

        /// </summary>

        private string _serviceUrlBase;



        /// <summary>

        /// Gets the service URL base.

        /// </summary>

        /// <value>

        /// The service URL base.

        /// </value>

        public string ServiceUrlBase

        {

            get

            {

                if (string.IsNullOrWhiteSpace(_serviceUrlBase))

                {

                    _serviceUrlBase = RentApp.Utils.Resources.ConnectionsResources.AppServiceUrl.ToString(CultureInfo.CurrentCulture);

                }

                return _serviceUrlBase;

            }

        }



        /// <summary>

        /// Gets or sets the service URL.

        /// </summary>

        /// <value>

        /// The service URL.

        /// </value>

        protected string ServiceUrl { get; set; }



        /// <summary>

        /// Gets or sets the token information.




        /// <summary>

        /// Initializes a new instance of the <see cref="ServiceClientBase{T}"/> class.

        /// </summary>

        /// <param name="controller">The controller.</param>

        protected ServiceClientBase(string controller)

        {



            ServiceUrl = $"{ServiceUrlBase}/{controller}";

            InicializeResponse();

        }



        /// <summary>

        /// Inicializes the response.

        /// </summary>

        private void InicializeResponse()

        {

            Response = new ResponseBack<T>

            {

                TransactionComplete = false,

                Data = new List<T>(),

                Message = new List<string>()

            };

        }



        /// <summary>

        /// Initializes a new instance of the <see cref="ServiceClientBase{T}"/> class.

        /// </summary>

        public ServiceClientBase()

        {
            ServiceUrl = $"{ServiceUrlBase}{typeof(T).Name}";

            InicializeResponse();

        }







        /// <summary>

        /// Gets the HTTP client.

        /// </summary>

        /// <returns></returns>

        protected HttpClient GetHttpClient()

        {


            HttpClient httpClient = new HttpClient();

            return httpClient;

        }



        /// <summary>

        /// Posts the specified request.

        /// </summary>

        /// <param name="request">The request.</param>

        /// <returns></returns>

        public async Task<string> Post(T request)

        {

            HttpClient client = GetHttpClient();

            string json = JsonConvert.SerializeObject(request);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PostAsync(ServiceUrl, content).ConfigureAwait(false);

            string responseString = string.Empty;



            if (response.IsSuccessStatusCode)

            {

                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            }



            return responseString;

        }



        /// <summary>

        /// Posts the specified request.

        /// </summary>

        /// <param name="request">The request.</param>

        /// <returns></returns>

        public async Task<ResponseBack<T>> Post(object request, string action = "")

        {

            try

            {

                HttpClient client = GetHttpClient();

                string json = JsonConvert.SerializeObject(request);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");



                var response = await client.PostAsync($"{ServiceUrl}/{action}", content);



                string responseString = string.Empty;



                if (response.IsSuccessStatusCode)

                {

                    responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                }



                var response1 = JsonConvert.DeserializeObject<ResponseBack<T>>(responseString);



                return response.StatusCode == System.Net.HttpStatusCode.Forbidden ?

                    new ResponseBack<T>

                    {

                        TransactionComplete = false,

                        ResponseCode = (int)response.StatusCode

                    }

                    : JsonConvert.DeserializeObject<ResponseBack<T>>(responseString);

            }

            catch (Exception)

            {



                throw;

            }

        }





        /// <summary>

        /// Get the specified parameters.

        /// </summary>

        /// <returns>A List.</returns>

        /// <param name="parameters">Parameters.</param>

        public async Task<List<T>> Get(string parameters)

        {

            HttpClient client = GetHttpClient();



            HttpResponseMessage response = await client.GetAsync($"{ServiceUrl}/{parameters}").ConfigureAwait(false);



            List<T> listItems = new List<T>();

            if (!response.IsSuccessStatusCode)

            {

                return listItems;

            }



            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            listItems = JsonConvert.DeserializeObject<List<T>>(responseString);



            return listItems;

        }



        /// <summary>

        /// Gets the specified parameters.

        /// </summary>

        /// <param name="parameters">The parameters.</param>

        /// <returns>Return entity filtered by parameters</returns>

        public async Task<T> Get(Dictionary<string, string> parameters)

        {

            HttpClient client = GetHttpClient();

            StringBuilder parametersConcated = new StringBuilder();



            if (parameters == null)

            {

                throw new ArgumentNullException(nameof(parameters));

            }



            foreach (KeyValuePair<string, string> pair in parameters)

            {

                parametersConcated.AppendFormat(CultureInfo.CurrentCulture, "{0}={1}&", pair.Key, pair.Value);

            }



            HttpResponseMessage response = await client.GetAsync($"{ServiceUrl}/{parametersConcated.ToString().TrimEnd('&')}").ConfigureAwait(false);



            T items = new T();



            if (!response.IsSuccessStatusCode)

            {

                return items;

            }



            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            items = JsonConvert.DeserializeObject<T>(responseString);



            return items;

        }



        /// <summary>

        /// Gets with one parameter.

        /// </summary>

        /// <returns>The one parameter.</returns>

        /// <param name="parameter">Parameter.</param>

        public async Task<bool> GetOneParameter(string parameter)

        {

            HttpClient client = GetHttpClient();

            StringBuilder parametersConcated = new StringBuilder();

            HttpResponseMessage response = await client.GetAsync($"{ServiceUrl}/{parameter}").ConfigureAwait(false);



            bool responseBool;



            if (!response.IsSuccessStatusCode)

            {

                return false;

            }



            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            responseBool = bool.Parse(responseString);



            return responseBool;

        }



        /// <summary>

        /// Method to return chain of characters

        /// </summary>

        /// <param name="parameter"></param>

        /// <returns>Chain of characters</returns>

        public async Task<string> GetString(string parameter)

        {

            HttpClient client = GetHttpClient();

            string responseString = string.Empty;

            HttpResponseMessage response = await client.GetAsync($"{ServiceUrl}/{parameter}").ConfigureAwait(false);



            if (response.IsSuccessStatusCode)

            {

                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            }

            return responseString;

        }

    }

}
