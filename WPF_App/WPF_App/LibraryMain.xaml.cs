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
    /// Logika interakcji dla klasy LibraryMain.xaml
    /// </summary>
    public partial class LibraryMain : Window
    {
        public LibraryMain()
        {
            InitializeComponent();

            LibraryDBEntities db = new LibraryDBEntities();
            //Wyswietla poczatkowy wyglad bazy (3 kolumny)
            var lib = from d in db.Libraries
                       select new
                       {
                           Title = d.Title,
                           Author = d.Author,
                           NumberOfPages = d.NumberOfPages,
                           //Borrower = d.Borrower,
                           //DateOfBorrow = d.DateOfBorrow,
                       };

            foreach (var item in lib)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Author);
                Console.WriteLine(item.NumberOfPages);
                //Console.WriteLine(item.Borrower);
                //Console.WriteLine(item.DateOfBorrow);
            }

            this.gridLibraries.ItemsSource = lib.ToList();
        }

        //Dodaje dane do bazy
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            LibraryDBEntities db = new LibraryDBEntities();

            Library libraryObject = new Library()
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                NumberOfPages = Convert.ToInt32(txtNumberOfPages.Text),
                Borrower = txtBorrower.Text,
                DateOfBorrow = Convert.ToDateTime(txtDateOfBorrow.Text)
            };

            db.Libraries.Add(libraryObject);
            db.SaveChanges();
            BtnLoad_Click(sender, e);
        }

        //Laduje zawartosc calej bazy do tabeli
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            LibraryDBEntities db = new LibraryDBEntities();

            this.gridLibraries.ItemsSource = db.Libraries.ToList();
        }

        //Modyfikuje dane w bazie
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            LibraryDBEntities db = new LibraryDBEntities();

            var r = from d in db.Libraries
                    where d.Id == this.updatingLibraryID
                    select d;

            Library obj = r.SingleOrDefault();

            if (obj != null)
            {
                obj.Title = this.txtTitle2.Text;
                obj.Author = this.txtAuthor2.Text;
                obj.NumberOfPages = Convert.ToInt32(txtNumberOfPages2.Text);
                obj.Borrower = this.txtBorrower2.Text;
                obj.DateOfBorrow = Convert.ToDateTime(txtDateOfBorrow2.Text);

                db.SaveChanges();
            }
            BtnLoad_Click(sender, e);
        }

        private int updatingLibraryID = 0;
        //Pobiera dane z tabeli/grid`a
        private void gridLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridLibraries.SelectedIndex >= 0)
            {
                if (this.gridLibraries.SelectedItems.Count >= 0)
                {
                    if (this.gridLibraries.SelectedItems[0].GetType() == typeof(Library))
                    {
                        Library lib = (Library)this.gridLibraries.SelectedItems[0];

                        this.txtTitle2.Text = lib.Title;
                        this.txtAuthor2.Text = lib.Author;
                        this.txtNumberOfPages2.Text = lib.NumberOfPages.ToString();
                        this.txtBorrower2.Text = lib.Borrower;
                        this.txtDateOfBorrow2.Text = lib.DateOfBorrow.ToString();
                        this.updatingLibraryID = lib.Id;
                    }
                }
            }
        }

        //Usuwa dane z bazy
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Zapobiega przypadkowemu usunieciu danych
            MessageBoxResult msgBoxResult = MessageBox.Show("Are you sure you want to remove this rent?",
                "The rent has been removed",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
                );

            if (msgBoxResult == MessageBoxResult.Yes)
            {
                LibraryDBEntities db = new LibraryDBEntities();

                var r = from d in db.Libraries
                        where d.Id == this.updatingLibraryID
                        select d;

                Library obj = r.SingleOrDefault();

                if (obj != null)
                {
                    db.Libraries.Remove(obj);

                    db.SaveChanges();
                }
            }
            BtnLoad_Click(sender, e);
        }
    }
}
