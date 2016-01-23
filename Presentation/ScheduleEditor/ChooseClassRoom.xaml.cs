﻿using System;
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
using Domain.Model;
using Presentation.Code;
using Domain.Services;

namespace Presentation
{
    /// <summary>
    /// Логика взаимодействия для ChooseClassRoom.xaml
    /// </summary>
    public partial class ChooseClassRoom : Window
    {
        private int TimeRows;
        ScheduleForEdit schedule;
        EntityStorage store;
        private StudentsClass clas;
        
        public ChooseClassRoom(int TimeRows, ScheduleForEdit schedule, StudentsClass clas)
        {
            InitializeComponent();
            this.TimeRows = TimeRows;
            this.schedule = schedule;
            this.clas = clas;
        }

        public ChooseClassRoom(int TimeRows, EntityStorage store, StudentsClass clas)
        {
            InitializeComponent();
            this.TimeRows = TimeRows;
            this.store = store;
            this.clas = clas;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TimeRows == -1)
            {
                radioButtonFree.IsEnabled = false;
                radioButtonAll.IsChecked = true;

            }
            else
            {
                radioButtonFree.IsEnabled = true;
                int k = schedule.GetListClasRoom(clas).Count;
                bool[] free = new bool[k];
                int z = 0;
                foreach (ClassRoom item in schedule.GetListClasRoom(clas))
                {
                    free[z] = schedule.ClassRoomFree(item, TimeRows);
                    z++;
                }
                radioButtonFree.IsChecked = true;
            }
            // listViewClassRoom.ItemsSource = schedule.GetListClasRoom(TimeRows, clas);
           

        }

        private void listViewClassRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewClassRoom.SelectedIndex != -1)
            {
                ClassRoom clas = (ClassRoom)listViewClassRoom.SelectedItem;
                listClassRoomProp.Items.Clear();
                foreach (ClassRoomType Type in clas.Types)
                {
                    listClassRoomProp.Items.Add(Type.Description);
                }
                if (TimeRows == -1)
                {
                    textClass.Text = "Неизвестно";
                    textTeacher.Text = "Неизвестно";
                    textGroop.Text = "Неизвестно";
                }
                else
                {
                    StudentsClass Sclas = schedule.GetStudentsClass(clas, TimeRows);
                    if (Sclas == null)
                    {
                        textClass.Text = "Отсуствует";
                        textTeacher.Text = "Отсуствует";
                        textGroop.Text = "Отсуствует";
                    }
                    else
                    {
                        textClass.Text = "";
                        textTeacher.Text = "";
                        textGroop.Text = "";
                        textClass.Text = Sclas.Name;
                        foreach (Teacher item in Sclas.Teacher)
                        {
                            textTeacher.Text += item.Name + " | ";
                        }

                        foreach (StudentSubGroup item in Sclas.SubGroups)
                        {
                            textGroop.Text += item.NameGroup + "(" + item.NumberSubGroup + ")" + " | ";
                        }
                    }
                }
                button.IsEnabled = true;
            }
        }

        private void radioButtonFree_Checked(object sender, RoutedEventArgs e)
        {
            
            listViewClassRoom.ItemsSource = schedule.GetListFreeClasRoom(TimeRows, schedule.GetListClasRoom(clas));
            listViewClassRoom.SelectedIndex = -1;
            button.IsEnabled = false;
        }
        private void radioButtonAll_Checked_1(object sender, RoutedEventArgs e)
        {
            if(schedule != null )
            {
                listViewClassRoom.ItemsSource = schedule.GetListClasRoom(clas);
            }
           else
            {
                listViewClassRoom.ItemsSource = ScheduleForEdit.GetListClasRoom(store, clas);
            }
            listViewClassRoom.SelectedIndex = -1;
            button.IsEnabled = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(schedule != null) { 
            EditSchedule main = this.Owner as EditSchedule;
            if (main != null)
            {
                main.listViewClassRoom.Items.Clear();
                main.listViewClassRoom.Items.Add(listViewClassRoom.SelectedItem);
                main.btnSet_Click(sender, e);
            }
            if (main.RemovelistBox.SelectedItem != null && TimeRows != -1 && main.listViewClassRoom.Items.Count != 0)
            { main.btnSet.IsEnabled = true; }
            this.Close();
            }
            else
            {
                VIPForm main = this.Owner as VIPForm;
                if (main != null)
                {
                    main.ClassRoomlistView.Items.Clear();
                    main.ClassRoomlistView.Items.Add(listViewClassRoom.SelectedItem);
                }
                this.Close();
            }
        }
    }
}
