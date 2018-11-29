using Microsoft.AspNet.OData.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hcs.Platform.Odata
{
    public class OdataQueryValidationFailureException : Exception
    {
        public OdataQueryValidationFailureException(string message) : base(message) { }
    }
    public class OdataValidator<TModel> : IOdataValidator<TModel>
    {
        public OdataValidationResult Validate(ODataQueryOptions<TModel> queryOptions, IOdataQueryPermission odataPermissions)
        {
            var container = new List<string>();
            var items = queryOptions?.SelectExpand?.SelectExpandClause?.SelectedItems;
            if (items != null)
            {
                items = items.Where(x => x is ExpandedNavigationSelectItem);
            }
            GetPaths(items, "", container);
            if (container.Any())
            {
                var allow = odataPermissions.GetByEntityType(typeof(TModel));
                foreach (var p in container)
                {
                    if (!allow.Contains(p))
                    {
                        var splited = p.Split('.');
                        if (!allow.Any(x =>
                        {
                            var allowSplited = x.Split('.');
                            return allowSplited.SequenceEqual(splited.Take(allowSplited.Length));
                        }))
                        {
                            return new OdataValidationResult { Message = $"expand {p} not allow", Valid = false };
                        }
                    }
                }
            }
            return new OdataValidationResult { Valid = true };
        }

        void GetPaths(IEnumerable<SelectItem> source, string currentPath, IList<string> contianer)
        {
            if (source == null)
            {
                return;
            }
            var navi = source.Where(x => x is ExpandedNavigationSelectItem).Cast<ExpandedNavigationSelectItem>().ToArray();
            var select = source.Where(x => !(x is ExpandedNavigationSelectItem)).Cast<PathSelectItem>().ToArray();
            var naviPaths = new List<string>();
            foreach (var item in navi)
            {
                var propertyName = (item.PathToNavigationProperty.FirstSegment as NavigationPropertySegment).NavigationProperty.Name;
                var name = (currentPath + "." + propertyName).Trim('.');
                if (item.SelectAndExpand.SelectedItems.Any())
                {
                    naviPaths.Add(propertyName);
                    GetPaths(item.SelectAndExpand.SelectedItems, name, contianer);
                }
                else
                {
                    contianer.Add($"{currentPath}.{((ODataExpandPath)item.PathToNavigationProperty).FirstSegment.Identifier}".Trim('.'));
                }
            }
            foreach (var item in select)
            {
                var propertyName = item.SelectedPath.FirstSegment.Identifier;
                if (!naviPaths.Contains(propertyName))
                {
                    contianer.Add($"{currentPath}.{propertyName}".Trim('.'));
                }
            }
        }
    }
}
