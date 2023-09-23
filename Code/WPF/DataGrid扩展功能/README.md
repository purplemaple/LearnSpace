## DataGrid扩展功能的代码部分

含三个演示项目，且逐级递进
1. DataFilter：演示对 DataGrid 中元素筛选的操作

2. SelectedItemsBinding：演示 DataGrid 中多选项如何绑定到 ViewModel

3. DataPage：演示 DataGrid 的分页功能
   - 3.1：使用项目 1 中的 Filter 实现(有缺陷)
     - 缺陷：所有数据过一遍过滤器，过滤器内还要获取数据下标，组合起来时间复杂度O(n^2)(即使像下面例子，使用 Dictionary 记录元素下标，但当遇到元素增减时，下标打乱又得重新计算，时间复杂度还是很高)
   - 3.2：使用 LINQ 的 Skip() 以及 Take() 操作数据集合实现分页与跳转(时间复杂度O(1))
   - 3.3：在 3.2 项目的基础上，实现当用户删除 dataGrid 中某一行时，依旧能正常分页与跳转