using System.Diagnostics;

//生成一个 1 ~ 20 的数组
int[] inputs = Enumerable.Range(1, 20).ToArray();

int[] outputs = new int[inputs.Length];

var sw = Stopwatch.StartNew();

//1. 传统 for 循环，耗时 2161 ms
/*for (int i = 0; i < inputs.Length; i++)
{
    outputs[i] = HeavyJob(inputs[i]);
}*/

//2. Parallel 里的 for 循环，并行执行循环里的所有条件，耗时 168 ms
//Parallel.For(0, outputs.Length, i => outputs[i] = HeavyJob(inputs[i]));

//3. PLINQ 可以用 AsOrdered()保证顺序，耗时 161 ms
outputs = inputs.AsParallel().AsOrdered().Select(x => HeavyJob(x)).ToArray();

Console.WriteLine("Elapsed time：" + sw.ElapsedMilliseconds + " ms");

PrintArray(outputs);

//计算平方值的函数
int HeavyJob(int input)
{
    Thread.Sleep(100);
    return input * input;
}

//打印
void PrintArray<T>(T[] array)
{
    foreach (var item in array)
        Console.Write(item + " ");
}