using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

namespace LSTABINTBot
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        public void Inicio()
        {

        }
        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
        public async void CheckListas()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var GetLastLista = await client.PostAsync(new Uri("http://localhost:87/Home/GetListas"), null);

                    var ReceiveLastLista = await client.PostAsync(new Uri("http://localhost:87/Home/SendListas"), null);

                    var FechaLastLista = await ReceiveLastLista.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async void CheckServiceTags()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var GetOracleCount = await client.PostAsync(new Uri("http://localhost:87/Home/GetOracleTransactions"), null);
                    var GetSQLCount = await client.PostAsync(new Uri("http://localhost:87/Home/GetSQLTransactions"), null);

                    var ReceiveOracleCount = await client.PostAsync(new Uri("http://localhost:87/Home/SendOracleTransactions"), null);
                    var ReceiveSQLCount = await client.PostAsync(new Uri("http://localhost:87/Home/SendSQLTransactions"), null);

                    var OracleCount = await ReceiveOracleCount.Content.ReadAsStringAsync();
                    var SQLCount = await ReceiveSQLCount.Content.ReadAsStringAsync();
                }
            }
            catch (Exception Ex)
            {
                
                throw;
            }
        }
    }
}
