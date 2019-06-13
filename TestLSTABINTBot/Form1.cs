using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;

namespace TestLSTABINTBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Btn_Inicio_Click(object sender, EventArgs e)
        {
            LSTABINTBot.Service1 service = new LSTABINTBot.Service1();
            service.Inicio();
        }
    }
}
