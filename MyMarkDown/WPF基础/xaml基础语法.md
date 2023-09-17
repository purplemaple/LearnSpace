# WPF基础语法
### Lable演示
```xml
    <!--
            宽度为width
            高度为height
            内容为content
            水平方向排列设置 需要用到HorizontalAlignment
            垂直方向排列设置 需要用到VerticalAlignment
            外边距 使用Margin 四个数字 对应方向分别为 左上右下,一个数字 代表所有方向的外边距 
            两个数字 分别代表 左右  上下
        -->
        <Label  Width="180" Height="30" Content="我是个label控件" 
                HorizontalAlignment="Right" 
                VerticalAlignment="Top" Margin="0,10" FontSize="18" Foreground="Blue"/>
```

### TextBlock演示
```xml

    <!--
        1.TextBlock 设置的text内容 如果标签里面有文本内容 会将其加到text内容后面
        2.如果想进行换行操作 可以在Textblock双标签里面 加上LineBreak标签
        在之后的内容就会换行
        -->
    <TextBlock  Text="我是个TextBlock33" FontSize="30" FontWeight="Light" Foreground="Red">
        我是文本一<LineBreak/>
        我是文本二<LineBreak/>
        我是文本三
    </TextBlock>
```

### Button演示
```xml
    <Button Width="100" Height="30" 
        HorizontalAlignment="Left" 
        VerticalAlignment="Top" 
        Content="我是个按钮" 
        Margin="20,20,0,0" 
        Background="Orange"
        Foreground="White"
        BorderThickness="5,0,10,0" 
        BorderBrush="Transparent" 
        Click="Button_Click_1"  
        MouseMove="Button_MouseMove"
        />

```

### Border演示
```xml
 <!--
     BorderThickness边框宽度默认为0 直接设置一个参数 代表的是四周宽度
    BorderThickness 有四个参数时 分别代表左边 上边 右边 下边
    我们设置边框颜色加上边框宽度 才能准确显示边框的效果
    如果我们需要设置角的弧度 需要使用CornerRadius属性
-->
    <Border Width="200" Height="80" Background="LightBlue"
        BorderBrush="Red"  BorderThickness="1,1,2,1" CornerRadius="15">

        <!--<Label HorizontalAlignment="Center" VerticalAlignment="Center"
        FontSize="20" Foreground="White">我是label1</Label>-->

        <Button Background="Transparent" 
            FontSize="20"  
            Foreground="White"
            Content="我是个按钮" 
            Click="Button_Click" 
            BorderBrush="Transparent">
        </Button>
            
        <!--
        <Grid>
            <Label 
                FontSize="20" Foreground="White">
                我是label1
            </Label>
            <Label HorizontalAlignment="Center"
                VerticalAlignment="Center"
                FontSize="20" Foreground="White">
                我是label2
            </Label>
        </Grid>-->
    </Border>
```

### RadioButton演示
```xml
    <!--
        radioButton 如果需要实现分组的效果 
        1. 使用布局容器来嵌套 （如grid /stackPanel等等）
        2. 可以使用groupName进行区分 分组
    -->
    <RadioButton Content="男" 
            GroupName="sex"  
            Margin="10,100"  
            FontSize="20" 
            Foreground="red" 
            VerticalAlignment="Top" 
            HorizontalAlignment="Left" ></RadioButton>

    <RadioButton Content="女" 
            GroupName="sex" 
            Margin="80,100"  
            FontSize="20" 
            Foreground="red" 
            VerticalAlignment="Top" 
            HorizontalAlignment="Left" ></RadioButton>

    <RadioButton Content="语文" 
            GroupName="course" 
            FontSize="20" 
            Foreground="Blue" 
            VerticalAlignment="Top" 
            HorizontalAlignment="Left" ></RadioButton>
    <RadioButton Content="数学"  
            GroupName="course" 
            Margin="70,0" 
            FontSize="20" 
         Foreground="Blue" 
            VerticalAlignment="Top" 
            HorizontalAlignment="Left" ></RadioButton>
    <RadioButton Content="历史"  
            GroupName="course" 
            Margin="140,0"  
            FontSize="20" 
            Foreground="Blue" 
            VerticalAlignment="Top" 
            HorizontalAlignment="Left" ></RadioButton>
    <RadioButton Content="外语"  
            GroupName="course" 
            Margin="210,0"  
            FontSize="20" 
            Foreground="Blue" 
            VerticalAlignment="Top" 
            HorizontalAlignment="Left" ></RadioButton>
```

### CheckBox演示
前台：
```xml
<Grid x:Name="gridMain">
        <Label FontWeight="Bold" HorizontalAlignment="Center"
               VerticalAlignment="Top" Background="DarkBlue"
               Foreground="White">选课系统</Label>

        <CheckBox Content="C语言" Margin="300,40,0,0" VerticalAlignment="Top"
                  HorizontalAlignment="Left" Width="150" Height="30"
                  FontSize="18"/>

        <CheckBox Content="计算机组成原理" Margin="300,80,0,0" VerticalAlignment="Top"
                  HorizontalAlignment="Left" Width="150" Height="30"
                  FontSize="18"/>

        <CheckBox Content="计算机网络" Margin="300,120,0,0" VerticalAlignment="Top"
                  HorizontalAlignment="Left" Width="150" Height="30"
                  FontSize="18"/>

        <Button x:Name="Button1" Content="获取选课" Width="120" Height="30" 
                Click="Button1_Click" HorizontalAlignment="Left" VerticalAlignment="Top"
                Margin="300,200,0,0"/>
</Grid>
```
后台：
```cs
    private void Button1_Click(object sender, RoutedEventArgs e)
    {
        UIElementCollection children = gridMain.Children;
        StringBuilder sbf = new StringBuilder("我的选课为：");
        foreach (UIElement item in children)
        {
            if(item is CheckBox && (item as CheckBox).IsChecked.Value)
            {
                sbf.Append((item as CheckBox).Content + ",");
            }
        }
        MessageBox.Show(sbf.ToString());
    }
```
```xml
<!--字体背景阴影-->
<StackPanel.Effect>
    <DropShadowEffect Color="LightGray"/>
</StackPanel.Effect>
```
.Effect有什么用？

```xml
  <TextBlock FontSize="60" FontFamily="Kaiti"
  Text="{Binding Restaurant.Name, StringFormat=欢迎光临-\{0\}}" />
```


