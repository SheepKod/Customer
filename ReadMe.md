პროექტის მარტივად დასტარტვის ინსტრუქციები:

1. ბაზასთან მარტივად დასაკავშირებლად, ქონექშენ სტრინგის ცვლილება რომ არ მოგიწიოთ,
   თან ვურთავ დოკერის სკრიპტს:  
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongP@ssw0rd1" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
2. AWS Key-ები გაწერილია launchSetting-ებში env variable-ებად, არ დამჰაკოთ გთხოვთ :')

    
