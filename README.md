# SaleBuyStock
**( If have issue with FastReport, please Add Reference "report.dll" at Solution Explorer. 若因FastReport錯誤訊息無法執行程式,請在方案總管加入參考"report.dll"即可執行 )**

![image](https://user-images.githubusercontent.com/122083665/235966324-afdfd648-13cd-45a8-8dc0-a299fd521877.png)

進銷存系統 特點
- 關聯表頭表身資料；兩forms間資料傳遞（用於表身之明細檔）；商品數量異動的庫存更新功能；可設計／列印資料的報表
- Relation 2 tables, data transfer in 2 forms, update stock quantity, report design & printing.
  
導覽 All Features
- Mmenu shows forms including shopcart, orders & purchase, stock, and client.
- can CRUD (create, read, update, delete) directly via input data in textboxes.
- click Checkout button to show data on richtextbox (via List <class>) in form Shopcart.

- **Please click parent-datagridview first to Relation child-datagridview.**
- as above, Click create button between 2 dgvs will open a new form to input data. (*berfore click RUD button, pls click the child-datagridview first)
- click Confirm button to write in database, which will alse update the stock qauntity. (*when update, need to click update btn before confirm)

資料處理 Data Report
- use FastReport to print datagridview and add additional data in A4 documents.
- use class ExportData to let sale-buy-stock data to save into Excel. 

優化功能 Advanced Features
- use Class for all datagridviews settings (color, font, resize, headerstyle, etc)
- Connection string setting uniform: no need to revise it in each form. (*open via github no need to attach database in SSMS)
- use Void for repeated codes, such as show table (ado.net), executeNonquery funtions.
- in Form FSingle.cs, using for loop instead of adding field one by one.

