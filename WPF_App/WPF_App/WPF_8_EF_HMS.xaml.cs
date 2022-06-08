using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_App
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_8_EF_HMS.xaml
    /// </summary>
    public partial class WPF_8_EF_HMS : Window
    {
        public WPF_8_EF_HMS()
        {
            InitializeComponent();
            HospitalDBEntities db = new HospitalDBEntities();
            var docs = from d in db.Doctors
                       select new
                       {
                           DoctorName = d.Name,
                           Speciality = d.Specialization
                       };

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.DoctorName);
                Console.WriteLine(doc.Speciality);
            }

            this.gridDoctors.ItemsSource = docs.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            HospitalDBEntities db = new HospitalDBEntities();

            Doctor doctorObject = new Doctor()
            {
                Name = txtName.Text,
                Specialization = txtSpecialization.Text,
                Qualification = txtQualification.Text
            };

            db.Doctors.Add(doctorObject);
            db.SaveChanges();
        }

        private void BtnLoadDoctors_Click(object sender, RoutedEventArgs e)
        {
            HospitalDBEntities db = new HospitalDBEntities();

            this.gridDoctors.ItemsSource = db.Doctors.ToList();
        }
        private void BtnUpdateDoctors_Click(object sender, RoutedEventArgs e)
        {
            HospitalDBEntities db = new HospitalDBEntities();

            var r = from d in db.Doctors
                    where d.Id == this.updatingDoctorID
                    select d;

            Doctor obj = r.SingleOrDefault();

            if (obj != null)
            {
                obj.Name = this.txtName2.Text;
                obj.Specialization = this.txtSpecialization2.Text;
                obj.Qualification = this.txtQualification2.Text;

                db.SaveChanges();
            }
        }

        private int updatingDoctorID = 0;
        private void gridDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridDoctors.SelectedIndex >= 0)
            {
                if (this.gridDoctors.SelectedItems.Count >= 0)
                {
                    if (this.gridDoctors.SelectedItems[0].GetType() == typeof(Doctor))
                    {
                        Doctor doctor = (Doctor)this.gridDoctors.SelectedItems[0];

                        this.txtName2.Text = doctor.Name;
                        this.txtSpecialization2.Text = doctor.Specialization;
                        this.txtQualification2.Text = doctor.Qualification;
                        this.updatingDoctorID = doctor.Id;
                    }
                }
            }
        }

        private void BtnDeleteDoctors_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgBoxResult = MessageBox.Show("Are you sure you want to remove this doctor?",
                "The doctor has been removed",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
                );

            if (msgBoxResult == MessageBoxResult.Yes)
            {
                HospitalDBEntities db = new HospitalDBEntities();

                var r = from d in db.Doctors
                        where d.Id == this.updatingDoctorID
                        select d;

                Doctor obj = r.SingleOrDefault();

                if (obj != null)
                {
                    db.Doctors.Remove(obj);

                    db.SaveChanges();
                }
            }
        }
    }
}
