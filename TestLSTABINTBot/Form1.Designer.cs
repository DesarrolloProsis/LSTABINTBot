﻿namespace TestLSTABINTBot
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_Inicio = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_Inicio
            // 
            this.Btn_Inicio.Location = new System.Drawing.Point(37, 12);
            this.Btn_Inicio.Name = "Btn_Inicio";
            this.Btn_Inicio.Size = new System.Drawing.Size(213, 90);
            this.Btn_Inicio.TabIndex = 0;
            this.Btn_Inicio.Text = "Iniciar";
            this.Btn_Inicio.UseVisualStyleBackColor = true;
            this.Btn_Inicio.Click += new System.EventHandler(this.Btn_Inicio_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 114);
            this.Controls.Add(this.Btn_Inicio);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Inicio;
    }
}

