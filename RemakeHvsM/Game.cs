using System;
using Constants;
using Stats;
using Checker;
using Menssages;

namespace Game
{
    class HVsM
    {
        public static void Main()
        {
            int roundCounter= 0,
            remainingAttempts = GameConstant.MaxAttempts;
            int[] heroes = { 0, 1, 2, 3 },
                remainHeroes = heroes,
                abilityEffect = { 2, 100, 3, 500 },
                coolDown = { Constant.SkillCd, Constant.SkillCd, Constant.SkillCd, Constant.SkillCd };
            
            float[,,] statsValues = { { {1500f , 3000, 1100, 2000,7000 } , { 2000, 3750, 1500, 2500, 10000} },
                                 { { 200, 150, 300, 70,300 } , { 300, 250, 400, 120,400 }  },
                                 { { 25, 35, 20, 25,20 } , { 35, 45, 35, 40,30 } }
            };

            float[,] maxValues = new float[5, 3],
                currentValues = new float[5, 3];

            bool validInput,
                hasRemainingAttemptsMenu = true,
                hasRemainingAttempts = true;
            bool[] isHero = { true, true, true, true, false };

            string userCommand; 

            string[] name = { Constant.ArcherName, Constant.BarbarianName, Constant.MageName, Constant.DruidName, Constant.MonsterName },
                twoValidInputs = [Constant.OneStr, Constant.ZeroStr],
                threeValidInputs =
                    [Constant.OneStr, Constant.TwoStr, Constant.ThreeStr],
                fourValidInputs =
                    [
                        Constant.OneStr,
                        Constant.TwoStr,
                        Constant.ThreeStr,
                        Constant.FourStr
                    ],
                StatsRequirementMsg =

                    [
                        Constant.HpMenuMsg + Constant.RangedInMsg,
                        Constant.AttackMenuMsg + Constant.RangedInMsg,
                        Constant.DmgReductionMenuMsg + Constant.RangedInMsg
                    ];
            

            do
            {
                userCommand = Console.ReadLine() ?? "";
                validInput = Check.ValidateInput(userCommand, twoValidInputs);
                Msg.ValidateInput(
                    ref remainingAttempts,
                    ref hasRemainingAttemptsMenu,
                    validInput,
                    Constant.ErrorEndMsg
                );
            } while (!validInput && hasRemainingAttemptsMenu);


            bool[] Alive = { true, true, true, true, true };
            for (int i = 0; i < maxValues.GetLength(0); i++)
            {
                Stat.SetPlayerCap(i, statsValues, maxValues, true, "1");

            }
            currentValues = maxValues;
        }
    }
}
