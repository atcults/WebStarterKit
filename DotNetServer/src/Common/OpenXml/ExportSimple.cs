using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Common.OpenXml
{
    public class ExportSimple
    {
        private readonly string _filePath;
        private readonly object _datas;

        private readonly Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();

        public ExportSimple(string filePath, object datas)
        {
            _filePath = filePath;
            _datas = datas;
        }

        public void CreatePackage()
        {
            var type = _datas.GetType();

            if (type.IsArray)
            {
                var a = (Array)_datas;
                type = a.GetValue(0).GetType();
            }

            if (type == null) throw new Exception("Invalid type");

            var infos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in from p in infos where p.CanRead let mget = p.GetGetMethod(false) where mget != null select p)
            {
                _properties.Add(p.Name, p);
            }

            using (var package = SpreadsheetDocument.Create(_filePath, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(package);
            }
        }

        private void CreateParts(SpreadsheetDocument document)
        {
            var extendedFileProperties = document.AddNewPart<ExtendedFilePropertiesPart>("rId1");
            GenerateExtendedFileProperties(extendedFileProperties);

            var workbookPart = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart);

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart);

            var sharedStringTablePart = workbookPart.AddNewPart<SharedStringTablePart>("rId2");
            GenerateSharedStringTable(sharedStringTablePart);

            SetPackageProperties(document);
        }

        private void GenerateExtendedFileProperties(ExtendedFilePropertiesPart extendedFilePropertiesPart)
        {
            var properties = new DocumentFormat.OpenXml.ExtendedProperties.Properties();
            properties.AddNamespaceDeclaration("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");

            properties.Append(new DocumentFormat.OpenXml.ExtendedProperties.Application { Text = "Microsoft Excel" },
                              new DocumentFormat.OpenXml.ExtendedProperties.DocumentSecurity { Text = "0" },
                              new DocumentFormat.OpenXml.ExtendedProperties.ScaleCrop { Text = "false" });

            var headingPairs = new DocumentFormat.OpenXml.ExtendedProperties.HeadingPairs();

            var headingVector = new DocumentFormat.OpenXml.VariantTypes.VTVector { BaseType = DocumentFormat.OpenXml.VariantTypes.VectorBaseValues.Variant, Size = 2U };

            var variant1 = new DocumentFormat.OpenXml.VariantTypes.Variant();
            variant1.Append(new OpenXmlElement[] { new DocumentFormat.OpenXml.VariantTypes.VTLPSTR { Text = "Worksheets" } });

            var variant2 = new DocumentFormat.OpenXml.VariantTypes.Variant();
            variant2.Append(new OpenXmlElement[] { new DocumentFormat.OpenXml.VariantTypes.VTInt32 { Text = "1" } });

            headingVector.Append(variant1, variant2);

            headingPairs.Append(new OpenXmlElement[] { headingVector });

            var titlesOfParts = new DocumentFormat.OpenXml.ExtendedProperties.TitlesOfParts();

            var titleOfPartsVector = new DocumentFormat.OpenXml.VariantTypes.VTVector { BaseType = DocumentFormat.OpenXml.VariantTypes.VectorBaseValues.Lpstr, Size = 1U };
            var vtlpstr = new DocumentFormat.OpenXml.VariantTypes.VTLPSTR { Text = "Sheet1" };

            titleOfPartsVector.Append(new OpenXmlElement[] { vtlpstr });

            titlesOfParts.Append(new OpenXmlElement[] { titleOfPartsVector });

            properties.Append(headingPairs, titlesOfParts);

            properties.Append(new DocumentFormat.OpenXml.ExtendedProperties.LinksUpToDate { Text = "false" },
                new DocumentFormat.OpenXml.ExtendedProperties.SharedDocument { Text = "false" },
                new DocumentFormat.OpenXml.ExtendedProperties.HyperlinksChanged { Text = "false" },
                new DocumentFormat.OpenXml.ExtendedProperties.ApplicationVersion { Text = "14.0300" });

            extendedFilePropertiesPart.Properties = properties;
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

            var sheets = new Sheets();
            var sheet = new Sheet { Name = "Sheet1", SheetId = 1U, Id = "rId1" };

            sheets.Append(new OpenXmlElement[] { sheet });

            var calculationProperties = new CalculationProperties { CalculationId = 145621U };

            workbook.Append(fileVersion, workbookProperties, bookViews, sheets, calculationProperties);

            workbookPart.Workbook = workbook;
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

            sheetViews.Append(new OpenXmlElement[] { sheetView });
            var sheetFormatProperties = new SheetFormatProperties { DefaultRowHeight = 15D, DyDescent = 0.25D };
            var sheetData = new SheetData();

            AppendHeaderRow(sheetData);

            if (_datas.GetType().IsArray)
            {
                foreach (var data in (IEnumerable)_datas)
                {
                    AppendDataRow(sheetData, data);
                }
            }
            else
            {
                AppendDataRow(sheetData, _datas);
            }

            var pageMargins = new PageMargins { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };

            worksheet.Append(sheetDimension, sheetViews, sheetFormatProperties, sheetData, pageMargins);

            worksheetPart.Worksheet = worksheet;
        }

        private void AppendHeaderRow(SheetData sheetData)
        {
            var colIndex = 'A';
            var row = new Row { RowIndex = 1U, DyDescent = 0.25D };

            for (var index = 0; index < _properties.Count; index++)
            {
                var cellRef = string.Format("{0}{1}", colIndex, 1U);
                var cell = new Cell { CellReference = cellRef, DataType = CellValues.SharedString };
                cell.Append(new OpenXmlElement[] { new CellValue { Text = index.ToString(CultureInfo.InvariantCulture) } });
                row.Append(new OpenXmlElement[] { cell });
                colIndex++;
            }

            sheetData.Append(new OpenXmlElement[] { row });
        }

        private void AppendDataRow(OpenXmlElement sheetData, object data)
        {
            var colIndex = 'A';
            var rowIndex = (UInt32)sheetData.Count() + 1;
            var row = new Row { RowIndex = rowIndex, DyDescent = 0.25D };
            foreach (var info in _properties)
            {
                var cellString = string.Empty;
                var cellVal = info.Value.GetValue(data, null);
                if (cellVal != null) cellString = cellVal.ToString();

                var cellRef = string.Format("{0}{1}", colIndex, rowIndex);
                var cell = new Cell { CellReference = cellRef, DataType = CellValues.String };
                cell.Append(new OpenXmlElement[] { new CellValue { Text = cellString } });
                row.Append(new OpenXmlElement[] { cell });
                colIndex++;
            }
            sheetData.Append(new OpenXmlElement[] { row });
        }

        private void GenerateSharedStringTable(SharedStringTablePart sharedStringTablePart)
        {
            var sharedStringTable = new SharedStringTable { Count = (UInt32)_properties.Count, UniqueCount = (UInt32)_properties.Count };

            foreach (var p in _properties)
            {
                var sharedStringItem = new SharedStringItem();
                var text = new Text { Text = p.Key };
                sharedStringItem.Append(new OpenXmlElement[] { text });
                sharedStringTable.Append(new OpenXmlElement[] { sharedStringItem });
            }

            sharedStringTablePart.SharedStringTable = sharedStringTable;
        }

        private void SetPackageProperties(OpenXmlPackage document)
        {
            document.PackageProperties.Creator = "Tool";
            document.PackageProperties.Created = DateTime.Now;
        }
    }
}
