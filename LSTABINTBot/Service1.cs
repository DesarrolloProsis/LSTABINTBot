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
using Telegram.Bot;

namespace LSTABINTBot
{
    public partial class Service1 : ServiceBase
    {

        private System.Timers.Timer timer = null;

        private static readonly TelegramBotClient Bot = new TelegramBotClient("834404388:AAG8JcPTHi9API16h1TF5C_EgsB78QToaP8");
        public Service1()
        {
            InitializeComponent();
        }

        public void Inicio()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 300000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 300000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                CheckListas();
                CheckServiceTags();
                timer.Enabled = true;
            }
            catch (Exception Ex)
            {
                Bot.SendTextMessageAsync(-364639169, "Oh oh, algo salió mal con el bot que monitorea los servicios, que ironía :(: " + Ex.Message , Telegram.Bot.Types.Enums.ParseMode.Markdown);
                timer.Enabled = true;
                throw;
            }

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
                    var GetLastLista = await client.PostAsync(new Uri("http://localhost:87/Home/GetHistoricoListas"), null);

                    var ReceiveLastLista = await client.PostAsync(new Uri("http://localhost:87/Home/SendHistoricoListas"), null);

                    VerifyLSTABINTSevice(await ReceiveLastLista.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public void VerifyLSTABINTSevice(string DateLastLista)
        {
            if (Convert.ToDateTime(DateLastLista) < DateTime.Now.AddMinutes(-30))
            {
                Bot.SendTextMessageAsync(-364639169, "Las LSTABINT no han sido actualizadas desde *" + DateLastLista + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
        public async void CheckServiceTags()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var GetLastRegister = await client.PostAsync(new Uri("http://localhost:87/Home/GetHistoricoTags"), null);

                    var ReceiveLastRegister = await client.PostAsync(new Uri("http://localhost:87/Home/SendHistoricoListas"), null);

                    VerifyServiceTags(await ReceiveLastRegister.Content.ReadAsStringAsync());
                }
            }
            catch (Exception Ex)
            {

                throw;
            }
        }
        public void VerifyServiceTags(string DateLastRegister)
        {
            if (Convert.ToDateTime(DateLastRegister) < DateTime.Now.AddMinutes(-30))
            {
                CompareTransacciones(DateLastRegister);
            }
        }
        public async void CompareTransacciones(string LastRegister)
        {
            using (var client = new HttpClient())
            {
                var GetOracleCount = await client.PostAsync(new Uri("http://localhost:87/Home/GetOracleTransactions"), null);
                var GetSQLCount = await client.PostAsync(new Uri("http://localhost:87/Home/GetSQLTransactions"), null);

                var ReceiveOracleCount = await client.PostAsync(new Uri("http://localhost:87/Home/SendOracleTransactions"), null);
                var ReceiveSQLCount = await client.PostAsync(new Uri("http://localhost:87/Home/SendSQLTransactions"), null);

                var OracleCount = await ReceiveOracleCount.Content.ReadAsStringAsync();
                var SQLCount = await ReceiveSQLCount.Content.ReadAsStringAsync();

                if (OracleCount != SQLCount)
                {
                    await Bot.SendTextMessageAsync(-364639169, "Las LSTABINT no han sido actualizadas desde *" + LastRegister + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }

            }
        }
    }
}
