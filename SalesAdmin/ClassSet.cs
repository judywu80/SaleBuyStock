using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;//
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace SaleBuyStock
{
    public class DgvSet
    {
        public void dgvSet(DataGridView dataGridView0) //至少1存取子(?)
        {   //原dataGridView為dataGridView1
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            dataGridView0.EnableHeadersVisualStyles = false;
            columnHeaderStyle.BackColor = Color.FromArgb(204, 255, 204);
            columnHeaderStyle.ForeColor = Color.FromArgb(0, 0, 255);                  //字體色 藍色;
            dataGridView0.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            DataGridViewCellStyle rowHeaderStyle = new DataGridViewCellStyle();
            dataGridView0.EnableHeadersVisualStyles = false;
            rowHeaderStyle.BackColor = Color.FromArgb(204, 255, 204);
            dataGridView0.RowHeadersDefaultCellStyle = rowHeaderStyle;
            dataGridView0.Font = new Font("Arial", 11, FontStyle.Regular);            //字體大小含欄位
            dataGridView0.ForeColor = Color.FromArgb(25, 25, 112);
            dataGridView0.DefaultCellStyle.BackColor = Color.FromArgb(255, 222, 222);                 //Cells背景色 淺橘色
            dataGridView0.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(171, 255, 255);  //偶數列背景色 淺藍色
            dataGridView0.AutoResizeColumns();    //欄寬自動調整
        }
    }
    //如何變類別公用, 存取子(建構子)? 變數改屬性 //看許程式寫法
    public class GetData
    {   //變數改屬性
        public string sqs {get;set;}
        public string sqs1 {get;set;}
        public SqlConnection cn {get;set;} 
        public SqlCommand cmd {get;set;} //不能直實作參數
        public DataTable dt {get;set;}
        public string cns { get; set; }
        public string tbname { get; set; }
        public static string LbTable
        {
            get { return LbTable; } //
            set
            {
                FSingle.LbTable = "YINVENTORY";
            }
        }
        //public void OpenTable()
        //{
        //    cn = new SqlConnection(cns)在form_load
        //    sqs = "SELECT * FROM " + tbname;
        //    SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
        // //   SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
        //    DataSet ds = new DataSet();     //回傳DataSet (跨平台班類別習題)
        //    adapt.Fill(ds); //改用comBx,原(ds, "Employee"),tname加雙引號 //置入Dataset ds 
        //    dt = ds.Tables[0];
        //    adapt.Update(dt);
        //    dataGridView4.DataSource = dt;//先讀不用bs //洪的OpenTable()似無此行
        //}
        public GetData() { } //控制各個Form (FormMain,FDouble...) 中靜態物件(變數,元件...)需此宣告，建立時一般類別建立方式 GetData dt=new GetData()
        public DataSet GetDataT(string sqs)
        {   //可寫進tb,但會有錯誤訊息
            cns = "Data Source=.\\sql2019; Database= YVMENUC1;" + //**改主機名**&資料庫(若無法改db名,重開即可)
            //      "User ID= sa; PWD= ";       //**改密碼**
                  "Trusted_Connection= True";
            tbname = LbTable; 

            cn = new SqlConnection(cns);
            sqs = "SELECT * FROM " + tbname; //tbname=null(?); 每個tbname都不一樣
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
            DataSet ds = new DataSet();
            adapt.Fill(ds); //'ConnectionString 屬性尚未初始化。'
            dt = ds.Tables[0];
            //cn.Close(); //在ExecuteQ方法裡
            
            return ds;  //方法要回傳值(or GetDataT紅字lol)
        }
    }

    public class CRUD
    {
        public string tbname;
        public DataTable dt;//原dt為null //為啥老師是private
        
        /*public CRUD(DataTable tb)
        {
            //listData = data;
            //aed = ae;
            //tb = dt;
            dt=tb;
            tbname = tb.TableName;
            //FieldType = CreateFtype(dt);
        }*/
        //SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
        //public 
        //DataSet ds = new DataSet();
        //adapt.Fill(ds);  //置入Dataset ds 
        //dt = ds.Tables[0];
        //dt = tbname;
        public string Insert() //
        {
            //dt=tb
            //tbname= dt.TableName;
            
            //dt= new DataTable(); //有進展

            string sqs0 = "Insert Into " + tbname + "("; //宣告string sqs0
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqs0 += dt.Columns[i].ColumnName + ",";
            }
            sqs0 = sqs0.Remove(sqs0.Length - 1, 1);  //最後逗點去掉
            sqs0 += ") Values (";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqs0 += "@" + dt.Columns[i].ColumnName + ",";
            }
            sqs0 = sqs0.Remove(sqs0.Length - 1, 1) + ")";

            return sqs0;//
        }
    }
}
