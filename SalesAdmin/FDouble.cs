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

namespace SaleBuyStock //忘資料夾名怎改(直改會找不到) //改名後左邊小工具圖"確認"後,Program.cs會幫改,只要改這就好
{
    public partial class FDouble : Form //如何改方案總管form順序(?)
    {
        public FDouble()
        {
            InitializeComponent();
        }
        public static string cns; //static共用, public不同form也可共用
        DgvSet dataGridView0; //
        public string sqs, sqs1;
        SqlConnection cn; SqlCommand cmd;//不能直接實作參數
        DataTable dt; //實體化在btn裡才不會資料一直往下重複
        //FDetail參數在下面
        public void OpenTable() //改pub供f2使用
        {
            sqs = "SELECT * FROM " + comboBox1.Text;//**crud完dgv才不會跳掉

            cn = new SqlConnection(cns);//從開檔移至此方法
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn); //有用sqs!
            //SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
            DataSet ds = new DataSet();
            adapt.Fill(ds); //改用comBx,原(ds, "Employee"),tname加雙引號 //置入Dataset ds 
            dt = ds.Tables[0];
            //adapt.Update(dt);*/
            dataGridView3.DataSource = dt;//先讀不用bs //洪的OpenTable()似無此行
        }
        public void ExecuteQue() //有把洪的smallint(OD),real(price)等改成int
        {
            cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
            cmd.Connection = cn;
            //洪說會再提供更方便的通用版 打sql指令&欄位(for迴圈column.count)
            cmd.CommandText = sqs1;
            if (comboBox1.Text == "YODR")
            {
                cmd.Parameters.Add(new SqlParameter("@order1", SqlDbType.NVarChar));//CUD皆新增以下
                cmd.Parameters["@order1"].Value = textBox1.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@odate", SqlDbType.Int));
                cmd.Parameters["@odate"].Value = int.Parse(textBox2.Text.Trim() == "" ? "0" : textBox2.Text.Trim());//整數要預設0,or null(需全部填才能cu);預設設0也無法因先後順序
                cmd.Parameters.Add(new SqlParameter("@cono", SqlDbType.NVarChar));
                cmd.Parameters["@cono"].Value = textBox3.Text.Trim();
            }
            if (comboBox1.Text == "YFGIO")
            {
                cmd.Parameters.Add(new SqlParameter("@vhno", SqlDbType.NVarChar));//CUD皆新增以下
                cmd.Parameters["@vhno"].Value = textBox1.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@vhdt", SqlDbType.Int));
                cmd.Parameters["@vhdt"].Value = int.Parse(textBox2.Text.Trim() == "" ? "0" : textBox2.Text.Trim());//整數要預設0,or null(需全部填才能cu);預設設0也無法因先後順序
                cmd.Parameters.Add(new SqlParameter("@cono", SqlDbType.NVarChar));
                cmd.Parameters["@cono"].Value = textBox3.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@DC", SqlDbType.NVarChar));
                cmd.Parameters["@DC"].Value = textBox4.Text.Trim();
            }
            cmd.ExecuteNonQuery(); //重複
            cn.Close();//沒有關會無法連續新增![為啥?]
        }
        public void ExecuteQue2() //雖元件在FD,但改權限讓f1使用
        {
            cn = new SqlConnection(cns);//從開檔移至此方法
            cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            if (comboBox1.Text == "YODR")
            {
                cmd.Parameters.Add(new SqlParameter("@ORDER1", SqlDbType.NVarChar));
                cmd.Parameters["@ORDER1"].Value = f2.txt1.Text.Trim();//前加f2供f1使用
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); //Pay Value這樣寫crud容易有出問題
                cmd.Parameters["@FGNO"].Value = f2.txt2.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@OD", SqlDbType.NVarChar));//先跟FGNO換個位置
                cmd.Parameters["@OD"].Value = int.Parse(f2.txt3.Text.Trim() == "" ? "0" : f2.txt3.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@QTY", SqlDbType.NVarChar)); //Aaron用複選,fan用foreach
                cmd.Parameters["@QTY"].Value = int.Parse(f2.txt4.Text.Trim() == "" ? "0" : f2.txt4.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.NVarChar));
                cmd.Parameters["@PRC"].Value = int.Parse(f2.txt5.Text.Trim() == "" ? "0" : f2.txt5.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@SQTY", SqlDbType.NVarChar)); //Aaron用複選,fan用foreach
                cmd.Parameters["@SQTY"].Value = int.Parse(f2.txt6.Text.Trim() == "" ? "0" : f2.txt6.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@NOTE1", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE1"].Value = f2.txt7.Text.Trim();
            }
            if (comboBox1.Text == "YFGIO")
            {
                cmd.Parameters.Add(new SqlParameter("@vhno", SqlDbType.NVarChar));
                cmd.Parameters["@vhno"].Value = f2.textBox1.Text.Trim();//前加f2供f1使用
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); //**原型別snallint
                cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@OD", SqlDbType.NVarChar));//先跟FGNO換個位置
                cmd.Parameters["@OD"].Value = int.Parse(f2.textBox3.Text.Trim() == "" ? "0" : f2.textBox3.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@QTY", SqlDbType.NVarChar)); //Aaron用複選,fan用foreach
                cmd.Parameters["@QTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.NVarChar));//**原型別real
                cmd.Parameters["@PRC"].Value = int.Parse(f2.textBox5.Text.Trim() == "" ? "0" : f2.textBox5.Text.Trim());
                cmd.Parameters.Add(new SqlParameter("@batch", SqlDbType.NVarChar)); //Aaron用複選,fan用foreach
                cmd.Parameters["@batch"].Value = f2.textBox6.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@ORDER1", SqlDbType.NVarChar));
                cmd.Parameters["@ORDER1"].Value = f2.textBox7.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@NOTE1", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE1"].Value = f2.textBox8.Text.Trim();
            }
            cmd.ExecuteNonQuery(); //原Customer未設cono為主索引鍵,不會擋重複cono
            cn.Close();//沒關會無法連續新增!
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
            cn = new SqlConnection(cns);//從開檔移至此方法
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn); //有用sqs! //被改成sqs1
            //SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
            DataSet ds = new DataSet();
            adapt.Fill(ds); //改用comBx,原(ds, "Employee"),tname加雙引號 //置入Dataset ds 
            dt = ds.Tables[0];

