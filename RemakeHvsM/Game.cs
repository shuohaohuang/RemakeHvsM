﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using BattleMethod;
using Checker;
using Constants;
using Menssages;
using Stats;
using Utilities;

namespace Game
{
    class HVsM
    {
        public static void Main()
        {
            int roundCounter = 0,
                remainingAttempts = Constant.MaxAttempts;
            int[] characters =  { 0, 1, 2, 3, 4 },
                randomTurns =  { 4, 4, 4, 4 },
                abilityEffect =  { 2, 100, 3, 500 },
                currentCoolDown =  { 0, 0, 0, 0 };
            float userValue;

            float[,,] statsValues =
            {
                {
                    { 1500f, 3000, 1100, 2000, 7000 },
                    { 2000, 3750, 1500, 2500, 10000 }
                },
                {
                    { 200, 150, 300, 70, 300 },
                    { 300, 250, 400, 120, 400 }
                },
                {
                    { 25, 35, 20, 25, 20 },
                    { 35, 45, 35, 40, 30 }
                }
            };

            float[,] maxValues = new float[5, 3],
                currentValues = new float[5, 3];

            bool validInput,
                hasRemainingAttemptsMenu = true,
                hasRemainingAttempts = true;
            bool[] isHero =  { true, true, true, true, false },
                isGuarding =  { false, false, false, false },
                Alive =  { true, true, true, true, true };

            string userCommand,
                difficulty = "0";

            string[] names =
                {
                    Constant.ArcherName,
                    Constant.BarbarianName,
                    Constant.MageName,
                    Constant.DruidName,
                    Constant.MonsterName
                },
                twoValidInputs = [Constant.OneStr, Constant.ZeroStr],
                threeValidInputs = [Constant.OneStr, Constant.TwoStr, Constant.ThreeStr],
                fourValidInputs =
                    [Constant.OneStr, Constant.TwoStr, Constant.ThreeStr, Constant.FourStr],
                StatsRequirementMsg =

                    [
                        Constant.HpMenuMsg + Constant.RangedInMsg,
                        Constant.AttackMenuMsg + Constant.RangedInMsg,
                        Constant.DmgReductionMenuMsg + Constant.RangedInMsg
                    ],
                boolValidInputs =  { Constant.Yes, Constant.No };

            Console.WriteLine(Constant.MenuMsg);
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

            if (userCommand.Equals(Constant.OneStr))
            {
                //difficulty Selector
                Console.WriteLine(Constant.DifficultyMenuMsg);
                do
                {
                    userCommand = Console.ReadLine() ?? "";
                    validInput = Check.ValidateInput(userCommand, fourValidInputs);
                    Msg.ValidateInput(
                        ref remainingAttempts,
                        ref hasRemainingAttemptsMenu,
                        validInput,
                        Constant.ErrorEndMsg
                    );
                    difficulty = userCommand;
                } while (!validInput && hasRemainingAttemptsMenu);

                if (hasRemainingAttemptsMenu)
                {
                    Console.WriteLine(Constant.RenameMsg);
                    do
                    {
                        userCommand = Console.ReadLine() ?? "";
                        validInput = Check.ValidateInput(userCommand, boolValidInputs);
                        Msg.ValidateInput(
                            ref remainingAttempts,
                            ref hasRemainingAttemptsMenu,
                            validInput,
                            Constant.ErrorEndMsg
                        );
                    } while (!validInput && hasRemainingAttemptsMenu);
                }

                if (userCommand.ToUpper().Equals(Constant.Yes))
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        string oldName = names[i];
                        Console.WriteLine(Constant.RequestNameMsg, names[i]);
                        names[i] = Utility.NameMayus(Console.ReadLine() ?? names[i]);
                    }
                }
                if (difficulty.Equals(Constant.DifficultyPersonalized))
                {
                    for (int character = 0; character < characters.Length; character++)
                    {
                        int minValueRow = 0,
                            maxValueRow = 1;

                        for (int statsType = 0; statsType < 3; statsType++)
                        {
                            do
                            {
                                Console.WriteLine(
                                    $"{Constant.InsertRequestMsg}\n {StatsRequirementMsg[statsType]}",
                                    statsValues[statsType, minValueRow, character],
                                    statsValues[statsType, maxValueRow, character]
                                );
                                userValue = Convert.ToSingle(Console.ReadLine());
                                validInput = Check.InRange(
                                    userValue,
                                    statsValues[statsType, 0, character],
                                    statsValues[statsType, 1, character]
                                );
                                if (!validInput)
                                {
                                    remainingAttempts--;
                                    hasRemainingAttempts = Check.GreaterThan(remainingAttempts);

                                    if (hasRemainingAttempts)
                                    {
                                        Console.WriteLine(Constant.ErrorMsg);
                                    }
                                    else
                                    {
                                        Console.WriteLine(Constant.DefaultHeroStatsMsg);
                                        maxValues[character, statsType] = statsValues[
                                            statsType,
                                            0,
                                            character
                                        ];
                                    }
                                }
                                else
                                {
                                    remainingAttempts = Constant.MaxAttempts;
                                    hasRemainingAttempts = Check.GreaterThan(remainingAttempts);
                                    maxValues[character, statsType] = userValue;
                                }
                            } while (!validInput && hasRemainingAttempts);
                        }
                    }
                }
                else
                {
                    for (int character = 0; character < characters.Length; character++)
                    {
                        Stat.SetPlayerCap(
                            character,
                            statsValues,
                            maxValues,
                            isHero[character],
                            difficulty
                        );
                    }
                }

                while ((Alive[0] || Alive[1] || Alive[2] || Alive[3]) && Alive[4])
                {
                    Battle.RandomOrder(randomTurns);
                    for (int action = 0; action < characters.Length; action++)
                    {
                        if (action < 4)
                        {
                            if (
                                Check.GreaterThan(
                                    currentValues[randomTurns[action], Constant.HpValueColumn]
                                )
                            )
                            {
                                Console.WriteLine(
                                    Constant.RequestCommandMsg,
                                    names[randomTurns[action]]
                                );
                                do
                                {
                                    userCommand = Console.ReadLine() ?? "";
                                    validInput = Check.ValidateInput(userCommand, threeValidInputs);
                                    Msg.ValidateInput(
                                        ref remainingAttempts,
                                        ref hasRemainingAttempts,
                                        validInput,
                                        Constant.DefaultCommandMsg
                                    );

                                    if (
                                        userCommand.Equals(Constant.TwoStr)
                                        && Check.GreaterThan(currentCoolDown[randomTurns[action]])
                                    )
                                    {
                                        Msg.NoticeOnCoolDown(currentCoolDown[randomTurns[action]]);
                                        validInput = !validInput;
                                    }
                                    ;
                                } while (!validInput && hasRemainingAttempts);
                                if (hasRemainingAttempts)
                                {
                                    switch (userCommand)
                                    {
                                        case "1":
                                            Battle.Attack(
                                            barbarian,
                                                monster,
                                                barbarianName,
                                                monsterName
                                            );
                                            break;
                                        case "2":
                                            barbarian[
                                                GameConstant.RowCurrentValues,
                                                GameConstant.ReductionValueColumn
                                            ] = barbarianAbilityEffect;
                                            barbarianAbilityCurrentCD = GameConstant.SkillCd;
                                            Console.WriteLine(
                                                GameConstant.BarbarianAbility,
                                                barbarianName,
                                                GameConstant.BarbarianSkillDuration
                                            );
                                            break;
                                        default:
                                            barbarianDefenseMode = !barbarianDefenseMode;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
