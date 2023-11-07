try
{
    //VoidAsync();          //捕获不到异常
    await TaskAsync();      //可以正常捕获
    //TaskAsync().GetAwaiter().GetResult();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

async void VoidAsync()
{
    await Task.Delay(1000);
    throw new Exception("Something was wrong!");
}

async Task TaskAsync()
{
    await Task.Delay(1000);
    throw new Exception("Something was wrong!");
}