using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace victoria_2_factory
{


    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string provinceJson = File.ReadAllText(Application.StartupPath + "\\province.json");
            var province = JsonConvert.DeserializeObject<List<province>>(provinceJson);

            foreach (var item in province)
            {
                if (!(listBox1.Items.Contains(item.Region.ToLower())))
                {
                    listBox1.Items.Add(item.Region.ToLower());
                }
            }

            listView1.View = View.LargeIcon;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("", 150);
            listView1.Columns.Add("", 150);
            listView2.View = View.LargeIcon;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("", 150);
            listView2.Columns.Add("", 150);

            string json = File.ReadAllText(Application.StartupPath + "\\rgo.json");
            var RGO = JsonConvert.DeserializeObject<List<RGO>>(json);
            int counter = 0;

            foreach (var product in RGO)
            {
                imageList1.Images.Add(Image.FromFile(Application.StartupPath + @"\images\" + product.Name + ".png"));
            }

            this.imageList1.ImageSize = new Size(26, 26);
            this.listView1.LargeImageList = this.imageList1;
            this.listView2.LargeImageList = this.imageList1;

            foreach (var product in RGO)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = counter;
                listView1.Items.Add(product.Name, counter);

                counter++;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string provinceJson = File.ReadAllText(Application.StartupPath + "\\province.json");
            var province = JsonConvert.DeserializeObject<List<province>>(provinceJson);

            foreach (var item in province)
            {
                if (item.Region.ToLower().Contains(textBox1.Text.ToLower()) && !(listBox1.Items.Contains(item.Region.ToLower())))
                {
                    listBox1.Items.Add(item.Region.ToLower());
                }
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0 && listView2.Items.Count != 0)
            {
                foreach (ListViewItem item in listView2.Items)
                {
                    if (item.Text == listView1.Items[intselectedindex].Text)
                    {
                        return;
                    }
                }
                listView2.Items.Add(listView1.Items[intselectedindex].Text, intselectedindex);
            }
            else
            {
                listView2.Items.Add(listView1.Items[intselectedindex].Text, intselectedindex);
            }


            LW2Countrol();
        }
        public void LW2Countrol()
        {
            string json = File.ReadAllText(Application.StartupPath + "\\factories.json");
            var factories = JsonConvert.DeserializeObject<List<Factories>>(json);
            int intselectedindex = listView1.SelectedIndices[0];

            foreach (var factory in factories)
            {
                foreach (var product in factory.Inputs)
                {
                    if (product == listView1.Items[intselectedindex].Text)
                    {
                        if (!listBox4.Items.Contains(listView1.Items[intselectedindex].Text) && !listBox4.Items.Contains(factory.Name))
                        {
                            listBox4.Items.Add(factory.Name);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            listBox4.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string json = File.ReadAllText(Application.StartupPath + "\\factories.json");
            var factories = JsonConvert.DeserializeObject<List<Factories>>(json);

            listBox2.Items.Clear();
            listBox3.Items.Clear();
            string provinceJson = File.ReadAllText(Application.StartupPath + "\\province.json");
            var province = JsonConvert.DeserializeObject<List<province>>(provinceJson);
            string selectedRegion = listBox1.SelectedItem.ToString();
            foreach (var item in province)
            {
                if (selectedRegion == item.Region.ToLower() && !listBox2.Items.Contains(item.Resource))
                {
                    listBox2.Items.Add(item.Resource);
                    foreach (var factory in factories)
                    {
                        foreach (var product in factory.Inputs)
                        {
                            if (product == item.Resource && !listBox3.Items.Contains(factory.Name))
                            {
                                listBox3.Items.Add(factory.Name);
                            }
                        }
                    }
                }
            }
        }
    }
    public class Factories
    {
        public string Name;
        public List<string> Inputs;
    }
    public class RGO
    {
        public string Name;
        public string Image;
    }
    public class province
    {
        public string Resource;
        public string Region;
    }
}