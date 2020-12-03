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
            form.KeyDown += new KeyEventHandler(Form_KeyDown);
            form.KeyUp += new KeyEventHandler(Form_KeyUp);
            
            World. Init(form);
            form.Show();
            //World.Draw();
            Application.Run(form);
        }

        private static void Form_KeyUp(object sender, KeyEventArgs e)
        {
            //World.SpeedCor()
            World.KeysPressed.Remove(e.KeyData);
            World.SpeedCor(World.KeysPressed);
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (!World.KeysPressed.Contains(e.KeyData))
            {
                World.KeysPressed.Add(e.KeyData);
                World.SpeedCor(World.KeysPressed);
            }
        }

        private static void Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '0') World.Shot();
        }
    }
}
