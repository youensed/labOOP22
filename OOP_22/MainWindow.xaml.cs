using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OOP_22
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }
        // змінення виділеного тексту
        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }
        // відкриття файлу
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }
        //збереження файлу
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|New Text Document (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }
        // вирівнювання тексту по лівій стороні
        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }
        // вирівнювання тексту по центру 
        private void btnCenter_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }
        // вирівнювання тексту по правій стороні
        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }
        // реалізація для кнопки "Копіювати"
        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rtbEditor.Copy();
        }
        //реалізація для кнопки "Вставити"
        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rtbEditor.Paste();
        }
        //реалізація для кнопки "Вирізати"
        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rtbEditor.Cut();
        }
        //реалізація для кнопки "Виділити все"
        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rtbEditor.SelectAll();
        }
        //реалізація кнопки для вставки зображення в файл
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // діалогове вікно для вибору файлу
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|Усі файли (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // шлях до вибраного файлу
                string imagePath = openFileDialog.FileName;

                // об'єкт BitmapImage і встановити йому шлях до файлу
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                // об'єкт Image з вибраного зображення
                Image imageControl = new Image();
                imageControl.Source = bitmapImage;

                // об'єкт InlineUIContainer для вставки зображення в текстовий редактор
                InlineUIContainer container = new InlineUIContainer(imageControl);

                // InlineUIContainer в поточне місце курсора в текстовому редакторі
                if (rtbEditor.Selection.IsEmpty)
                {
                    Paragraph paragraph = new Paragraph(container);
                    rtbEditor.Document.Blocks.Add(paragraph);
                }
            }
        }
        //зміна мови на українську
        private void Ukrainian_Click(object sender, RoutedEventArgs e)
        {
            File.Header = "Файл";
            Open.Header = "Відкрити";
            SaveAs.Header = "Зберегти як...";
            Edit.Header = "Правити";
            Copy.Header = "Копіювати";
            SelectAll.Header = "Вибрати все";
            Paste.Header = "Вставити";
            Cut.Header = "Вирізати";
            Language.Header = "Мова";
            Ukrainian.Header = "Українська";
            English.Header = "English";
        }
        //змінa мови на англійську
        private void English_Click(object sender, RoutedEventArgs e)
        {
            File.Header = "File";
            Open.Header = "Open";
            SaveAs.Header = "Save as...";
            Edit.Header = "Edit";
            Copy.Header = "Copy";
            SelectAll.Header = "Select All";
            Paste.Header = "Paste";
            Cut.Header = "Cut";
            Language.Header = "Language";
            Ukrainian.Header = "Українська";
            English.Header = "English";
        }
        //додавання нумерованого списку
        private void btnNumList_Click(object sender, RoutedEventArgs e)
        {
            List list = new List();
            list.MarkerStyle = TextMarkerStyle.Decimal;
            ListItem listItem = new ListItem(new Paragraph(new Run(" ")));
            list.ListItems.Add(listItem);
            rtbEditor.Document.Blocks.Add(list);

        }
    }
}
