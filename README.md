# FiddlerToPostman
Export captured Fiddler sessions as a Postman collection

# Steps to add Postman-Collection export format to fiddler:

1) Copy the FiddlerToPostman.dll from the %yourRoot%\FiddlerToPostman\FiddlerToPostman\bin\Release directory.

2) Navigate to where Fiddler is located in Program Files.
  i) C:\Program Files (x86)\Fiddler2
  
3) Paste FiddlerToPostman.dll into the ImportExport folder inside the previous directory.

4) If Fiddler is currently open, close the application.

5) Start Fiddler and the Postman-Collection export format will be under File -> Export Sessions -> All Sessions / Selected Sessions.

6) Postman-Collection will be an export format option in the dropdown menu.