            //dataGridView2.DataSource = null;
            dataGridView2.DataSource = dt;//不在detaIl裡,在f1
            //dataGridView2.DataSource = ds;//不在detaIl裡,在f1
        }
        Shopcart f3=new Shopcart();
        public void ExecuteQue3() //雖元件在FD,但改權限讓f1使用
        {
            cn = new SqlConnection(cns);//從開檔移至此方法
            cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            //if (comboBox1.Text == "YFGIO")
            //{
            cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); //**原型別snallint
            cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim(); //fgno沒變
            cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar)); //原@QTY
            //cmd.Parameters["@IQTY"].Value = int.Parse((int.Parse(f2.textBox4.Text.Trim()) + int.Parse(f3.textBox7.Text.Trim())).ToString() == "" ? "0" : (int.Parse(f2.textBox4.Text.Trim()) + int.Parse(f3.textBox7.Text.Trim())).ToString());//1+0=10;原三元運算子後沒改到lol
            //cmd.Parameters["@IQTY"].Value = int.Parse($"{f2.textBox4.Text.Trim()}+{f3.textBox7.Text.Trim()}" == "" ? "0" : $"{f2.textBox4.Text.Trim()}+{f3.textBox7.Text.Trim()}");//1+0=10;原三元運算子後沒改到lol
            //cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar)); //原@QTY
            cmd.Parameters["@IQTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());//三元運算子後沒改到lol
            //cmd.Parameters.Add(new SqlParameter("@qty", SqlDbType.NVarChar)); //加這2行變10? //與iqty相反
            //cmd.Parameters["@qty"].Value = int.Parse(f3.textBox7.Text.Trim() == "" ? "0" : f3.textBox7.Text.Trim());
            //}
            cmd.ExecuteNonQuery(); //原Customer未設cono為主索引鍵,不會擋重複cono
            cn.Close();//沒關會無法連續新增!
        }
        private void button1_Click(object sender, EventArgs e)
        {   //dataGridView3.DataSource = null; //or按其他btn無法顯現
            //sqs = "SELECT * FROM " + comboBox1.Text; //把這行加到OpenT()裡
            //cn = new SqlConnection(cns);
            //開檔案免下cmd CRUD(ExecuteNonQuery)指令
            OpenTable();

        }
        private void button2_Click(object sender, EventArgs e) //下cmd "C"RUD指令
        {   //原有2個sqs(adapter &crud),@order1突有錯說要輸入純量變數,但有寫進tb
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Insert Into YODR(order1, odate, cono) Values (@order1, @odate, @cono)";//"@"變數t-sql寫法?
            }//sqs1 = "Insert Into YODR(order1, odate, cono) Values ('"+textBox1.Text.Trim()+ "', '"+textBox2.Text.Trim()+"', '"+textBox3.Text.Trim()+"')";//"@"變數t-sql寫法?
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Insert Into YFGIO(vhno, vhdt, cono, dc) Values (@vhno, @vhdt, @cono, @dc)";//"@"變數t-sql寫法?
            }
            ExecuteQue(); //若把此欄移到OpenT法下還是會執行,只是dgv看不到(此為蔡作法,洪似先OpenT,再ExeQ,最後又OpenT..)
            //Show data on dgv(不加以下資料還是會寫入tb,只是dgv看不到)
            OpenTable(); //這也有用到sqs,or無法在物件'order'上插入重複索引鍵。 
            //蔡CRUD完都會重整"SELECT * FROM dept",or資料突消失(但有更新),功能同OpenTable法?
        }
        private void button5_Click(object sender, EventArgs e) //已修好:原更正完排序變少,OpenT裡sqs有再select*(all)
        {   //下cmd CR"U"D指令
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Update YODR Set odate=@odate, cono=@cono Where order1=@order1"; //在編號x時修改
            }
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Update YFGIO Set vhdt=@vhdt, cono=@cono, dc=@dc Where vhno=@vhno"; //copy sql指令要記得**改dt name**
            }
            ExecuteQue();
            //Show data on dgv(不加以下資料還是會寫入tb,只是dgv看不到)
            OpenTable();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {   //原sql語法有問題,用debug+F10顯示sqs內容值去找
                if (comboBox1.Text == "YODR")
                {
                    sqs = "Select * From YODR Where "; //下sql條件,注意where後要空1格,否則與下行字串相連,吃不到指令
                    if (textBox1.Text.Trim() != "")
                    { sqs += "order1 like '%" + textBox1.Text.Trim() + "%' "; }//Sql原寫法: like '%a%'
                    else { sqs += "1=1"; }//該欄位不打資料就可查詢 //再確認查詢範圍
                    if (textBox2.Text.Trim() != "")
                    { sqs += "AND odate like '%" + textBox2.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                    else { sqs += "AND 1=1"; }//該欄位不打資料就可查詢
                    if (textBox3.Text.Trim() != "")
                    { sqs += "AND cono like '%" + textBox3.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                    else { sqs += "AND 1=1"; }//該欄位不打資料就可查詢
                }
                if (comboBox1.Text == "YFGIO")
                {
                    sqs = "Select * From YFGIO Where "; //下sql條件,注意where後要空1格,否則與下行字串相連,吃不到指令
                    if (textBox1.Text.Trim() != "")
                    { sqs += "vhno like '%" + textBox1.Text.Trim() + "%' "; }//Sql原寫法: like '%a%'
                    else { sqs += "1=1"; }//該欄位不打資料就可查詢 //再確認查詢範圍
                    if (textBox2.Text.Trim() != "")
                    { sqs += "AND vhdt like '%" + textBox2.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                    else { sqs += "AND 1=1"; }//該欄位不打資料就可查詢
                    if (textBox3.Text.Trim() != "")
                    { sqs += "AND cono like '%" + textBox3.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                    else { sqs += "AND 1=1"; }
                    if (textBox4.Text.Trim() != "")
                    { sqs += "AND dc like '%" + textBox4.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                    else { sqs += "AND 1=1"; }
                }
                MessageBox.Show(sqs);
                //"查詢"用Reader就好,不用ExecuteNonQery()

                cn.Open();
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = sqs; //先宣告sqs,再傳給cmd.cmdText

                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable(); //實體化在這才不會資料一直往下重複
                dt.Load(dr);    //載入SqlDataReader的資料  //原dr(from db)有資料,讀db上dt的資料
                                //dr.Close(); cmd.Dispose(); //似無變化
                cn.Close(); //*放這裡無誤,往前放會有問題
                dataGridView3.DataSource = dt; //為啥Table(應是View?)會自動排(假的嗎),數字&日期也排對?
                                               //**Ans：DataTable也可以有自動排序，數字&日期也都會排對，如果使用DataView還可以有多欄位排序，及filte功能
            }
            catch (Exception ex) //try後沒打catch會紅字提醒 //finally不打可以嗎
            {
                cn.Close();
                MessageBox.Show("資料發生錯誤:" + ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e) //刪除考慮子明細,會有孤兒
        {   //原因整數關係,3欄都要打的跟資料一樣才能刪, 應不能連續刪(要先查詢)
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Delete From YODR Where order1=@order1"; //刪單號第x
            }//以下是用事件呼叫
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Delete From YFGIO Where vhno=@vhno";
            }
            ExecuteQue();
            //Show data on dgv(不加以下資料還是會寫入tb,只是dgv看不到)
            OpenTable();
        }
        private void 產品資料表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Shopcart.LbTable = "Product";//沒有這行執行後label消失; 直接在這先指定要開的table
            Shopcart.LbTable = "YFGMAST";
            Shopcart shopcart = new Shopcart();
            shopcart.Show();
        }
        private void 訂單檔維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {   //可先放在主頁
        }
        private void 客戶資料表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FClient.LbTable = "Customer";//先在這指定
            FClient c = new FClient();
            c.Show();
        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e) //關聯sqs //yodrdt有2主索引鍵(因與表頭重複,按ctrl一起右鍵),不亂動洪設定
        {   //在明細檔新增order1=2,找無表頭是因表頭無order1=2(孤兒,刪不掉)->要設定避免!
            if (e.RowIndex >= 0)
            {
                if (comboBox1.Text == "YODR")
                {
                    textBox1.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
                    textBox3.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    //蔡主檔dept,明細檔emp,關聯dno
                    sqs = $"Select * From YODRDT Where YODRDT.order1 = '{textBox1.Text.Trim()}' ";//為啥原tB後有加號?
                }
                if (comboBox1.Text == "YFGIO")
                {
                    textBox1.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
                    textBox3.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    textBox4.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();//不之可否跟上面合併
                    //蔡主檔dept,明細檔emp,關聯dno
                    sqs = $"Select * From YFGIODT Where YFGIODT.vhno = '{textBox1.Text.Trim()}' ";//為啥原tB後有加號?
                }
                //為啥我照做關聯不到? 帆把雙引號跟加號改 { }即可關聯
                cn = new SqlConnection(cns);
                //開檔案免下cmd CRUD指令
                SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
                //SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
                DataSet ds = new DataSet();
                adapt.Fill(ds); //改用comBx,原(ds, "Employee"),tname加雙引號 //置入Dataset ds 
                dt = ds.Tables[0];
                //adapt.Update(dt);
                dataGridView2.DataSource = dt;//先讀不用bs //此為dgv2,用蔡作法(洪用relation?)
            }
        }
        FDetail f2; //FDetail f2 = new FDetail();
        public string op; //各命令字串: C,U,R,D //看可否不用
        public bool allow = false; //預設紅燈,!allow=綠燈
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) //明細檔crud
        {   //U&R前要先把文字傳至f2,C則免(因無文字) //ru前要先按本事件,太麻煩! 待改程式
            if (e.RowIndex >= 0)
            {   //把dGV資料傳到f2 tB, f2 tB皆改為public (開綠燈讓f1資料進來) //tB名未改
                if (comboBox1.Text == "YODR")
                {
                    f2 = new FDetail();//原被註解
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
                    f2 = new FDetail();//原被註解
                    f2.textBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    f2.textBox2.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    f2.textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    f2.textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    f2.textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                    f2.textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
                    f2.textBox7.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
                    f2.textBox8.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString(); //
                }
                allow = true; //?
            }
        }
        private void button10_Click(object sender, EventArgs e) //蔡不行改copy洪relation 2t裡就有crud
        {   //新增          
            op = "C";  //or I:Insert
            f2 = new FDetail();  //新開視窗         
            //clear_emp_control(); //清資料
            if (comboBox1.Text == "YODR") //tb大小寫竟有差!
            { f2.txt1.Text = textBox1.Text; } //先不用自動填 
            if (comboBox1.Text == "YFGIO") //大小寫竟有差!
            {
                f2.textBox1.Text = textBox1.Text;
                //f2.textBox7.Text = textBox1.Text; //order1連結問題
            }
            f2.Show(this);
        }
        private void button9_Click(object sender, EventArgs e)
        {   //查詢 (蔡需在dgv2按一下後,在按查詢即可跳出f2)
            if (!allow) return;//
            //f2 = new FDetail(); //新開一個空白的(行不通),等於大家都用新增的(?)
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
            //deleteEmp();//只刪一行
            if (comboBox1.Text == "YODR")
            {
                sqs1 = "Delete from Yodrdt where fgno=@fgno"; //蔡刪另1參數,因刪order1會全被刪掉 //原@order1打錯字,紅字提醒'須宣告純量變數@order1'

                cn = new SqlConnection(cns);//從開檔移至此方法
                cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
                cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
                cmd.Connection = cn;
                cmd.CommandText = sqs1;
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = f2.txt2.Text.Trim();
            }
            if (comboBox1.Text == "YFGIO")
            {
                sqs1 = "Delete from YFGIOdt where FGNO=@FGNO";//**明細檔不同vhno,但同fgno會被刪掉! //原刪完看商品tb數量還在因還未寫庫存ru'd' 

                cn = new SqlConnection(cns);//從開檔移至此方法
                cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
                cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
                cmd.Connection = cn;
                cmd.CommandText = sqs1;
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); //相同,想合併(之後研究)
                cmd.Parameters["@FGNO"].Value = f2.textBox2.Text.Trim();
            }
            cmd.ExecuteNonQuery(); //原Customer未設cono為主索引鍵,不會擋重複cono
            cn.Close();//沒有關會無法連續新增!
            MessageBox.Show($"刪除成功。");

            //庫存刪除 (若一次刪多筆要迴圈; (未寫)刪主檔,明細檔也要刪)
            sqs1 = "Update YFGMAST Set IQTY= iqty- @IQTY Where fgno=@fgno"; //相減
            cn = new SqlConnection(cns);//從開檔移至此方法
            cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar));
            cmd.Parameters["@IQTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); //相同,想合併(之後研究)
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
            cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.NVarChar));
            cmd.Parameters["@IQTY"].Value = int.Parse(f2.textBox4.Text.Trim() == "" ? "0" : f2.textBox4.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar)); //相同,想合併(之後研究)
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
            
            OpenTable();
        }
    }
}
