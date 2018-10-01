namespace Lands2.Services
{
    using Lands2.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class APIServices
    {
        public async Task<Response> CheckConnection()
        {
            if(!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Porfavor conectese a internet desde su movil"
                };
            }

            var isRecheable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if(!isRecheable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Porfavor, Revise su conexion a internet"
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok"
            };
                
        }

        public async Task<TokenResponse> GetToken(string urlBase, string userName, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token", 
                    new StringContent(string.Format(
                    "grant_type=password&username={0}&password={1}",
                    userName, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));

                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJson);
                return result; 
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Response> Get<T> (
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);

                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);

                var response = await client.GetAsync(url);

                if(!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok"
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                var model = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok"
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                var client = new HttpClient();                
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
