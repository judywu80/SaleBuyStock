# SaleBuyStock
**if have issue running program with FastReport, pls Add Reference "report.dll" at Solution Explorer. 
  若因FastReport錯誤訊息無法執行程式,請在方案總管加入參考"report.dll"即可執行**

Features
- open via github no need to attach database in SSMS.
- Connection string setting uniform: no need to revise it in each form.
- Mmenu shows forms including shopcart, orders & purchase, stock, and client.
- can CRUD (create, read, update, delete) directly via input data in textboxes.

- pls click parent-datagridview first to Relation child-datagridview.
- as above, Click create button between 2 dgvs will open a new form to input data.
- * berfore click RUD button, pls click the child-datagridview first.
- click Confirm button to write in database, which will alse update the stock qauntity.

- use Class for all datagridviews settings (color, font, resize, headerstyle, etc)
- use Void for repeated codes, such as show table (ado.net), executeNonquery funtions.
- use FastReport to print datagridview and add additional data in A4 documents.
- click Checkout button to show data on richtextbox (via List <class>).
- in Form FSingle.cs, using for loop instead of adding field one by one.

