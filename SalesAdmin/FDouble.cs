using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using FastReport;

namespace SaleBuyStock
{
    public partial class FDouble : Form
    {
        public FDouble()
        {
            InitializeComponent();
        }
        public static string cns; //static共用, public在不同form可共用
        DgvSet dataGridView0; //
        public string sqs, sqs1;
        SqlConnection cn; SqlCommand cmd;
        DataTable dt; 
        //FDetail參數在下面
        public void OpenTable() //改public供form2使用
        {
            sqs = "SELECT * FROM " + comboBox1.Text;//**crud完dgv即可顯示
            cn = new SqlConnection(cns);//從btn開檔移至此方法
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn); //有用sqs
            //SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
            DataSet ds = new DataSet();
            adapt.Fill(ds); //改用comBx,原(ds, "Employee"),tname加雙引號 //置入Dataset ds 
            dt = ds.Tables[0];
            //adapt.Update(dt);*/
            dataGridView3.DataSource = dt;//先讀不用bs
        }
        public void ExecuteQue()
        {
            cn.Open();
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個sqs
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            if (comboBox1.Text == "YODR")
            {
                cmd.Parameters.Add(new SqlParameter("@order1", SqlDbType.NVarChar));//CUD皆新增以下
                cmd.Parameters["@order1"].Value = textBox1.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@odate", SqlDbType.Int));
                cmd.Parameters["@odate"].Value = int.Parse(textBox2.Text.Trim() == "" ? "0" : textBox2.Text.Trim());//整數要預設0,or null(需全部填才能cu);無法在ssms預設設0也因先後順序
                cmd.Parameters.Add(new SqlParameter("@cono", SqlDbType.NVarChar));
                cmd.Parameters["@cono"].Value = textBox3.Text.Trim();
            }
            if (comboBox1.Text == "YFGIO")
            {
                cmd.Parameters.Add(new SqlParameter("@vhno", SqlDbType.NVarChar));
                cmd.Parameters["@vhno"].Value = textBox1.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@vhdt", SqlDbType.Int));
                cmd.Parameters["@vhdt"].Value = int.Parse(textBox2.Text.Trim() == "" ? "0" : textBox2.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@cono", SqlDbType.NVarChar));
                cmd.Parameters["@cono"].Value = textBox3.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@DC", SqlDbType.NVarChar));
                cmd.Parameters["@DC"].Value = textBox4.Text.Trim();
            }
            cmd.ExecuteNonQuery();
            cn.Close();//沒有關會無法連續新增
        }
        public void ExecuteQue2() //雖元件在FDetail,但改權限讓form1使用
        {
            cn = new SqlConnection(cns);
            cn.Open(); 
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            if (comboBox1.Text == "YODR")
            {
                cmd.Parameters.Add(new SqlParameter("@ORDER1", SqlDbType.NVarChar));
                cmd.Parameters["@ORDER1"].Value = f2.txt1.Text.Trim();//前加f2供f1使用
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); 
                cmd.Parameters["@FGNO"].Value = f2.txt2.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@OD", SqlDbType.NVarChar));
                cmd.Parameters["@OD"].Value = int.Parse(f2.txt3.Text.Trim() == "" ? "0" : f2.txt3.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@QTY", SqlDbType.NVarChar)); 
                cmd.Parameters["@QTY"].Value = int.Parse(f2.txt4.Text.Trim() == "" ? "0" : f2.txt4.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.NVarChar));
                cmd.Parameters["@PRC"].Value = int.Parse(f2.txt5.Text.Trim() == "" ? "0" : f2.txt5.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@SQTY", SqlDbType.NVarChar)); 
                cmd.Parameters["@SQTY"].Value = int.Parse(f2.txt6.Text.Trim() == "" ? "0" : f2.txt6.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@NOTE1", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE1"].Value = f2.txt7.Text.Trim();
            }
            if (comboBox1.Text == "YFGIO")
            {
                cmd.Parameters.Add(new SqlParameter("@vhno", SqlDbType.NVarChar));
                cmd.Parameters["@vhno"].Value = f2.textBox1.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); 
                cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@OD", SqlDbType.NVarChar));
                cmd.Parameters["@OD"].Value = int.Parse(f2.textBox3.Text.Trim() == "" ? "0" : f2.textBox3.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@QTY", SqlDbType.NVarChar)); 
                cmd.Parameters["@QTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.NVarChar));
                cmd.Parameters["@PRC"].Value = int.Parse(f2.textBox5.Text.Trim() == "" ? "0" : f2.textBox5.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@batch", SqlDbType.NVarChar)); 
                cmd.Parameters["@batch"].Value = f2.textBox6.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@ORDER1", SqlDbType.NVarChar));
                cmd.Parameters["@ORDER1"].Value = f2.textBox7.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@NOTE1", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE1"].Value = f2.textBox8.Text.Trim();
            }
            cmd.ExecuteNonQuery(); 
            cn.Close();
        }
        public void ShowDetail() //dgv不同
        {
            if (comboBox1.Text == "YODR")
            {
                sqs = "SELECT * FROM " + "YODRDT";//明細檔不能用comboBox1.Text
            }
            if (comboBox1.Text == "YFGIO")
            {
                sqs = "SELECT * FROM " + "YFGIODT";
            }
            cn = new SqlConnection(cns);
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn); 
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dt = ds.Tables[0];
            //dataGridView2.DataSource = null;
            dataGridView2.DataSource = dt;//不在detaIl裡,在f1
        }
        Shopcart f3=new Shopcart();
        public void ExecuteQue3() 
        {
            cn = new SqlConnection(cns);
            cn.Open(); 
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = sqs1;

            cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); 
            cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim(); 
            cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar)); 
            cmd.Parameters["@IQTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());

            cmd.ExecuteNonQuery();
            cn.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {   //dataGridView3.DataSource = null; //or按其他btn無法顯現
            //sqs = "SELECT * FROM " + comboBox1.Text; //把這行加到OpenT()裡
            //cn = new SqlConnection(cns);
            OpenTable();  //開檔案免下cmd CRUD(ExecuteNonQuery)指令
        }
        private void button2_Click(object sender, EventArgs e) //下cmd "C"RUD指令
        {   
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Insert Into YODR(order1, odate, cono) Values (@order1, @odate, @cono)";
            }//sqs1 = "Insert Into YODR(order1, odate, cono) Values ('"+textBox1.Text.Trim()+ "', '"+textBox2.Text.Trim()+"', '"+textBox3.Text.Trim()+"')";
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Insert Into YFGIO(vhno, vhdt, cono, dc) Values (@vhno, @vhdt, @cono, @dc)";
            }
            ExecuteQue();
            
            OpenTable(); //Show data on dgv,若無仍會執行(只是dgv看不到) //這也有用到sqs 
        }
        private void button5_Click(object sender, EventArgs e) 
        {   //下cmd CR"U"D指令
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Update YODR Set odate=@odate, cono=@cono Where order1=@order1"; //在編號x時修改
            }
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Update YFGIO Set vhdt=@vhdt, cono=@cono, dc=@dc Where vhno=@vhno";
            }
            ExecuteQue();

            OpenTable();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {   
                if (comboBox1.Text == "YODR")
                {
                    sqs = "Select * From YODR Where "; //下sql條件,注意where後要空1格,否則與下行字串相連,會吃不到指令
                    if (textBox1.Text.Trim() != "")
                    { sqs += "order1 like '%" + textBox1.Text.Trim() + "%' "; }//Sql原寫法: like '%a%'
                    else { sqs += "1=1"; }//該欄位不打資料就可查詢
                    if (textBox2.Text.Trim() != "")
                    { sqs += "AND odate like '%" + textBox2.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                    else { sqs += "AND 1=1"; }
                    if (textBox3.Text.Trim() != "")
                    { sqs += "AND cono like '%" + textBox3.Text.Trim() + "%' "; }
                    else { sqs += "AND 1=1"; }
                }
                if (comboBox1.Text == "YFGIO")
                {
                    sqs = "Select * From YFGIO Where "; 
                    if (textBox1.Text.Trim() != "")
                    { sqs += "vhno like '%" + textBox1.Text.Trim() + "%' "; }
                    else { sqs += "1=1"; }
                    if (textBox2.Text.Trim() != "")
                    { sqs += "AND vhdt like '%" + textBox2.Text.Trim() + "%' "; }
                    else { sqs += "AND 1=1"; }
                    if (textBox3.Text.Trim() != "")
                    { sqs += "AND cono like '%" + textBox3.Text.Trim() + "%' "; }
                    else { sqs += "AND 1=1"; }
                    if (textBox4.Text.Trim() != "")
                    { sqs += "AND dc like '%" + textBox4.Text.Trim() + "%' "; }
                    else { sqs += "AND 1=1"; }
                }
                MessageBox.Show(sqs);

                //"查詢"用Reader就好,不用ExecuteNonQery()
                cn.Open();
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = sqs; //先宣告sqs,再傳給cmd.cmdText

                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);    //載入SqlDataReader的資料  //原dr(from db)有資料,讀db上dt的資料
                cn.Close(); 
                dataGridView3.DataSource = dt; 
            }
            catch (Exception ex) //try後沒打catch會紅字提醒 //finally不打可以嗎
            {
                cn.Close();
                MessageBox.Show("資料發生錯誤:" + ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e) //刪除考慮子明細,or會有孤兒
        {   
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Delete From YODR Where order1=@order1"; 
            } 
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Delete From YFGIO Where vhno=@vhno";
            }
            ExecuteQue();

            OpenTable();
        }
        private void 產品資料表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shopcart.LbTable = "YFGMAST"; //沒有這行執行後label消失; 直接在這先指定要開的table
            Shopcart shopcart = new Shopcart();
            shopcart.Show();
        }
        private void 客戶資料表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FClient.LbTable = "Customer";//先在這指定
            FClient c = new FClient();
            c.Show();
        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e) //關聯sqs //yodrdt有2主索引鍵(因與表頭重複,按ctrl一起右鍵)
        {   //在明細檔新增order1=2,找無表頭是因表頭無order1=2(孤兒,刪不掉)->要設定避免
            if (e.RowIndex >= 0)
            {
                if (comboBox1.Text == "YODR")
                {
                    textBox1.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
                    textBox3.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();                    
                    //sqs = $"Select * From YODRDT Where YODRDT.order1 = '{textBox1.Text.Trim()}' ";
                    sqs = "Select * From YODRDT Where YODRDT.order1 = '" + textBox1.Text.Trim() + "' "; //單引號內無空格
                }
                if (comboBox1.Text == "YFGIO")
                {
                    textBox1.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
                    textBox3.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    textBox4.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
                    
                    sqs = $"Select * From YFGIODT Where YFGIODT.vhno = '{textBox1.Text.Trim()}' ";
                }
                cn = new SqlConnection(cns);
                //開檔免下cmd CRUD指令
                SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
                DataSet ds = new DataSet();
                adapt.Fill(ds); //置入Dataset ds 
                dt = ds.Tables[0];
                dataGridView2.DataSource = dt;
            }
        }
        FDetail f2; //在btn裡才 new FDetail();
        public string op; //各命令字串: C,U,R,D 
        public bool allow = false; //預設紅燈,!allow=綠燈
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) //明細檔crud
        {   //U&R前要先把文字傳至f2,C則免(因無文字) //ru前要先按本事件,待優化
            if (e.RowIndex >= 0)
            {   //把dGV資料傳到f2 tB, f2 tB皆改為public (開綠燈讓f1資料進來)
                if (comboBox1.Text == "YODR")
                {
                    f2 = new FDetail();
                    f2.txt1.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    f2.txt2.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    f2.txt3.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    f2.txt4.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    f2.txt5.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                    f2.txt6.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
                    f2.txt7.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
                }
                if (comboBox1.Text == "YFGIO")
                {
                    f2 = new FDetail();
                    f2.textBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    f2.textBox2.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    f2.textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    f2.textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    f2.textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                    f2.textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
                    f2.textBox7.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
                    f2.textBox8.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString(); //
                }
                allow = true; //讓文字通過到f2
            }
        }
        private void button10_Click(object sender, EventArgs e) 
        {   //新增          
            op = "C";  //or I:Insert
            f2 = new FDetail();  //新開視窗         
            //clear_emp_control(); //清資料
            if (comboBox1.Text == "YODR") //*Text的tb大小寫有差
            { f2.txt1.Text = textBox1.Text; }  
            if (comboBox1.Text == "YFGIO") 
            {
                f2.textBox1.Text = textBox1.Text;
            }
            f2.Show(this);
        }
        private void button9_Click(object sender, EventArgs e)
        {   //查詢 (需在dgv2按一下後,再按查詢即可跳出f2)
            if (!allow) return;//
            //f2 = new FDetail(); //不再新開一個form,共用'C'新增的new form
            op = "R";//
            f2.Show(this);
        }
        private void button8_Click(object sender, EventArgs e)
        {   //更新
            if (!allow) return;

            op = "U";
            f2.Show(this);
        }
        private void button6_Click(object sender, EventArgs e)
        {   //刪除
            if (!allow) return;
            op = "D";
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Delete from Yodrdt where fgno=@fgno"; //刪order1會全被刪掉

                cn = new SqlConnection(cns);
                cn.Open(); 
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = sqs1;
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = f2.txt2.Text.Trim();
            }
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Delete from YFGIOdt where FGNO=@FGNO";//**明細檔不同vhno,但同fgno會被刪掉,指令同; 需再加 AND vhno=@vhno佳

                cn = new SqlConnection(cns);
                cn.Open();
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = sqs1;
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim();
            }
            cmd.ExecuteNonQuery(); 
            cn.Close();
            MessageBox.Show($"刪除成功。");

            //庫存刪除 (若一次刪多筆要迴圈; (未寫)刪主檔,明細檔也要刪)
            sqs1 = "Update YFGMAST Set IQTY= iqty- @IQTY Where fgno=@fgno"; //相減
            cn = new SqlConnection(cns);
            cn.Open(); 
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar));
            cmd.Parameters["@IQTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); 
            cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim();
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show($"庫存刪除成功。");

            ShowDetail();//dgv2
        }
        public void DelChild()
        {   //庫存*為更新用的*刪除 (若一次刪多筆要迴圈; (未寫)刪主檔,明細檔也要刪)
            sqs1 = "Update YFGMAST Set IQTY= iqty- @IQTY Where fgno=@fgno"; //相減
            cn = new SqlConnection(cns);//從開檔移至此方法
            cn.Open(); 
            cmd = new SqlCommand(); 
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar));
            cmd.Parameters["@IQTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); 
            cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim();
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show($"庫存準備更新。");
        }
        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void 盤點檔維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSingle.LbTable = "YINVENTORY"; //也要FSingle裡宣告LbTable
            FSingle f = new FSingle();
            f.Show();
        }
        private void 進銷檔維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "YFGIO";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "  系統時間：" + DateTime.Now.ToString("yyyy/MM/dd  hh:mm:ss");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cns = "Data Source=.\\sql2019; Database= YVMENUC1;" + //**改主機名**&資料庫(若無法改db名,重開即可)
            //      "User ID= sa; PWD= ";       //**改密碼**
                  "Trusted_Connection= True";  //改信任方式登入
            comboBox1.SelectedIndex = 0;//執行後直接顯示資料表,不用手動選

            dataGridView0 = new DgvSet(); //類別要先實作化 (老師也放form_load),or null
            dataGridView0.dgvSet(dataGridView2); //原發現有多個dgv1. 老師放開擋
            dataGridView0.dgvSet(dataGridView3); //原發現有多個dgv1
        }
    }
}
