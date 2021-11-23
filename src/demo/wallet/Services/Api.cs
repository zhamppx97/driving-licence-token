using GrpcService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace wallet.Services
{
    public class Api : IApi
    {
        public async Task<string> GetBalanceToken(string address)
        {
            var requestBoby = new { address };
            var client = new RestClient("http://localhost:5001/v1/getbalance")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(requestBoby));
            var response = await client.ExecuteAsync(request);
            var responseContent = JsonConvert.DeserializeObject<BalanceResponse>(response.Content);
            return responseContent.Balance.ToString();
        }
    }
}
