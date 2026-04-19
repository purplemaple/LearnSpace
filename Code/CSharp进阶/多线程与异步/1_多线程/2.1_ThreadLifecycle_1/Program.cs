/* 
 * 本例用于查看传参与不传参时，源码的不同
 */

Thread t1 = new(ThreadMethod1);
t1.Start();

Thread t2 = new(ThreadMethod2);
//传集合、委托等都行
//弊端：Start() 传参时参数是 object? 类，因此可能有类型不一致问题
t2.Start(123444);

void ThreadMethod1() {/* do something */}

void ThreadMethod2(object? obj) {/* do something */}


/* 编译后的源码：
 * 无参：
 * new Thread(new ThreadStart(ThreadMethod1)).Start()
 * 
 * 传参：
 * new Thread(new ParameterizedThreadStart(ThreadMethod2)).Start()
 */
