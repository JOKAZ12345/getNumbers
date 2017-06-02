namespace getNumbers
{
    partial class Resultados_Prospecao
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.prabitarDataSet = new getNumbers.prabitarDataSet();
            this.potencialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.potencialTableAdapter = new getNumbers.prabitarDataSetTableAdapters.PotencialTableAdapter();
            this.candidatoIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telefoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tituloAnuncioDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uRLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prabitarDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.potencialBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.candidatoIDDataGridViewTextBoxColumn,
            this.telefoneDataGridViewTextBoxColumn,
            this.nomeDataGridViewTextBoxColumn,
            this.tituloAnuncioDataGridViewTextBoxColumn,
            this.uRLDataGridViewTextBoxColumn,
            this.precoDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.potencialBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(2, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(846, 557);
            this.dataGridView1.TabIndex = 0;
            // 
            // prabitarDataSet
            // 
            this.prabitarDataSet.DataSetName = "prabitarDataSet";
            this.prabitarDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // potencialBindingSource
            // 
            this.potencialBindingSource.DataMember = "Potencial";
            this.potencialBindingSource.DataSource = this.prabitarDataSet;
            // 
            // potencialTableAdapter
            // 
            this.potencialTableAdapter.ClearBeforeFill = true;
            // 
            // candidatoIDDataGridViewTextBoxColumn
            // 
            this.candidatoIDDataGridViewTextBoxColumn.DataPropertyName = "Candidato_ID";
            this.candidatoIDDataGridViewTextBoxColumn.HeaderText = "Candidato_ID";
            this.candidatoIDDataGridViewTextBoxColumn.Name = "candidatoIDDataGridViewTextBoxColumn";
            this.candidatoIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.candidatoIDDataGridViewTextBoxColumn.Width = 97;
            // 
            // telefoneDataGridViewTextBoxColumn
            // 
            this.telefoneDataGridViewTextBoxColumn.DataPropertyName = "Telefone";
            this.telefoneDataGridViewTextBoxColumn.HeaderText = "Telefone";
            this.telefoneDataGridViewTextBoxColumn.Name = "telefoneDataGridViewTextBoxColumn";
            this.telefoneDataGridViewTextBoxColumn.ReadOnly = true;
            this.telefoneDataGridViewTextBoxColumn.Width = 74;
            // 
            // nomeDataGridViewTextBoxColumn
            // 
            this.nomeDataGridViewTextBoxColumn.DataPropertyName = "Nome";
            this.nomeDataGridViewTextBoxColumn.HeaderText = "Nome";
            this.nomeDataGridViewTextBoxColumn.Name = "nomeDataGridViewTextBoxColumn";
            this.nomeDataGridViewTextBoxColumn.ReadOnly = true;
            this.nomeDataGridViewTextBoxColumn.Width = 60;
            // 
            // tituloAnuncioDataGridViewTextBoxColumn
            // 
            this.tituloAnuncioDataGridViewTextBoxColumn.DataPropertyName = "TituloAnuncio";
            this.tituloAnuncioDataGridViewTextBoxColumn.HeaderText = "TituloAnuncio";
            this.tituloAnuncioDataGridViewTextBoxColumn.Name = "tituloAnuncioDataGridViewTextBoxColumn";
            this.tituloAnuncioDataGridViewTextBoxColumn.ReadOnly = true;
            this.tituloAnuncioDataGridViewTextBoxColumn.Width = 97;
            // 
            // uRLDataGridViewTextBoxColumn
            // 
            this.uRLDataGridViewTextBoxColumn.DataPropertyName = "URL";
            this.uRLDataGridViewTextBoxColumn.HeaderText = "URL";
            this.uRLDataGridViewTextBoxColumn.Name = "uRLDataGridViewTextBoxColumn";
            this.uRLDataGridViewTextBoxColumn.ReadOnly = true;
            this.uRLDataGridViewTextBoxColumn.Width = 54;
            // 
            // precoDataGridViewTextBoxColumn
            // 
            this.precoDataGridViewTextBoxColumn.DataPropertyName = "Preco";
            this.precoDataGridViewTextBoxColumn.HeaderText = "Preco";
            this.precoDataGridViewTextBoxColumn.Name = "precoDataGridViewTextBoxColumn";
            this.precoDataGridViewTextBoxColumn.ReadOnly = true;
            this.precoDataGridViewTextBoxColumn.Width = 60;
            // 
            // Resultados_Prospecao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 558);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.Name = "Resultados_Prospecao";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resultados_Prospecao";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Resultados_Prospecao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prabitarDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.potencialBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private prabitarDataSet prabitarDataSet;
        private System.Windows.Forms.BindingSource potencialBindingSource;
        private prabitarDataSetTableAdapters.PotencialTableAdapter potencialTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn candidatoIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telefoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tituloAnuncioDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uRLDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn precoDataGridViewTextBoxColumn;
    }
}