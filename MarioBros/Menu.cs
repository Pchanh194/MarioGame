using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarioBros
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void tilesetBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void saveFile_FileOk(object sender, CancelEventArgs e)
        {
            saveFile.ShowDialog();

            if (saveFile.FileName.Length == 0)
                return;

            // MarioEditorXML.Save_To_XML(saveFile.FileName, Objects);
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
