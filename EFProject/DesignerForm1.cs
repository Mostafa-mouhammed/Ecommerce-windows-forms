using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors.TextEditController.Win32;
using DXAppProject.DBContext;
using DXAppProject.models;
using EFProject.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EFProject;

public partial class DesignerForm1 : DevExpress.XtraBars.Ribbon.RibbonForm
{
    public DesignerForm1()
    {
        InitializeComponent();
    }
    internal static Dictionary<Product, int> carts = new();
    private async void barbtnOffice_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPageOffice;
        MyContext context = new();
        dataGridViewOfficeGet.DataSource = await context.offices.ToListAsync();
    }

    private async void barbtnEmployee_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPageEmployee;
        MyContext context = new();
        dataGridViewEmployee.DataSource = await context.employees.ToListAsync();
        dataGridViewEmployee.Columns["office"].Visible = false;
        dataGridViewEmployee.Columns["employee"].Visible = false;
    }

    private async void barbtnCustomer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPageCustomer;
        MyContext context = new();
        dataGridViewCustomers.DataSource = await context.customers.ToListAsync();
    }

    private async void barbtnCategory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPageCategory;
        MyContext context = new();
        dataGridViewCategory.DataSource = await context.Category.ToListAsync();
    }

    private async void barbtnProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPageProduct;
        MyContext context = new();
        comboBoxProductCategoryGet.DataSource = await context.Category.ToListAsync();
        comboBoxProductCategoryGet.DisplayMember = "Name";
        comboBoxProductCategoryGet.ValueMember = "ID";
    }

    private async void barbtnOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPageOrder;
        OrderFetchData();

    }

    private async void OrderFetchData()
    {
        MyContext context = new();
        comboBoxOrderchooseCustomerPlace.DataSource = null;
        comboBoxOrderchooseCustomerPlace.DataSource = await context.customers.ToListAsync();
        comboBoxOrderchooseCustomerPlace.DisplayMember = "FullName";
        comboBoxOrderchooseCustomerPlace.ValueMember = "ID";

        comboBoxChooseProduct.DataSource = null;
        comboBoxChooseProduct.DataSource = await context.products.Where(p => p.QtyInStock > 0).ToListAsync();
        comboBoxChooseProduct.DisplayMember = "Name";
        comboBoxChooseProduct.ValueMember = "Code";
    }

    private async void barbtnPayment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        NavFrameMain.SelectedPage = MainNavPagePayment;
        MyContext context = new();
        comboBoxPaymentViewCustomer.DataSource = await context.customers.ToListAsync();
        comboBoxPaymentViewCustomer.DisplayMember = "FullName";
        comboBoxPaymentViewCustomer.ValueMember = "ID";
    }

    private async void btnOfficeUpDel_Click(object sender, System.EventArgs e)
    {
        textBoxOfficeUpCity.Text = "";
        textBoxOfficeUpAddress.Text = "";
        textBoxOfficeUpPost.Text = "";
        textBoxOfficeUpPhone.Text = "";

        NavFrameOffice.SelectedPage = NavPageOfficeUpDel;
        MyContext context = new();
        comboBoxOfficeUpdate.DataSource = await context.offices.ToListAsync();
        comboBoxOfficeUpdate.DisplayMember = "Code";
        comboBoxOfficeUpdate.ValueMember = "Code";
    }

    private void btnOfficePost_Click(object sender, System.EventArgs e)
    {
        NavFrameOffice.SelectedPage = NavPageOfficePost;
    }

    private async void btnOfficeGet_Click(object sender, System.EventArgs e)
    {
        NavFrameOffice.SelectedPage = NavPageOfficeGet;
        MyContext context = new();
        dataGridViewOfficeGet.DataSource = await context.offices.ToListAsync();
    }

    private async void dataGridViewOfficeGet_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridViewOfficeGet.CurrentRow != null)
        {
            Office office = dataGridViewOfficeGet.CurrentRow.DataBoundItem as Office;
            MyContext context = new();
            List<Employee> empsinoffice = await context.employees.Where(e => e.OfficeCode == office.Code).ToListAsync();
            dataGridViewOfficeGetEmps.DataSource = empsinoffice;
            dataGridViewOfficeGetEmps.Columns["office"].Visible = false;
            dataGridViewOfficeGetEmps.Columns["employee"].Visible = false;
        }
    }

    private async void dataGridViewOfficeGet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        Office office = dataGridViewOfficeGet.CurrentRow.DataBoundItem as Office;
        MyContext context = new();
        await context.SaveChangesAsync();
    }

    private async void btnCreateOffice_Click(object sender, System.EventArgs e)
    {

        int.TryParse(textBoxOfficeUpPost.Text, out int postal);
        Office newoffice = new Office()
        {
            City = textBoxCreateOfficeCity.Text,
            Address = textBoxCreateOfficeAddress.Text,
            Phone = textBoxCreateOfficePhone.Text,
            Postalcode = postal
        };
        MyContext context = new();
        await context.offices.AddAsync(newoffice);
        await context.SaveChangesAsync();
        MessageBox.Show($"a new office with id {newoffice.Code} is created");
        textBoxCreateOfficePhone.Text = string.Empty;
        textBoxCreateOfficeAddress.Text = string.Empty;
        textBoxCreateOfficeCity.Text = string.Empty;
        textBoxCreateOfficePostC.Text = string.Empty;
    }

    private async void buttonOfficeUpdate_Click(object sender, System.EventArgs e)
    {

        if (comboBoxOfficeUpdate.SelectedValue != null)
        {

            int officeid;
            if (comboBoxOfficeUpdate.SelectedValue is int)
            {
                officeid = (int)comboBoxOfficeUpdate.SelectedValue;
            }
            else
            {
                officeid = (comboBoxOfficeUpdate.SelectedValue as Office).Code;
            }

            int.TryParse(textBoxOfficeUpPost.Text, out int postal);
            MyContext context = new();

            Office oldoffice = await context.offices.FirstOrDefaultAsync(o => o.Code == officeid);
            oldoffice.Address = textBoxOfficeUpAddress.Text;
            oldoffice.Postalcode = postal;
            oldoffice.Phone = textBoxOfficeUpPhone.Text;
            oldoffice.City = textBoxOfficeUpCity.Text;
            textBoxOfficeUpCity.Text = "";
            textBoxOfficeUpAddress.Text = "";
            textBoxOfficeUpPost.Text = "";
            textBoxOfficeUpPhone.Text = "";
            await context.SaveChangesAsync();


            comboBoxOfficeUpdate.DataSource = null;
            comboBoxOfficeUpdate.DataSource = await context.offices.ToListAsync();
            comboBoxOfficeUpdate.DisplayMember = "Code";
            comboBoxOfficeUpdate.ValueMember = "Code";
        }
    }

    private async void buttonOfficeDelete_Click(object sender, System.EventArgs e)
    {

        if (comboBoxOfficeUpdate.SelectedValue != null)
        {

            int officeid;
            if (comboBoxOfficeUpdate.SelectedValue is int)
            {
                officeid = (int)comboBoxOfficeUpdate.SelectedValue;
            }
            else
            {
                officeid = (comboBoxOfficeUpdate.SelectedValue as Office).Code;
            }
            textBoxOfficeUpCity.Text = "";
            textBoxOfficeUpAddress.Text = "";
            textBoxOfficeUpPost.Text = "";
            textBoxOfficeUpPhone.Text = "";

            MyContext context = new();

            Office office = await context.offices.FirstOrDefaultAsync(o => o.Code == officeid);
            context.Remove(office);
            await context.SaveChangesAsync();

            comboBoxOfficeUpdate.DataSource = null;
            comboBoxOfficeUpdate.DataSource = await context.offices.ToListAsync();
            comboBoxOfficeUpdate.DisplayMember = "Code";
            comboBoxOfficeUpdate.ValueMember = "Code";
        }
    }

    private async void comboBoxOfficeUpdate_SelectedValueChanged(object sender, EventArgs e)
    {

        if (comboBoxOfficeUpdate.SelectedValue != null)
        {

            int officeid;
            if (comboBoxOfficeUpdate.SelectedValue is int)
            {
                officeid = (int)comboBoxOfficeUpdate.SelectedValue;
            }
            else
            {
                officeid = (comboBoxOfficeUpdate.SelectedValue as Office).Code;
            }
            textBoxOfficeUpCity.Text = "";
            textBoxOfficeUpAddress.Text = "";
            textBoxOfficeUpPost.Text = "";
            textBoxOfficeUpPhone.Text = "";

            MyContext context = new();
            Office oldoffice = await context.offices.FirstOrDefaultAsync(o => o.Code == officeid);
            textBoxOfficeUpCity.Text = oldoffice.City.ToString();
            textBoxOfficeUpAddress.Text = oldoffice.Address.ToString();
            textBoxOfficeUpPost.Text = oldoffice.Postalcode.ToString();
            textBoxOfficeUpPhone.Text = oldoffice.Phone.ToString();
        }
    }

    private async void btnEmployeeGet_Click(object sender, EventArgs e)
    {
        NavFrameEmployee.SelectedPage = NavPageEmployeeGet;
        MyContext context = new();
        dataGridViewEmployee.DataSource = await context.employees.ToListAsync();
        dataGridViewEmployee.Columns["office"].Visible = false;
        dataGridViewEmployee.Columns["employee"].Visible = false;
    }

    private async void btnEmployeePost_Click(object sender, EventArgs e)
    {
        NavFrameEmployee.SelectedPage = NavPageEmployeePost;
        MyContext context = new();
        comboBoxEmployeeOffice.DataSource = await context.offices.ToListAsync();
        comboBoxEmployeeOffice.ValueMember = "Code";
        comboBoxEmployeeOffice.DisplayMember = "Code";

        comboBoxEmployeeReportto.DataSource = await context.employees.ToListAsync();
        comboBoxEmployeeReportto.ValueMember = "ID";
        comboBoxEmployeeReportto.DisplayMember = $"FullName";

    }

    private async void buttonEmployeeCreate_Click(object sender, EventArgs e)
    {
        if (comboBoxEmployeeOffice.SelectedValue != null)
        {

            int officeid;
            if (comboBoxEmployeeOffice.SelectedValue is int)
            {
                officeid = (int)comboBoxEmployeeOffice.SelectedValue;
            }
            else
            {
                officeid = (comboBoxEmployeeOffice.SelectedValue as Office).Code;
            }
            MyContext context = new();
            Employee newemp = new Employee();

            newemp.FirstName = textBoxEmployeeFName.Text;
            newemp.LastName = textBoxEmployeeLName.Text;
            newemp.Email = textBoxEmployeeEmail.Text;
            newemp.OfficeCode = officeid;

            if (comboBoxEmployeeReportto.SelectedValue != null)
                newemp.reportsto = Convert.ToInt32(comboBoxEmployeeReportto.SelectedValue);

            await context.AddAsync(newemp);
            await context.SaveChangesAsync();


            textBoxEmployeeFName.Text = "";
            textBoxEmployeeLName.Text = "";
            textBoxEmployeeEmail.Text = "";
            comboBoxEmployeeOffice.DataSource = await context.offices.ToListAsync();
            comboBoxEmployeeReportto.DataSource = await context.employees.ToListAsync();
            MessageBox.Show($"An Employee with id {newemp.ID} is created");
        }
    }

    private async void btnEmployeeUpDel_Click(object sender, EventArgs e)
    {
        NavFrameEmployee.SelectedPage = NavPageEmployeeUpDel;
        MyContext context = new();

        comboBoxEmployeeUpDelMain.DataSource = await context.employees.ToListAsync();
        comboBoxEmployeeUpDelMain.ValueMember = "ID";
        comboBoxEmployeeUpDelMain.DisplayMember = $"FullName";

    }

    private async void comboBoxEmployeeUpDel_SelectedIndexChanged(object sender, EventArgs e)
    {
        FetchEmpsDataUpdateDelete();
    }
    private async void FetchEmpsDataUpdateDelete()
    {
        MyContext context = new();
        textBoxEmpUpDelFName.Text = "";
        textBoxEmpUpDelLNam.Text = "";
        textBoxEmpUpDelEmail.Text = "";

        if (comboBoxEmployeeUpDelMain.SelectedValue != null)
        {

            int selectedid;
            if (comboBoxEmployeeUpDelMain.SelectedValue is int)
            {
                selectedid = (int)comboBoxEmployeeUpDelMain.SelectedValue;
            }
            else
            {
                selectedid = (comboBoxEmployeeUpDelMain.SelectedValue as Employee).ID;
            }


            Employee selectedemp = await context.employees.FirstOrDefaultAsync(e => e.ID == selectedid);

            if (selectedemp != null)
            {
                textBoxEmpUpDelFName.Text = selectedemp.FirstName;
                textBoxEmpUpDelLNam.Text = selectedemp.LastName;
                textBoxEmpUpDelEmail.Text = selectedemp.Email;

                comboBoxEmpUpDelOffice.DataSource = null;
                comboBoxEmpUpDelOffice.DataSource = await context.offices.ToListAsync();

                Employee emptyemp = new Employee()
                {
                    ID = 0,
                    FirstName = "No One"
                };

                List<Employee> EmployeesList = new List<Employee> { emptyemp };
                EmployeesList.AddRange(await context.employees.Where(e => e.ID != selectedid).ToListAsync());


                comboBoxEmpUpDelReportto.DataSource = null;
                comboBoxEmpUpDelReportto.DataSource = EmployeesList;


                comboBoxEmpUpDelOffice.DisplayMember = "Code";
                comboBoxEmpUpDelOffice.ValueMember = "Code";
                comboBoxEmpUpDelOffice.SelectedValue = selectedemp.OfficeCode;

                comboBoxEmpUpDelReportto.DisplayMember = "FullName";
                comboBoxEmpUpDelReportto.ValueMember = "ID";
                comboBoxEmpUpDelReportto.SelectedValue = selectedemp.reportsto ?? 0;
            }

        }


        int selectedrepemp = 0;
        if (comboBoxEmployeeReportto.SelectedValue is int)
        {
            selectedrepemp = (int)comboBoxEmployeeReportto.SelectedValue;
        }
        else
        {
            Employee temp = comboBoxEmployeeReportto.SelectedValue as Employee;
            if (temp != null)
                selectedrepemp = (comboBoxEmployeeReportto.SelectedValue as Employee).ID;
        }

        Employee selectedReportemp = await context.employees.FirstOrDefaultAsync(e => e.ID == selectedrepemp);

        if (selectedReportemp != null)
        {
            comboBoxEmpUpDelReportto.SelectedValue = selectedReportemp;
        }

    }
    private async void btnEmpUpdate_Click(object sender, EventArgs e)
    {
        if (comboBoxEmployeeUpDelMain.SelectedValue != null)
        {

            int empid;
            if (comboBoxEmployeeUpDelMain.SelectedValue is int)
            {
                empid = (int)comboBoxEmployeeUpDelMain.SelectedValue;
            }
            else
            {
                empid = (comboBoxEmployeeUpDelMain.SelectedValue as Employee).ID;
            }

            MyContext context = new();
            Employee emp = await context.employees.FirstOrDefaultAsync(e => e.ID == empid);
            int reptoid = Convert.ToInt32(comboBoxEmpUpDelReportto.SelectedValue);
            if (reptoid == 0)
            {
                emp.reportsto = null;
            }
            else
            {
                emp.reportsto = reptoid;
            }
            emp.OfficeCode = Convert.ToInt32(comboBoxEmpUpDelOffice.SelectedValue);
            emp.FirstName = textBoxEmpUpDelFName.Text;
            emp.LastName = textBoxEmpUpDelLNam.Text;
            emp.Email = textBoxEmpUpDelEmail.Text;
            await context.SaveChangesAsync();

            comboBoxEmployeeUpDelMain.DataSource = await context.employees.ToListAsync();
            comboBoxEmployeeUpDelMain.ValueMember = "ID";
            comboBoxEmployeeUpDelMain.DisplayMember = $"FullName";
        }
    }

    private async void btnEmpDelete_Click(object sender, EventArgs e)
    {

        MyContext context = new();
        comboBoxEmployeeUpDelMain.DataSource = null;
        comboBoxEmployeeUpDelMain.DataSource = await context.employees.ToListAsync();
        comboBoxEmployeeUpDelMain.ValueMember = "ID";
        comboBoxEmployeeUpDelMain.DisplayMember = $"FullName";

        if (comboBoxEmployeeUpDelMain.SelectedValue != null)
        {
            int empid;
            if (comboBoxEmployeeUpDelMain.SelectedValue is int)
            {
                empid = (int)comboBoxEmployeeUpDelMain.SelectedValue;
            }
            else
            {
                empid = (comboBoxEmployeeUpDelMain.SelectedValue as Employee).ID;
            }
            Employee emp = await context.employees.FirstOrDefaultAsync(e => e.ID == empid);

            await context.employees.Where(e => e.reportsto == emp.ID).ForEachAsync(e =>
            {
                e.reportsto = null;
            });
            context.Remove(emp);
            await context.SaveChangesAsync();
            FetchEmpsDataUpdateDelete();
        }
    }

    private async void btnCusomerGet_Click(object sender, EventArgs e)
    {
        NavFrameCustomer.SelectedPage = navPageCustomerGet;
        MyContext context = new();
        dataGridViewCustomers.DataSource = await context.customers.ToListAsync();
    }

    private async void btnCusomerPost_Click(object sender, EventArgs e)
    {
        MyContext context = new();

        NavFrameCustomer.SelectedPage = navPageCustomerPost;
        comboBoxCustomerReportedEmp.DataSource = await context.employees.ToListAsync();
        comboBoxCustomerReportedEmp.DisplayMember = "FullName";
        comboBoxCustomerReportedEmp.ValueMember = "ID";
    }

    private async void btnCusomerUpDel_Click(object sender, EventArgs e)
    {
        NavFrameCustomer.SelectedPage = navPageCustomerUpDel;
        FetchCustomers();
    }

    private async void btnCustomerCreate_Click(object sender, EventArgs e)
    {
        MyContext context = new();
        int repemp = Convert.ToInt32(comboBoxCustomerReportedEmp.SelectedValue);
        if (repemp > 0)
        {
            Customer newCustomer = new()
            {
                FirstName = textBoxCustomersFName.Text,
                LastName = textBoxCustomersLName.Text,
                City = textBoxCustomersCity.Text,
                Phone = textBoxCustomersPhone.Text,
                SalesRepEmployeeNum = repemp
            };
            await context.customers.AddAsync(newCustomer);
            await context.SaveChangesAsync();
            textBoxCustomersFName.Text = "";
            textBoxCustomersLName.Text = "";
            textBoxCustomersCity.Text = "";
            textBoxCustomersPhone.Text = "";
            MessageBox.Show($"Employee with id {newCustomer.ID} is created");
        }
        else
        {
            MessageBox.Show("There is No Employees to sign it with");
        }
    }

    private async void dataGridViewEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridViewEmployee.CurrentRow != null)
        {
            Employee employee = dataGridViewEmployee.CurrentRow.DataBoundItem as Employee;
            MyContext context = new();
            dataGridViewEmployeesCustomers.DataSource = await context.customers.Where(c => c.SalesRepEmployeeNum == employee.ID).ToListAsync();

        }
    }

    private async void dataGridViewCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridViewCustomers.CurrentRow != null)
        {
            Customer customer = dataGridViewCustomers.CurrentRow.DataBoundItem as Customer;
            MyContext context = new();
            dataGridViewCustomerOrder.DataSource = await context.orders.Where(o => o.CustomerID == customer.ID).ToListAsync();

        }
    }

    private async void comboBoxCustomerUpDelMain_SelectedIndexChanged(object sender, EventArgs e)
    {


        int customid = 0;
        if (comboBoxCustomerUpDelMain.SelectedValue is int)
        {
            customid = (int)comboBoxCustomerUpDelMain.SelectedValue;
        }
        else
        {
            customid = (comboBoxCustomerUpDelMain.SelectedValue as Customer).ID;
        }

        if (customid != 0)
        {

            MyContext context = new();
            Customer customer = await context.customers.FirstOrDefaultAsync(c => c.ID == customid);
            textBoxCustomerUpDelFName.Text = customer.FirstName;
            textBoxCustomerUpDelLName.Text = customer.LastName;
            textBoxCustomerUpDelPhone.Text = customer.Phone;
            textBoxCustomersUpDelCity.Text = customer.City;
            comboBoxCutomerUpdelRepEmp.SelectedValue = customer.SalesRepEmployeeNum;

        }
    }

    private async void btnCustomerUpdate_Click(object sender, EventArgs e)
    {
        if (comboBoxCustomerUpDelMain.SelectedValue != null)
        {

            int customid;
            if (comboBoxCustomerUpDelMain.SelectedValue is int)
            {
                customid = (int)comboBoxCustomerUpDelMain.SelectedValue;
            }
            else
            {
                customid = (comboBoxCustomerUpDelMain.SelectedValue as Customer).ID;
            }

            MyContext context = new();

            Customer customer = await context.customers.FirstOrDefaultAsync(c => c.ID == customid);
            customer.FirstName = textBoxCustomerUpDelFName.Text;
            customer.LastName = textBoxCustomerUpDelLName.Text;
            customer.Phone = textBoxCustomerUpDelPhone.Text;
            customer.City = textBoxCustomersUpDelCity.Text;
            customer.SalesRepEmployeeNum = (int)comboBoxCutomerUpdelRepEmp.SelectedValue;

            await context.SaveChangesAsync();

            MessageBox.Show($"Customer Updated!");
            FetchCustomers();
        }
    }
    public async void FetchCustomers()
    {
        textBoxCustomerUpDelFName.Text = string.Empty;
        textBoxCustomerUpDelLName.Text = string.Empty;
        textBoxCustomerUpDelPhone.Text = string.Empty;
        textBoxCustomersUpDelCity.Text = string.Empty;

        MyContext context = new();
        comboBoxCustomerUpDelMain.DataSource = await context.customers.ToListAsync();
        comboBoxCustomerUpDelMain.DisplayMember = "FullName";
        comboBoxCustomerUpDelMain.ValueMember = "ID";

        comboBoxCutomerUpdelRepEmp.DataSource = await context.employees.ToListAsync();
        comboBoxCutomerUpdelRepEmp.DisplayMember = "FullName";
        comboBoxCutomerUpdelRepEmp.ValueMember = "ID";
    }

    private async void btnCategoryGet_Click(object sender, EventArgs e)
    {
        NavFrameCategory.SelectedPage = NavPageCategoryGet;
        MyContext context = new();
        dataGridViewCategory.DataSource = await context.Category.ToListAsync();
    }

    private async void dataGridViewCategory_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridViewCategory.CurrentRow != null)
        {
            Category category = dataGridViewCategory.CurrentRow.DataBoundItem as Category;
            MyContext context = new();
            List<Product> products = await context.products.Where(p => p.Categoryid == category.ID).ToListAsync();
            dataGridViewCategoryProducts.DataSource = products;
            //dataGridViewOfficeGetEmps.Columns["office"].Visible = false;
            //dataGridViewOfficeGetEmps.Columns["employee"].Visible = false;
        }
    }

    private async void btnCategoryCreate_Click(object sender, EventArgs e)
    {
        MyContext context = new();

        Category category = new()
        {
            Name = textBoxCategoryName.Text,
            image = textBoxCategoryImage.Text
        };
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        textBoxCategoryName.Text = "";
        textBoxCategoryImage.Text = "";

        MessageBox.Show("Category is created");
    }

    private void btnCategoryPost_Click(object sender, EventArgs e)
    {
        NavFrameCategory.SelectedPage = NavPageCategoryPost;
    }

    private async void btnCategoryUpDel_Click(object sender, EventArgs e)
    {
        MyContext context = new();
        NavFrameCategory.SelectedPage = NavPageCategoryUpDel;
        comboBoxCategoryUpDelMain.DataSource = await context.Category.ToListAsync();
        comboBoxCategoryUpDelMain.ValueMember = "ID";
        comboBoxCategoryUpDelMain.DisplayMember = "Name";
    }

    private async void comboBoxCategoryUpDelMain_SelectedIndexChanged(object sender, EventArgs e)
    {

        int categoryid;
        if (comboBoxCategoryUpDelMain.SelectedValue is int)
        {
            categoryid = (int)comboBoxCategoryUpDelMain.SelectedValue;
        }
        else
        {
            categoryid = (comboBoxCategoryUpDelMain.SelectedValue as Category).ID;
        }

        MyContext context = new();
        Category category = await context.Category.FirstOrDefaultAsync(c => c.ID == categoryid);

        TextboxCategoryUpDelName.Text = category.Name;
        TextboxCategoryUpDelimage.Text = category.image;
    }

    private async void btnCategoryUpdate_Click(object sender, EventArgs e)
    {
        if (comboBoxCategoryUpDelMain.SelectedValue != null)
        {

            int categoryid;
            if (comboBoxCategoryUpDelMain.SelectedValue is int)
            {
                categoryid = (int)comboBoxCategoryUpDelMain.SelectedValue;
            }
            else
            {
                categoryid = (comboBoxCategoryUpDelMain.SelectedValue as Category).ID;
            }
            if (categoryid > 0)
            {
                MyContext context = new();
                Category category = await context.Category.FirstOrDefaultAsync(c => c.ID == categoryid);

                category.Name = TextboxCategoryUpDelName.Text;
                category.image = TextboxCategoryUpDelimage.Text;
                MessageBox.Show("Category updated");
                await context.SaveChangesAsync();
                FetchCateoryData();
            }
        }
    }

    private async void btnCategoryDelete_Click(object sender, EventArgs e)
    {
        if (comboBoxCategoryUpDelMain.SelectedValue != null)
        {
            int categoryid;
            if (comboBoxCategoryUpDelMain.SelectedValue is int)
            {
                categoryid = (int)comboBoxCategoryUpDelMain.SelectedValue;
            }
            else
            {
                categoryid = (comboBoxCategoryUpDelMain.SelectedValue as Category).ID;
            }
            if (categoryid != 0 || categoryid != null)
            {
                MyContext context = new();
                Category category = await context.Category.FirstOrDefaultAsync(c => c.ID == categoryid);
                context.Category.Remove(category);
                await context.SaveChangesAsync();
                MessageBox.Show("Category Deleted");
                FetchCateoryData();
            }
        }
    }
    private async void FetchCateoryData()
    {
        MyContext context = new();
        TextboxCategoryUpDelName.Text = "";
        TextboxCategoryUpDelimage.Text = "";
        comboBoxCategoryUpDelMain.DataSource = await context.Category.ToListAsync();
    }

    private async void button4_Click(object sender, EventArgs e)
    {
        NavFrameProducts.SelectedPage = NavPageProductGet;
        MyContext context = new();
        comboBoxProductCategoryGet.DataSource = await context.Category.ToListAsync();
        comboBoxProductCategoryGet.DisplayMember = "Name";
        comboBoxProductCategoryGet.ValueMember = "ID";
    }

    private void btnProductPost_Click(object sender, EventArgs e)
    {
        NavFrameProducts.SelectedPage = NavPageProductPost;
        ResetProductCreate();
    }

    private async void btnProductCreate_Click(object sender, EventArgs e)
    {
        MyContext context = new();

        if (comboBoxPrdouctCreateCategory.SelectedValue != null)
        {

            int categoryid;
            if (comboBoxPrdouctCreateCategory.SelectedValue is int)
            {
                categoryid = (int)comboBoxPrdouctCreateCategory.SelectedValue;
            }
            else
            {
                categoryid = (comboBoxPrdouctCreateCategory.SelectedValue as Category).ID;
            }

            Product product = new()
            {
                Name = textBoxProductCreateName.Text,
                PdtDescription = textBoxProductCreateDesc.Text,
                BuyPrice = Decimal.TryParse(textBoxProductCreatePrice.Text, out decimal price) ? price : null,
                QtyInStock = int.TryParse(textBoxProductCreateQunt.Text, out int qunt) ? qunt : null,
                Categoryid = categoryid
            };
            await context.products.AddAsync(product);
            await context.SaveChangesAsync();
            MessageBox.Show("Product created");
            ResetProductCreate();
        }
    }
    private async void ResetProductCreate()
    {
        MyContext context = new();
        textBoxProductCreateName.Text = "";
        textBoxProductCreateDesc.Text = "";
        textBoxProductCreatePrice.Text = "";
        textBoxProductCreateQunt.Text = "";

        comboBoxPrdouctCreateCategory.DataSource = await context.Category.ToListAsync();
        comboBoxPrdouctCreateCategory.DisplayMember = "Name";
        comboBoxPrdouctCreateCategory.ValueMember = "ID";
    }

    private async void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        MyContext context = new();

        if (comboBoxProductCategoryGet.SelectedValue != null)
        {
            int categoryid;
            if (comboBoxProductCategoryGet.SelectedValue is int)
            {
                categoryid = (int)comboBoxProductCategoryGet.SelectedValue;
            }
            else
            {
                categoryid = (comboBoxProductCategoryGet.SelectedValue as Category).ID;
            }

            dataGridViewProducts.DataSource = await context.products.Where(p => p.Categoryid == categoryid).ToListAsync();
        }
    }

    private async void textBox5_TextChanged(object sender, EventArgs e)
    {
        if (comboBoxProductCategoryGet.SelectedValue != null)
        {
            int categoryid;
            if (comboBoxProductCategoryGet.SelectedValue is int)
            {
                categoryid = (int)comboBoxProductCategoryGet.SelectedValue;
            }
            else
            {
                categoryid = (comboBoxProductCategoryGet.SelectedValue as Category).ID;
            }
            MyContext context = new();
            string keyword = textBoxSearchProduct.Text;
            dataGridViewProducts.DataSource = await context.products
                .Where(p => p.Categoryid == categoryid && p.Name.Contains(keyword)).ToListAsync();
        }
    }

    private async void btnProductUpDel_Click(object sender, EventArgs e)
    {
        MyContext context = new();
        NavFrameProducts.SelectedPage = NavPageProductUpDel;
        comboBoxProductUpDel.DataSource = null;
        comboBoxProductUpDel.DataSource = await context.products.ToListAsync();
        comboBoxProductUpDel.DisplayMember = "Name";
        comboBoxProductUpDel.ValueMember = "Code";

        comboBoxProductCategoryUpDel.DataSource = null;
        comboBoxProductCategoryUpDel.DataSource = await context.Category.ToListAsync();
        comboBoxProductCategoryUpDel.DisplayMember = "Name";
        comboBoxProductCategoryUpDel.ValueMember = "ID";
    }

    private async void comboBoxProductUpDel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxProductUpDel.SelectedValue != null)
        {
            int productid;
            if (comboBoxProductUpDel.SelectedValue is int)
            {
                productid = (int)comboBoxProductUpDel.SelectedValue;
            }
            else
            {
                productid = (comboBoxProductUpDel.SelectedValue as Product).Code;
            }
            MyContext context = new();
            Product product = await context.products.FirstOrDefaultAsync(p => p.Code == productid);

            textBoxProductUpDelName.Text = "";
            textBoxProductUpDelDesc.Text = "";
            textBoxProductUpDelPrice.Text ="";
            textBoxProductUpDelQunt.Text = "";

            textBoxProductUpDelName.Text = product.Name;
            textBoxProductUpDelDesc.Text = product.PdtDescription;
            textBoxProductUpDelPrice.Text = product.BuyPrice.ToString();
            textBoxProductUpDelQunt.Text = product.QtyInStock.ToString();
            comboBoxProductCategoryUpDel.SelectedValue = product.Categoryid;
        }

    }

    private async void button8_Click(object sender, EventArgs e)
    {
        if (comboBoxProductUpDel.SelectedValue != null)
        {
            int productid;
            if (comboBoxProductUpDel.SelectedValue is int)
            {
                productid = (int)comboBoxProductUpDel.SelectedValue;
            }
            else
            {
                productid = (comboBoxProductUpDel.SelectedValue as Product).Code;
            }

            int categoryid;
            if (comboBoxProductCategoryUpDel.SelectedValue is int)
            {
                categoryid = (int)comboBoxProductCategoryUpDel.SelectedValue;
            }
            else
            {
                categoryid = (comboBoxProductCategoryUpDel.SelectedValue as Product).Code;
            }
            MyContext context = new();
            Product product = await context.products.FirstOrDefaultAsync(p => p.Code == productid);
            product.Name = textBoxProductUpDelName.Text;
            product.PdtDescription = textBoxProductUpDelDesc.Text;
            product.BuyPrice = Decimal.TryParse(textBoxProductUpDelPrice.Text, out decimal price) ? price : null;
            product.QtyInStock = int.TryParse(textBoxProductUpDelQunt.Text, out int qunt) ? qunt : null;
            product.Categoryid = categoryid;
            await context.SaveChangesAsync();
            MessageBox.Show("Product Updated");
            NavFrameProducts.SelectedPage = NavPageProductUpDel;
            comboBoxProductUpDel.DataSource = null;
            comboBoxProductUpDel.DataSource = await context.products.ToListAsync();
            comboBoxProductUpDel.DisplayMember = "Name";
            comboBoxProductUpDel.ValueMember = "Code";

            comboBoxProductCategoryUpDel.DataSource = null;
            comboBoxProductCategoryUpDel.DataSource = await context.Category.ToListAsync();
            comboBoxProductCategoryUpDel.DisplayMember = "Name";
            comboBoxProductCategoryUpDel.ValueMember = "ID";

        }

    }

 
    private async void btnPlace_Click(object sender, EventArgs e)
    {
        NavFrameOrders.SelectedPage = NavPagePlaceOrder;
        MyContext context = new();
        comboBoxOrderchooseCustomerPlace.DataSource = await context.customers.ToListAsync();
        comboBoxOrderchooseCustomerPlace.DisplayMember = "FullName";
        comboBoxOrderchooseCustomerPlace.ValueMember = "ID";
    }


    private async void comboBoxChooseProduct_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (comboBoxChooseProduct.SelectedValue != null)
        {
            int productid;
            if (comboBoxChooseProduct.SelectedValue is int)
            {
                productid = (int)comboBoxChooseProduct.SelectedValue;
            }
            else
            {
                productid = (comboBoxChooseProduct.SelectedValue as Product).Code;
            }
            MyContext context = new();
            Product Product = await context.products.FirstOrDefaultAsync(p => p.Code == productid);
            comboBoxchooseQuantity.DataSource = await context.customers.ToListAsync();

            if (Product.QtyInStock > 0)
            {

                List<int> nums = new();
                for (int i = 1; i <= Product.QtyInStock; i++)
                {
                    nums.Add(i);
                }
                comboBoxchooseQuantity.DataSource = nums;
            }
            else
            {
                comboBoxchooseQuantity.DataSource = null;
            }
        }
    }

    private async void BtnAddToCart_Click(object sender, EventArgs e)
    {
        if (comboBoxChooseProduct.SelectedValue != null)
        {
            int productid;
            if (comboBoxChooseProduct.SelectedValue is int)
            {
                productid = (int)comboBoxChooseProduct.SelectedValue;
            }
            else
            {
                productid = (comboBoxChooseProduct.SelectedValue as Product).Code;
            }


            MyContext context = new();
            Product Product = await context.products.FirstOrDefaultAsync(p => p.Code == productid);


            if (carts.Keys.Any(p => p.Code == Product.Code))
            {
                Product existedproduct = carts.Keys.FirstOrDefault(p => p.Code == Product.Code);
                carts[existedproduct] = (int)comboBoxchooseQuantity.SelectedValue; // Update quantity
            }
            else
            {
                carts.Add(Product, (int)comboBoxchooseQuantity.SelectedValue);
            }


            OrderFetchData();
            comboBoxCart.DataSource = null;
            comboBoxCart.DataSource = carts.Keys.ToList();
            comboBoxCart.DisplayMember = "Name";
            comboBoxCart.ValueMember = "Code";

            comboBoxCartQuantity.DataSource = null;
            comboBoxCartQuantity.DataSource = carts.Values.ToList();
            CalcTotalPrice();
        }
    }
    private void CalcTotalPrice()
    {
        decimal Tot = 0;
        foreach (var item in carts)
        {
            decimal.TryParse(item.Key.BuyPrice.ToString(), out decimal parsetotal);
            Tot += (parsetotal * item.Value);
        }
        label84.Text = Tot.ToString();

    }
    private void buttonRemoveFromCart_Click(object sender, EventArgs e)
    {
        if (comboBoxCart.SelectedValue == null)
        {
            MessageBox.Show("Your Cart is Empty");
            return;
        }
        int productid;
        if (comboBoxCart.SelectedValue is int)
        {
            productid = (int)comboBoxCart.SelectedValue;
        }
        else
        {
            productid = (comboBoxCart.SelectedValue as Product).Code;
        }


        if (carts.Keys.Any(p => p.Code == productid))
        {
            Product existedproduct = carts.Keys.FirstOrDefault(p => p.Code == productid);
            carts.Remove(existedproduct);
        }
        else
        {
            MessageBox.Show("This product is not exeist");
        }
        OrderFetchData();

        comboBoxCart.DataSource = null;
        comboBoxCart.DataSource = carts.Keys.ToList();
        comboBoxCart.DisplayMember = "Name";
        comboBoxCart.ValueMember = "Code";

        comboBoxCartQuantity.DataSource = null;
        comboBoxCartQuantity.DataSource = carts.Values.ToList();
        CalcTotalPrice();
    }

    private void comboBoxCart_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxCart.SelectedValue == null) return;

        int productid;
        if (comboBoxCart.SelectedValue is int)
        {
            productid = (int)comboBoxCart.SelectedValue;
        }
        else
        {
            productid = (comboBoxCart.SelectedValue as Product).Code;
        }

        Product product = carts.Keys.FirstOrDefault(x => x.Code == productid);
        comboBoxCartQuantity.SelectedItem = carts[product];

    }

    private async void PlaceOrder_Click(object sender, EventArgs e)
    {
        if (comboBoxOrderchooseCustomerPlace.SelectedValue == null || carts.Count <= 0) return;

        int CustomerID;
        if (comboBoxOrderchooseCustomerPlace.SelectedValue is int)
        {
            CustomerID = (int)comboBoxOrderchooseCustomerPlace.SelectedValue;
        }
        else
        {
            CustomerID = (comboBoxOrderchooseCustomerPlace.SelectedValue as Customer).ID;
        }
        MyContext context = new();
        Order NewOrder = new()
        {
            CustomerID = CustomerID,
            Status = "Pending"
        };

        var createdOrder = await context.orders.AddAsync(NewOrder);
        await context.SaveChangesAsync();

        foreach (var item in carts)
        {
            Order_Product orderproduct = new()
            {
                OrderID = createdOrder.Entity.ID,
                ProductCode = item.Key.Code,
                Qty = item.Value,
                PriceEach = item.Key.BuyPrice

            };
            await context.Order_Product.AddAsync(orderproduct);
            Product product = await context.products.FirstOrDefaultAsync(p => p.Code == item.Key.Code);
            product.QtyInStock -= item.Value;
        }
        await context.SaveChangesAsync();
        carts.Clear();
        comboBoxCart.DataSource = null;
        comboBoxCart.DataSource = carts.Keys.ToList();
        comboBoxCart.DisplayMember = "Name";
        comboBoxCart.ValueMember = "Code";

        comboBoxCartQuantity.DataSource = null;
        comboBoxCartQuantity.DataSource = carts.Values.ToList();
        OrderFetchData();
        CalcTotalPrice();
    }

    private async void button3_Click(object sender, EventArgs e)
    {
        NavFrameOrders.SelectedPage = NavPageViewOrders;
        MyContext context = new();
        comboBoxOrderViewCustomers.DataSource = await context.customers.ToListAsync();
    }

    private async void comboBoxOrderViewCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxOrderViewCustomers.SelectedValue == null) return;

        int CustomerID;
        if (comboBoxOrderViewCustomers.SelectedValue is int)
        {
            CustomerID = (int)comboBoxOrderViewCustomers.SelectedValue;
        }
        else
        {
            CustomerID = (comboBoxOrderViewCustomers.SelectedValue as Customer).ID;
        }
        MyContext context = new();
        dataGridViewOrdersView.DataSource = await context.orders.Where(o => o.CustomerID == CustomerID).ToListAsync();
    }

    private async void dataGridViewOrdersView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridViewOrdersView.CurrentRow != null)
        {
            Order order = dataGridViewOrdersView.CurrentRow.DataBoundItem as Order;
            MyContext context = new();

            var orderproducts = await context.Order_Product
                .Where(op => op.OrderID == order.ID).Include(op => op.product)
                .Select(op => new
                {
                    ProductName = op.product.Name,
                    ProductPrice = op.product.BuyPrice,
                    paidQuantity = op.Qty
                }).ToListAsync();

            dataGridViewViewOrderProducts.DataSource = orderproducts;

        }
    }

    private async void button4_Click_1(object sender, EventArgs e)
    {
        NavFramePayment.SelectedPage = NavPageCheckout;
    }

    private async void comboBoxPaymentViewCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxPaymentViewCustomer.SelectedValue == null)
        {
            comboBoxPaymentViewOrder.DataSource = null;
            return;
        };

        int CustomerID;
        if (comboBoxPaymentViewCustomer.SelectedValue is int)
        {
            CustomerID = (int)comboBoxPaymentViewCustomer.SelectedValue;
        }
        else
        {
            CustomerID = (comboBoxPaymentViewCustomer.SelectedValue as Customer).ID;
        }
        MyContext context = new();
        comboBoxPaymentViewOrder.DataSource = await context.orders
            .Where(o => o.CustomerID == CustomerID && o.Status == "pending").ToListAsync();
        comboBoxPaymentViewOrder.DisplayMember = "ID";
        comboBoxPaymentViewOrder.ValueMember = "ID";
    }

    private async void buttonCheckout_Click(object sender, EventArgs e)
    {
        if (comboBoxPaymentViewOrder.SelectedValue == null)
        {
            MessageBox.Show("There is No Orders");
            return;
        };

        int Orderid;
        if (comboBoxPaymentViewOrder.SelectedValue is int)
        {
            Orderid = (int)comboBoxPaymentViewOrder.SelectedValue;
        }
        else
        {
            Orderid = (comboBoxPaymentViewOrder.SelectedValue as Order).ID;
        }

        MyContext context = new();
        Order order = await context.orders.FirstOrDefaultAsync(o => o.ID == Orderid);
        order.Status = "Done";

        var productsorders = await context.Order_Product.Where(op => op.OrderID == order.ID).ToListAsync();
        decimal Total = 0;

        foreach (var item in productsorders)
        {
            Total += Convert.ToDecimal(item.Qty * item.PriceEach);
        }

        Payment payment = new()
        {
            CustomerID = order.CustomerID,
            Amount = Total
        };
        await context.payment.AddAsync(payment);
        await context.SaveChangesAsync();

        comboBoxPaymentViewCustomer.DataSource = null;
        comboBoxPaymentViewCustomer.DataSource = await context.customers.ToListAsync();
        comboBoxPaymentViewCustomer.DisplayMember = "FullName";
        comboBoxPaymentViewCustomer.ValueMember = "ID";
        MessageBox.Show("Transaction done successfully");
    }


    private async void button1_Click(object sender, EventArgs e)
    {
        NavFramePayment.SelectedPage = NavPagePaymentView;
        MyContext context = new();
        comboBoxPaymentViewPageCustomers.DataSource = await context.customers.ToListAsync();
        comboBoxPaymentViewPageCustomers.ValueMember = "ID";
        comboBoxPaymentViewPageCustomers.DisplayMember = "FullName";

    }

    private async void comboBoxPaymentViewPageCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxPaymentViewPageCustomers.SelectedValue == null) return;

        int CustomerID;
        if (comboBoxPaymentViewPageCustomers.SelectedValue is int)
        {
            CustomerID = (int)comboBoxPaymentViewPageCustomers.SelectedValue;
        }
        else
        {
            CustomerID = (comboBoxPaymentViewPageCustomers.SelectedValue as Customer).ID;
        }
        MyContext context = new();
        dataGridViewTransactions.DataSource = await context.payment.Where(p => p.CustomerID == CustomerID).ToListAsync();
    }

    private void comboBoxOfficeUpdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        textBoxOfficeUpCity.Text = "";
        textBoxOfficeUpAddress.Text = "";
        textBoxOfficeUpPost.Text = "";
        textBoxOfficeUpPhone.Text = "";
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        if (comboBoxCustomerUpDelMain.SelectedValue != null)
        {
            int customerid;
            if (comboBoxCustomerUpDelMain.SelectedValue is int)
            {
                customerid = (int)comboBoxCustomerUpDelMain.SelectedValue;
            }
            else
            {
                customerid = (comboBoxCustomerUpDelMain.SelectedValue as Customer).ID;
            }
            if (customerid != 0 || customerid != null)
            {
                MyContext context = new();
                Customer customer = await context.customers.FirstOrDefaultAsync(c => c.ID == customerid);
                context.customers.Remove(customer);
                await context.SaveChangesAsync();
                FetchCustomers();

                MessageBox.Show("Customer Deleted");
            }
        }
    }

    private async void button7_Click(object sender, EventArgs e)
    {
        if (comboBoxProductUpDel.SelectedValue != null)
        {
            int productid;
            if (comboBoxProductUpDel.SelectedValue is int)
            {
                productid = (int)comboBoxProductUpDel.SelectedValue;
            }
            else
            {
                productid = (comboBoxProductUpDel.SelectedValue as Product).Code;
            }
            if (productid != 0 || productid != null)
            {
                MyContext context = new();
                Product product = await context.products.FirstOrDefaultAsync(c => c.Code == productid);
                context.products.Remove(product);
                await context.SaveChangesAsync();
                textBoxProductUpDelName.Text = "";
                textBoxProductUpDelDesc.Text = "";
                textBoxProductUpDelPrice.Text = "";
                textBoxProductUpDelQunt.Text = "";
                FetchCustomers();

                comboBoxProductUpDel.DataSource = null;
                comboBoxProductUpDel.DataSource = await context.products.ToListAsync();
                comboBoxProductUpDel.DisplayMember = "Name";
                comboBoxProductUpDel.ValueMember = "Code";

                comboBoxProductCategoryUpDel.DataSource = null;
                comboBoxProductCategoryUpDel.DataSource = await context.Category.ToListAsync();
                comboBoxProductCategoryUpDel.DisplayMember = "Name";
                comboBoxProductCategoryUpDel.ValueMember = "ID";

                MessageBox.Show("Product Deleted");

            }
        }
    }
}
