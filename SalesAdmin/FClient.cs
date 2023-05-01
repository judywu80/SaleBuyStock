using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SaleBuyStock
{
    public partial class FClient : Form
    {
        public FClient()
        {
            InitializeComponent();
        }  
        public static string LbTable;//
        DgvSet dataGridView0; //
        string cns, sqs, tbname, sqs1;
        SqlConnection cn; SqlCommand cmd;
        DataTable dt; 
        void OpenTable()
        {   //cn = new SqlConnection(cns)在form_load
            sqs = "SELECT * FROM " + tbname;
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
            DataSet ds = new DataSet();
            adapt.Fill(ds); 
            dt = ds.Tables[0];
            dataGridView4.DataSource = dt;
        }
        void ExecuteQue()
        {
            cn.Open(); 
            cmd = new SqlCommand(); 
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            //checkedListBox1.SelectedIndex = 0; //不可用,新增時會強制寫入現金
            cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
            cmd.Parameters["@CONO"].Value = textBox1.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar));
            cmd.Parameters["@NAME"].Value = textBox2.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@PAY", SqlDbType.NVarChar)); //Pay Value這樣寫crud容易有出問題
            //MessageBox.Show($"{checkedListBox1.Items[checkedListBox1.SelectedIndex]}");
            //MessageBox.Show($"{checkedListBox1.SelectedIndices}"); //複數試不出來
            //foreach (var p in checkedListBox1.SelectedIndices) {
            //    MessageBox.Show($"{checkedListBox1.SelectedIndices}");}
            cmd.Parameters["@PAY"].Value = checkedListBox1.Items[checkedListBox1.SelectedIndex];//看反藍,非勾選的! //括號內原數字寫法改選取by fan
            cmd.Parameters.Add(new SqlParameter("@IADD", SqlDbType.NVarChar)); //Aaron用複選,fan用foreach
            cmd.Parameters["@IADD"].Value = comboBox1.Text + textBox3.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@EMAIL", SqlDbType.NVarChar));
            cmd.Parameters["@EMAIL"].Value = textBox4.Text.Trim();
            cmd.ExecuteNonQuery(); 
            cn.Close();//
        }
        private void button2_Click(object sender, EventArgs e) //
        {   
            sqs1 = "Insert Into Customer (CONO,NAME,PAY,IADD,EMAIL) Values (@CONO,@NAME,@PAY,@IADD,@EMAIL)";
            checkedListBox1.SelectedIndex = 0;//or皆現金/ indx = -1紅字

            ExecuteQue();

            OpenTable();
        }
        private void button3_Click(object sender, EventArgs e)
        {   //查詢
            try
            {   //原sql語法有問題,用debug+F10顯示sqs內容值去找 //無效資料行: 沒改到tabe名
                sqs = "Select * From Customer Where "; //下sql條件,注意where後要空1格,否則與下行字串相連,吃不到指令
                //checkedListBox1.GetItemText = ""; checkedListBox.??=false
                //checkedListBox1.SelectedIndex = 0; //如何設定 //radio btn才是單選,類作業4,書7-23,3個rbtn共用事件valueChanged,用Convert.ToString(x)物件轉值,就可寫入sql
                /*foreach (int i in checkedListBox1.CheckedIndices)
                { checkedListBox1.SetItemCheckState(i, CheckState.Unchecked); }*/
                comboBox1.Text = "";//查詢時不下任何字串條件,含"縣市". //cono只打1,其他所有條件皆出現(cLB未好)
                if (textBox1.Text.Trim() != "") { sqs += "CONO like '%" + textBox1.Text.Trim() + "%' "; }//Sql原寫法: like '%a%'
                else { sqs += "1=1"; }//該欄位不打資料就可查詢 //再確認查詢範圍
                if (textBox2.Text.Trim() != "") { sqs += "AND NAME like '%" + textBox2.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }//該欄位不打資料就可查詢
                //if (checkedListBox1.Items[checkedListBox1.SelectedIndex].ToString() != "") { sqs += "AND PAY like '%" + checkedListBox1.Items[checkedListBox1.SelectedIndex] + "%' "; }//Sql多條件寫法加: And
                //if (checkedListBox1.Items[checkedListBox1.SelectedIndex].ToString() != "") { sqs += "AND PAY like '%" + checkedListBox1.Items[checkedListBox1.SelectedIndex] + "%' "; }//Sql多條件寫法加: And
                //else { sqs += "AND 1=1"; }//如何該欄位不勾資料就可查詢?
                
                //if (checkedListBox1 != null) { sqs += $"and pay like'%{checkedListBox1.CheckedItems[0]}%'"; }
                //else { sqs+="and 1=1";}

                if (comboBox1.Text + textBox3.Text.Trim() != "") { sqs += "AND IADD like '%" + comboBox1.Text + textBox3.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }//選單如何改+該欄位不打資料就可查詢
                if (textBox4.Text.Trim() != "") { sqs += "AND EMAIL like '%" + textBox4.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }
                MessageBox.Show(sqs);

                cn.Open(); 
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = sqs; 

                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                cn.Close(); //*
                dataGridView4.DataSource = dt;
            }
            catch (Exception ex) //try後沒打catch會紅字提醒 //finally不打可
            {
                cn.Close();
                MessageBox.Show("資料發生錯誤:" + ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {   //更正
            sqs1 = "Update Customer Set NAME=@NAME,PAY=@PAY,IADD=@IADD,EMAIL=@EMAIL Where CONO=@CONO";//"@"變數t-sql寫法?
            checkedListBox1.SelectedIndex = 0;//or皆現金/ indx = -1紅字

            ExecuteQue();

            OpenTable();
        }
        private void button5_Click(object sender, EventArgs e)
        {   
            sqs1 = "Delete From Customer Where CONO=@CONO";
            checkedListBox1.SelectedIndex = 0;//or皆現金/ indx = -1紅字/ cn.Open開關問題

            ExecuteQue();

            OpenTable();
        }
        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {   //將cLB改單選
            if (e.NewValue == CheckState.Checked)
            {
                foreach (int i in checkedListBox1.CheckedIndices)
                { checkedListBox1.SetItemCheckState(i, CheckState.Unchecked); }
            }
        }
        private void FCustomer_Load(object sender, EventArgs e)
        {
            /*cns = "Data Source=.\\sql2019; Database= YVMENUC;" + //**改主機名**&資料庫
            //      "User ID= sa; PWD= ";       //**改密碼**
                  "Trusted_Connection= True";  //改信任方式登入*/
            cns = FDouble.cns; // //form1的string cns前加public static,右式Form1.cns的cns即消除
            cn = new SqlConnection(cns); //*從form1 OpenT至此
            LbTbName.Text = LbTable;
            tbname = LbTable;
            OpenTable();

            dataGridView0 = new DgvSet(); //類別要先實作化 
            dataGridView0.dgvSet(dataGridView4); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tbname = LbTable;
            //sqs = "SELECT * FROM " + tbname;
            //建類別OpenTable()供各form用(洪沒建,似double不同法開檔)
            OpenTable();
        }
    }
}
