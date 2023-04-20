using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SaleBuyStock
{
    public partial class FDetail : Form
    {
        public FDetail()
        {
            InitializeComponent();
        }
        public static string cns;
        //string sqs, sqs1; //要重新宣告?
        //SqlConnection cn; SqlCommand cmd;//不能直接實作參數
        //DataTable dt;
        private void button1_Click(object sender, EventArgs e)
        {
            FDouble f1 = (FDouble)Owner; //讓f1資料在f2也可使用(?)
            try
            {
                if (f1.op == "U")
                {   //f1.updateEmp();
                    f1.sqs1 = "Update YODRDT Set FGNO=@FGNO,OD=@OD,QTY=@QTY,PRC=@PRC,SQTY=@SQTY,NOTE1=@NOTE1 Where ORDER1=@ORDER1";//原資料表名寫錯,確認沒反應

                    f1.ExecuteQue2();//前記得加f1
                    MessageBox.Show($"更新成功。");
                    f1.ShowDetail();

                    Close();
                }
                if (f1.op == "C")  //Create的接續程式碼
                {   //f1.clear_emp_control();
                    //f1.addEmp(); //蔡有自動輸欄未碼
                    f1.sqs1 = "Insert Into YODRDT (ORDER1,FGNO,OD,QTY,PRC,SQTY,NOTE1) Values (@ORDER1,@FGNO,@OD,@QTY,@PRC,@SQTY,@NOTE1)";//原資料表名寫錯,確認沒反應
                    
                    f1.ExecuteQue2();//前記得加f1
                    MessageBox.Show($"新增成功。");
                    f1.ShowDetail();

                    Close();
                }
                if (f1.op == "R")  //只是查閱資料
                {
                    Close();
                }
            }
            catch {}
            finally
            { f1.allow = false; }
        }
        private void FDetail_Load(object sender, EventArgs e)
        {

        }
        //SqlConnection cn;
        private void button2_Click(object sender, EventArgs e)
        {
            FDouble f1 = (FDouble)Owner; //讓f1資料在f2也可使用(?)
            try
            {
                if (f1.op == "U") //確定鈕共用CRU
                {   //f1.updateEmp();
                    f1.sqs1 = "Update YFGIODT Set FGNO=@FGNO,OD=@OD,QTY=@QTY,PRC=@PRC,BATCH=@BATCH,ORDER1=@ORDER1,NOTE1=@NOTE1 Where vhno=@vhno";//原資料表名寫錯,確認沒反應
                    
                    f1.ExecuteQue2();//前記得加f1
                    MessageBox.Show($"更新成功。");
                    f1.ShowDetail();

                    //U(只有更新時)庫存異動要用泛型; 若用兩段(刪除+新增),中間不改,需加回來.
                    /*f1.DelChild();
                    f1.ShowDetail();*/

                    f1.sqs1 = "Update YFGMAST Set IQTY= iqty+ @IQTY Where fgno=@fgno";//
                    f1.ExecuteQue3();

                    MessageBox.Show($"庫存更新成功。");
                    f1.ShowDetail();

                    Close();
                }
                if (f1.op == "C")  //Create的接續程式碼
                {   //f1.clear_emp_control();
                    //f1.addEmp(); //蔡有自動輸欄未碼
                    f1.sqs1 = "Insert Into YFGIODT (vhno,FGNO,OD,QTY,PRC,BATCH,ORDER1,NOTE1) Values (@vhno,@FGNO,@OD,@QTY,@PRC,@BATCH,@ORDER1,@NOTE1)";//原Que2的@vhno寫成@ORDER1 ==必須宣告純量變數 "@vhno"。','變數名稱 '@ORDER1' 已經宣告。變數名稱在一個查詢批次或預存程序內必須是唯一的。
                    f1.ExecuteQue2();//前記得加f1
                    MessageBox.Show($"新增成功。");
                    f1.ShowDetail();

                    //更新YFGMAST庫存
                    //Shopcart s=new Shopcart();//不用new商品form //無法將FDouble轉為Shopcart(?)
                    //s.sqs1 = "Update YFGMAST Set IQTY=@IQTY Where fgno=@fgno"; //未加@QTY //@IQTY沒寫
                    //f1.sqs1 = "Update YFGMAST Set IQTY=@IQTY+@qty Where fgno=@fgno"; //加了qty變10(?)
                    f1.sqs1 = "Update YFGMAST Set IQTY= iqty+ @IQTY Where fgno=@fgno"; //i++原理(加上原本身) //原未加@QTY, @IQTY沒寫
                    //s.OpenTable(); //'Fill:SelectCommand.Connection 屬性尚未初始化。':SaleBuyStock.Double/Shopcart.OpenTable() 位於 Double/Shopcart.cs;SaleBuyStock.FDetail.button2_Click(object, System.EventArgs) 位於 FDetail.cs//cn並未將物件參考設定為物件的執行個體cn = new SqlConnection(cns);
                    //f1.OpenTable();
                    //s.ExecuteQue();
                    f1.ExecuteQue3(); //SaleBuyStock.ExecuteQue3() 位於 Double.cs; SaleBuyStock.FDetail.button2_Click 位於 FDetail.cs//'必須宣告純量變數 "@vhno"。'
                    MessageBox.Show($"庫存更新成功。");
                    f1.ShowDetail();

                    Close();
                }
                if (f1.op == "R")  //只是查閱資料
                {
                    Close();
                }
            }
            catch { }
            finally
            { f1.allow = false; }
        }
        private void button3_Click(object sender, EventArgs e)
        {   //為更新,先刪除用
            FDouble f1 = (FDouble)Owner; //讓f1資料在f2也可使用(?)
            try
            {
                if (f1.op == "U") //確定鈕共用CRU
                {   
                    //U(只有更新時)庫存異動要用泛型; 若用兩段(刪除+新增),中間不改,需加回來.
                    f1.DelChild();
                    f1.ShowDetail();
                }
                Close();
            }
            catch { }
            finally
            { f1.allow = false; }
        }
    }
}
