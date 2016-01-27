﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;
using Domain.Model;
using Domain.FactorInterfaces;

namespace OtherFactors
{
    class TwoClassesInWeek : IFactor, IFactorProgramData
    {
        int fine;
        bool isBlock;
        StudentsClass[,] sClasses;

        #region IFactor

        public int GetFineOfAddedClass(ISchedule schedule, EntityStorage eStorage)
        {
            if (sClasses == null)
            { return 0; }
            if (ClassesInWeek.LotOfClassesInWeek(2, sClasses, schedule, schedule.GetTempClass()))
            {
                if (isBlock)
                    return Constants.BLOCK_FINE;
                else
                    return fine;
            }
            return 0;
        }
        
        public int GetFineOfFullSchedule(ISchedule schedule, EntityStorage eStorage)
        {
            int fineResult = 0;
            if (sClasses == null)
            { return fineResult; }
            for (int specialClassIndex = 0; specialClassIndex < sClasses.GetLength(0); specialClassIndex++)
            {
                if (ClassesInWeek.LotOfClassesInWeek(2, sClasses, schedule, sClasses[specialClassIndex, 0]))
                {
                    if (isBlock)
                        return Constants.BLOCK_FINE;
                    else
                        fineResult += fine;
                }
            }
            return fineResult;
        }

        

        public string GetDescription()
        {
            return "Если четыре пары за две недели, то каждую неделю должно быть по две пары";
        }
        public string GetName()
        {
            return "Две пары в неделю";
        }

        public void Initialize(int fine = 0, bool isBlock = false, object data = null)
        {
            if (fine >= 0 && fine <= 100)
            {
                this.fine = fine;
                this.isBlock = isBlock;
                if (fine == 100)
                    this.isBlock = true;
            }
            if(data != null)
            { 
            try
            {
                StudentsClass[,] tempArray = (StudentsClass[,]) data;
                sClasses = new StudentsClass[tempArray.GetLength(0), tempArray.GetLength(1)];

                for (int rowIndex = 0; rowIndex < tempArray.GetLength(0); rowIndex++)
                {
                    //в получаемом массиве, в каждой строке должно быть по 4 пары - по две на каждую неделю
                    for (int classIndex = 0; classIndex < 4; classIndex++)
                    {
                        if (tempArray[rowIndex, classIndex] != null)
                            sClasses[rowIndex, classIndex] = tempArray[rowIndex, classIndex];
                        else
                            throw new NullReferenceException();
                    }
                }
            }
            catch(Exception ex)
            {
                new Exception("Неверный формат данных. Требуется двумерный массив Nx4 типа StudentsClass. " + ex.Message);
            }
            }
            else { sClasses = null; }
        }
        public Guid? GetDataTypeGuid()
        {
            return new Guid("CF2B2F17-D8D0-4848-878C-DE9B9B988392");
        }

        #endregion

        #region IFactorProgramData

        public object CreateAndReturnData(EntityStorage eStorage)
        {
            return GroupClasses.GetGroupFourSameClasses(eStorage.Classes);
        }

        #endregion
    }
}
