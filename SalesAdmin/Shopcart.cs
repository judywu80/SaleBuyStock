using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace SaleBuyStock
{
    public partial class Shopcart : Form
    {
        public Shopcart()
        {
            InitializeComponent();
        }
        public static string LbTable;//
        DgvSet dataGridView0; //注意命名 //原少r;重打原綠字(dgv少1)不見
        public string cns, sqs, tbname, sqs1;
        SqlConnection cn; SqlCommand cmd;//不能直接實作參數
        DataTable dt; //實體化在btn裡才不會資料一直往下重複
        public void OpenTable() //變更簽章(?)
        {   //cn = new SqlConnection(cns)在form_load
            sqs = "SELECT * FROM " + tbname;
            SqlDataAdapter adapt = new SqlDataAdapter(sqs, cn);
            //SqlCommandBuilder scb = new SqlCommandBuilder(adapt); //有採用DataSet，必須有adapt，又採用adapt.Update()
            DataSet ds = new DataSet();
            adapt.Fill(ds); //改用comBx,原(ds,"Employee"),tname加雙引號 //置入Dataset ds 
            dt = ds.Tables[0];
            //adapt.Update(dt);*/
            dataGridView1.DataSource = dt;//先讀不用bs //洪的OpenTable()似無此行
        }
        public void ExecuteQue()
        {
            cn.Open(); //常見錯誤訊息:'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
            cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
            cmd.Connection = cn;
            cmd.CommandText = sqs1;
            cmd.Parameters.Add(new SqlParameter("@fgno", SqlDbType.NVarChar));//
            cmd.Parameters["@fgno"].Value = textBox3.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@fgname", SqlDbType.NVarChar));
            cmd.Parameters["@fgname"].Value = textBox1.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@cono", SqlDbType.NVarChar));//
            cmd.Parameters["@cono"].Value = textBox4.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@prc", SqlDbType.NVarChar));
            cmd.Parameters["@prc"].Value = int.Parse(textBox2.Text.Trim() == "" ? "0" : textBox2.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@zwet", SqlDbType.NVarChar));//
            cmd.Parameters["@zwet"].Value = int.Parse(textBox5.Text.Trim() == "" ? "0" : textBox5.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@note", SqlDbType.NVarChar));//
            cmd.Parameters["@note"].Value = textBox6.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@iqty", SqlDbType.NVarChar));//
            cmd.Parameters["@iqty"].Value = int.Parse(textBox7.Text.Trim() == "" ? "0" : textBox7.Text.Trim());
            cmd.Parameters.Add(new SqlParameter("@cdate", SqlDbType.NVarChar)); //暫把型別date改NVarChar
            cmd.Parameters["@cdate"].Value = textBox8.Text.Trim();
            //cmd.Parameters["@cdate"].Value = Convert.ToDateTime(textBox8.Text.Trim() == "" ? "DateTime.Today.ToString()" : textBox8.Text.Trim());//這樣寫不行"DateTime.Today.ToString();"
            cmd.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar));//
            cmd.Parameters["@picture"].Value = textBox9.Text.Trim();
            cmd.Parameters.Add(new SqlParameter("@yn", SqlDbType.NVarChar));//
            cmd.Parameters["@yn"].Value = textBox10.Text.Trim();
            //cmd.Parameters.Add(new SqlParameter("@iqty", SqlDbType.NVarChar)); //Pay Value這樣寫crud容易有出問題
            //cmd.Parameters["@iqty"].Value = int.Parse(comboBox1.Text.Trim() == "" ? "0" : comboBox1.Text.Trim());//看反藍,非勾選的! //括號內原數字寫法改選取by fan
            cmd.ExecuteNonQuery(); //原Customer未設cono為主索引鍵,不會擋重複cono
            cn.Close();//沒有關會無法連續新增!
        }
        int n, a, amt,tamt, qty,tqty; //應新增2table所有tb(&label)欄位以便修改,老師範例都有
        int lst_removeat, er = 0; //ec = 0;
        class Meals //資料庫用原來的(class&List)即可,因同之前在tB(s)寫入 //泛型和類別用於寫入購物車CRUD
        {   
            public string 商品 { get; set;}// //原m=meal,此類別名
            public int 價格 { get; set; }
            public string 數量 { get; set; }
        }   //作業4沒用到泛型,只使用陣列(用找字串方式列印出來)
        List<string> lst0 = new List<string>() { "7-11", "熊貓", "訂餐" };//如何將ary ait轉進list lst0? 試將ait移到靜態區也無法,or後段需在區域變數里?(前斷留靜態需共用)
        List<Meals>lst=new List<Meals>(); //另新增listBox的泛型(不同個)
        private void Form1_Load(object sender, EventArgs e)  //listBox資料用於商品CRUD
        {   //蔡OpenT放這不用按開檔,待研究是否可行
            string[] ait = new string[] {"7-11","熊貓","訂餐"}; //a=ary,為寫入lstBx
            listBox1.Items.Clear();
            listBox1.Items.AddRange(ait);
            //int[] aprc = new int[] { 80, 90, 60};
            string[] aqty=new string[] {"1","2","3"}; //為寫入comBx,如何加入dgv?
            comboBox1.Items.AddRange(aqty);   //只能加字串嗎?

            cns = FDouble.cns; //form1的string cns前加public static,右式Form1.cns的cns即消除
            cn = new SqlConnection(cns); //*從form1 OpenT至此
            LbTbName.Text = LbTable;//為啥消失? //原這段沒貼到,adapt.Fill(ds)有問題

            dataGridView0 = new DgvSet(); //類別要先實作化 (老師放form_load),or null
            dataGridView0.dgvSet(dataGridView1); //原發現有多個dgv1
            dataGridView0.dgvSet(dataGridView5);
        }
        private void button1_Click(object sender, EventArgs e) //(x?)listBox一定要用泛型lst0
        {
            listBox1.DataSource = null;
            lst0.Add(textBox1.Text.Trim()); //金額未設
            listBox1.DataSource = lst0;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox1.Text = "";
            if (listBox1.SelectedIndex >= 0)   //若有找到的話
            {
                textBox1.Text = listBox1.SelectedItem.ToString();  //為更新和刪除須先查詢該物
                comboBox1.SelectedIndex = 0;  //預設=1(位置0)
                if (listBox1.SelectedItem.ToString() == "7-11")   //單數item!(打到複數items沒反應)
                    textBox2.Text = "80";
                else if (listBox1.SelectedItem.ToString() == "熊貓")
                    textBox2.Text = "90";
                else if (listBox1.SelectedItem.ToString() == "訂餐")
                    textBox2.Text = "60";
                else textBox2.Text = "0";

                string filename = listBox1.SelectedItem.ToString();
                pictureBox1.Image = Image.FromFile("jpg\\" + filename + ".png");
            }
        }
        private void button2_Click(object sender, EventArgs e) //update 改完會變第1個
        {
            n=listBox1.SelectedIndex;
            //listBox1.Items[listBox1.SelectedIndex] = textBox1.Text; //'已設定 DataSource 屬性時，無法修改項目集合。'
            lst0[n] = textBox1.Text.Trim();
            listBox1.DataSource = null;
            listBox1.DataSource = lst0;
        }
        private void button3_Click(object sender, EventArgs e) 
        {
            if (listBox1.SelectedIndex >= 0)
            {
                lst0.RemoveAt(listBox1.SelectedIndex);
                //listBox1.Items.RemoveAt(listBox1.SelectedIndex);  //已設定DataSource屬性時，無法修改項目集合。(?)
                listBox1.DataSource = null; //有加以下2句,tB資料才會不見
                listBox1.DataSource = lst0; //lst有問題,因不同List<>lst0
            }
            //else { MessageBox.Show("Not found"); }
            textBox1.Text = ""; textBox2.Text = "";
        }
        private void button6_Click(object sender, EventArgs e) //列印ToString, listBoc to rTB, 蔡講義有
        {   //印全部不含總金額等
            richTextBox1.Text = "";  
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
            {   //richTextBox1.Text += listBox1.SelectedIndices[i].ToString();  //序號從1開始看洪進階做法(設j, for迴圈, j++)
                richTextBox1.Text += listBox1.SelectedItems[i].ToString()+" "+textBox2.Text+"*"+ comboBox1.SelectedItem+"\n";
            }
        }
        private void button13_Click(object sender, EventArgs e) //結帳(作業4)lst全部內容
        {
            string st = "     餐飲清單 \n", st1="", st0="",st2="";
            tqty = 0;tamt = 0; //否則會累計
            foreach(Meals p in lst)  //此為Calss p(之前是int p),搞懂代替Meal obj,有加進的dgv的lst才有
            {   //直接從m泛型抓,快又方便!
                qty = Convert.ToInt32(p.數量);
                tqty += qty;
                amt = p.價格 * Convert.ToInt32(p.數量);
                tamt += amt;
                st += $"{p.商品 } {p.價格} * {p.數量} = {amt} \n";
            } //改成作業4比較漂亮
            st2 = "------------------\n";
            st0=$"總數量 = {tqty} 份 \n"; //2個$符號不好加
            st1 = $"總金額 = {tamt} 元\n ** 謝謝光臨！**";
            richTextBox2.Text = st+st2+st0+st1;
        }
        private void button4_Click(object sender, EventArgs e) //query 查詢只打1字去查,非找到金額
        {   //待設定選擇反藍: listBox p11全選修改成特定 //n = lst0.IndexOf(textBox1.Text.Trim()); //n = lst.FindIndex(r => r.商品.Equals(7-11));listBox裡的文字怎麼表示
            if (textBox1.Text.Contains("7") || textBox1.Text.Contains("-") || textBox1.Text.Contains("1"))
            { textBox2.Text = "80"; textBox1.Text = "7-11"; } //各加中括號,or容易錯誤
            if (textBox1.Text.Contains("熊") || textBox1.Text.Contains("貓")) 
            { textBox2.Text = "90"; textBox1.Text = "熊貓"; }
            if (textBox1.Text.Contains("訂") || textBox1.Text.Contains("餐"))
            { textBox2.Text = "60"; textBox1.Text = "訂餐"; }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; textBox2.Text = "";//comboBox1.Text = "";
            richTextBox1.Text = "";
            //加以下會全部不見, 改回到form_load狀態就好 (洪SingleTable有refresh功能)
            /*listBox1.DataSource = lst; //MessageBox.Show("STOP");
            listBox1.DataSource = null;   //reset listBox1.DataSource before list changed
            lst.Clear();    //或lst=null;         
            listBox1.DataSource = lst;*/
        }
        private void button12_Click(object sender, EventArgs e)
        {   //如何抓同商品,更新數量就好,避免商品重複? 滄的範例有作法

            /*dataGridView1.DataSource = null; //下行有無更快預設寫入方法?(洪i)
            lst.Add(new Meals() { 商品 = listBox1.SelectedItem.ToString(), 價格 = int.Parse(textBox2.Text), 數量 = comboBox1.SelectedItem.ToString() }); //textBox3.Text = comboBox1.SelectedItem.ToString();不須tB3,可直接打在本式;同tB1
            dataGridView1.DataSource = lst;*/  //泛型lst顯現在dgv上用"ds"(?) //MessageBox.Show("ADD OK");
            
            //sqs1 = "Insert Into Product (fgname,prc,iqty) Values (@fgname,@prc,@iqty)";//"@"變數**非t-sql寫法
            sqs1 = "Insert Into " + tbname + "(";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqs1 += dt.Columns[i].ColumnName + ",";
            }
            sqs1 = sqs1.Remove(sqs1.Length - 1, 1);  //最後逗點去掉
            sqs1 += ") Values (";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqs1 += "@" + dt.Columns[i].ColumnName + ",";
            }
            sqs1 = sqs1.Remove(sqs1.Length - 1, 1) + ")";  //最後逗點去掉 + 加上最後括號


            ExecuteQue();
            OpenTable();
        }
        private void button5_Click(object sender, EventArgs e)
        {   //已設定按2次為反向
            listBox1.DataSource = null;
            a++;
            if (a % 2 == 1) { lst0.Sort(); }
            else { lst0.Reverse(); }
            listBox1.DataSource = lst0;
        }
        private void button11_Click(object sender, EventArgs e) //在上面改
        {   //n=
            /*dataGridView1.DataSource = null;    //Reset dataGridView1
            dataGridView1.DataSource = lst;*///這段似會干擾更正後的開檔 //(?)lst[n].商品 = textBox1.Text.Trim();//lst[n].價格 = int.Parse(textBox2.Text.Trim());//lst[n].數量 = comboBox1.Text;
            
            //sqs1 = "Update Product Set prc=@prc,iqty=@iqty Where fgname=@fgname";//"@"變數t-sql寫法?
            sqs1 = "Update " + tbname + " Set ";
            //string tep = dt.Columns[0].ColumnName;
            for (int i = 1; i < dt.Columns.Count; i++) //i=1(非0) //原for (int i = 0; i < dt.Columns[i].ColumnName.Count/Length; i++)
            {
                sqs1 += dt.Columns[i].ColumnName + "=@" + dt.Columns[i].ColumnName + ",";
            }   //一開始@前等號"="沒寫進去,找不到資料行4
            sqs1 = sqs1.Remove(sqs1.Length - 1, 1);  //最後逗點要去掉==除去尾端","
            sqs1 += " where " + dt.Columns[0].ColumnName + "=@" + dt.Columns[0].ColumnName;
            ExecuteQue();
            OpenTable();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        {  //按一下儲存格內"的內容"時發生。//和CellClick差別: 發生於按一下儲存格的"任何部分"時。
        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0, linesPerPage = 0;
            // Sets the value of charactersOnPage to the number of characters of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, this.Font,e.MarginBounds.Size, StringFormat.GenericTypographic,out charactersOnPage, out linesPerPage);
            // Draws the string within the bounds of the page.
            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black, e.MarginBounds, StringFormat.GenericTypographic);
            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);
            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
            // If there are no more pages, reset the string to be printed.
            if (!e.HasMorePages) { stringToPrint = documentContents; }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void button17_Click(object sender, EventArgs e)
        {   //數量為庫存數量,待做庫存表
            dataGridView5.DataSource = null;//原dgv1看不到東西因沒改到+新增只能1樣不能累計lol(還重開檔XD) //下行更快預設寫入方法(洪i)
            lst.Add(new Meals() { 商品 = textBox1.Text, 價格 = int.Parse(textBox2.Text), 數量 = comboBox1.Text });//用原來的lst和class即可(?)
            dataGridView5.DataSource = lst;
        }
        private void button18_Click(object sender, EventArgs e)
        {   //如何設定"Not found!"?
            List<Meals> parlst = lst.FindAll(r => r.商品.Contains(textBox1.Text));
            foreach (var p in parlst)
            {
                MessageBox.Show(p.商品.ToString()); //if (!Contains.p.商品.ToString()) { MessageBox.Show("Not found"); }
            }
        }
        private void button19_Click(object sender, EventArgs e)
        {

        }
        private void button20_Click(object sender, EventArgs e)
        {
            lst.RemoveAt(lst_removeat); //搭配CellClick事件
            dataGridView5.DataSource = null;    //Reset dataGridView1
            dataGridView5.DataSource = lst;
        }
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            er = e.RowIndex; //ec = e.ColumnIndex;
            lst_removeat = e.RowIndex; //點選移除位置
        }
        private void button23_Click(object sender, EventArgs e)
        {
            dataGridView5.DataSource = null;    //Reset
            a++;
            if (a % 2 == 1) { lst.Sort((x, y) => x.商品.CompareTo(y.商品)); } //只排名字
            else { lst.Sort((x, y) => -x.商品.CompareTo(y.商品)); }
            dataGridView5.DataSource = lst;
        }
        private void button21_Click(object sender, EventArgs e)
        {
            string st = "           餐飲清單 \n\n", st1 = "", st0 = "", st2 = "";
            tqty = 0; tamt = 0; //否則會累計
            foreach (Meals p in lst)  //此為Calss p(之前是int p),搞懂代替Meal obj,有加進的dgv的lst才有
            {   //直接從m泛型抓,快又方便!
                qty = Convert.ToInt32(p.數量);
                tqty += qty;
                amt = p.價格 * Convert.ToInt32(p.數量);
                tamt += amt;
                st += $"{p.商品} {p.價格} * {p.數量} = {amt} \n";
            } //改成作業4比較漂亮
            st2 = "------------------------------------\n";
            st0 = $"總數量 = {tqty} 份 \n"; //2個$符號不好加
            st1 = $"總金額 = {tamt} 元\n\n       ** 謝謝光臨！**";
            richTextBox3.Text = st + st2 + st0 + st1;
        }
        private void button22_Click(object sender, EventArgs e)
        {
            printDocument1.DocumentName = richTextBox3.Text;
            documentContents = richTextBox3.Text;
            stringToPrint = richTextBox3.Text;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            tbname = LbTable; //原雙檔處未寫form.LbTable= Product; sqs = "SELECT * FROM " + "Product";
            OpenTable(); //建類別OpenTable()供各form用(洪沒建,似double不同法開檔)
        }
        private void listBox1_DoubleClick(object sender, EventArgs e) //點2下可新增至dgv,但數量只有1
        {
            dataGridView1.DataSource = null; //下行有無更快預設寫入方法?(洪i)
            lst.Add(new Meals() { 商品 = listBox1.SelectedItem.ToString(), 價格 = int.Parse(textBox2.Text), 數量 = comboBox1.SelectedItem.ToString() }); //textBox3.Text = comboBox1.SelectedItem.ToString();不須tB3,可直接打在本式;同tB1
            dataGridView1.DataSource = lst;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            er = e.RowIndex; //ec = e.ColumnIndex;
            lst_removeat = e.RowIndex; //點選移除位置

            richTextBox2.Text = "";
            richTextBox2.Font = new Font("Arial", 12, FontStyle.Regular);
            richTextBox2.Text = "商品名稱: " + dataGridView1.Rows[er].Cells[0].Value.ToString() + "\n";
            richTextBox2.Text += "商品價格: " + dataGridView1.Rows[er].Cells[1].Value.ToString() + "\n";
            richTextBox2.Text += "商品數量: " + dataGridView1.Rows[er].Cells[2].Value.ToString() + "\n";

            if (e.RowIndex >= 0)
            {
                //textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(); //
                comboBox1.Text = "1";//comboBox1.Text.Trim();//dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }

            //string filename = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            string filename = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            pictureBox1.Image = Image.FromFile("ItPicture\\" + filename);
        }
        private void button10_Click(object sender, EventArgs e) //已改FullRowSelect
        {   //n = dataGridView1.SelectedRows; if (dataGridView1.SelectedRows >= 0) //再研究MS寫法,不用tB查詢去刪 lst.RemoveAt(dataGridView1.SelectedIndex);  //只寫n會有問題,只刪到第一個
            /*lst.RemoveAt(lst_removeat); //搭配CellClick事件
            dataGridView1.DataSource = null;    //Reset dataGridView1
            dataGridView1.DataSource = lst;*/ //這段沒註解會干擾

            //sqs1 = "Delete From Product Where fgNAME=@fgNAME";//原寫成FGNO
            sqs1 = "Delete From " + tbname + " Where " + dt.Columns[0].ColumnName + "=@" + dt.Columns[0].ColumnName + "";
            ExecuteQue();
            OpenTable();
        }
        private void button9_Click(object sender, EventArgs e) //dgv查詢跟liB查詢查複?
        {   //string s=""; //foreach(var p in lst/dataGridView1.Rows) //{ s += p.商品;} //MessageBox.Show(s/p.ToString());//n = lst.FindIndex(r => r.商品.Equals(textBox1.Text));
            List<Meals> parlst = lst.FindAll(r => r.商品.Contains(textBox1.Text));
            foreach (var p in parlst)
            {
                MessageBox.Show(p.商品.ToString()); //if (!Contains.p.商品.ToString()) { MessageBox.Show("Not found"); }
            }
            //分隔線
            try
            {   //原sql語法有問題,用debug+F10顯示sqs內容值去找 //無效資料行: 沒改到tabe名
                sqs = "Select * From Product Where "; //下sql條件,注意where後要空1格,否則與下行字串相連,吃不到指令
                comboBox1.Text = "";//查詢時不下任何字串條件,含"縣市". //cono只打1,其他所有條件皆出現(cLB未好)
                if (textBox1.Text.Trim() != "") { sqs += "fgname like '%" + textBox1.Text.Trim() + "%' "; }//Sql原寫法: like '%a%'
                else { sqs += "1=1"; }//該欄位不打資料就可查詢 //再確認查詢範圍
                if (textBox2.Text.Trim() != "") { sqs += "AND prc like '%" + textBox2.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }//該欄位不打資料就可查詢
                if (comboBox1.Text.Trim() != "") { sqs += "AND iqty like '%" + comboBox1.Text.Trim() + "%' "; }//Sql多條件寫法加: And
                else { sqs += "AND 1=1"; }//選單如何改+該欄位不打資料就可查詢
                MessageBox.Show(sqs);

                cn.Open(); //'ExecuteNonQuery 必須有開啟與可用的 Connection。連接目前的狀態已關閉。'
                cmd = new SqlCommand(); //先不指定sqs內容,因可能有多個
                cmd.Connection = cn;
                cmd.CommandText = sqs; //先宣告sqs,再傳給cmd.cmdText (or要按2次才能查)
                SqlDataReader dr = cmd.ExecuteReader(); //"查詢"用Reader就好,不用ExecuteNonQery()
                dt = new DataTable(); //實體化在這才不會資料一直往下重複
                dt.Load(dr);    //載入SqlDataReader的資料  //原dr(from db)有資料,讀db上dt的資料 //dr.Close(); cmd.Dispose();似無變化
                cn.Close(); //*放這裡無誤
                dataGridView1.DataSource = dt; //為啥Table(應是View?)會自動排(假的嗎),數字&日期也排對? **Ans：DataTable也可以有自動排序，數字&日期也都會排對，如果使用DataView還可以有多欄位排序，及filte功能
            }
            catch (Exception ex) //try後沒打catch會紅字提醒 //finally不打可
            { cn.Close(); MessageBox.Show("資料發生錯誤:" + ex.Message);}
        }
        private void button8_Click(object sender, EventArgs e) //list無預設排序
        {   //成功增加按第二下為反序
            dataGridView1.DataSource = null;    //Reset dataGridView1
            a++;
            if (a % 2 == 1) { lst.Sort((x, y) => x.商品.CompareTo(y.商品)); } //只排名字
            else { lst.Sort((x, y) => -x.商品.CompareTo(y.商品));}
            dataGridView1.DataSource = lst;
        }
        string documentContents;// Declare a string to hold the entire document contents.
        string stringToPrint;// Declare a variable to hold the portion of the document that is not printed.
        private void button7_Click(object sender, EventArgs e) //怎麼印dgv單筆or lst到rTB?
        {   //只印選的
            /*richTextBox2.Text = "";
            //Meals obj = lst[dataGridView1.GetCellCount];
            //for (int i = 0; i < dataGridView1.SelectedIndices.Count; i++)
            //{ richTextBox2.Text += dataGridView1.SelectedIndices[i].ToString();}  //序號從1開始看洪進階做法(設j, for迴圈, j++)
            string st = ""; //st1 = "", st0 = "";
            foreach (Meals p in lst)  //此為Calss p(之前是int p),搞懂代替Meal obj,有加進的dgv的lst才有
            {   //直接從m泛型抓,快又方便!
                qty = Convert.ToInt32(p.數量);
                amt = p.價格 * Convert.ToInt32(p.數量);
                st += $"{p.商品} {p.價格} * {p.數量} = {amt} \n";   }
            richTextBox2.Text = st ;*/
            printDocument1.DocumentName = richTextBox2.Text;
            documentContents = richTextBox2.Text;
            stringToPrint = richTextBox2.Text;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();  //在printdoc元件上點2下
        }
        private void button15_Click(object sender, EventArgs e) //取消同lisB
        {   //怎麼清空dgv就好?
            //listBox1.DataSource = lst;  //MessageBox.Show("STOP");
            dataGridView1.DataSource = null;    //reset listBox1.DataSource before list changed
            //lst.Clear();    //或lst=null;   //dataGridView1.DataSource = lst;
            richTextBox2.Text = "";
        }
    }
}
