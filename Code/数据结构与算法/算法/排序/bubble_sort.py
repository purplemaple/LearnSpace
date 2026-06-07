def bubble_sort(arr):
    n = len(arr)
    for i in range(n):
        for j in range(0, n - i - 1):
            if arr[j] > arr[j + 1]:
                arr[j], arr[j + 1] = arr[j + 1], arr[j]
        print("After pass {}: {}".format(i + 1, arr))
    return arr

# 优化后的冒泡排序算法，增加了一个标志变量来检测是否发生了交换，如果没有发生交换，说明数组已经有序，可以提前结束排序
# 这种优化可以在最佳情况下（数组已经有序）将时间复杂度降低到 O(n)，而在最坏情况下仍然是 O(n^2)。
# 但是对于[1 ,2, 3, 4, 5, 1]这种前面有序，局部严重逆序数组，几乎没有任何提升，因为冒泡排序每次只能移动一位元素，所以仍然需要进行 n-1 次比较和交换才能将最后一个元素移动到正确的位置。
def bubble_sort_optimized(arr):
    n = len(arr)
    for i in range(n):
        swapped = False
        for j in range(0, n - i - 1):
            if arr[j] > arr[j + 1]:
                arr[j], arr[j + 1] = arr[j + 1], arr[j]
                swapped = True
                print("Swapped {} and {}: {}".format(arr[j + 1], arr[j], arr))
        print("After pass {}: {}".format(i + 1, arr))
        print("i:{}, j:{}, swapped: {}".format(i, j, swapped))
        if not swapped:     # 如果没有发生交换，说明数组已经有序，可以提前结束排序
            break
    return arr

def test_bubble_sort():
    arr = [64, 34, 25, 12, 22, 11, 90, 95, 1]
    print("Original array is:", arr)
    sorted_arr = bubble_sort(arr)
    print("Sorted array is:", sorted_arr)

def test_bubble_sort_optimized():
    arr2 = [64, 34, 25, 12, 22, 11, 90, 95, 1]
    print("\nOriginal array is:", arr2)
    sorted_arr2 = bubble_sort_optimized(arr2)
    print("Sorted array is:", sorted_arr2)

if __name__ == "__main__":
    test_bubble_sort()
    test_bubble_sort_optimized()