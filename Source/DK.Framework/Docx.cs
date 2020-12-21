using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.Docx;

namespace DK.Framework
{
    public class DocField
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public static class Docx
    {
        public static void FillField(string tempFileName, string outputFileName, params object[] values)
        {
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);

            File.Copy(tempFileName, outputFileName);


            var valuesToFill = new Content();
            foreach (var item in values)
            {
                valuesToFill.Fields.Add(new FieldContent(item.GetType().GetProperty("Name").GetValue(item).ToString(), item.GetType().GetProperty("Value").GetValue(item).ToString()));
            }
            
            using (var outputDocument = new TemplateProcessor(outputFileName)
                .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
        }

        public static void FillSingleTable(string tempFileName, string outputFileName, List<List<DocField>> values)
        {
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);

            File.Copy(tempFileName, outputFileName);

            var valuesToFill = new Content();
            var table = new TableContent("Table data");
            foreach (var item in values)
            {
                var columns = new List<FieldContent>();
                foreach (var field in item)
                {
                    columns.Add(new FieldContent(field.Name, field.Value));
                }
                table.AddRow(columns.ToArray());
            }

            using (var outputDocument = new TemplateProcessor(outputFileName)
                .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
        }

        public static void FillSingleTable(string tempFileName, string outputFileName, List<List<DocField>> values, List<DocField> fields)
        {
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);

            File.Copy(tempFileName, outputFileName);

            var valuesToFill = new Content();
            var table = new TableContent("Table data");
            foreach (var item in values)
            {
                var columns = new List<FieldContent>();
                foreach (var field in item)
                {
                    columns.Add(new FieldContent(field.Name, field.Value));
                }
                table.AddRow(columns.ToArray());
            }

            valuesToFill.Tables.Add(table);
            foreach (var item in fields)
            {
                valuesToFill.Fields.Add(new FieldContent(item.Name, item.Value));
            }
            using (var outputDocument = new TemplateProcessor(outputFileName)
                .SetRemoveContentControls(true))
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
        }
    }
}
