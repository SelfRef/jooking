dotnet aspnet-codegenerator controller -outDir Controllers -dc HotelsContext -async -api -f -name HotelsController -m Hotel
dotnet aspnet-codegenerator controller -outDir Controllers -dc HotelsContext -async -api -f -name RoomsController -m Room
dotnet aspnet-codegenerator controller -outDir Controllers -dc HotelsContext -async -api -f -name ReservationsController -m Reservation
dotnet aspnet-codegenerator controller -outDir Controllers -dc HotelsContext -async -api -f -name UsersController -m User