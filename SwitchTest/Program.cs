// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SwitchTest;
public class Program
{

    public static void Main(string[] args)
    {

        var tz = TimeZone.CurrentTimeZone.StandardName;
        Console.WriteLine(tz);
        Console.WriteLine("Hello, World!");

        var ob = new DbContextOptionsBuilder();
        ob.UseNpgsql("Host=localhost;Port=5432;Database=dbtest;Username=postgres;Password=db_dev");
        using var db = new MyDbContext(ob.Options);
        //db.Books.FirstOrDefault();
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var today = DateTime.Today;
        QueryTest(db, today);
        TestInsert(db, today);
        TestInsert(db, today.ToUniversalTime());
        Console.ReadKey();
    }

    private static void Clear(MyDbContext db)
    {
        #region Clear Data
        var list = db.Books.ToList();
        db.RemoveRange(list);
        db.SaveChanges();
        #endregion
    }

    static void TestInsert(MyDbContext db, DateTime today)
    {
        Console.WriteLine("-------started---------------");
        Clear(db);
        Console.WriteLine($"today : {today:O}");
        var book = new Book() { Name = "Test book 1", CreatedDate = DateTime.Now, ReleaseDate = today };
        db.Add(book);
        db.SaveChanges();
        var local = today.ToLocalTime();
        var q1 = db.Books.Where(x => x.ReleaseDate.Date == local);
        var b1 = q1.FirstOrDefault();
        Console.WriteLine($"b1[local] is null :{b1 == null}");

        var utc = today.ToUniversalTime();
        var q2 = db.Books.Where(x => x.ReleaseDate == utc);
        var b2 = q2.FirstOrDefault();
        Console.WriteLine($"b2[utc] is null :{b2 == null}");

        var q3 = db.Books.Where(x => x.CreatedDate.Date == local);
        var b3 = q3.FirstOrDefault();
        Console.WriteLine($"b3[local,trunc] is null :{b3 == null}");

        var q4 = db.Books.Where(x => x.CreatedDate.Date == utc);
        var b4 = q4.FirstOrDefault();
        Console.WriteLine($"b4[utc,trunc] is null :{b4 == null}");
        Console.WriteLine("-------finshed---------------");
    }
    static void QueryTest(MyDbContext db, DateTime today)
    {
        Console.WriteLine("-------started---------------");
      
        Console.WriteLine($"today : {today:O}");
       
        //var local = today.ToLocalTime();
        //var q1 = db.Books.Where(x => x.ReleaseDate.Date == local);
        //var b1 = q1.FirstOrDefault();
        //Console.WriteLine($"b1[local] is null :{b1 == null}");

        var utc = today.ToUniversalTime();
        var q2 = db.Books.Where(x => x.ReleaseDate == utc);
        var b2 = q2.FirstOrDefault();
        Console.WriteLine($"b2[utc] is null :{b2 == null}");

        //var q3 = db.Books.Where(x => x.CreatedDate.Date == local);
        //var b3 = q3.FirstOrDefault();
        //Console.WriteLine($"b3[local,trunc] is null :{b3 == null}");

        var q4 = db.Books.Where(x => x.CreatedDate.Date == utc);
        var b4 = q4.FirstOrDefault();
        Console.WriteLine($"b4[utc,trunc] is null :{b4 == null}");
        Console.WriteLine("-------finshed---------------");
    }
}
