using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
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
        int intervalos;
        private static readonly TelegramBotClient Bot = new TelegramBotClient("834404388:AAG8JcPTHi9API16h1TF5C_EgsB78QToaP8");
        public Service1()
        {
            InitializeComponent();
        }

        public void Inicio()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 30000;
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
        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Stop();
            try
            {
                var LSTABINTWorking = await CheckListas();
                var ServiceTagsWorking = await CheckServiceTags();
                intervalos++;
                if (intervalos >= 36 && (LSTABINTWorking && ServiceTagsWorking) && DateTime.Now.Hour > 9)
                {
                    await Bot.SendTextMessageAsync(-364639169, "Funcionando correctamente todos los servicios", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    intervalos = 0;
                }
                timer.Enabled = true;
                timer.Start();

            }
            catch (Exception Ex)
            {
                await Bot.SendTextMessageAsync(-364639169, "Oh oh, algo salió mal con el bot que monitorea los servicios, que ironía :(: " + Ex.StackTrace);
                intervalos = 36;
                timer.Enabled = true;
                timer.Start();
            }

        }
        protected override void OnStop()
        {
            File.WriteAllText(@"C:\temporal\LSTABINTBotStopped.txt", "Se detuvo");     
        }
        public async Task<bool> CheckListas()
        {

            using (var client = new HttpClient())
            {
                var GetLastLista = await client.PostAsync(new Uri("http://localhost:87/Home/GetHistoricoListas"), null);

                var ReceiveLastLista = await client.PostAsync(new Uri("http://localhost:87/Home/SendHistoricoListas"), null);

                return VerifyLSTABINTSevice(await ReceiveLastLista.Content.ReadAsStringAsync());
            }


        }
        public bool VerifyLSTABINTSevice(string DateLastLista)
        {
            if (Convert.ToDateTime(DateLastLista) < DateTime.Now.AddMinutes(-30))
            {
                Bot.SendTextMessageAsync(-364639169, "Las LSTABINT no han sido actualizadas desde *" + DateLastLista + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<bool> CheckServiceTags()
        {
            using (var client = new HttpClient())
            {
                var GetLastRegister = await client.PostAsync(new Uri("http://localhost:87/Home/GetHistoricoTags"), null);

                var ReceiveLastRegister = await client.PostAsync(new Uri("http://localhost:87/Home/SendHistoricoTags"), null);

                var LastRegister = await ReceiveLastRegister.Content.ReadAsStringAsync();

                var Verificado = Convert.ToBoolean(await VerifyServiceTags(LastRegister));

                return Verificado;
            }
        }
        public async Task<bool> VerifyServiceTags(string DateLastRegister)
        {
            if (Convert.ToDateTime(DateLastRegister) < DateTime.Now.AddMinutes(-30))
            {
                return Convert.ToBoolean(await CompareTransacciones(DateLastRegister));
            }
            else
            {
                return true;
            }
        }
        public async Task<bool> CompareTransacciones(string LastRegister)
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
                    await Bot.SendTextMessageAsync(-364639169, "ServiceTags no funciona desde: *" + LastRegister + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
