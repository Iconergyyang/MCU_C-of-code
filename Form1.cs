using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.IO.Ports;
using ZedGraph;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static string connectString = "";// = "Data Source=(LocalDB)\\v11.0;Initial Catalog=testdb;Integrated Security=True";
        private static SqlConnection sqlCnt = null;// = new SqlConnection(connectString);
        private static bool Closing_serial = false, start_or_pause = false;
        private static  List<byte> buffer = new List<byte>(4096);
        private static byte[] binary_data_1 = new byte[9];
        private static byte ack0, ack1;
        private static ushort volt, temperature, soc;
        private static short current;
        public static Thread th;
        public static Thread th_graph;

        // 绘制曲线的辅助变量
        private static PointPairList v1List = new PointPairList();
        private static PointPairList v2List = new PointPairList();
        private static PointPairList v3List = new PointPairList();

        private static int sampleCnt = 0;

        static object _object = new object();
        public delegate void UpdateGridView();
        public UpdateGridView updategrdv;

        public delegate void UpdateGraph(List<int> commdata);
        public UpdateGraph updategrph;

        //private void DoWork()
        //{
        //    Cinvokes ivk = new Cinvokes(Updata);
        //    BeginInvoke(ivk, null);
        //}

        //private void Updata(SqlConnection conn)
        public void UpdateGra(List<int> commdata)
        {
            double time = sampleCnt;                           //曲线的横坐标点
            v1List.Add(time, commdata[0]);  // 电流
            v2List.Add(time, commdata[1]);  // 电压
            v3List.Add(time, commdata[2]);  // 温度
            graph.GraphPane.YAxis.Scale.MajorStepAuto = true;    //自动Y轴的step
            graph.GraphPane.YAxis.Scale.MinorStepAuto = true;
            graph.AxisChange();                  // 更新曲线的x轴
            graph.Invalidate();                  // 更新曲线
            sampleCnt++;                                           //曲线的X轴添加横坐标点
        }

        private void Updata()
        {
            //lock (_object)
            //{
                //string connectString = "Data Source=C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\Microsoft SQL Server Local DB\\Instances\\v11.0;Initial Catalog=C:\\Users\\Administrator\\testdb;Integrated Security=True";
                //string connectString = "Data Source=(LocalDB)\\v11.0;Initial Catalog=testdb;Integrated Security=True";//(localdb)\v11.0 C:\\Users\\Administrator\\
                //SqlConnection sqlCnt = new SqlConnection(connectString);
                try
                {
                    if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("open fail" + ex.ToString());
                    return;
                }
                string sql = "select * from [dbo].[Table1]";
                //SqlCommand cmd = sqlCnt.CreateCommand();              // 创建SqlCommand对象
                SqlCommand cmd = new SqlCommand(sql, sqlCnt);
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "select * from [dbo].[Table1]";      // sql语句

                SqlDataReader reader = cmd.ExecuteReader();		      //执行SQL，返回一个“流”
                int cnt = 0;
                int colIdx = 0;
                //BeginInvoke((MethodInvoker)delegate()
                //{
                    dataGridView1.Rows.Clear();       
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(1);
                        colIdx = 0;
                        dataGridView1.Rows[cnt].Cells[colIdx++].Value = cnt + 1;
                        dataGridView1.Rows[cnt].Cells[colIdx++].Value = reader["curr"];
                        dataGridView1.Rows[cnt].Cells[colIdx++].Value = reader["vol"];
                        dataGridView1.Rows[cnt].Cells[colIdx++].Value = reader["temp"];
                        cnt++;
                        //MessageBox.Show(reader["curr"] + " " + reader["vol"] + " " + reader["temp"]);
                    }             
                    reader.Dispose();
                    //int tmp = dataGridView1.ColumnHeadersHeight + 
                    //    (dataGridView1.Rows.Count * dataGridView1.Rows[0].Height);
                    //if (tmp > dataGridView1.Height)
                    //{
                    //    dataGridView1 =
                    //        dataGridView1.FirstDisplayedScrollingRowIndex * dataGridView1.Rows[0].Height + 
                    //        dataGridView1.ColumnHeadersHeight;
                    //}
                    dataGridView1.FirstDisplayedScrollingRowIndex = cnt;
                //});
                //closeConn();
            //}
        }

        public void graphwork()
        {
            while (true)
            {
                List<int> comm = new List<int>(3) { current , volt, temperature};
                //comm.Add(current);
                //comm.Add(volt);
                //comm.Add(temperature);
                this.BeginInvoke(updategrph, comm);
                Thread.Sleep(timer1.Interval);
            }
        }
        
        public void threadwork()
        {
            //this.BeginInvoke(updategrdv);
            while (true) 
            {
                this.BeginInvoke(updategrdv); 
                Thread.Sleep(timer1.Interval); 
            }
        }

        public void openconn()
        {
            try
            {
                sqlCnt.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("open fail" + ex.ToString());
                return;
            }
        }

        public void closeConn()
        {
            sqlCnt.Close();
            sqlCnt.Dispose();
        }


        public void CreateChart(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;               //设置一个曲线对象变量
            myPane.Chart.Border.IsVisible = false;          //是否显示边框

            myPane.Title.Text = "电流 电压 温度曲线";                  //曲线标题
            myPane.XAxis.Title.Text = "采样计数";            //曲线x轴标题
            myPane.YAxis.Title.Text = "参数值";            //曲线y轴标题
              
            /************************************************************************/
            /*             设置第1条曲线                                           */
            /************************************************************************/
            // Generate a red curve with diamond symbols, and "Velocity" in the legend
            LineItem myCurve = myPane.AddCurve("电压",
               v1List, Color.Red, SymbolType.None);                     //添加曲线电压1
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);                //符号的颜色
            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;           //刻度颜色   
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;       //标题颜色   
            myPane.Legend.FontSpec.Size = 20;                       //图例大小
            myPane.YAxis.Color = Color.Red;                     //Y轴颜色
            myPane.YAxis.MajorTic.Color = Color.Red;             //Y轴大跨度颜色
            myPane.YAxis.MinorTic.Color = Color.Red;              //Y轴小跨度颜色
            myPane.YAxis.MajorTic.Size = 5;                      // Y轴大跨度 字体大小     
            myPane.YAxis.MajorTic.PenWidth = 2;     // Y轴大跨度 字体宽度
            myPane.YAxis.MinorTic.Size = 2.5f;//Y轴小跨度大小
            myPane.YAxis.MinorTic.PenWidth = 2;//Y轴小跨度厚度
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;       //大跨度是否双向
            myPane.YAxis.MinorTic.IsOpposite = false; //小跨度是否双向
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;      //零点线
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;       //Y轴和Y2轴是否同一方向
            myPane.YAxis.Scale.MaxAuto = true; //最大值自动
            myPane.YAxis.Scale.MinAuto = true;     //最小值自动
            myPane.YAxis.Scale.MajorStep = 0.5;        //设置大跨度的刻度间隔
            myPane.YAxis.Scale.MinorStep = 0.1;        //设置小跨度的刻度间隔
            /************************************************************************/
            /*              设置第2条曲线                                        */
            /************************************************************************/
            // Generate a blue curve with circle symbols, and "Acceleration" in the legend
            myCurve = myPane.AddCurve("电流",
               v2List, Color.Blue, SymbolType.None);

            myCurve.Symbol.Fill = new Fill(Color.White);

            myPane.Y2Axis.IsVisible = true;                 //Y2轴是否显示（右边）
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;        //设定y2轴线的颜色
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue; //设定y2轴标题的颜色
            myPane.Y2Axis.Color = Color.Blue;                   //设定Y2对应曲线的颜色
            myPane.Y2Axis.MajorTic.Color = Color.Blue;              //设定Y2轴刻度上大跨度的颜色
            myPane.Y2Axis.MinorTic.Color = Color.Blue;        //设定Y2轴刻度上小跨度的颜色
            myPane.Y2Axis.MajorTic.Size = 5;                     //设定Y2轴大跨度字体的厚度
            myPane.Y2Axis.MajorTic.PenWidth = 2;            //设定Y2轴大跨度字体的大小
            myPane.Y2Axis.MinorTic.Size = 2.5f;              //设定Y2轴小跨度字体的厚度
            myPane.Y2Axis.MinorTic.PenWidth = 2;         //设定Y2轴小跨度字体的厚度
            myPane.Y2Axis.MajorGrid.IsZeroLine = false;         //是否显示零点线
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            myPane.Y2Axis.MajorTic.IsOpposite = false;          //Y2轴线大跨度是否双向
            myPane.Y2Axis.MinorTic.IsOpposite = false;           //Y2轴线小跨度是否双向
            // Display the Y2 axis grid lines
            myPane.Y2Axis.MajorGrid.IsVisible = true;           //Y2轴线是否显示
            // Align the Y2 axis labels so they are flush to the axis
            myPane.Y2Axis.Scale.Align = AlignP.Inside;          //Y2轴刻度线在内部或外部
            /************************************************************************/
            /*              设置第3条曲线                                           */
            /************************************************************************/
            // Generate a green curve with square symbols, and "Distance" in the legend
            myCurve = myPane.AddCurve("温度",
               v3List, Color.Green, SymbolType.None);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            YAxis yAxis3 = new YAxis("");
            myPane.YAxisList.Add(yAxis3);
            yAxis3.Scale.FontSpec.FontColor = Color.Green;
            yAxis3.Title.FontSpec.FontColor = Color.Green;
            yAxis3.Color = Color.Green;
            yAxis3.MajorTic.Color = Color.Green;
            yAxis3.MinorTic.Color = Color.Green;
            yAxis3.MajorTic.Size = 5;
            yAxis3.MajorTic.PenWidth = 2;
            yAxis3.MinorTic.Size = 2.5f;
            yAxis3.MinorTic.PenWidth = 2;
            yAxis3.Scale.FontSpec.IsBold = true;
            yAxis3.MajorTic.IsInside = true;
            yAxis3.MinorTic.IsInside = true;
            yAxis3.MajorTic.IsOpposite = false;
            yAxis3.MinorTic.IsOpposite = false;
            yAxis3.Scale.Align = AlignP.Inside;
            yAxis3.MajorGrid.IsZeroLine = false;
            /******************************************/
            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.Color = Color.Gray;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.Gray;
            myPane.XAxis.MajorTic.Color = Color.Transparent;
            myPane.XAxis.MinorTic.Color = Color.Transparent;
            myPane.XAxis.Color = Color.Transparent;
            // Fill the axis background with a gradient
            //  myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            myPane.Chart.Fill = new Fill(Color.White, Color.WhiteSmoke, 45.0f);
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            connectString = "Data Source=(LocalDB)\\v11.0;Initial Catalog=testdb;Integrated Security=True";
            sqlCnt = new SqlConnection(connectString);
            //comboBox1.SelectedIndex = 1;
            //th = new Thread(Updata);
            //th = new Thread(new ThreadStart(DoWork));
            //th = new Thread(new ThreadStart(delegate
            //{
            //    threadwork();
            //}));
            //objThread.Start();
            CreateChart(graph);

            updategrdv = new UpdateGridView(Updata);
            updategrph = new UpdateGraph(UpdateGra);
        }

        private void button_insert_Click(object sender, EventArgs e)
        {
            //string connectString = "Data Source=C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\Microsoft SQL Server Local DB\\Instances\\v11.0;Initial Catalog=C:\\Users\\Administrator\\testdb;Integrated Security=True";
            //string connectString = "Data Source=(LocalDB)\\v11.0;Initial Catalog=testdb;Integrated Security=True";
            if (sqlCnt.State == ConnectionState.Closed) openconn();
            SqlCommand cmd = sqlCnt.CreateCommand();              // 创建SqlCommand对象
            cmd.CommandType = CommandType.Text;


            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "") cmd.CommandText = "INSERT INTO [dbo].[Table1] ([curr], [vol], [temp]) VALUES (" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + ")";      // sql语句
            else { MessageBox.Show("插入数据为空 请输入后重试~"); return; }
            //MessageBox.Show(cmd.CommandText);
            //cmd.Parameters.Add("@curr", label_cur.Text);
            //cmd.Parameters.Add("@vol", label_vol.Text);
            //cmd.Parameters.Add("@temp", label_temp.Text);
            
            try
            {
                int isInsertSuccess = cmd.ExecuteNonQuery();

                if (isInsertSuccess == 1) { }
                    //MessageBox.Show("insert( " + textBox1.Text + ", " + textBox2.Text + ", " + textBox3.Text + " )success!");
            }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.ToString());
            }
            finally
            {
                //closeConn();
            }
        }

        private void button_query_Click(object sender, EventArgs e)
        {
            lock (_object)
            {
                //string connectString = "Data Source=C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\Microsoft SQL Server Local DB\\Instances\\v11.0;Initial Catalog=C:\\Users\\Administrator\\testdb;Integrated Security=True";
                //string connectString = "Data Source=(LocalDB)\\v11.0;Initial Catalog=testdb;Integrated Security=True";//(localdb)\v11.0 C:\\Users\\Administrator\\
                //SqlConnection sqlCnt = new SqlConnection(connectString);
                try
                {
                    if (sqlCnt.State == ConnectionState.Closed) sqlCnt.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("open fail" + ex.ToString());
                    return;
                }
                string sql = "select * from [dbo].[Table1]";
                //SqlCommand cmd = sqlCnt.CreateCommand();              // 创建SqlCommand对象
                SqlCommand cmd = new SqlCommand(sql, sqlCnt);
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "select * from [dbo].[Table1]";      // sql语句

                SqlDataReader reader = cmd.ExecuteReader();		      //执行SQL，返回一个“流”
                int cnt = 0;
                int colIdx = 0;
                dataGridView1.Rows.Clear();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(1);
                    colIdx = 0;
                    dataGridView1.Rows[cnt].Cells[colIdx++].Value = cnt + 1;
                    dataGridView1.Rows[cnt].Cells[colIdx++].Value = reader["curr"];
                    dataGridView1.Rows[cnt].Cells[colIdx++].Value = reader["vol"];
                    dataGridView1.Rows[cnt].Cells[colIdx++].Value = reader["temp"];
                    cnt++;
                    //MessageBox.Show(reader["curr"] + " " + reader["vol"] + " " + reader["temp"]);
                }

                reader.Dispose();
                //closeConn();
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = "COM7";
                serialPort1.BaudRate = 115200;
                serialPort1.Open();
                button_connect.Text=serialPort1.IsOpen?"关闭串口":"打开串口";
                timer1.Enabled=serialPort1.IsOpen?true:false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        private void button_sample_Click(object sender, EventArgs e)
        {
            start_or_pause = !start_or_pause;
            if (start_or_pause)
            {
                th = new Thread(new ThreadStart(delegate
                {
                    threadwork();
                }));
                th.Start();

                th_graph = new Thread(new ThreadStart(delegate
                {
                    graphwork();
                }));
                th_graph.Start();

                button_sample.Text = "停止采样";
                byte[] package = new byte[9];
                package[0] = 0xaa;          //帧头
                package[1] = 6;             //lenth   
                package[2] = 0;             //ff代表正常，00代表正在处理其他事情
                package[3] = 0xA2;          //命令     
                package[4] = 0x90;          //数据
                package[5] = 0x90;          //数据    
                package[6] = 0x90;          //数据 
                package[7] = 0x90;          //数据  
                package[8] = 0x0d;          //帧尾
                try
                {
                    serialPort1.Write(package, 0, 9);    //发9个字节     
                }
                catch (System.InvalidOperationException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                button_sample.Text = "开始采样";
                th.Abort();

                th_graph.Abort();
            }
        }
        private void deal_byte()
        {
            switch (binary_data_1[3])       //根据命令来解析数据 
            {
                case 0:
                    ack0 = (byte)(binary_data_1[4] * 256 + binary_data_1[5]);     //识别码1
                    ack1 = (byte)(binary_data_1[6] * 256 + binary_data_1[7]);     //识别码2
                    break;
                case 1:
                    volt = (UInt16)(binary_data_1[4] * 256 + binary_data_1[5]);    //电压                 
                    temperature = (UInt16)(binary_data_1[6] * 256 + binary_data_1[7]);     //温度   
                    break;
                case 2:
                    soc = (UInt16)(binary_data_1[4] * 256 + binary_data_1[5]);   //SOC
                    current = (short)(binary_data_1[6] * 256 + binary_data_1[7]);   //电流   
                    break;
               
            }
        }
        //协议 0xaa 0x44 len(5) cmd(01) data(uint16) data1(unin16) XOR
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (Closing_serial) return;                             //关闭窗体时做一个标志位
            try
            {
               // Listening = true;                                   //串口事件发生标志位
                int n = serialPort1.BytesToRead;                     //读取串口事件发生时的字节数
                byte[] buf = new byte[n];                           //申明数组保存一帧数据      
                serialPort1.Read(buf, 0, n);                         //读取缓冲数据                
                bool data_1_catched = false;                        //缓存记录数据是否捕获到
                buffer.AddRange(buf);                               //缓存到listbuffer里面去
                while (buffer.Count >= 4)                           //这里用while是因为里面有break 和continue
                {
                    if (buffer[0] == 0xbb && buffer[1] == 0x44)         //判断头
                    {
                        int len = buffer[2];                            //下位机发送的字节数
                        if (buffer.Count < len + 4) break;              //如果接受数据数小于字节数，继续接受 
                        byte checksum = 0;                              //异或效验变量
                        for (int i = 3; i < len + 3; i++)               //len=5           
                        { checksum ^= buffer[i]; }                      //得到效验值
                        if (checksum != buffer[len + 3])                //如果效验失败，这个数据不要，继续接受下一个数据
                        { buffer.RemoveRange(0, len + 4); continue; }   //这里的continue是说while循环，不是if
                        buffer.CopyTo(0, binary_data_1, 0, len + 4);    //复制一条完整数据到具体的数据缓存                                     
                        data_1_catched = true;
                        buffer.RemoveRange(0, len + 4);                 //正确分析一条数据，从缓存中移除数据。
                    }
                    else
                    { buffer.RemoveAt(0); }                             //如果包的第一个数据错误，则重新开始
                }
                if (data_1_catched)
                {
                     deal_byte();                                    //用变量保存想要的数据           
                }
            }
            catch(System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sqlCnt.State == ConnectionState.Closed) 
                openconn();
            label1.Text = sqlCnt.State.ToString();// +timer1.Interval.ToString();
            SqlCommand cmd = sqlCnt.CreateCommand();              // 创建SqlCommand对象
            cmd.CommandType = CommandType.Text;

            //if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "") cmd.CommandText = "INSERT INTO [dbo].[Table1] ([curr], [vol], [temp]) VALUES (" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + ")";      // sql语句
            if (serialPort1.IsOpen && start_or_pause)
            {
                //if (current == 0 && volt == 0 && temperature == 0)
                //{ MessageBox.Show("未开始采样 请点击开始采样~"); return; }
                //else
                cmd.CommandText = "INSERT INTO [dbo].[Table1] ([curr], [vol], [temp]) VALUES (" + current + "," + volt + "," + temperature + ")";      // sql语句
                try
                {
                    string tmp = cmd.CommandText;
                    int isInsertSuccess = cmd.ExecuteNonQuery();

                    if (isInsertSuccess == 1) { }
                        //MessageBox.Show("insert" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + "success!");
                        //MessageBox.Show("insert (" + current + ", " + volt + ", " + temperature + " )success!");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    //MessageBox.Show(ex.ToString());
                    return;
                }
            }
            //closeConn();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //string tmp = comboBox1.SelectedText;
                    timer1.Interval = Convert.ToInt32(comboBox1.Text);
                    break;
                case 1:
                    timer1.Interval = Convert.ToInt32(comboBox1.Text);
                    break;
                case 2:
                    timer1.Interval = Convert.ToInt32(comboBox1.Text);
                    break;
                default: break;
            }
        }

        private void button_clear_db_Click(object sender, EventArgs e)
        {
            if (sqlCnt.State == ConnectionState.Closed) openconn();
            SqlCommand cmd = sqlCnt.CreateCommand();              // 创建SqlCommand对象
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "TRUNCATE TABLE [dbo].[Table1]";
            
            try
            {
                int isInsertSuccess = cmd.ExecuteNonQuery();

                if (isInsertSuccess == -1)
                {
                    MessageBox.Show("数据库数据已全部清除");
                    dataGridView1.Rows.Clear();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //closeConn();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                closeConn();
                th.Abort();
                th_graph.Abort();
            }
            catch (System.Exception ex)
            {
                return;
            }
     
        }

    }
}
