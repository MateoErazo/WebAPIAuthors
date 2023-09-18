# WebAPIAuthors

How can I execute this API and try It?

INSTRUCTIONS:

1. Download the repository and open the project with Visual Studio (In this case,
was used Microsoft Visual Studio Community 2022).

2. You need download SQL Server (In this case I used Microsoft SQL Server Management Studio 18)
because we need save all the information of our API.

3. In Visual Studio, in the solution explorer we are going to search the appsettings.json file
and we must edit the connection String with our information DB and save the changes.

4. In Visual Studio, you must goint to the path: Tools/NuGet Package Manager/Package Manager Console
and write the comand: Add-Migration AnythingName and then press Enter key and write the command: Update-Database 
and press Enter key again. This command will create all tables that the API need on DB.

5. Execute the API with Visual Studio.

