﻿using Domain.DataFiles;
using Domain.Model;
using Domain.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Code
{
    static class Save
    {
        public static void SaveSchedule(Schedule schedule)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("schedule.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, schedule);
            }
        }
        public static void SaveSchedule(Schedule schedule, string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path + @"\schedule.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, schedule);
            }
        }

        public static Schedule LoadSchedule()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Schedule schedule = null;
            using (FileStream fs = new FileStream("schedule.dat", FileMode.OpenOrCreate))
            {
                schedule = (Schedule)formatter.Deserialize(fs);
            }
            return schedule;
        }

        public static void SaveSettings(FacultAndGroop sett)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("Settings.dat", FileMode.Create))
            {
                formatter.Serialize(fs, sett);
            }
        }
        public static FacultAndGroop LoadSettings()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FacultAndGroop sett = null;
            using (FileStream fs = new FileStream("Settings.dat", FileMode.OpenOrCreate))
            {
                sett = (FacultAndGroop)formatter.Deserialize(fs);
            }
            return sett;
        }
    }
}
