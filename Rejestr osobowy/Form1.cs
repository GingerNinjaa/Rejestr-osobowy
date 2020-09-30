using Rejestr_osobowy.Interface;
using Rejestr_osobowy.Model;
using Rejestr_osobowy.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rejestr_osobowy
{
    public partial class Form1 : Form, IDeserialize, IValidateEntry
    {
        private string Path  = Environment.CurrentDirectory + "-SpisLudności.xml";
        //private const string Path = "C:\\Serialization.xml";
        List<Person> persons = new List<Person>();

        public Form1()
        {
            InitializeComponent();
            panelSubMenu.Visible = false;

            PopulateDataGridView();
            PopulateComboBox();
        }

        #region Interfaces
        public void Deserialize()
        {
            List<Person> usersList = null;
            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
            StreamReader reader = new StreamReader(Path);
            persons = usersList = (List<Person>)serializer.Deserialize(reader);
            reader.Close();
        }
        public bool Validate()
        {
            //chceck if null or whitespace
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) && string.IsNullOrWhiteSpace(txtLastName.Text)
                && string.IsNullOrWhiteSpace(txtAge.Text) && string.IsNullOrWhiteSpace(txtGender.Text)
               && string.IsNullOrWhiteSpace(txtCity.Text) && string.IsNullOrWhiteSpace(txtPostalCode.Text)
               && string.IsNullOrWhiteSpace(txtStreat.Text) && string.IsNullOrWhiteSpace(txtNumber.Text))
            { this.Alert("Sprawdź wprowadzone dane!", Messages.enmType.Error); return false; }


            //chceck if age is number
            if (!int.TryParse(txtAge.Text, out int n))
            {
                this.Alert("Sprawdź wprowadzone dane!", Messages.enmType.Error); return false;
            }
            return true;
        }
        #endregion

        #region Populate
        private void PopulateComboBox()
        {

            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            comboSource.Add("id", "Id");
            comboSource.Add("name", "Imię");
            comboSource.Add("lastName", "Nazwisko");
            comboSource.Add("age", "Wiek");
            comboSource.Add("gender", "Płeć");
            comboSource.Add("city", "Miasto");
            comboSource.Add("postalCode", "Kod Pocztowy");
            comboSource.Add("streat", "Ulica");
            comboSource.Add("number", "Numer Domu/Mieszkania");
            cbSelectFilter.DataSource = new BindingSource(comboSource, null);
            cbSelectFilter.DisplayMember = "Value";
            cbSelectFilter.ValueMember = "Key";

        }
        private void PopulateDataGridView()
        {
            int count = 0;

            if (File.Exists(Path))
            {
                Deserialize();

                var xDoc = XDocument.Load(Path);
                count = xDoc.Descendants("Osoba").Count();
                this.lblCounter.Text = count.ToString();

                dataGridView1.DataSource = persons;

                //Column names
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].HeaderText = "Imię";
                dataGridView1.Columns[2].HeaderText = "Nazwisko";
                dataGridView1.Columns[3].HeaderText = "Wiek";
                dataGridView1.Columns[4].HeaderText = "Płeć";
                dataGridView1.Columns[5].HeaderText = "Miasto";
                dataGridView1.Columns[6].HeaderText = "Kod Pocztowy";
                dataGridView1.Columns[7].HeaderText = "Ulica";
                dataGridView1.Columns[8].HeaderText = "Numer Domu/Mieszkania";

                this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
        private void populateTextBoxs(Person model)
        {
            try
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
            catch (Exception)
            {
            }

        }
        #endregion

        #region Logic
        private void showSubMenu(Panel subMenu)
        {
            // chceck if submenu is close
            if (subMenu.Visible == false)
            {
                //Show submenu
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void AddNewPerson(int count)
        {
            try
            {
                Person personsViewModel = new Person
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
                this.Alert("Dodano nową osobę", Messages.enmType.Success);
                this.lblCounter.Text = count.ToString();
            }
            catch (Exception)
            {
                // this.Alert("Ooops", Messages.enmType.Error);
            }
        }
        private void EditNewPerson(int Id)
        {
            var checker = 0;
            try
            {
                Person personsViewModel = new Person
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
                checker = index;
                this.Alert($"Edytowano osobe o id = {Id}", Messages.enmType.Success);
            }
            catch (Exception)
            {
                if (checker <= 0)
                {
                    this.Alert("Nie Znaleziono pliku - trwa naprawa", Messages.enmType.Error);
                }
                else
                {
                    this.Alert("Ooops", Messages.enmType.Error);
                }
            }
        }
        private void DeletePerson(int Id)
        {
            try
            {
                if (Id > 1)
                {
                    var index = persons.FindIndex(x => x.id == Id);
                    persons.RemoveAt(index);
                    int count = Id - 1;
                    this.lblCounter.Text = count.ToString();
                    this.Alert($"Usunieto osobe o id = {Id}", Messages.enmType.Success);
                }
            }
            catch (Exception)
            {
                this.Alert("Ooops", Messages.enmType.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int Id = 0;
            try
            {
                Id = Convert.ToInt32(this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
            }


            var user = persons.Find(x => x.id == Id);

            populateTextBoxs(user);

        }

        //Window with informations
        private void Alert(string msg, Messages.enmType type)
        {
            Messages popup = new Messages();
            popup.showAlert(msg, type);
        }
        private void ClearAllTextBoxs()
        {
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtAge.Text = string.Empty;
            this.txtGender.Text = string.Empty;
            this.txtCity.Text = string.Empty;
            this.txtPostalCode.Text = string.Empty;
            this.txtStreat.Text = string.Empty;
            this.txtNumber.Text = string.Empty;
        }

        //Search
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var category = ((KeyValuePair<string, string>)cbSelectFilter.SelectedItem).Key;
            switch (category)
            {
                case "id":
                    dataGridView1.DataSource = persons.FindAll(x => x.id.ToString().Equals(txtSearch.Text)).ToList().OrderBy(x => x.id);
                    break;
                case "name":
                    dataGridView1.DataSource = persons.Where(x => x.name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                    break;
                case "lastName":
                    dataGridView1.DataSource = persons.Where(x => x.lastName.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                    break;
                case "age":
                    dataGridView1.DataSource = persons.FindAll(x => x.age.ToString().Contains(txtSearch.Text)).ToList().OrderBy(x => x.age);
                    break;
                case "gender":
                    dataGridView1.DataSource = persons.Where(x => x.gender.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                    break;
                case "city":
                    dataGridView1.DataSource = persons.Where(x => x.city.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                    break;
                case "postalCode":
                    dataGridView1.DataSource = persons.Where(x => x.postalCode.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                    break;
                case "streat":
                    dataGridView1.DataSource = persons.Where(x => x.streat.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                    break;
                case "number":
                    dataGridView1.DataSource = persons.Where(x => x.number.ToLower().Equals(txtSearch.Text.ToLower())).ToList();
                    break;

            }

            if (txtSearch.Text == string.Empty)
            {
                dataGridView1.DataSource = persons;
            }


        }

        #endregion

        #region Butons
        private void btnMenu_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
            showSubMenu(panelSubMenu);
            this.btnAction.Visible = false;
            this.btnClear.Visible = false;
            ClearAllTextBoxs();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
            this.btnClear.Visible = true;
            this.btnAction.Visible = true;
            this.btnAction.Text = "Dodaj";
            ClearAllTextBoxs();

        }
        private void btnDeletePerson_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
            this.btnClear.Visible = true;
            this.btnAction.Visible = true;
            this.btnAction.Text = "Usuń";
            ClearAllTextBoxs();
        }
        private void btnEditPerson_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
            this.btnClear.Visible = true;
            this.btnAction.Visible = true;
            this.btnAction.Text = "Edytuj";
            ClearAllTextBoxs();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllTextBoxs();
        }
        private void btnAction_Click(object sender, EventArgs e)
        {
            string action = this.btnAction.Text;

            XmlSerializer xs = new XmlSerializer(typeof(List<Person>));

            var count = 0;

            if (File.Exists(Path))
            {
                var xDoc = XDocument.Load(Path);
                count = xDoc.Descendants("Osoba").Count();
                this.lblCounter.Text = count.ToString();
            }
            try
            {
                if (!Validate())
                {
                    throw new Exception();
                }
                
            }
            catch (Exception)
            {
            }
           

            switch (action)
            {
                case "Dodaj":
                    AddNewPerson(count);
                    break;
                case "Edytuj":
                    EditNewPerson(count);
                    break;
                case "Usuń":
                    DeletePerson(count);
                    break;
            }
            try
            {
                TextWriter txtWriter = new StreamWriter(Path);

                xs.Serialize(txtWriter, persons);
                txtWriter.Close();
            }
            catch (Exception)
            {
                this.Alert("Nie Znaleziono pliku - trwa naprawa", Messages.enmType.Error);
            }


            PopulateDataGridView();
        }
        #endregion

    }
}
