from timeit import Timer

def test1():
    l = []
    for i in range(1000):
        l = l + [i]



def test2():
    l = []
    for i in range(1000):
        l.append(i)

def test3():
    l = [i for i in range(1000)]

def test4():
    l = list(range(1000))

def test5():
    l = []
    for i in range(1000):
        l += [i]

def test6():
    l = []
    for i in range(1000):
        l.extend([i])

def test7():
    l = []
    for i in range(1000):
        l.insert(0, i)

t1 = Timer('test1()', 'from __main__ import test1')
t2 = Timer('test2()', 'from __main__ import test2')
t3 = Timer('test3()', 'from __main__ import test3')
t4 = Timer('test4()', 'from __main__ import test4')
t5 = Timer('test5()', 'from __main__ import test5')
t6 = Timer('test6()', 'from __main__ import test6')
t7 = Timer('test7()', 'from __main__ import test7')
print('test1: {}'.format(t1.timeit(number=1000)))
print('test2: {}'.format(t2.timeit(number=1000)))
print('test3: {}'.format(t3.timeit(number=1000)))
print('test4: {}'.format(t4.timeit(number=1000)))
print('test5: {}'.format(t5.timeit(number=1000)))
print('test6: {}'.format(t6.timeit(number=1000)))
print('test7: {}'.format(t7.timeit(number=1000)))
