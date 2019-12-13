using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public static class PrintMessage
    {
        public static void WarningMessage(string text,string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void InformationMessage(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void WarningMessage(string text)
        {
            MessageBox.Show(text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void InformationMessage(string text)
        {
            MessageBox.Show(text, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
