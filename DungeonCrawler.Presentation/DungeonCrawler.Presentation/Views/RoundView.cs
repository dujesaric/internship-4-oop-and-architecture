﻿using DungeonCrawler.Data;
using DungeonCrawler.Data.Abstractions;
using DungeonCrawler.Data.Enums;
using DungeonCrawler.Domain.Factories;
using DungeonCrawler.Domain.Services;
using DungeonCrawler.Domain.Utils;
using System;

namespace DungeonCrawler.Presentation.Views
{
    public static class RoundView
    {
        public static void PlayRound(Game game, Battle battle)
        {
            Round round = RoundFactory.CreateNewIn(battle);

            if (round.IsStunRound)
            {
                AttackView.HandleStunning(battle);

                return;
            }

            Console.WriteLine("\nPlease choose attack strategy for this round (choose wisely!):" +
                "\n0. Direct attack (or just enter)" +
                "\n1. Side attack" +
                "\n2. Counter attack" +
                "\n");

            var isMoveValid = false;
            var chosenMoveType = 0;
            while (isMoveValid == false)
            {
                int.TryParse(Console.ReadLine(), out chosenMoveType);

                if (!IsMoveValid(chosenMoveType))
                {
                    Console.WriteLine("\nWrong move input, please try again.\n");
                    continue;
                }

                isMoveValid = true;
            }

            Move heroMove = MoveFactory.CreateNewByTypeFor((MoveType)chosenMoveType, battle.Hero);
            Move monsterMove = RandomMoveGenerator.GenerateFor(battle.Monster);

            Console.WriteLine($"\n[Move battle: Hero - {heroMove} VS Monster - {monsterMove}]\n");

            var roundWinner = MoveHandler.Handle(heroMove, monsterMove);

            if (roundWinner is Hero hero)
            {
                Console.WriteLine("Hero won in move battle and attacks monster.\n");
                AttackView.HandleAttack(hero, battle.Monster, battle, game);
            }
            else if (roundWinner is Monster monster)
            {
                Console.WriteLine("Monster won in move battle and attacks hero.\n");
                AttackView.HandleAttack(monster, battle.Hero, battle, game);
            }
            else
            {
                Console.WriteLine("Move battle ended with draw and no one attacks in this round.\n");
            }

            if (game.IsJumbusActive)
            {
                JumbusHandler.Handle(game);
                ConsoleWriter.ColoredWriteColoredBackground("\n!*!*!*!*!* J-U-M-B-U-S done *!*!*!*!*!\n", ConsoleColor.White, ConsoleColor.DarkMagenta);
            }

            Console.WriteLine($"\n\n{battle}\n\n");
        }

        private static bool IsMoveValid(int chosenMoveType)
        {
            return chosenMoveType == (int)MoveType.DirectAttack || chosenMoveType == (int)MoveType.SideAttack || chosenMoveType == (int)MoveType.CounterAttack;
        }
    }
}
