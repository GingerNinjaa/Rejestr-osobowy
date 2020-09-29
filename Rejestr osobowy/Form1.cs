using Rejestr_osobowy.Model;
using Rejestr_osobowy.ViewModel;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rejestr_osobowy
{
    public partial class Form1 : Form
    {
        private const string Path = "D:\\Serialization.xml";
        List<PersonsViewModel> persons = new List<PersonsViewModel>();

        //private static string Flag = btnAction.Text;
        public Form1()
        {
            InitializeComponent();
            panelSubMenu.Visible = false;

            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            if (File.Exists(Path))
            {
                List<PersonsViewModel> user1 = null;
                string path = "cars.xml";

                XmlSerializer serializer = new XmlSerializer(typeof(List<PersonsViewModel>));

                StreamReader reader = new StreamReader(Path);
                persons = user1 = (List<PersonsViewModel>)serializer.Deserialize(reader);
                reader.Close();

                dataGridView1.DataSource = user1;

                //Column names
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Imię";
                dataGridView1.Columns[2].HeaderText = "Nazwisko";
                dataGridView1.Columns[3].HeaderText = "Wiek";
                dataGridView1.Columns[4].HeaderText = "Płeć";
                dataGridView1.Columns[5].HeaderText = "Miasto";
                dataGridView1.Columns[6].HeaderText = "Kod Pocztowy";
                dataGridView1.Columns[7].HeaderText = "Ulica";
                dataGridView1.Columns[8].HeaderText = "Numer Domu/Mieszkania";

                //this.dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void showSubMenu(Panel subMenu)
        {
            // jeśli submenu jest schowane 
            if (subMenu.Visible == false)
            {
                //chowaj wszystkie submenu
                // hideSubMenu();

                //pokaż to nasze submenu
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu);
            this.btnAction.Visible = false;
        }


  
        private void AddNewPerson(int count)
        {
            PersonsViewModel personsViewModel = new PersonsViewModel
            {
                id = count + 1,
                name = this.txtFirstName.Text,
                lastName = this.txtLastName.Text,
                age = int.Parse(this.txtAge.Text),
                gender = this.txtGender.Text,
                city = this.txtCity.Text,
                postalCode = this.txtPostalCode.Text,
                streat = this.txtStreat.Text,
                number = this.txtNumber.Text
            };

            persons.Add(personsViewModel);
        }

        private void EditNewPerson(int Id)
        {
            PersonsViewModel personsViewModel = new PersonsViewModel
            {
                id = Id,
                name = this.txtFirstName.Text,
                lastName = this.txtLastName.Text,
                age = int.Parse(this.txtAge.Text),
                gender = this.txtGender.Text,
                city = this.txtCity.Text,
                postalCode = this.txtPostalCode.Text,
                streat = this.txtStreat.Text,
                number = this.txtNumber.Text
            };

            var index = persons.FindIndex(x => x.id == Id);
            persons[index] = personsViewModel;
        }
        private void DeletePerson(int Id)
        {
            var index = persons.FindIndex(x => x.id == Id);
            persons.RemoveAt(index);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int Id = Convert.ToInt32(this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            var user = persons.Find(x => x.id == Id);

            populateTextBoxs(user);

        }

        private void populateTextBoxs(PersonsViewModel model)
        {
            this.txtFirstName.Text = model.name;
            this.txtLastName.Text = model.lastName;
            this.txtAge.Text = model.age.ToString();
            this.txtGender.Text = model.gender;
            this.txtCity.Text = model.city;
            this.txtPostalCode.Text = model.postalCode;
            this.txtStreat.Text = model.streat;
            this.txtNumber.Text = model.number;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            this.btnAction.Visible = true;
            this.btnAction.Text = "Dodaj";

        }

        private void btnDeletePerson_Click(object sender, EventArgs e)
        {
            this.btnAction.Visible = true;
            this.btnAction.Text = "Usuń";
        }

        private void btnEditPerson_Click(object sender, EventArgs e)
        {
            this.btnAction.Visible = true;
            this.btnAction.Text = "Edytuj";
        }

        private void btnAction_Click(object sender, EventArgs e)
        {

        }
    }
}
