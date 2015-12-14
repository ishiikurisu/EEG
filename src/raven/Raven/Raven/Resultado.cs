﻿using System;
using System.Windows.Forms;

namespace Raven
{
    public partial class Resultado : Form
    {
        public Resultado()
        {
            InitializeComponent();
        }

        public Resultado(int resultado) : this()
        {
            lblResultado.Text = resultado.ToString();
        }

        public Resultado(string resultado) : this()
        {
            lblResultado.Text = resultado;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Close();
        }
    }
}
