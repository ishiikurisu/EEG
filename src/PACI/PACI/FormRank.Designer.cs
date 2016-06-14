﻿namespace PACI
{
    partial class FormRank
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableRankings = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableRankings, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonContinue, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 261);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableRankings
            // 
            this.tableRankings.ColumnCount = 1;
            this.tableRankings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRankings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableRankings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableRankings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableRankings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRankings.Location = new System.Drawing.Point(3, 46);
            this.tableRankings.Name = "tableRankings";
            this.tableRankings.RowCount = 3;
            this.tableRankings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableRankings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableRankings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableRankings.Size = new System.Drawing.Size(278, 167);
            this.tableRankings.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 43);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ranking";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonContinue.Location = new System.Drawing.Point(104, 227);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(75, 23);
            this.buttonContinue.TabIndex = 2;
            this.buttonContinue.Text = "Continuar";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // FormRank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormRank";
            this.Text = "Ranking";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #region My own UI code
        private TableLine[] Lines;

        private void UpdateGoals()
        {
            int limit = Goals.Count;
            Lines = new TableLine[limit];

            tableRankings.RowStyles.Clear();
            tableRankings.RowCount = limit;

            for (int i = 0; i < limit; i++)
            {
                Lines[i] = new TableLine(i, Goals[i]);
                Lines[i].SetCallback(new System.EventHandler<TableEventArgs>(UpdateList));
                tableRankings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                tableRankings.Controls.Add(Lines[i]);
            }

        }

        public void UpdateList(object sender, TableEventArgs e)
        {
            int ranking = e.Ranking;
            int step = e.Step;
            int limit = Lines.Length;

            if (!((ranking == 0 && step < 0) || (ranking == limit-1 && step > 0)))
            {
                string temp = Lines[ranking + step].GetGoal();
                Lines[ranking + step].SetGoal(Lines[ranking].GetGoal());
                Lines[ranking].SetGoal(temp);
            }
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableRankings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonContinue;
    }

    public class TableEventArgs : System.EventArgs
    {
        public int Ranking;
        public int Step;

        public TableEventArgs(int ranking, int step)
        {
            Ranking = ranking;
            Step = step;
        }
    }

    class TableLine : System.Windows.Forms.TableLayoutPanel
    {
        private System.Windows.Forms.Label Goal;
        private System.Windows.Forms.Button Down;
        private System.Windows.Forms.Button Up;
        public int Ranking;
        private System.EventHandler<TableEventArgs> UpdateList;

        public TableLine() : base()
        {
            this.Goal = new System.Windows.Forms.Label();
            this.Goal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Goal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Down = new System.Windows.Forms.Button();
            this.Down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Down.Text = "v";
            this.Down.Click += DownButtonCallback;
            this.Up = new System.Windows.Forms.Button();
            this.Up.Text = "^";
            this.Up.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Up.Click += this.UpButtonCallback;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RowCount = 1;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.Controls.Add(Goal, 0, 0);
            this.Controls.Add(Up, 1, 0);
            this.Controls.Add(Down, 2, 0);
        }

        public TableLine(int ranking, string goal) : this()
        {
            this.Ranking = ranking;
            this.Goal.Text = goal;
        }

        public string GetGoal()
        {
            return Goal.Text;
        }

        public void SetGoal(string it)
        {
            Goal.Text = it;
        }

        public void SetCallback(System.EventHandler<TableEventArgs> handler)
        {
            UpdateList = handler;
        }


        private void DownButtonCallback(object sender, System.EventArgs e)
        {
            UpdateList(this, new TableEventArgs(this.Ranking, 1));
        }

        private void UpButtonCallback(object sender, System.EventArgs e)
        {
            UpdateList(this, new TableEventArgs(this.Ranking, -1));
        }
    }
}