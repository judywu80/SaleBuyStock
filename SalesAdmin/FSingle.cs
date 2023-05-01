using FastReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SaleBuyStock;//

namespace SaleBuyStock
{
    public partial class FSingle : Form //**CRUD各forms應可共用, 正把此改為萬用sql模板** //因欄位格式不同
    {
        public FSingle()
        {
            InitializeComponent();
        }
        public static string LbTable;//
        DgvSet dataGridView0;        //
        //GetData sqs0;

        string cns, sqs, tbname, sqs1;
        SqlConnection cn; SqlCommand cmd;
        DataTable dt; 
        void OpenTable()
        {   //cn = new SqlConnection(cns)在form_load
            sqs = "SELECT * FROM " + tbname; 
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
            DataSet ds = new DataSet();
            adapt.Fill(ds);  //置入Dataset ds 
            dt = ds.Tables[0]; //adapt.Update(dt);
            dataGridView4.DataSource = dt;
        }
        void ExecuteQue()
        {
            cn.Open();
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[0].ColumnName+"", SqlDbType.NVarChar));
            cmd.Parameters["@"+dt.Columns[0].ColumnName+""].Value = int.Parse(textBox1.Text.Trim() == "" ? "0" : textBox1.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[1].ColumnName+"", SqlDbType.NVarChar));
            cmd.Parameters["@"+dt.Columns[1].ColumnName+""].Value = textBox2.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[2].ColumnName+"", SqlDbType.NVarChar)); 
            cmd.Parameters["@"+dt.Columns[2].ColumnName+""].Value = int.Parse(textBox3.Text.Trim() == "" ? "0" : textBox1.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[3].ColumnName+"", SqlDbType.NVarChar));
            cmd.Parameters["@"+dt.Columns[3].ColumnName+""].Value = int.Parse(textBox4.Text.Trim() == "" ? "0" : textBox1.Text.Trim());
            cmd.ExecuteNonQuery(); 
            cn.Close();//
        }
        private void button3_Click(object sender, EventArgs e) //試萬用模板成功^^
        {
            try
            {   
                sqs = "Select * From "+ tbname +" Where "; 
                if (textBox1.Text.Trim() != "") { sqs += dt.Columns[0].ColumnName+ " like '%" + textBox1.Text.Trim() + "%' "; }
                else { sqs += "1=1"; }
                if (textBox2.Text.Trim() != "") { sqs += "AND "+dt.Columns[1].ColumnName+" like '%" + textBox2.Text.Trim() + "%' "; }
                else { sqs += "AND 1=1"; }
                if ( textBox3.Text.Trim() != "") { sqs += "AND "+dt.Columns[2].ColumnName+" like '%" + textBox3.Text.Trim() + "%' "; }
                else { sqs += "AND 1=1"; }
                if (textBox4.Text.Trim() != "") { sqs += "AND "+dt.Columns[3].ColumnName+" like '%" + textBox4.Text.Trim() + "%' "; }
                else { sqs += "AND 1=1"; }
                MessageBox.Show(sqs);

                cn.Open(); 
                cmd = new SqlCommand(); 
                cmd.Connection = cn;
                cmd.CommandText = sqs; 
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable(); 
                dt.Load(dr); //載入SqlDataReader的資料 
                cn.Close(); //*
                dataGridView4.DataSource = dt; 
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show("資料發生錯誤:" + ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {   //已改for迴圈 //原類似寫法sqs1 = "Update Product Set prc=@prc,iqty=@iqty Where fgname=@fgname";
            sqs1 = "Update " + tbname + " Set ";
            //string tep = dt.Columns[0].ColumnName;
            for (int i = 1; i < dt.Columns.Count; i++) //i=1(非0) //原for (int i = 0; i < dt.Columns[i].ColumnName.Count/Length; i++)
            {
                sqs1 += dt.Columns[i].ColumnName + "=@" + dt.Columns[i].ColumnName + ","; 
            }   //@前等號"="也寫進去
            sqs1 = sqs1.Remove(sqs1.Length - 1, 1);  //最後逗點要去掉==除去尾端","
            sqs1 += " where " + dt.Columns[0].ColumnName+"=@"+ dt.Columns[0].ColumnName;

            ExecuteQue();
            OpenTable();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            sqs1 = "Delete From " + tbname + " Where " + dt.Columns[0].ColumnName + "=@" + dt.Columns[0].ColumnName + "";
            ExecuteQue();
            OpenTable();
        }
        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
        string path0;
        private void button7_Click(object sender, EventArgs e)
        {   //待簡化路徑參數
            path0 = dir.Parent.Parent.Parent.FullName;

            Report report = new Report(); //有1點錯就無法預覽列印(ctrl+p)
            if (comboBox1.Text == "開新報表")
            {   //開新報表設計
                report.Design();
            }
            else if (comboBox1.Text == "預覽列印")
            {   //預覽(報表)列印
                report.Load(path0 + @"\\Order_Tabu.frx");
                report.Show();
            }
            else
            {   //報表設計 -- 已存在報表
                //本案例不需提供ds，由報表設計設定 DataSource
                report.Load(path0 + @"\\Order_Tabu.frx");
                report.Design();
            }
            report.Dispose();
        }
        private void FSingle_Load(object sender, EventArgs e)
        {
            cns = FDouble.cns;  //form1的string cns前加public static,右式Form1.cns的cns即消除
            cn = new SqlConnection(cns); //*從form1 OpenT至此
            LbTbName.Text = LbTable;
            tbname = LbTable; //從開檔移至此處,因未用動態產生(欄位無法共用)
            OpenTable(); //

            dataGridView0 = new DgvSet(); //類別要先實作化
            dataGridView0.dgvSet(dataGridView4);
        }
        CRUD sqs0;
        private void button2_Click(object sender, EventArgs e) 
        {   //以下已改迴圈, 原sqs1 = "Insert Into "+ tbname + "("+dt.Columns[0].ColumnName+ ","+dt.Columns[1].ColumnName+ ","+dt.Columns[2].ColumnName+","+dt.Columns[3].ColumnName+") Values (@" + dt.Columns[0].ColumnName+ ",@"+dt.Columns[1].ColumnName+",@"+dt.Columns[2].ColumnName+",@"+dt.Columns[3].ColumnName+")";//Insert Into 後面要有空格 //原:必須宣告純量變數"打錯的欄位"
            ///*
            sqs1 = "Insert Into " + tbname + "(";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqs1+= dt.Columns[i].ColumnName + ",";
            }
            sqs1 = sqs1.Remove(sqs1.Length - 1, 1);  //最後逗點去掉
            sqs1 += ") Values (";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqs1 += "@"+dt.Columns[i].ColumnName + ",";
            }
            sqs1 = sqs1.Remove(sqs1.Length - 1, 1) + ")";  //最後逗點去掉 + 加上最後括號
            //sqs1 += ")";
            //*/

            //sqs0 = new CRUD();
            //sqs0.Insert();

            ExecuteQue();
            OpenTable();
            //string sqs = "";
            //sqs0 = new GetData();
            //sqs0.GetDataT(sqs);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tbname = LbTable;
            OpenTable();
        }
    }
}
