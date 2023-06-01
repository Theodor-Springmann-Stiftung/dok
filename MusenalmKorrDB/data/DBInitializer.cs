namespace MusenalmKorrDB.Data;
using MusenalmKorrDB.Models;

// Seeding the Database
public static class DbInitializer
{
    public static void Initialize(MusenalmContext context)
    {
        if (context.Akteure.Any()) {
            return;   // DB has been seeded
        }

        
    }
}