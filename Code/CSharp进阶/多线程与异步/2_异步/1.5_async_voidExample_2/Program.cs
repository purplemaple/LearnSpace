var demo = new Demo();

/*
 * Func 事件，可以注册 Task 或者 async，因为这两者都是带返回值的(返回值都是 Task)
 */
demo.CallbackFunc += TaskAsync;

/*
 * 因为 Action 事件只能注册不含返回值的方法，因此只能用 async void 来声明方法
 */
demo.CallbackAction += VoidAsync;


async Task TaskAsync()
{
    return;
}

async void VoidAsync()
{

}

class Demo
{
    //带返回值的事件
    public event Func<Task> CallbackFunc;

    //不带返回值的事件
    public event Action CallbackAction;
}