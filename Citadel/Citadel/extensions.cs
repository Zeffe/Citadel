using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Extensions
{
    public static class extensions
    {
        static bool isNumber(string str)
        {
            return str.All(c => c >= '0' && c <= '9');
        }

        public static void Sort(this int[] arr)
        {
            int temp = 0;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i + 1] < arr[i])
                {
                    temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                }
            }
        }

        public static void dFormat(this DataGridViewCellEventArgs e, DataGridView dgv)
        {
            DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string cellText = "";
            try
            {
                cellText = cell.Value.ToString();
            }
            catch { }

            if (e.ColumnIndex == 0 && !isNumber(cellText))
            {
                cell.Value = "";
            }
            
            if (e.ColumnIndex == 3)
            {
                Double feeDbl;
                Double.TryParse(cellText, out feeDbl);

                // Format the number, so the user doesn't have to.
                if (!cellText.Contains("$"))
                {
                    cellText = "$" + cellText;
                }
                if (!cellText.Contains("."))
                {
                    cellText += ".00";
                }
                String[] temp = cellText.Split('.');
                if (temp[1].Length == 1)
                {
                    temp[1] += "0";
                    cellText = temp[0] + "." + temp[1];
                }

                cell.Value = cellText;
            }

        }

        public static String firstName(this String[,] arr, int firstDimension)
        {
            return arr[firstDimension, 1];
        }
    }
}
