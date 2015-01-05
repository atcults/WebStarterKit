using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Common.OpenXml
{
    public class SimpleTable
    {
        public SimpleTable(string tableName)
        {
            Columns = new List<Column>();
            TableName = tableName;
        }

        public string TableName { get; private set; }

        public List<Column> Columns { get; private set; }
        public int TotalRows { get; set; }
        public class Column
        {
            public Column()
            {
                Values = new List<object>();
            }

            public string HeaderText { get; set; }
            public int Width { get; set; }
            public Type DataType { get; set; }
            public List<object> Values { get; private set; }
        }

        public void AddColumn(string columnName, Type dataType, int width = 100)
        {
            if (Columns.Count > 0 && Columns[0].Values.Count > 0)
            {
                throw new Exception("You can not add columns because one row is exist");
            }

            Columns.Add(new Column
            {
                HeaderText = columnName,
                DataType = dataType,
                Width = width
            });
        }

        public void AddRow(params object[] val)
        {
            if (Columns.Count == 0) throw new Exception("No columns are defined");

            if (Columns.Count != val.Count())
            {
                throw new Exception("Mismatch supplied values. Expecting:" + Columns.Count + ", Supplied:" + val.Count());
            }

            TotalRows++;

            for (var i = 0; i < val.Count(); i++)
            {
                Columns[i].Values.Add(val[i]);
            }
        }
    }

    public class SimpleTableExport
    {
        private readonly SimpleTable _tableDetail;
        
        public SimpleTableExport(SimpleTable tableDetail)
        {
            _tableDetail = tableDetail;
        }

        public void Generate(string filePath)
        {
            using (var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                var extendedFileProperties = document.AddNewPart<ExtendedFilePropertiesPart>("rId1");
                extendedFileProperties.Properties = new DocumentFormat.OpenXml.ExtendedProperties.Properties();
                
                GenerateExtendedFileProperties(extendedFileProperties.Properties);

                var workbookPart = document.AddWorkbookPart();
                GenerateWorkbookPartContent(workbookPart);

                var sharedStringTablePart = workbookPart.AddNewPart<SharedStringTablePart>("rId2");
                GenerateSharedStringTable(sharedStringTablePart);

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId1");
                GenerateWorksheetPartContent(worksheetPart);

                document.PackageProperties.Creator = "Sanelib";
                document.PackageProperties.Created = DateTime.Now;
            }
        }

        private static void GenerateExtendedFileProperties(DocumentFormat.OpenXml.ExtendedProperties.Properties properties)
        {
            properties.AddNamespaceDeclaration("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");

            properties.Append(new DocumentFormat.OpenXml.ExtendedProperties.Application { Text = "Microsoft Excel" },
                new DocumentFormat.OpenXml.ExtendedProperties.DocumentSecurity { Text = "0" },
                new DocumentFormat.OpenXml.ExtendedProperties.ScaleCrop { Text = "false" });

            var headingPairs = new DocumentFormat.OpenXml.ExtendedProperties.HeadingPairs();

            var headingVector = new DocumentFormat.OpenXml.VariantTypes.VTVector { BaseType = DocumentFormat.OpenXml.VariantTypes.VectorBaseValues.Variant, Size = 2U };

            var variant1 = new DocumentFormat.OpenXml.VariantTypes.Variant();
            variant1.Append(new OpenXmlElement[]{ new DocumentFormat.OpenXml.VariantTypes.VTLPSTR { Text = "Worksheets" }});

            var variant2 = new DocumentFormat.OpenXml.VariantTypes.Variant();
            variant2.Append(new OpenXmlElement[]{ new DocumentFormat.OpenXml.VariantTypes.VTInt32 { Text = "1" }});

            headingVector.Append(variant1, variant2);

            headingPairs.Append(new OpenXmlElement[]{headingVector});

            var titlesOfParts = new DocumentFormat.OpenXml.ExtendedProperties.TitlesOfParts();

            var titleOfPartsVector = new DocumentFormat.OpenXml.VariantTypes.VTVector { BaseType = DocumentFormat.OpenXml.VariantTypes.VectorBaseValues.Lpstr, Size = 1U };
            var vtlpstr = new DocumentFormat.OpenXml.VariantTypes.VTLPSTR { Text = "Sheet1" };

            titleOfPartsVector.Append(new OpenXmlElement[]{ vtlpstr });

            titlesOfParts.Append(new OpenXmlElement[] { titleOfPartsVector });

            properties.Append(headingPairs, titlesOfParts);

            properties.Append(new DocumentFormat.OpenXml.ExtendedProperties.LinksUpToDate { Text = "false" },
                new DocumentFormat.OpenXml.ExtendedProperties.SharedDocument { Text = "false" },
                new DocumentFormat.OpenXml.ExtendedProperties.HyperlinksChanged { Text = "false" },
                new DocumentFormat.OpenXml.ExtendedProperties.ApplicationVersion { Text = "14.0300" });
        }

        private void GenerateWorkbookPartContent(WorkbookPart workbookPart)
        {
            var workbook = new Workbook();
            workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            var fileVersion = new FileVersion { ApplicationName = "xl", LastEdited = "5", LowestEdited = "5", BuildVersion = "9302" };
            var workbookProperties = new WorkbookProperties { DefaultThemeVersion = 124226U };

            var bookViews = new BookViews();
            var workbookView = new WorkbookView { XWindow = 240, YWindow = 75, WindowWidth = 20115U, WindowHeight = 7995U };

            bookViews.Append(new OpenXmlElement[] { workbookView });

            var sheet = new Sheet { Name = _tableDetail.TableName, SheetId = 1U, Id = "rId1" };

            var sheets = new Sheets(sheet);

            var calculationProperties = new CalculationProperties { CalculationId = 145621U };

            workbook.Append(fileVersion, workbookProperties, bookViews, sheets, calculationProperties);

            workbookPart.Workbook = workbook;
        }

        private void GenerateSharedStringTable(SharedStringTablePart sharedStringTablePart)
        {
            var sharedStringTable = new SharedStringTable { Count = (UInt32)_tableDetail.Columns.Count, UniqueCount = (UInt32)_tableDetail.Columns.Count };

            foreach (var p in _tableDetail.Columns)
            {
                var sharedStringItem = new SharedStringItem();
                sharedStringItem.Append(new OpenXmlElement[] { new Text { Text = p.HeaderText } });
                sharedStringTable.Append(new OpenXmlElement[] { sharedStringItem });
            }

            sharedStringTablePart.SharedStringTable = sharedStringTable;
        }

        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart)
        {
            var worksheet = new Worksheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            var sheetDimension = new SheetDimension { Reference = "A1" };

            var sheetViews = new SheetViews();
            var sheetView = new SheetView { WorkbookViewId = 0U };

            sheetViews.Append(new OpenXmlElement[]{ sheetView });
            var sheetFormatProperties = new SheetFormatProperties { DefaultRowHeight = 15D, DyDescent = 0.25D };
            var sheetData = new SheetData();

            var pageMargins = new PageMargins { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };

            worksheet.Append(sheetDimension, sheetViews, sheetFormatProperties, sheetData, pageMargins);

            worksheetPart.Worksheet = worksheet;

            

            

            
            var rows = new OpenXmlElement[_tableDetail.TotalRows + 1];
            var dataTypes = new CellValues[_tableDetail.Columns.Count];
            var columns = new Columns();
            var colIndex = 'A';
            rows[0] = new Row { RowIndex = 1U, DyDescent = 0.25D };
            var cells = new OpenXmlElement[_tableDetail.Columns.Count];
            for (UInt32Value index = 0; index < _tableDetail.Columns.Count; index++)
            {
                var inputColumn = _tableDetail.Columns[(int)index.Value];
                var type = inputColumn.DataType;
                columns.Append(new OpenXmlElement[] { new Column { Min = index + 1, Max = index + 1, Width = inputColumn.Width, CustomWidth = true } });
                if (type == typeof (int) || type == typeof(double) || type == typeof(decimal) || type == typeof(float)) dataTypes[index] = CellValues.Number;
                else if (type == typeof (DateTime)) dataTypes[index] = CellValues.Date;
                else dataTypes[index] = CellValues.String;

                var cellRef = string.Format("{0}{1}", colIndex, 1U);
                cells[index] = new Cell { CellReference = cellRef, DataType = CellValues.SharedString };
                cells[index].Append(new[] { (OpenXmlElement)new CellValue { Text = index.Value.ToString(CultureInfo.InvariantCulture) } });
                colIndex++;
            }
            rows[0].Append(cells);
            
            sheetData.Append(new OpenXmlElement[]{ columns });

            for (UInt32Value r = 2; r <= _tableDetail.TotalRows + 1; r++)
            {
                rows[r - 1] = new Row { RowIndex = r, DyDescent = 0.25D };
                var row = rows[r - 1];
                colIndex = 'A';
                cells = new OpenXmlElement[_tableDetail.Columns.Count];
                for (var index = 0; index < _tableDetail.Columns.Count; index++)
                {
                    var cellRef = string.Format("{0}{1}", colIndex, r);
                    cells[index] = new Cell { CellReference = cellRef, DataType = dataTypes[index] };
                    cells[index].Append(new[] { (OpenXmlElement)new CellValue { Text = _tableDetail.Columns[index].Values[(int) (r-2)].ToString() } });
                    colIndex++;
                }
                row.Append(cells);
            }

            sheetData.Append(rows);
        }
    }
}
