﻿using Hotcakes.Commerce.Marketing.PromotionQualifications;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KliensAlk
{
	public partial class Form1 : Form
	{
		ApiResponse<List<ProductDTO>> termekadatok = new ApiResponse<List<ProductDTO>>();	
		private static Api ApiKapcs()
		{
			string url = "http://20.234.113.211:8107";
			string key = "1-79771cd1-cb22-4710-a786-b360d8a92c2f";
			var p = new Api(url, key);
			return p;
		}

		public Form1()
		{
			InitializeComponent();
			Api p = ApiKapcs();
			termekadatok = p.ProductsFindAll();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			TermekNevSzures();

		}

		private void TermekNevSzures()
		{
			var trmk = from x in termekadatok.Content.ToList()
					   where x.ProductName.Contains(textBox1.Text)
					   select x;
			listBox1.DataSource = trmk.ToList();
			listBox1.DisplayMember = "ProductName";
			//listBox1.ValueMember = "";
		}


		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			TermekNevSzures();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string bvin = termekadatok.Content[listBox1.SelectedIndex].Bvin;

			Api p = ApiKapcs();
			var keszlet = p.ProductInventoryFindAll().Content[listBox1.SelectedIndex];
			textBox2.Text = keszlet.QuantityOnHand.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Api p = ApiKapcs();
			var keszlet = p.ProductInventoryFindAll().Content[listBox1.SelectedIndex];
			keszlet.QuantityOnHand += 1;
			ApiResponse<ProductInventoryDTO> response = p.ProductInventoryUpdate(keszlet);
			textBox2.Text = keszlet.QuantityOnHand.ToString();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Api p = ApiKapcs();
			var keszlet = p.ProductInventoryFindAll().Content[listBox1.SelectedIndex];
			if (keszlet.QuantityOnHand > 0) { keszlet.QuantityOnHand -= 1; }
			ApiResponse<ProductInventoryDTO> response = p.ProductInventoryUpdate(keszlet);
			textBox2.Text = keszlet.QuantityOnHand.ToString();
		}
	}
}
