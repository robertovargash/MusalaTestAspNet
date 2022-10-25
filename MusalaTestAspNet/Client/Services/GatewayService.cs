using Microsoft.AspNetCore.Components;
using MusalaTestAspNet.Shared.Models;
using System;
using System.Net.Http.Json;

namespace MusalaTestAspNet.Client.Services
{
    public interface IGatewayService
    {
        public string Message { get; set; }
        public Code Code { get; set; }
        List<Gateway> Gateways { get; set; }
        List<Peripheral> Peripherals { get; set; }

        Task FillGateways();
        Task<Gateway> GetGateway(int Id);
        Task CreateGateway(Gateway gateway);
        Task DeleteGateway(int Id);
        Task UpdateGateway(Gateway gateway);

        Task FillPeripherials(Gateway gateway);
        Task<Peripheral> GetPeripheral(int Id);
        Task CreatePeripherial(Peripheral peripherial);
        Task UpdatePeripherial(Peripheral peripherial);
        Task DeletePeripherial(int Id);
    }

    public class GatewayService : IGatewayService
    {
        private readonly HttpClient http;
        private readonly NavigationManager navigationManager;
        public List<Gateway> Gateways { get; set; }
        public List<Peripheral> Peripherals { get; set; }
        public string Message { get; set; }
        public Code Code { get; set; }

        public GatewayService(HttpClient http, NavigationManager navigationManager)
        {
            this.http = http;
            this.navigationManager = navigationManager;
        }

        #region Gateway
        public async Task CreateGateway(Gateway gateway)
        {
            var result = await http.PostAsJsonAsync("api/gateway", gateway);
            var response = await result.Content.ReadFromJsonAsync<MyResponse<List<Gateway>>>();
            NavigateWithMessage(response, "gateway");
        }

        private void NavigateWithMessage(MyResponse<List<Gateway>>? response, string url)
        {
            Gateways = response.Data;
            Code = response.Code;
            Message = response.Message;
            navigationManager.NavigateTo(url);
        }

        public async Task DeleteGateway(int Id)
        {
            var result = await http.DeleteAsync($"api/gateway/{Id}");
            var response = await result.Content.ReadFromJsonAsync<MyResponse<List<Gateway>>>();
            NavigateWithMessage(response, "gateway");
        }

        public async Task<Gateway> GetGateway(int Id)
        {
            var result = await http.GetFromJsonAsync<Gateway>($"api/gateway/{Id}");
            if (result != null)
                return result;
            throw new Exception("NotFound");
        }

        public async Task FillGateways()
        {
            var result = await http.GetFromJsonAsync<List<Gateway>>("api/gateway");
            if (result != null)
            {
                Gateways = result;
            }
        }

        public async Task FillPeripherials(Gateway gateway)
        {
            var result = await http.GetFromJsonAsync<List<Peripheral>>($"api/gateway/getperipherials/{gateway.Id}");
            if (result != null)
            {
                Peripherals = result;
            }
        }

        public async Task UpdateGateway(Gateway gateway)
        {
            var result = await http.PutAsJsonAsync($"api/gateway/{gateway.Id}", gateway);
            var response = await result.Content.ReadFromJsonAsync<MyResponse<List<Gateway>>>();
            NavigateWithMessage(response, "gateway");
        }

        public async Task<Peripheral> GetPeripheral(int Id)
        {
            var result = await http.GetFromJsonAsync<Peripheral>($"api/peripherial/{Id}");
            if (result != null)
                return result;
            throw new Exception("NotFound");
        }

        public async Task CreatePeripherial(Peripheral peripherial)
        {
            var result = await http.PostAsJsonAsync("api/peripherial", peripherial);
            var response = await result.Content.ReadFromJsonAsync<MyResponse<List<Peripheral>>>();
            Peripherals = response.Data;
            Code = response.Code;
            Message = response.Message;
        }

        public async Task UpdatePeripherial(Peripheral peripherial)
        {
            var result = await http.PutAsJsonAsync($"api/peripherial/{peripherial.Id}", peripherial);
            var response = await result.Content.ReadFromJsonAsync<MyResponse<List<Peripheral>>>();
            Peripherals = response.Data;
            Code = response.Code;
            Message = response.Message;
        }

        public async Task DeletePeripherial(int Id)
        {
            var result = await http.DeleteAsync($"api/peripherial/{Id}");
            var response = await result.Content.ReadFromJsonAsync<MyResponse<List<Peripheral>>>();
            Peripherals = response.Data;
            Code = response.Code;
            Message = response.Message;
        }
        #endregion

    }
}
