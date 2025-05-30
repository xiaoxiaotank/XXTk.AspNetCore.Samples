using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data;
public static class DataTableExtensions
{
    public static List<Dictionary<string, object>> ToDictionaryList(this DataTable table)
    {
        if (table is null)
        {
            return [];
        }

        return table.Rows.OfType<DataRow>()
            .Select(row => table.Columns.OfType<DataColumn>().ToDictionary(col => col.ColumnName, c => row[c] == DBNull.Value ? null : row[c]))
            .ToList();
    }
}
