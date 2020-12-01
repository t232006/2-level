using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace home1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var form = new Form()
            {
                //event KeyEventHandler KeyPress,
                MaximumSize = new System.Drawing.Size(800, 600),
                MinimumSize = new System.Drawing.Size(800, 600),
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "space",
                KeyPreview = true,
            };  form.KeyPress += new KeyPressEventHandler(Form_KeyPress);
            World. Init(form);
            form.Show();
            //World.Draw();
            Application.Run(form);
        }

        private static void Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '0') World.Shot();
        }
    }
}
