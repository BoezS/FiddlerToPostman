# FiddlerToPostman
Export captured Fiddler sessions as a Postman collection

# Steps to add Postman-Collection export format to fiddler:

1) Copy the FiddlerToPostman.dll from the %yourRoot%\FiddlerToPostman\FiddlerToPostman\bin\Release directory.

2) Navigate to where Fiddler is located in Program Files.
  i) C:\Program Files (x86)\Fiddler2
  
3) Paste FiddlerToPostman.dll into the ImportExport folder inside the previous directory.

4) Copy the Newtonsoft.Json.dll from the %yourRoot%\FiddlerToPostman\FiddlerToPostman\bin\Release directory.

5) Navigate to where fiddler is located in Program Files.
  i) C:\Program Files (x86)\Fiddler2
  
6) Paste Newtonsoft.Json.dll inside the previous directory.

7) If Fiddler is currently open, close the application.

8) Start Fiddler and the Postman-Collection export format will be under File -> Export Sessions -> All Sessions / Selected Sessions.

9) Postman-Collection will be an export format option in the dropdown menu.
