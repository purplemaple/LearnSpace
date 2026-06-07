def quick_sort(arr):
    if len(arr) <= 1:
        return arr

    pivot = arr[len(arr) // 2]

    # left_half = quick_sort([x for x in arr if x < pivot])
    # middle_half = [x for x in arr if x == pivot]
    # right_half = quick_sort([x for x in arr if x > pivot])

    left_half = []
    middle_half = []
    right_half = []
    for index in range(len(arr)):
        if(arr[index]) < pivot:
            left_half.append(arr[index])
        elif arr[index] == pivot:
            middle_half.append(arr[index])
        else:
            right_half.append(arr[index])
    
    return quick_sort(left_half) + middle_half + quick_sort(right_half)

def test_quick_sort():
    arr = [64, 34, 25, 12, 22, 11, 90, 95, 1]
    print("Original array is:", arr)
    sorted_arr = quick_sort(arr)
    print("Sorted array is:", sorted_arr)

test_quick_sort()
