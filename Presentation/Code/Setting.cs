﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Domain;
using Domain.Model;
using Domain.Services;
using MandarinCore;

namespace Presentation.Code
{
    class Setting
    {
        public IRepository Repo { get; private set; }
        public List<VIPClasesBin> LVIPB { get; set; }
        public List<FixedClasses> LVIP { get; set; }
        public EntityStorage storage { get; set; }
        public StudentsClass[] Clases { get; set; }

        public Setting()
        {
            LVIP = new List<FixedClasses>();
            storage = CurrentBase.EStorage;
            this.Clases = storage.Classes;
            LVIPB = new List<VIPClasesBin>();
            BinaryFormatter formatter = new BinaryFormatter();


            if (File.Exists("Setting.dat"))
            {

                using (FileStream fs = new FileStream("Setting.dat", FileMode.OpenOrCreate))
                {
                    LVIPB = (List<VIPClasesBin>)formatter.Deserialize(fs);
                }

                foreach (var item in LVIPB)
                {
                    FixedClasses n = new FixedClasses(Clases[item.Cla], item.Time, storage.ClassRooms[item.Aud]);
                    LVIP.Add(n);
                }

            }
        }

        public Setting(EntityStorage storage, StudentsClass[] Clases)
        {
            LVIP = new List<FixedClasses>();
            LVIPB = new List<VIPClasesBin>();
            this.storage = storage;
            this.Clases = Clases;
            BinaryFormatter formatter = new BinaryFormatter();


            if (File.Exists("Setting.dat"))
            {

                using (FileStream fs = new FileStream("Setting.dat", FileMode.OpenOrCreate))
                {
                    LVIPB = (List<VIPClasesBin>)formatter.Deserialize(fs);
                }

                foreach (var item in LVIPB)
                {
                    FixedClasses n = new FixedClasses(Clases[item.Cla], item.Time, storage.ClassRooms[item.Aud]);
                    LVIP.Add(n);
                }

            }
        }

       

        public List<StudentsClass> GetListClases(Teacher teach)
        {
            List<StudentsClass> List = new List<StudentsClass>();
            foreach (StudentsClass item in Clases)
            {
                if (item.Teacher.Contains(teach)) List.Add(item);
            }
            return List;
        }

        public StudentsClass[] GetVipClasses()
        {
            if (LVIP != null)
            {
                StudentsClass[] vipClasses = new StudentsClass[LVIP.Count];
                for (int classIndex = 0; classIndex < LVIP.Count; classIndex++)
                {
                    vipClasses[classIndex] = LVIP[classIndex].sClass;
                }
                return vipClasses;
            }
            else
            {
                return new StudentsClass[0];
            }
        }        
    }

    [Serializable]
    class VIPClasesBin
    {
        public int Cla { get; set; }
        public int Time { get; set; }
        public int Aud { get; set; }

        public VIPClasesBin(int cla, int time, int aud)
        {
            this.Cla = cla;
            this.Time = time;
            this.Aud = aud;
        }
    }    
}
