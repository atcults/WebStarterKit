using System;
using Common.OpenXml;

namespace UnitTests.Entity
{
    //[SheetHeader("")]

    public class TestExport
    {
        public void DoExport()
        {
            var sam1 = new StudentRoster
            {
                Address = "add",
                Id = Guid.NewGuid(),
                Limit = 3,
                Name = "Name",
                Role = "role"
            };

            var sam2 = new[]
            {
                new StudentRoster
                {
                    Id = Guid.NewGuid(),
                    Name = "Sunny",
                    Address = "Ahmedabad",
                    Role = "Role1",
                    Limit = 6
                }
            };

            var sam3 = new[]
            {
                new StudentRoster
                {
                    Id = Guid.NewGuid(),
                    Name = "Sunny",
                    Address = "Ahmedabad",
                    Role = "Role1",
                    Limit = 6
                },
                new StudentRoster
                {
                    Id = Guid.NewGuid(),
                    Name = "Mahendra",
                    Address = "Surat",
                    Role = "Role2",
                    Limit = 3
                },
                new StudentRoster
                {
                    Id = Guid.NewGuid(),
                    Name = "Alpesh",
                    Address = "Ahmedabad",
                    Role = "Role2",
                    Limit = 2
                },
                new StudentRoster
                {
                    Id = Guid.NewGuid(),
                    Name = "Jaimin",
                    Address = "Ahmedabad",
                    Role = "Role7",
                    Limit = 4
                }
            };

            var export1 = new DynamicExport("sam1.xlsx", sam1);
            var export2 = new DynamicExport("sam2.xlsx", sam2);
            var export3 = new DynamicExport("sam3.xlsx", sam3);

            export1.CreatePackage();
            export2.CreatePackage();
            export3.CreatePackage();
        }

        public void DoExport2()
        {
            var table = new SimpleTable("Test Table");
            table.AddColumn("Sr.No.", typeof (int), 500);
            table.AddColumn("Name", typeof (string), 120);
            table.AddRow(1, "Sunny");
            table.AddRow(2, "Gagan");

            var export = new SimpleTableExport(table);
            export.Generate("table.xlsx");
        }
    }
}