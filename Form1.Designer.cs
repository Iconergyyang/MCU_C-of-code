namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_sample = new System.Windows.Forms.Button();
            this.button_connect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_insert = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button_query = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label_cur = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_vol = new System.Windows.Forms.Label();
            this.label_temp = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.temp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_clear_db = new System.Windows.Forms.Button();
            this.graph = new ZedGraph.ZedGraphControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel1.Controls.Add(this.label_cur);
            this.splitContainer1.Panel1.Controls.Add(this.label_temp);
            this.splitContainer1.Panel1.Controls.Add(this.label_vol);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.graph);
            this.splitContainer1.Panel2.Controls.Add(this.button_clear_db);
            this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel2.Controls.Add(this.button_sample);
            this.splitContainer1.Panel2.Controls.Add(this.button_connect);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.button_insert);
            this.splitContainer1.Panel2.Controls.Add(this.textBox3);
            this.splitContainer1.Panel2.Controls.Add(this.button_query);
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1079, 422);
            this.splitContainer1.SplitterDistance = 474;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.number,
            this.curr,
            this.vol,
            this.temp});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(474, 422);
            this.dataGridView1.TabIndex = 15;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "200",
            "500",
            "1000"});
            this.comboBox1.Location = new System.Drawing.Point(12, 133);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button_sample
            // 
            this.button_sample.Location = new System.Drawing.Point(12, 74);
            this.button_sample.Name = "button_sample";
            this.button_sample.Size = new System.Drawing.Size(75, 23);
            this.button_sample.TabIndex = 17;
            this.button_sample.Text = "启动采样";
            this.button_sample.UseVisualStyleBackColor = true;
            this.button_sample.Click += new System.EventHandler(this.button_sample_Click);
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(12, 45);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 16;
            this.button_connect.Text = "打开串口";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(19, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 27);
            this.label1.TabIndex = 15;
            this.label1.Text = "label1";
            // 
            // button_insert
            // 
            this.button_insert.Location = new System.Drawing.Point(12, 244);
            this.button_insert.Name = "button_insert";
            this.button_insert.Size = new System.Drawing.Size(91, 27);
            this.button_insert.TabIndex = 7;
            this.button_insert.Text = "insert";
            this.button_insert.UseVisualStyleBackColor = true;
            this.button_insert.Click += new System.EventHandler(this.button_insert_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(93, 103);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(51, 21);
            this.textBox3.TabIndex = 11;
            // 
            // button_query
            // 
            this.button_query.Location = new System.Drawing.Point(12, 202);
            this.button_query.Name = "button_query";
            this.button_query.Size = new System.Drawing.Size(91, 27);
            this.button_query.TabIndex = 6;
            this.button_query.Text = "query";
            this.button_query.UseVisualStyleBackColor = true;
            this.button_query.Click += new System.EventHandler(this.button_query_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(93, 76);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(51, 21);
            this.textBox2.TabIndex = 12;
            // 
            // label_cur
            // 
            this.label_cur.AutoSize = true;
            this.label_cur.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_cur.Location = new System.Drawing.Point(395, 339);
            this.label_cur.Name = "label_cur";
            this.label_cur.Size = new System.Drawing.Size(79, 19);
            this.label_cur.TabIndex = 8;
            this.label_cur.Text = "current";
            this.label_cur.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(93, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 21);
            this.textBox1.TabIndex = 13;
            // 
            // label_vol
            // 
            this.label_vol.AutoSize = true;
            this.label_vol.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_vol.Location = new System.Drawing.Point(395, 367);
            this.label_vol.Name = "label_vol";
            this.label_vol.Size = new System.Drawing.Size(79, 19);
            this.label_vol.TabIndex = 10;
            this.label_vol.Text = "voltage";
            this.label_vol.Visible = false;
            // 
            // label_temp
            // 
            this.label_temp.AutoSize = true;
            this.label_temp.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_temp.Location = new System.Drawing.Point(425, 398);
            this.label_temp.Name = "label_temp";
            this.label_temp.Size = new System.Drawing.Size(49, 19);
            this.label_temp.TabIndex = 9;
            this.label_temp.Text = "temp";
            this.label_temp.Visible = false;
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // number
            // 
            this.number.HeaderText = "序号";
            this.number.Name = "number";
            // 
            // curr
            // 
            this.curr.HeaderText = "current";
            this.curr.Name = "curr";
            this.curr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // vol
            // 
            this.vol.HeaderText = "voltage";
            this.vol.Name = "vol";
            this.vol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // temp
            // 
            this.temp.HeaderText = "temperature";
            this.temp.Name = "temp";
            this.temp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button_clear_db
            // 
            this.button_clear_db.Location = new System.Drawing.Point(12, 277);
            this.button_clear_db.Name = "button_clear_db";
            this.button_clear_db.Size = new System.Drawing.Size(75, 23);
            this.button_clear_db.TabIndex = 19;
            this.button_clear_db.Text = "清空数据库";
            this.button_clear_db.UseVisualStyleBackColor = true;
            this.button_clear_db.Click += new System.EventHandler(this.button_clear_db_Click);
            // 
            // graph
            // 
            this.graph.Location = new System.Drawing.Point(139, 47);
            this.graph.Name = "graph";
            this.graph.ScrollGrace = 0D;
            this.graph.ScrollMaxX = 0D;
            this.graph.ScrollMaxY = 0D;
            this.graph.ScrollMaxY2 = 0D;
            this.graph.ScrollMinX = 0D;
            this.graph.ScrollMinY = 0D;
            this.graph.ScrollMinY2 = 0D;
            this.graph.Size = new System.Drawing.Size(438, 363);
            this.graph.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 422);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_insert;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button_query;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label_cur;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_vol;
        private System.Windows.Forms.Label label_temp;
        private System.Windows.Forms.Button button_connect;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button button_sample;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
        private System.Windows.Forms.DataGridViewTextBoxColumn curr;
        private System.Windows.Forms.DataGridViewTextBoxColumn vol;
        private System.Windows.Forms.DataGridViewTextBoxColumn temp;
        private System.Windows.Forms.Button button_clear_db;
        private ZedGraph.ZedGraphControl graph;

    }
}

