using System.Windows.Forms;

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
            this.Angariador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.potencialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.prabitarDataSet = new getNumbers.prabitarDataSet();
            this.potencialTableAdapter = new getNumbers.prabitarDataSetTableAdapters.PotencialTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.candidatoIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telefoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tituloAnuncioDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uRLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.angariadorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.potencialBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prabitarDataSet)).BeginInit();
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
            this.Angariador,
            this.candidatoIDDataGridViewTextBoxColumn,
            this.telefoneDataGridViewTextBoxColumn,
            this.nomeDataGridViewTextBoxColumn,
            this.tituloAnuncioDataGridViewTextBoxColumn,
            this.uRLDataGridViewTextBoxColumn,
            this.precoDataGridViewTextBoxColumn,
            this.angariadorDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.potencialBindingSource;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(2, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(846, 463);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_DoubleClick);
            // 
            // Angariador
            // 
            this.Angariador.DataPropertyName = "Angariador";
            this.Angariador.HeaderText = "Angariador";
            this.Angariador.Name = "Angariador";
            this.Angariador.Width = 83;
            // 
            // potencialBindingSource
            // 
            this.potencialBindingSource.DataMember = "Potencial";
            this.potencialBindingSource.DataSource = this.prabitarDataSet;
            this.potencialBindingSource.CurrentChanged += new System.EventHandler(this.potencialBindingSource_CurrentChanged);
            // 
            // prabitarDataSet
            // 
            this.prabitarDataSet.DataSetName = "prabitarDataSet";
            this.prabitarDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // potencialTableAdapter
            // 
            this.potencialTableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 483);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Telefone";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(67, 480);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            this.telefoneDataGridViewTextBoxColumn.Width = 74;
            // 
            // nomeDataGridViewTextBoxColumn
            // 
            this.nomeDataGridViewTextBoxColumn.DataPropertyName = "Nome";
            this.nomeDataGridViewTextBoxColumn.HeaderText = "Nome";
            this.nomeDataGridViewTextBoxColumn.Name = "nomeDataGridViewTextBoxColumn";
            this.nomeDataGridViewTextBoxColumn.Width = 60;
            // 
            // tituloAnuncioDataGridViewTextBoxColumn
            // 
            this.tituloAnuncioDataGridViewTextBoxColumn.DataPropertyName = "TituloAnuncio";
            this.tituloAnuncioDataGridViewTextBoxColumn.HeaderText = "TituloAnuncio";
            this.tituloAnuncioDataGridViewTextBoxColumn.Name = "tituloAnuncioDataGridViewTextBoxColumn";
            this.tituloAnuncioDataGridViewTextBoxColumn.Width = 97;
            // 
            // uRLDataGridViewTextBoxColumn
            // 
            this.uRLDataGridViewTextBoxColumn.DataPropertyName = "URL";
            this.uRLDataGridViewTextBoxColumn.HeaderText = "URL";
            this.uRLDataGridViewTextBoxColumn.Name = "uRLDataGridViewTextBoxColumn";
            this.uRLDataGridViewTextBoxColumn.Width = 54;
            // 
            // precoDataGridViewTextBoxColumn
            // 
            this.precoDataGridViewTextBoxColumn.DataPropertyName = "Preco";
            this.precoDataGridViewTextBoxColumn.HeaderText = "Preco";
            this.precoDataGridViewTextBoxColumn.Name = "precoDataGridViewTextBoxColumn";
            this.precoDataGridViewTextBoxColumn.Width = 60;
            // 
            // angariadorDataGridViewTextBoxColumn
            // 
            this.angariadorDataGridViewTextBoxColumn.DataPropertyName = "Angariador";
            this.angariadorDataGridViewTextBoxColumn.HeaderText = "Angariador";
            this.angariadorDataGridViewTextBoxColumn.Name = "angariadorDataGridViewTextBoxColumn";
            this.angariadorDataGridViewTextBoxColumn.Width = 83;
            // 
            // Resultados_Prospecao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 558);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.Name = "Resultados_Prospecao";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resultados_Prospecao";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Resultados_Prospecao_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.potencialBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prabitarDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private prabitarDataSet prabitarDataSet;
        private System.Windows.Forms.BindingSource potencialBindingSource;
        private prabitarDataSetTableAdapters.PotencialTableAdapter potencialTableAdapter;
        private DataGridViewTextBoxColumn Angariador;
        private Label label1;
        private TextBox textBox1;
        private DataGridViewTextBoxColumn candidatoIDDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn telefoneDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nomeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn tituloAnuncioDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn uRLDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn precoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn angariadorDataGridViewTextBoxColumn;
    }
}