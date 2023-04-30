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
    public partial class FSingle : Form //**CRUD各forms應可共用, 正把此改為洪萬用sql模板^^!** //因欄位格式不同
    {
        public FSingle()
        {
            InitializeComponent();
        }
        public static string LbTable;//
        DgvSet dataGridView0;        //
        //GetData sqs0;

        string cns, sqs, tbname, sqs1;
        SqlConnection cn; SqlCommand cmd;//不直實作參數
        DataTable dt; //實作化在btn裡才不會資料一直往下重複
        void OpenTable()
        {   //cn = new SqlConnection(cns)在form_load
            sqs = "SELECT * FROM " + tbname; //原未寫dgv會跳掉或空白,crud後重讀全部佳
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
            //SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
            DataSet ds = new DataSet();
            adapt.Fill(ds);  //置入Dataset ds 
            dt = ds.Tables[0]; //adapt.Update(dt);
            dataGridView4.DataSource = dt;//先讀不用bs //洪的OpenT()似無此行
        }
        void ExecuteQue()
        {
            cn.Open();
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[0].ColumnName+"", SqlDbType.NVarChar));//變數後+""(?) //原型別smallint?
            cmd.Parameters["@"+dt.Columns[0].ColumnName+""].Value = int.Parse(textBox1.Text.Trim() == "" ? "0" : textBox1.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[1].ColumnName+"", SqlDbType.NVarChar));
            cmd.Parameters["@"+dt.Columns[1].ColumnName+""].Value = textBox2.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[2].ColumnName+"", SqlDbType.NVarChar)); //Aaron用複選,fan用foreach
            cmd.Parameters["@"+dt.Columns[2].ColumnName+""].Value = int.Parse(textBox3.Text.Trim() == "" ? "0" : textBox1.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@"+dt.Columns[3].ColumnName+"", SqlDbType.NVarChar));
            cmd.Parameters["@"+dt.Columns[3].ColumnName+""].Value = int.Parse(textBox4.Text.Trim() == "" ? "0" : textBox1.Text.Trim());
            cmd.ExecuteNonQuery(); //原Customer未設cono為主索引鍵,不會擋重複cono
            cn.Close();//沒有關會無法連續新增!
        }
        private void button3_Click(object sender, EventArgs e) //試萬用模板成功^^
        {
            try
            {   
                sqs = "Select * From "+ tbname +" Where "; //下sql條件,注意 tbname 前後 & where後 要空1格,否則與下行字串相連,吃不到指令
                if (textBox1.Text.Trim() != "") { sqs += dt.Columns[0].ColumnName+ " like '%" + textBox1.Text.Trim() + "%' "; }//Sql原寫法: like '%a%'
                else { sqs += "1=1"; }//該欄位不打資料就可查詢 //再確認查詢範圍
                if (textBox2.Text.Trim() != "") { sqs += "AND "+dt.Columns[1].ColumnName+" like '%" + textBox2.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }
                if ( textBox3.Text.Trim() != "") { sqs += "AND "+dt.Columns[2].ColumnName+" like '%" + textBox3.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }//選單如何改+該欄位不打資料就可查詢
                if (textBox4.Text.Trim() != "") { sqs += "AND "+dt.Columns[3].ColumnName+" like '%" + textBox4.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }//該欄位不打資料就可查詢
                MessageBox.Show(sqs);

                cn.Open(); //'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
                cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
                cmd.Connection = cn;
                cmd.CommandText = sqs; //先宣告sqs,再傳給cmd.cmdText (or要按2次才能查)
                //"查詢"用Reader就好,不用ExecuteNonQery()
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable(); //實體化在這才不會資料一直往下重複
                dt.Load(dr); //載入SqlDataReader的資料 //原dr(from db)有資料,讀db上dt的資料 //dr.Close(); cmd.Dispose();似無變化
                cn.Close(); //*放這裡無誤
                dataGridView4.DataSource = dt; //為啥Table(應是View?)會自動排(假的嗎),數字&日期也排對?**Ans：DataTable也可以有自動排序，數字&日期也都會排對，如果使用DataView還可以有多欄位排序，及filte功能
            }
            catch (Exception ex) //try後沒打catch會紅字提醒 //finally不打可以嗎
            {
                cn.Close();
                MessageBox.Show("資料發生錯誤:" + ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {   //已改for迴圈,洪sql.edit有寫 //原類似寫法sqs1 = "Update Product Set prc=@prc,iqty=@iqty Where fgname=@fgname";//"@"變數t-sql寫法?
            sqs1 = "Update " + tbname + " Set ";
            //string tep = dt.Columns[0].ColumnName;
            for (int i = 1; i < dt.Columns.Count; i++) //i=1(非0) //原for (int i = 0; i < dt.Columns[i].ColumnName.Count/Length; i++)
            {
                sqs1 += dt.Columns[i].ColumnName + "=@" + dt.Columns[i].ColumnName + ","; 
            }   //一開始@前等號"="沒寫進去,找不到資料行4
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

            dataGridView0 = new DgvSet(); //類別要先實作化 (老師也放form_load),or null
            dataGridView0.dgvSet(dataGridView4);
        }
        CRUD sqs0;
        private void button2_Click(object sender, EventArgs e) //欄位如何改洪萬用(?)
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
