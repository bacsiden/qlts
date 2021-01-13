using MongoDB.Driver;
using DK.Application.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace DK.Application.Repositories
{
    public class ViewFieldRepository : BaseRepository<ViewField>, IViewFieldRepository
    {
        public ViewFieldRepository(IMongoDatabase db) : base(db)
        {
        }
        public List<ViewField> ListForTaiSan(string username)
        {
            var lst = Find(m => m.CreatedBy == username && m.ObjectName == nameof(TaiSan)).ToList();
            var updated = false;
            var displayFields = typeof(TaiSan).GetProperties().Where(m => m.GetCustomAttributes(typeof(DisplayAttribute), true).Any());
            var views = new List<ViewField>();
            foreach (var item in displayFields)
            {
                var displayAttribute = (DisplayAttribute)typeof(TaiSan).GetProperty(item.Name).GetCustomAttributes(typeof(DisplayAttribute), true).First();
                var indexAttribute = (ColumnIndexAttribute)typeof(TaiSan).GetProperty(item.Name).GetCustomAttributes(typeof(ColumnIndexAttribute), true).FirstOrDefault();

                views.Add(new ViewField
                {
                    CreatedBy = username,
                    FieldName = item.Name,
                    DisplayText = displayAttribute.Name,
                    ObjectName = nameof(TaiSan),
                    Order = indexAttribute == null ? 1000 : indexAttribute.Index
                });
            }
            var shouldBeRemoved = lst.Where(m => !views.Any(x => x.ObjectName == nameof(TaiSan) && x.Order == m.Order && x.CreatedBy == m.CreatedBy && x.FieldName == m.FieldName && x.DisplayText == m.DisplayText));
            foreach (var item in shouldBeRemoved)
            {
                updated = true;
                Delete(item.Id);
            }

            var shouldBeAdded = views.Where(m => !lst.Any(x => x.ObjectName == nameof(TaiSan) && x.Order == m.Order && x.CreatedBy == m.CreatedBy && x.FieldName == m.FieldName && x.DisplayText == m.DisplayText));

            if (shouldBeAdded.Any())
            {
                AddRange(shouldBeAdded);
                updated = true;
            }
            if (updated)
                lst = Find(m => m.CreatedBy == username && m.ObjectName == nameof(TaiSan)).ToList();
            return lst.OrderBy(m => m.Order).ToList();
        }
    }
}
