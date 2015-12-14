﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Raven
{
    public class Preparador
    {
        public string[] Caminhos { get; private set; }
        public string[] Testes { get; private set; }
        private string CaminhoAtual { get; set; }
        public string[] ImagensPrincipais { get; private set; }
        public int[] Opcoes { get; private set; }
        public int[] Respostas { get; private set; }

        public Preparador()
        {
            //CaminhoAtual = @"C:\Users\cris\Documents\work\EEG\src\raven\Raven\Raven\";
            //CaminhoAtual = @"..\..\";
            CaminhoAtual = @".\";
        }

        public void CarregarTeste()
        {
            StreamReader file = new StreamReader(this.CaminhoAtual + "versions.txt");
            List<string> paths = new List<string>();
            List<string> tests = new List<string>();

            for (string line = file.ReadLine(); line != null; line = file.ReadLine())
            {
                string[] data = line.Split(' ');
                paths.Add(data[0]);
                tests.Add(data[1]);
            }

            this.Caminhos = paths.ToArray();
            this.Testes = tests.ToArray();
        }

        public void CarregarOpcoes(string teste)
        {
            string config = CaminhoAtual + teste + ".txt";
            string caminhoDados = CaminhoAtual + teste + @"\";
            List<string> imgs = new List<string>();
            List<int> ops = new List<int>();
            List<int> ans = new List<int>();
            StreamReader file = new StreamReader(config);

            for (string line = file.ReadLine(); line != null; line = file.ReadLine())
            {
                string[] data = line.Split(' ');
                imgs.Add(data[0]);
                ops.Add(int.Parse(data[1]));
                ans.Add(int.Parse(data[2]));
            }

            ImagensPrincipais = imgs.ToArray();
            Opcoes = ops.ToArray();
            Respostas = ans.ToArray();
        }

        internal int CalcularResultado(string nomeTeste, 
                                       int noRespostasCorretas, 
                                       int idade)
        {
            string csvFileName = CaminhoAtual + nomeTeste + ".csv";


            throw new NotImplementedException();
        }

        public string[] CarregarImagens(string test, string img, int noImgs)
        {
            string[] imgs = new string[noImgs+1];

            for (int i = 0; i <= noImgs; ++i)
            {
                imgs[i] = CaminhoAtual + test + @"\" + img + "." + i + ".png";
            }

            return imgs;
        }

    }
}
