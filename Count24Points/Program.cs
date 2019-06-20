using System;
using System.Collections.Generic;
using System.Linq;

namespace Count24Points
{

    class Program
    {
        static void Main(string[] args)
        {
            ReInput:
            Console.WriteLine("输入4个数字");

            try
            {
                List<string> strArr = new List<string>(Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries));
                if (strArr.Count < 4)
                {
                    goto ReInput;
                }

                List<float> arr = strArr.Take(4).Select(x =>
                {
                    float i = 0f;
                    if (float.TryParse(x, out i))
                    {
                        return i;
                    }
                    Console.WriteLine("输入错误");
                    throw new Exception();
                }
                ).ToList();

                var result = Check24Begin(new Status()
                {
                    StepNum = 4,
                    NumList = arr,
                    NextCal = null,
                    NextStatus = null
                });
                if (result != null)
                {
                    List<string> printList = PrintResult(result);
                    foreach (var p in printList)
                    {
                        Console.WriteLine(p);
                    }
                    goto ReInput;
                }
                else
                {
                    Console.WriteLine("无解");
                    goto ReInput;
                }
            }
            catch
            {
                goto ReInput;
            }
            Console.ReadKey();
        }

        public static List<string> PrintResult(Status result)
        {
            string op = "";
            switch (result.NextCal.Operate)
            {
                case Operate.加:
                    op = "+";
                    break;
                case Operate.减:
                    op = "-";
                    break;
                case Operate.乘:
                    op = "*";
                    break;
                case Operate.除:
                    op = "/";
                    break;
                default:
                    break;
            }
            if (result.StepNum == 3)
            {

                return new List<string>() { string.Format("{0}{1}{2}={3}", result.NextCal.Num1, op, result.NextCal.Num2, result.NextCal.ResultNum) };
            }
            else
            {
                var l = PrintResult(result.NextStatus);
                l.Add(string.Format("{0}{1}{2}={3}", result.NextCal.Num1, op, result.NextCal.Num2, result.NextCal.ResultNum));
                return l;
            }
        }

        public static Status Check24Begin(Status status)
        {
            if (status.StepNum == 1)
            {
                if (status.NumList[0] == 24)
                {
                    return status;
                }
                else
                {
                    return null;
                }
            }

            List<Status> next = new List<Status>();
            for (int i = 1; i <= 4; i++)
            {
                Operate op = (Operate)i;
                for (int a = 0; a < status.NumList.Count; a++)
                {
                    for (int b = 0; b < status.NumList.Count; b++)
                    {
                        if (a != b)
                        {
                            List<float> newList = new List<float>();
                            for (int nl = 0; nl < status.NumList.Count; nl++)
                            {
                                if (nl != a && nl != b)
                                {
                                    newList.Add(status.NumList[nl]);
                                }
                            }
                            float result = 0f;
                            switch (op)
                            {
                                case Operate.加:
                                    result = status.NumList[a] + status.NumList[b];
                                    break;
                                case Operate.减:
                                    result = status.NumList[a] - status.NumList[b];
                                    break;
                                case Operate.乘:
                                    result = status.NumList[a] * status.NumList[b];
                                    break;
                                case Operate.除:
                                    if (status.NumList[b] != 0)
                                    {
                                        result = status.NumList[a] / status.NumList[b];
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                default:
                                    break;
                            }
                            newList.Add(result);
                            next.Add(new Status()
                            {
                                StepNum = status.StepNum - 1,
                                NextStatus = status,
                                NumList = newList,
                                NextCal = new LastCal()
                                {
                                    Num1 = status.NumList[a],
                                    Num2 = status.NumList[b],
                                    ResultNum = result,
                                    Operate = op
                                }
                            });
                        }
                    }
                }
            }

            foreach (var n in next)
            {
                var s = Check24Begin(n);
                if (s == null)
                    continue;
                return s;
            }
            return null;
        }


        public class Status
        {
            public int StepNum { get; set; }

            public List<float> NumList { get; set; }

            public LastCal NextCal { get; set; }

            public Status NextStatus { get; set; }
        }
        public class LastCal
        {
            public float Num1 { get; set; }

            public float Num2 { get; set; }

            public float ResultNum { get; set; }

            public Operate Operate { get; set; }
        }
        public enum Operate
        {
            加 = 1,
            减 = 2,
            乘 = 3,
            除 = 4
        }
    }
}
