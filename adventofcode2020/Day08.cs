using System;
using System.Collections.Generic;
using System.IO;

namespace adventofcode
{
    class Day08
    {
        class Instruction
        {
            public string Op { get; set; }
            public int Arg { get; set; }
            public bool Used { get; set; }

            public const string ACC = "acc";
            public const string JMP = "jmp";
            public const string NOP = "nop";
        }

        class Machine
        {
            public List<Instruction> Instructions { get; set; }

            public int Acc { get; set; }

            public int Ip { get; set; }

            public string Status { get; set; } = STATUS_NOT_RUN;

            public const string STATUS_SUCCESS = "SUCCESS";
            public const string STATUS_ERROR = "ERROR";
            public const string STATUS_NOT_RUN = "NOT RUN";

            public Machine(List<Instruction> instructions)
            {
                Instructions = instructions;
            }

            public void Run()
            {
                Reset();

                while (Ip != Instructions.Count)
                {
                    if (Instructions[Ip].Used)
                    {
                        Status = STATUS_ERROR;
                        return;
                    }

                    Instructions[Ip].Used = true;

                    switch (Instructions[Ip].Op)
                    {
                        case Instruction.JMP:
                            Ip += Instructions[Ip].Arg;
                            break;
                        case Instruction.ACC:
                            Acc += Instructions[Ip].Arg;
                            Ip += 1;
                            break;
                        case Instruction.NOP:
                        default:
                            Ip += 1;
                            break;
                    }
                }

                Status = STATUS_SUCCESS;
            }

            public IEnumerable<Machine> GetNopJmpMutations()
            {
                var mutationIndex = 0;

                while (mutationIndex < Instructions.Count)
                {
                    if (Instructions[mutationIndex].Op == Instruction.JMP)
                    {
                        Instructions[mutationIndex].Op = Instruction.NOP;
                        yield return this;
                        Instructions[mutationIndex].Op = Instruction.JMP;
                    }

                    else if (Instructions[mutationIndex].Op == Instruction.NOP)
                    {
                        Instructions[mutationIndex].Op = Instruction.JMP;
                        yield return this;
                        Instructions[mutationIndex].Op = Instruction.NOP;
                    }

                    mutationIndex += 1;
                }
            }

            private void Reset()
            {
                Status = STATUS_NOT_RUN;
                Acc = default;
                Ip = default;

                foreach (var instruction in Instructions)
                {
                    instruction.Used = false;
                }
            }
        }

        public static void start()
        {
            part1();
            part2();
        }

        // Immediately before any instruction is executed a second time, what value is in the accumulator?
        static void part1()
        {
            var machine = new Machine(ReadInstructions());
            machine.Run();
            Console.WriteLine($"Day 08: After running into an error, acc = {machine.Acc}."); // 1487
        }

        // What is the value of the accumulator after the program terminates?
        static void part2()
        {
            var machine = new Machine(ReadInstructions());
            machine.Run();

            foreach (var mutation in machine.GetNopJmpMutations())
            {
                mutation.Run();

                if (mutation.Status == Machine.STATUS_SUCCESS)
                {
                    Console.WriteLine($"Day 08: After changing and running successfully, acc = {mutation.Acc}."); //
                    break;
                }
            }
        }

        private static List<Instruction> ReadInstructions()
        {
            var instructions = new List<Instruction>();

            foreach (var line in File.ReadLines("input/8"))
            {
                var op = line.Split(" ")[0];
                var arg = Int32.Parse(line.Split(" ")[1]);

                instructions.Add(new Instruction(){ Op = op, Arg = arg });
            }

            return instructions;
        }
    }
}
