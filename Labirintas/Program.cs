using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Labirintas
{
    class Program
    {
        static Boolean SuVienetukais = true;

        static Boolean DeadEnd;
        static Boolean End;

        static StreamWriter protocol;

        static Position startPos;

        static Lenta Board;
        static int countAll = 1;

        static Stack<int> stack = new Stack<int>();

        static Position GetXYfromP(int arg)
        {
            Position move = new Position();
            switch (arg)
            {
                case 1:
                    move.X = -1;
                    break;
                case 2:
                    move.Y = -1;
                    break;
                case 3:
                    move.X = 1;
                    break;
                case 4:
                    move.Y = 1;
                    break;
            }
            return move;
        } 
        static void Eiti(int arg)
        {
            stack.Push(arg);
            Position move = GetXYfromP(arg);
            Board.CurPos.X += move.X;
            Board.CurPos.Y += move.Y;
            Board.MoveCount++;
        }
        static int Grizti(Boolean backtrack = false)
        {
            if (backtrack)
            {
                if (SuVienetukais)
                {
                    Board.Map[Board.CurPos.X, Board.CurPos.Y] = -1;
                }
                else
                {
                    Board.Map[Board.CurPos.X, Board.CurPos.Y] = 0;
                }
            }
            int arg = stack.Pop();
            Board.MoveCount--;
            Position move = GetXYfromP(arg);
            Board.CurPos.X -= move.X;
            Board.CurPos.Y -= move.Y;

            #region output protocol
            if (backtrack)
            {
                protocol.WriteLine("Backtrack");
            }
            #endregion

            return arg;
        }
        static Boolean Tikrinimas()
        {
            int arg = stack.Peek();
            if ((Board.CurPos.X < Board.SizeX) && (Board.CurPos.Y < Board.SizeY) && (Board.CurPos.X >= 0) && (Board.CurPos.Y >= 0))
            {
                if (Board.Map[Board.CurPos.X, Board.CurPos.Y] == 0)
                {
                    #region output protocol
                    arg = stack.Peek();
                    String line = countAll.ToString("000000");
                    for (int i = 0; i < Board.MoveCount - 1; i++) line += "-";
                    line += String.Format("R{0}. X={1} Y={2}. L={3}. Laisva", arg.ToString(), Board.CurPos.X + 1, Board.CurPos.Y + 1, Board.MoveCount);
                    protocol.WriteLine(line);
                    countAll++;
                    #endregion
                    Board.Map[Board.CurPos.X, Board.CurPos.Y] = Board.MoveCount;
                    return false;
                }
                else if (Board.Map[Board.CurPos.X, Board.CurPos.Y] == 1)
                {
                    #region output protocol
                    arg = stack.Peek();
                    String line = countAll.ToString("000000");
                    for (int i = 0; i < Board.MoveCount - 1; i++) line += "-";
                    line += String.Format("R{0}. X={1} Y={2}. L={3}. Siena", arg.ToString(), Board.CurPos.X + 1, Board.CurPos.Y + 1, Board.MoveCount);
                    protocol.WriteLine(line);
                    countAll++;
                    #endregion
                    return true;
                }
                else
                {
                    #region output protocol
                    arg = stack.Peek();
                    String line = countAll.ToString("000000");
                    for (int i = 0; i < Board.MoveCount - 1; i++) line += "-";
                    line += String.Format("R{0}. X={1} Y={2}. L={3}. Siulas", arg.ToString(), Board.CurPos.X + 1, Board.CurPos.Y + 1, Board.MoveCount);
                    protocol.WriteLine(line);
                    countAll++;
                    #endregion
                    return true;
                }
            }
            else
            {
                End = true;
                return true;
            }
        }
        static void Main(string[] args)
        {

            DeadEnd = false;
            End = false;

            protocol = new StreamWriter("History.txt");

            #region FileRead
            string[] lines = File.ReadAllLines("Start.txt");
            lines = lines.Reverse().ToArray();
            Board = new Lenta((lines[0].Length + 1)/2, lines.Count(), new Position());
            for(int i = 0; i < Board.SizeX; i++)
            {
                for(int j = 0; j < Board.SizeY; j++)
                {
                    Board.Map[i, j] = Int32.Parse(lines[j].ElementAt(i * 2).ToString());
                    if(Board.Map[i, j] == 2)
                    {
                        Board.CurPos.X = i;
                        Board.CurPos.Y = j;
                        startPos = new Position(i, j);
                    }
                }
            }
            #endregion
            #region Protocol 1 dalis
            protocol.WriteLine("1 DALIS. Duomenys");
            protocol.WriteLine("1.1. Labirintas");
            protocol.WriteLine("");
            for (int i = Board.SizeY - 1; i >= 0; i--)
            {
                string line = (i + 1).ToString("00") + " | ";
                for (int j = 0; j < Board.SizeX; j++)
                {
                    line += (Board.Map[j, i]).ToString("00");
                    line += " ";
                }
                protocol.WriteLine(line);
            }
            string line1 = "---+";
            string line2 = "   | ";
            for (int j = 0; j < Board.SizeX; j++)
            {
                line1 += "---";
                line2 += (j + 1).ToString("00") + " ";
            }
            line1 += ">";
            protocol.WriteLine(line1);
            protocol.WriteLine(line2);
            protocol.WriteLine("");
            protocol.WriteLine(String.Format("1.2. Pradinė padėtis X={0}, Y={1}. L={2}.", Board.CurPos.X + 1, Board.CurPos.Y + 1, Board.MoveCount));
            #endregion
            protocol.WriteLine("");
            protocol.WriteLine("2 DALIS. Vykdymas");
            protocol.WriteLine("");
            int arg = 1;
            do
            {
                if (arg > 4)
                {
                    if (stack.Count == 0)
                    {
                        DeadEnd = true;
                    }
                    else
                    {
                        arg = Grizti(true);
                        arg++;
                    }
                }
                else
                {
                    Eiti(arg);
                    if (Tikrinimas() == true)
                    {
                        arg = Grizti();
                        arg++;
                    }
                    else
                    {
                        arg = 1;
                    }
                }
            } while (!End && !DeadEnd);
            protocol.WriteLine("");
            protocol.WriteLine("3 DALIS. Rezultatai");
            protocol.WriteLine("");

            if (DeadEnd)
            {
                protocol.WriteLine("3.1. Kelias neegzistuoja");
            }
            else
            {
                protocol.WriteLine("3.1. Kelias rastas");
                protocol.WriteLine("3.2. Kelias grafiškai");
                protocol.WriteLine("");
                for (int i = Board.SizeY - 1; i >= 0; i--)
                {
                    string line = (i + 1).ToString("00") + " | ";
                    for (int j = 0; j < Board.SizeX; j++)
                    {
                        line += (Board.Map[j, i]).ToString(Board.Map[j, i] == -1 ? "" : "00");
                        line += " ";
                    }
                    protocol.WriteLine(line);
                    Console.WriteLine(line);
                }
                line1 = "---+";
                line2 = "   | ";
                for (int j = 0; j < Board.SizeX; j++)
                {
                    line1 += "---";
                    line2 += (j + 1).ToString("00") + " ";
                }
                line1 += ">";
                protocol.WriteLine(line1);
                protocol.WriteLine(line2);
                Console.WriteLine(line1);
                Console.WriteLine(line2);
                protocol.WriteLine("");
                line1 = "3.3. Kelias taisyklėmis: ";
                line2 = String.Format("3.4. Kelias viršūnėmis: [X={0}, Y={1}], ", startPos.X + 1, startPos.Y + 1);
                Stack<int> x = new Stack<int>(stack);
                while (x.Count > 0)
                {
                    int ixas = x.Pop();
                    line1 += String.Format("R{0}, ", ixas.ToString());
                    Position move = GetXYfromP(ixas);
                    startPos.X += move.X;
                    startPos.Y += move.Y;
                    line2 += String.Format("[X={0}, Y={1}], ", startPos.X + 1, startPos.Y + 1);
                }
                protocol.WriteLine(line1.Substring(0, line1.Length - 2) + ".");
                protocol.WriteLine(line2.Substring(0, line2.Length - 2) + ".");
                Console.WriteLine("");

            }
            protocol.Close();
        }
    }
}
