def merge_sort(arr):
    if len(arr) <= 1:
        return arr
    
    # 递归地分割数组成两半，直到每个子数组只有一个元素
    mid_index = len(arr) // 2
    left_half = merge_sort(arr[:mid_index])
    right_half = merge_sort(arr[mid_index:])

    print(left_half, right_half)
    result = [];

    # 在当前递归层中比较左右两半的元素，按照顺序将较小的元素添加到结果数组中
    left_index = 0
    right_index = 0
    while left_index < len(left_half) and right_index < len(right_half):
        if left_half[left_index] < right_half[right_index]:
            result.append(left_half[left_index])
            left_index += 1
        else:
            result.append(right_half[right_index])
            right_index += 1

    # 将剩余的元素添加到结果数组中
    # 这里不能使用append，因为当index == len时，append将[]添加到结果数组中，出现[1, 2, 3, []]这种错误)
    # result += left_half[left_index:]
    # result += right_half[right_index:]
    result.extend(left_half[left_index:])
    result.extend(right_half[right_index:])

    return result

def test_merge_sort():
    arr = [64, 34, 25, 12, 22, 11, 90, 95, 1]
    print("Original array is:", arr)
    sorted_arr = merge_sort(arr)
    print("Sorted array is:", sorted_arr)

test_merge_sort()

            

